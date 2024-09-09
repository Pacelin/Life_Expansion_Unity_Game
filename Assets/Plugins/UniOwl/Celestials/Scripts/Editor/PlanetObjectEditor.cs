using System;
using UniOwl.Components;
using UniOwl.Components.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace UniOwl.Celestials.Editor
{
    [CustomEditor(typeof(PlanetObject), editorForChildClasses: true)]
    public class PlanetObjectEditor : ScriptableComponentListWithPrefabEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = base.CreateInspectorGUI();
            root.TrackSerializedObjectValue(serializedObject, PlanetChanged);
            return root;
        }

        private void PlanetChanged(SerializedObject _)
        {
            foreach (ScriptableComponent component in targetComponentList.Components)
                UpdateComponent(component);
        }

        protected override void ComponentChanged(ScriptableComponent component)
        {
            UpdateComponent(component);
        }

        private void UpdateComponent(ScriptableComponent component)
        {
            var planetComponent = (PlanetComponent)component;
            
            var path = AssetDatabase.GetAssetPath(rootGO);
            using var scope = new PrefabUtility.EditPrefabContentsScope(path);

            var index = Array.IndexOf(targetComponentList.Components, component);
            var editableChild = scope.prefabContentsRoot.transform.GetChild(index).gameObject;
            planetComponent.UpdateVisual(editableChild);
        }
    }
}