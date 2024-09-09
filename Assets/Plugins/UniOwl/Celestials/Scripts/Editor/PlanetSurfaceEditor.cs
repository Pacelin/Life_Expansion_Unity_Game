using System;
using System.Collections.Generic;
using System.Linq;
using UniOwl.Components.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UniOwl.Celestials.Editor
{
    [CustomEditor(typeof(PlanetSurface))]
    public class PlanetSurfaceEditor : ScriptableComponentEditor
    {
        private static readonly int s_normalMap = Shader.PropertyToID("_NormalMap");
        private static readonly int s_heightMap = Shader.PropertyToID("_HeightMap");

        // TODO: убрать отсюда
        private static readonly Dictionary<TextureFormat, TextureFormat> compressFormatMap = new()
        {
            { TextureFormat.R8, TextureFormat.BC4 },
            { TextureFormat.RG16, TextureFormat.BC5 },
            { TextureFormat.RGB24, TextureFormat.BC7 },
            { TextureFormat.RGBA32, TextureFormat.BC7 },
        };
        
        private Material[] quadMaterials;

        private PlanetSurface _surface => (PlanetSurface)serializedObject.targetObject;
        
        private void OnEnable()
        {
            quadMaterials = _surface.Variant.GetComponentsInChildren<MeshRenderer>().Select(renderer => renderer.sharedMaterial).ToArray();
        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = base.CreateInspectorGUI();

            var generateButton = new Button { text = "Generate Terrain", };
            root.contentContainer.Add(generateButton);
            generateButton.clicked += GenerateTerrainEditor;
            return root;
        }

        // TODO: криво
        private void GenerateTerrainEditor()
        {
            try
            {
                PlanetProgressReporter.progressUpdated += UpdateProgress;
                GenerateTerrain();
            }
            finally
            {
                PlanetProgressReporter.progressUpdated -= UpdateProgress;
                EditorUtility.ClearProgressBar();
            }
        }

        // TODO: не сюда
        private static void UpdateProgress(string stage, string description)
        {
            EditorUtility.DisplayProgressBar(stage, description, -1f);
        }
        
        private void GenerateTerrain()
        {
            var planet = (PlanetObject)_component.List;
            var surface = planet.GetComponent<PlanetSurface>();
            int index = Array.IndexOf(planet.Components, surface);
            
            var go = surface.Variant;
            var path = AssetDatabase.GetAssetPath(go);
            
            // TODO: работа с ассетами ниже не сюда.
            CreateTextures();
            
            // TODO: сделать свой метод для создания мешей.
            using var scope = new PrefabUtility.EditPrefabContentsScope(path);
            var surfaceGO = scope.prefabContentsRoot.transform.GetChild(index).gameObject;

            var filters = surfaceGO.GetComponentsInChildren<MeshFilter>();

            // TODO: очень похоже на содержимое TryCreateTexture(). попробовать обобщить. не сюда
            for (int face = 0; face < 6; face++)
            {
                var mesh = filters[face].sharedMesh;
                
                if (mesh)
                    AssetDatabase.RemoveObjectFromAsset(mesh);

                // TODO: названия
                mesh = new Mesh();
                mesh.name = "a";
                filters[face].sharedMesh = mesh;
                
                AssetDatabase.AddObjectToAsset(mesh, go);
            }

            PlanetSurfaceGenerator.GeneratePlanetTerrain(surface, surfaceGO);
            CompressTextures();
        }

        // TODO: не сюда
        private void CreateTextures()
        {
            for (int face = 0; face < 6; face++)
            {
                var mat = quadMaterials[face];
                
                if (_surface.Textures.generateNormals)
                    TryCreateTexture(mat, s_normalMap, TextureFormat.RGB24);
                if (_surface.Textures.generateHeights)
                    TryCreateTexture(mat, s_heightMap, TextureFormat.R8);
            }
            AssetDatabase.SaveAssets();
        }

        // TODO: не сюда.
        private void TryCreateTexture(Material mat, int id, TextureFormat format)
        {
            var texture = mat.GetTexture(id);
            
            if (texture)
                AssetDatabase.RemoveObjectFromAsset(texture);
            
            texture = new Texture2D(
                _surface.Textures.resolution,
                _surface.Textures.resolution,
                format,
                false,
                true,
                true)
            {
                hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector,
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp,
                // TODO: название
                name = "a"
            };
            mat.SetTexture(id, texture);
            
            AssetDatabase.AddObjectToAsset(texture, mat);
        }

        // TODO: не сюда. попробовать AssetPostProcessor?
        private void CompressTextures()
        {
            if (!_surface.Textures.compression) return;
            
            for (int face = 0; face < 6; face++)
            {
                var mat = quadMaterials[face];
                
                if (_surface.Textures.generateNormals)
                    CompressTexture(mat, s_normalMap, TextureFormat.RGB24);
                if (_surface.Textures.generateHeights)
                    CompressTexture(mat, s_heightMap, TextureFormat.R8);
            }
            AssetDatabase.SaveAssets();
        }

        // TODO: не сюда.
        private void CompressTexture(Material mat, int id, TextureFormat format)
        {
            var texture = (Texture2D)mat.GetTexture(id);
            EditorUtility.CompressTexture(texture, compressFormatMap[format], _surface.Textures._compressionQuality);
        }
    }
}