using UnityEditor;
using UnityEngine;

namespace UniOwl.Celestials.Editor
{
    [CustomEditor(typeof(PlanetSettings))]
    public class PlanetSettingsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            GUI.enabled = false;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            GUI.enabled = true;
         
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_planet"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_model"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_textures"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_physical"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_generation"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("GrassColor"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("RockColor"));
            EditorGUILayout.Space();
            
            if (GUILayout.Button("Random Seed"))
            {
                serializedObject.FindProperty("_generation").FindPropertyRelative("seed").intValue = Random.Range(int.MinValue, int.MaxValue);
            }
            if (GUILayout.Button("Generate Planet"))
            {
                var settings = (PlanetSettings)target;
                PlanetCreatorEditor.CreatePlanet(settings);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}