using System;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UniOwl.Components.Editor
{
    [CustomEditor(typeof(ScriptableComponentListWithPrefab<>), editorForChildClasses: true)]
    public class ScriptableComponentListWithPrefabEditor : ScriptableComponentListEditor
    {
        [SerializeField]
        protected GameObject skybox;
        
        private VisualElement _contentViewport;
        
        private SerializedProperty _rootProp;

        public GameObject rootGO => (GameObject)_rootProp.objectReferenceValue;
        public GameObject editableRoot => _editableRoot;

        private Scene _previewScene;
        private GameObject _editableRoot;
        private Camera _previewCamera;
        private Light _previewLight;
        private Texture2D _previewTexture;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            _rootProp = serializedObject.FindProperty("_root");

            // OnEnable() is called twice: on SO creation and SO confirmation. AssetPostprocessor is being called after creation, so on first call rootGO is null. 
            if (!rootGO) return;
            
            _previewScene = PreviewSceneUtils.CreatePreviewScene();
            _editableRoot = PreviewSceneUtils.LoadPrefabIntoPreviewScene(_previewScene, rootGO);
            _previewCamera = PreviewSceneUtils.CreatePreviewCameraAndAddToScene(_previewScene);
            _previewLight = PreviewSceneUtils.CreatePreviewLightAndAddToScene(_previewScene);
            PreviewSceneUtils.AddAdditionalPrefabsToScene(_previewScene, skybox);
            _previewTexture = PreviewSceneUtils.CreatePreviewTexture();
            UpdatePreviewImage();
        }

        protected virtual void OnDisable()
        {
            if (!rootGO) return;
            
            foreach (ScriptableComponentEditor componentEditor in componentEditors)
                ((ScriptableComponentWithPrefabEditor)componentEditor).OnComponentSave();
            
            PreviewSceneUtils.UnloadPrefabAndClosePreviewScene(_previewScene, rootGO);
        }

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = base.CreateInspectorGUI();
            root.TrackSerializedObjectValue(serializedObject, ListChanged);
            root.styleSheets.Remove(_celestialStyleSheet);

            var scrollView = new ScrollView(ScrollViewMode.Vertical)
            {
                verticalScrollerVisibility = ScrollerVisibility.AlwaysVisible,
            };

            scrollView.AddToClassList("inner-scroll");
            scrollView.Add(root);
            
            VisualElement previewFoldout = CreatePreviewFoldout();
            
            var outerRoot = new VisualElement();
            outerRoot.styleSheets.Add(_celestialStyleSheet);
            outerRoot.AddToClassList("outer-root");
            outerRoot.Add(scrollView);
            outerRoot.Add(previewFoldout);
            
            outerRoot.RegisterCallback<GeometryChangedEvent, VisualElement>((evt, rootArgs) =>
            {
                // Only do this if the view port is null to save resources.
                if (_contentViewport == null)
                {
                    // Find the template container.
                    TemplateContainer rootVisualContainer = FindParent<TemplateContainer>(rootArgs);
                    if (rootVisualContainer != null)
                    {
                        // Find the view port element.
                        _contentViewport = rootVisualContainer.Q<VisualElement>("unity-content-viewport");
                    }
                }

                // The viewport exists.
                if (_contentViewport != null)
                {
                    // Update the root size to match the entire inspector.
                    rootArgs.style.height = _contentViewport.resolvedStyle.height - 65f;
                }
            }, outerRoot);
            
            return outerRoot;
        }
        
        private static T FindParent<T>(VisualElement element, string name = null) where T : VisualElement
        {
            VisualElement parent = element;
            do
            {
                parent = parent.parent;
                if (parent == null || parent.GetType() != typeof(T)) continue;

                if (!string.IsNullOrEmpty(name) && parent.name != name)
                    continue;

                return (T)parent;
            } while (parent != null);

            return null;
        }
        
        private VisualElement CreatePreviewFoldout()
        {
            var previewFoldout = new Foldout
            {
                text = "Prefab preview",
            };
            previewFoldout.AddToClassList("prefab-preview");

            var previewImage = new VisualElement();
            previewImage.AddToClassList("prefab-preview-image");
            previewImage.contentContainer.style.backgroundImage = _previewTexture;
            
            var saveButton = new Button()
            {
                text = "Save Preview...",
            };
            saveButton.clicked += SavePreviewTexture;
            previewFoldout.Add(previewImage);
            previewFoldout.Add(saveButton);
            
            return previewFoldout;
        }

        private const string PreviewTexturePath = "Assets/Plugins/UniOwl/Editor Default Resources/T_Preview.png";

        private Texture2D GetPreviewTexture()
        {
            // TODO: move inside PreviewSceneUtils
            int size = 512;
            Texture2D texture = new Texture2D(size, size);

            RenderTexture oldRT = _previewCamera.targetTexture;
            RenderTexture targetRT = RenderTexture.GetTemporary(size, size, 24, RenderTextureFormat.ARGB32);

            var rect = new Rect(Vector2.zero, new Vector2(size, size));

            _previewCamera.rect = rect;
            _previewCamera.targetTexture = targetRT;
            _previewCamera.forceIntoRenderTexture = true;

            var flags = _previewCamera.clearFlags;
            var bg = _previewCamera.backgroundColor;
            
            _previewCamera.clearFlags = CameraClearFlags.Nothing;
            _previewCamera.backgroundColor = Color.clear;
            
            _previewCamera.Render();

            _previewCamera.clearFlags = flags;
            _previewCamera.backgroundColor = bg;
            
            RenderTexture tempActive = RenderTexture.active;
            RenderTexture.active = targetRT;
            
            texture.ReadPixels(rect, 0, 0);
            texture.Apply();

            RenderTexture.active = tempActive;

            _previewCamera.targetTexture = oldRT;
            RenderTexture.ReleaseTemporary(targetRT);

            return texture;
        }
        
        private void SavePreviewTexture()
        {
            var copy = GetPreviewTexture();
            
            string assetName = AssetDatabase.GenerateUniqueAssetPath(PreviewTexturePath);

            File.WriteAllBytes(assetName, copy.EncodeToPNG()); 
            AssetDatabase.ImportAsset(assetName, ImportAssetOptions.ForceUpdate);
            AssetDatabase.Refresh();
        }

        protected override void AddComponent(Type type)
        {
            base.AddComponent(type);
        }

        protected override ScriptableComponent CreateComponent(Type type, ScriptableComponentList list)
        {
            var component = (ScriptableComponentWithPrefab)base.CreateComponent(type, list);
            return component;
        }

        protected override int RemoveComponent(ScriptableComponent component)
        {
            int index = base.RemoveComponent(component);
            DestroyImmediate(_editableRoot.transform.GetChild(index).gameObject);
            return index;
        }

        protected override void EnableStateChanged(ScriptableComponent component, bool value)
        {
            var index = Array.IndexOf(targetComponentList.Components, component);
            var variant = _editableRoot.transform.GetChild(index).gameObject;
            variant.SetActive(value);
        }

        protected override void ComponentChanged(ScriptableComponent component)
        {
            base.ComponentChanged(component);
            UpdateComponent(component);
            UpdatePreviewImage();
        }

        private void UpdateComponent(ScriptableComponent component)
        {
            var prefabComponent = (ScriptableComponentWithPrefab)component;
            
            var index = Array.IndexOf(targetComponentList.Components, component);
            var editableChild = _editableRoot.transform.GetChild(index).gameObject;
            
            prefabComponent.UpdateVisual(editableChild);
        }
        
        protected override void ListChanged(SerializedObject so)
        {
            base.ListChanged(so);
            foreach (ScriptableComponent component in targetComponentList.Components)
                UpdateComponent(component);
            UpdatePreviewImage();
        }

        public void UpdatePreviewImage()
        {
            var bounds = new Bounds();
            var renderers = _editableRoot.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
                bounds.Encapsulate(renderer.bounds);

            Vector3 cameraPosition = bounds.center + new Vector3(0f, bounds.size.y, -bounds.size.z);
            
            _previewCamera.transform.position = cameraPosition;
            _previewCamera.transform.LookAt(bounds.center);
            
            PreviewSceneUtils.RenderPreviewScene(_previewCamera, _previewTexture);
        }
    }
}