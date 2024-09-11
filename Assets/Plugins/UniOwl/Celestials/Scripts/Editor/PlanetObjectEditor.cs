using UniOwl.Components.Editor;
using UnityEditor;

namespace UniOwl.Celestials.Editor
{
    [CustomEditor(typeof(PlanetObject))]
    public class PlanetObjectEditor : ScriptableComponentListWithPrefabEditor
    {
        protected override void OnEnable()
        {
            base.OnEnable();

            if (rootGO)
            {
                var planet = (PlanetObject)serializedObject.targetObject;
                var rootSO = new SerializedObject(rootGO.GetComponent<Planet>());
                rootSO.FindProperty("_planetObject").objectReferenceValue = planet;
                rootSO.ApplyModifiedProperties();
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }
    }
}