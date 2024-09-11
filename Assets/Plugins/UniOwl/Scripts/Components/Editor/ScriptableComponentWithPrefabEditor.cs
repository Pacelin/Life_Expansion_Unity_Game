using System;
using UnityEditor;
using UnityEngine;

namespace UniOwl.Components.Editor
{
    [CustomEditor(typeof(ScriptableComponentWithPrefab), editorForChildClasses: true)]
    public class ScriptableComponentWithPrefabEditor : ScriptableComponentEditor
    {
        protected GameObject Root => ((ScriptableComponentListWithPrefabEditor)_baseEditor).rootGO;

        protected GameObject EditableRoot => ((ScriptableComponentListWithPrefabEditor)_baseEditor).editableRoot;

        protected GameObject EditableGO
        {
            get
            {
                int index = Array.IndexOf(_component.List.Components, _component);
                return EditableRoot.transform.GetChild(index).gameObject;
            }
        }

        public override void OnComponentCreate()
        {
            PrefabManager.CreatePrefabVariant(EditableRoot, ((ScriptableComponentWithPrefab)_component).Prefab);
            PrefabManager.CreateMaterialVariantsFromOriginals(Root, EditableRoot, EditableRoot.transform.childCount - 1);
        }

        public override void OnComponentDestroy()
        {
            PrefabManager.DestroyChildrenSharedMaterials(EditableGO);
        }
    }
}