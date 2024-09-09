using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace UniOwl.Components.Editor
{
    [CustomEditor(typeof(ScriptableComponentListWithPrefab<>), editorForChildClasses: true)]
    public class ScriptableComponentListWithPrefabEditor : ScriptableComponentListEditor
    {
        private SerializedProperty _rootProp;

        [SerializeField]
        private GameObject prefab;
        
        protected GameObject rootGO => (GameObject)_rootProp.objectReferenceValue;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _rootProp = serializedObject.FindProperty("_root");
            TryCreatePrefabVariant();
        }

        private void TryCreatePrefabVariant()
        {
            serializedObject.Update();
            
            if (rootGO != null) return;
            
            var folderPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(serializedObject.targetObject));
            string namePrefix = serializedObject.targetObject.name;
            
            var variant = PrefabManager.CreatePrefabVariant(folderPath, namePrefix, prefab);
            
            _rootProp.objectReferenceValue = variant;
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
            component.Variant = variant;
            PrefabManager.CreateMaterialVariantsFromOriginals(rootGO, rootGO.transform.childCount - 1);
            
            return component;
        }

        protected override int RemoveComponent(ScriptableComponent component)
        {
            var index = base.RemoveComponent(component);

            PrefabManager.DestroyChildrenSharedMaterials(((ScriptableComponentWithPrefab)component).Variant);
            PrefabManager.RemoveChildObjectFromPrefab(rootGO, index);

            return index;
        }

        protected override void EnableStateChanged(ScriptableComponent component, bool value)
        {
            var index = Array.IndexOf(targetComponentList.Components, component);
            PrefabManager.SetChildObjectActive(rootGO, index, value);
        }
    }
}