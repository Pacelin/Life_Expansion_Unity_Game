using System;
using UniOwl.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using EditorStyles = UniOwl.Editor.EditorStyles;

namespace UniOwl.Components.Editor
{
    [CustomEditor(typeof(ScriptableComponent), editorForChildClasses: true)]
    public class ScriptableComponentEditor : UnityEditor.Editor
    {
        private static readonly string[] ExcludeFields = { "m_ObjectHideFlags", "m_Script", "_active", "_prefab", "_variant", };

        private string _editorPrefExpanded; 
        public bool Expanded
        {
            get => EditorPrefs.HasKey(_editorPrefExpanded) ? EditorPrefs.GetBool(_editorPrefExpanded) : true;
            set => EditorPrefs.SetBool(_editorPrefExpanded, value);
        }

        protected ScriptableComponent _component;

        public event Func<ScriptableComponent, int> componentDestroyed;
        public event Action<ScriptableComponent, bool> componentActiveStateChanged;
        public event Action<SerializedObject> componentStateChanged;

        public void Init()
        {
            _component = (ScriptableComponent)serializedObject.targetObject;

            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(_component, out string guid, out long localId);
            _editorPrefExpanded = $"{typeof(ScriptableComponentEditor).FullName}/{guid}/{localId}";
        }
        
        public override VisualElement CreateInspectorGUI()
        {
            var foldout = CreateFoldout(serializedObject);
            foldout.TrackSerializedObjectValue(serializedObject, componentStateChanged);
            return foldout;
        }
        
        private VisualElement CreateFoldout(SerializedObject component)
        {
            var foldout = CreateFoldoutHeader(!Expanded);
            foldout.Bind(component);
            foldout.RegisterValueChangedCallback(evt => Expanded = evt.newValue);

            var content = GetFoldoutContent();
            foldout.contentContainer.Add(content);
            
            return foldout;
        }
        
        private Foldout CreateFoldoutHeader(bool folded)
        {
            var activeProp = serializedObject.FindProperty("_active");
            var active = activeProp.boolValue;
            
            var compType = _component.GetType();
            var displayName = ScriptableComponentUtils.GetDisplayName(compType);
            
            var foldout = new Foldout
            {
                text = displayName,
                value = !folded,
            };
            foldout.AddToClassList("custom-foldout");
            foldout.EnableInClassList("custom-foldout-disabled", !active);
            
            var enableToggle = new Toggle
            {
                value = active,
                toggleOnLabelClick = false,
            };
            enableToggle.AddToClassList("custom-foldout-enable-toggle");
            enableToggle.BindProperty(activeProp);
            
            enableToggle.RegisterValueChangedCallback(evt =>
            {
                foldout.EnableInClassList("custom-foldout-disabled", !evt.newValue);
                _component.SetActiveInternal(evt.newValue);
                componentActiveStateChanged?.Invoke(_component, evt.newValue);
            });

            foldout.Q<Toggle>().contentContainer.Insert(0, enableToggle);

            var destroyButton = new Button
            {
                style = { backgroundImage = EditorStyles.binIcon, },
            };
            destroyButton.AddToClassList("icon-button");
            destroyButton.AddToClassList("icon-button-12px");

            destroyButton.clicked += () =>
            {
                foldout.RemoveFromHierarchy();
                componentDestroyed?.Invoke(_component);
            };
            
            foldout.Q<Toggle>().contentContainer.Add(destroyButton);

            var moreButton = new Button
            {
                style = { backgroundImage = EditorStyles.paneOptionsIcon, },
            };
            moreButton.AddToClassList("icon-button");
            moreButton.AddToClassList("icon-button-12px");
                
            foldout.Q<Toggle>().contentContainer.Add(moreButton);

            return foldout;
        }

        private VisualElement GetFoldoutContent()
        {
            VisualElement container = new();
            
            foreach (SerializedProperty child in serializedObject.GetChildren(ExcludeFields))
            {
                var propertyField = new PropertyField(child);
                propertyField.BindProperty(child);
                container.Add(propertyField);
            }

            return container;
        }
    }
}