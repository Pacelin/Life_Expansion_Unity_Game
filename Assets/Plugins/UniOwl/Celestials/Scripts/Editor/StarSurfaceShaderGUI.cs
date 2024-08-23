using System;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
public class StarSurfaceShaderGUI : ShaderGUI
{
    private static readonly string[] physicalProperties = new string[] { "Temperature", "_Emission" };
    private static readonly string[] noiseSamplingProperties = new string[] { "Flattening", "Offset_Speed", "Warp_Speed" };
    private static readonly string[] noiseSurfaceProperties = new string[] { "Octaves", "Persistence", "Scale", "Warp_Scale" };
    private static readonly string[] noiseDetailProperties = new string[] { "Detail_Threshold", "Detail_Scale_Factor", "Detail_Redistribution" };
    private static readonly string coloringThresholdProperty = "Color_Threshold";
    private static readonly string coloringOutlineProperty = "Outline_Power";

    public bool foldoutPhysical;
    public bool foldoutNoise;
    public bool foldoutColoring;

    Material targetMat;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        if (targetMat == null)
        {
            targetMat = materialEditor.target as Material;
            foldoutPhysical = EditorPrefs.GetBool(nameof(foldoutPhysical), false);
            foldoutNoise = EditorPrefs.GetBool(nameof(foldoutNoise), false);
            foldoutColoring = EditorPrefs.GetBool(nameof(foldoutColoring), false);
        }
        DrawPhysicalSettings(materialEditor, properties);
        DrawNoiseSettings(materialEditor, properties);
        DrawColoringSettings(materialEditor, properties);

        SaveState();
    }

    private void SaveState()
    {
        EditorPrefs.SetBool(nameof(foldoutPhysical), foldoutPhysical);
        EditorPrefs.SetBool(nameof(foldoutNoise), foldoutNoise);
        EditorPrefs.SetBool(nameof(foldoutColoring), foldoutColoring);
    }

    private void DrawPhysicalSettings(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        foldoutPhysical = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutPhysical, "Physical", EditorStyles.foldoutHeader);
        if (foldoutPhysical)
            DisplayShaderProperties(materialEditor, properties, physicalProperties);
        EditorGUILayout.EndFoldoutHeaderGroup();
    }

    private void DrawNoiseSettings(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        EditorGUIUtility.hierarchyMode = true;

        foldoutNoise = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutNoise, "Noise");
        if (foldoutNoise)
        {
            EditorGUILayout.LabelField("Sampling", EditorStyles.boldLabel);
            DisplayShaderProperties(materialEditor, properties, noiseSamplingProperties);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Surface", EditorStyles.boldLabel);
            DisplayShaderProperties(materialEditor, properties, noiseSurfaceProperties);
            EditorGUILayout.Space();

            bool enableDetail = Array.IndexOf(targetMat.shaderKeywords, "DETAIL") != -1;
            EditorGUI.BeginChangeCheck();
            enableDetail = EditorGUILayout.ToggleLeft("Detail", enableDetail, EditorStyles.boldLabel);
            if (EditorGUI.EndChangeCheck())
                if (enableDetail)
                    targetMat.EnableKeyword("DETAIL");
                else
                    targetMat.DisableKeyword("DETAIL");
            if (enableDetail)
                DisplayShaderProperties(materialEditor, properties, noiseDetailProperties);
            EditorGUILayout.Space();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }

    private void DrawColoringSettings(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        foldoutColoring = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutColoring, "Coloring", EditorStyles.foldoutHeader);
        if (foldoutColoring)
        {
            DisplayShaderProperty(materialEditor, properties, coloringThresholdProperty);

            bool enableOutline = Array.IndexOf(targetMat.shaderKeywords, "OUTLINE") != -1;
            EditorGUI.BeginChangeCheck();
            enableOutline = EditorGUILayout.ToggleLeft("Outline", enableOutline, EditorStyles.boldLabel);
            if (EditorGUI.EndChangeCheck())
                if (enableOutline)
                    targetMat.EnableKeyword("OUTLINE");
                else
                    targetMat.DisableKeyword("OUTLINE");
            if (enableOutline)
            {
                EditorGUI.indentLevel++;
                DisplayShaderProperty(materialEditor, properties, coloringOutlineProperty);
                EditorGUI.indentLevel--;
            }

        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }

    private void DisplayShaderProperties(MaterialEditor materialEditor, MaterialProperty[] properties, string[] propertyNames)
    {
        EditorGUI.indentLevel++;
        foreach (var name in propertyNames)
            DisplayShaderProperty(materialEditor, properties, name);
        EditorGUI.indentLevel--;
    }

    private void DisplayShaderProperty(MaterialEditor materialEditor, MaterialProperty[] properties, string propertyName)
    {
        MaterialProperty prop = FindProperty(propertyName, properties);
        materialEditor.ShaderProperty(prop, prop.displayName);
    }
}
