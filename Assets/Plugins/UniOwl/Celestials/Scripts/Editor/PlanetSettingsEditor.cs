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
            
            var settings = (PlanetSettings)target;
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_planet"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_model"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_textures"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_physical"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_generation"));

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_appearance"));
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("tempLevel"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("atmosphereLevel"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("overallLevel"));
            
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                
                if (settings.Planet)
                {
                    settings.Planet.SetOverallLevel(settings.overallLevel);
                    settings.Planet.SetAtmosphereLevel(settings.atmosphereLevel);
                    settings.Planet.SetTemperatureLevel(settings.tempLevel);
                    settings.Planet.UpdatePlanetAppearance();
                    PrefabUtility.SavePrefabAsset(settings.Planet.gameObject);
                }
            }
            
            EditorGUILayout.Space();
            
            if (GUILayout.Button("Random Seed"))
            {
                serializedObject.FindProperty("_generation").FindPropertyRelative("seed").uintValue = unchecked((uint)Random.Range(int.MinValue, int.MaxValue));
            }
            if (GUILayout.Button("Generate Planet"))
            {
                PlanetCreatorEditor.CreatePlanet(settings);
                settings.Planet.SetOverallLevel(settings.overallLevel);
                settings.Planet.SetAtmosphereLevel(settings.atmosphereLevel);
                settings.Planet.SetTemperatureLevel(settings.tempLevel);
                settings.Planet.UpdatePlanetAppearance();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}