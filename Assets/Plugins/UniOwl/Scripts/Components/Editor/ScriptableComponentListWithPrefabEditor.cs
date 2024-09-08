using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace UniOwl.Components.Editor
{
    [CustomEditor(typeof(ScriptableComponentListWithPrefab<>), editorForChildClasses: true)]
    public class ScriptableComponentListWithPrefabEditor : ScriptableComponentListEditor
    {
        protected SerializedProperty rootProp;

        [SerializeField]
        private GameObject prefab;
        
        protected GameObject rootGO => (GameObject)rootProp.objectReferenceValue;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            serializedObject.Update();

            rootProp = serializedObject.FindProperty("_root");
            TryCreatePrefabVariant();
        }

        private void TryCreatePrefabVariant()
        {
            serializedObject.Update();
            
            if (rootGO != null) return;
            
            var folderPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(serializedObject.targetObject));
            var name = serializedObject.targetObject.name + $"_{prefab.name}_Variant.prefab";
            var path = Path.Combine(folderPath, name);
            path = AssetDatabase.GenerateUniqueAssetPath(path);
            
            var source = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            var root = PrefabUtility.SaveAsPrefabAsset(source, path);
            DestroyImmediate(source);

            rootProp.objectReferenceValue = root;
            serializedObject.ApplyModifiedProperties();
        }

        protected override void AddComponent(Type type)
        {
            base.AddComponent(type);
        }

        protected override ScriptableComponent CreateComponent(Type type, ScriptableComponentList list)
        {
            var component = (ScriptableComponentWithPrefab)base.CreateComponent(type, list);

            var variant = PrefabManager.AddPrefabToRootPrefabAsVariant(rootGO,component.Prefab);
            component.Instance = variant;
            CreateMaterialVariantsFromOriginals();
            
            return component;
        }

        protected override int RemoveComponent(ScriptableComponent component)
        {
            var index = base.RemoveComponent(component);

            DestroyMaterialVariantsFromOriginals(((ScriptableComponentWithPrefab)component).Instance);
            PrefabManager.RemoveChildObjectFromPrefab(rootGO, index);

            return index;
        }

        protected override void EnableStateChanged(ScriptableComponent component, bool value)
        {
            var index = Array.IndexOf(targetComponentList.Components, component);
            PrefabManager.SetChildObjectActive(rootGO, index, value);
        }

        private void CreateMaterialVariantsFromOriginals()
        {
            var path = AssetDatabase.GetAssetPath(rootGO);
            using var scope = new PrefabUtility.EditPrefabContentsScope(path);
            
            var componentGO = scope.prefabContentsRoot.transform.GetChild(scope.prefabContentsRoot.transform.childCount - 1);
            foreach (var renderer in componentGO.GetComponentsInChildren<Renderer>())
            {
                var materials = renderer.sharedMaterials;

                for (int i = 0; i < materials.Length; i++)
                {
                    var materialVariant = new Material(materials[i])
                    {
                        parent = materials[i],
                        hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector,
                    };
                    AssetDatabase.AddObjectToAsset(materialVariant, rootGO);

                    materials[i] = materialVariant;
                }

                renderer.sharedMaterials = materials;
            }
        }

        private void DestroyMaterialVariantsFromOriginals(GameObject go)
        {
            foreach (var renderer in go.GetComponentsInChildren<Renderer>())
            {
                var materials = renderer.sharedMaterials;

                foreach (Material mat in materials)
                    DestroyImmediate(mat, true);
            }
        }
    }
}