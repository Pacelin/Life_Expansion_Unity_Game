using UniOwl.Components.Editor;
using UnityEditor;

namespace UniOwl.Celestials.Editor
{
    [CustomEditor(typeof(PlanetSurface))]
    public class PlanetSurfaceEditor : ScriptableComponentWithPrefabEditor
    {
        private PlanetSurface _surface => (PlanetSurface)serializedObject.targetObject;

        private bool objectUpdated;
        
        public override void Init(ScriptableComponentListEditor baseEditor)
        {
            base.Init(baseEditor);
            baseEditor.listOrChildUpdated += ListOrChildUpdated;
        }

        private void GenerateTerrainEditor()
        {
            try
            {
                PlanetProgressReporter.progressUpdated += UpdateProgress;
                PlanetSurfaceGenerator.GeneratePlanetTerrain(_surface, EditableGO);
                ((ScriptableComponentListWithPrefabEditor)_baseEditor).UpdatePreviewImage();
            }
            finally
            {
                PlanetProgressReporter.progressUpdated -= UpdateProgress;
                EditorUtility.ClearProgressBar();
            }
        }

        private static void UpdateProgress(string stage, string description)
        {
            EditorUtility.DisplayProgressBar(stage, description, -1f);
        }

        public override void OnComponentCreate()
        {
            base.OnComponentCreate();
            PlanetAssetUtils.CreateMeshes(_surface, EditableGO);
            PlanetAssetUtils.CreateTextures(_surface, EditableGO);
            EditorPlanetAssetUtils.SaveMeshes(Root, EditableGO);
            EditorPlanetAssetUtils.SaveTextures(Root, EditableGO);
        }

        public override void OnComponentSave()
        {
            base.OnComponentSave();
            if (!objectUpdated) return;

            EditorPlanetAssetUtils.ReinitializeTexturesIfNeeded(EditableGO, _surface.Textures.resolution);
            GenerateTerrainEditor();
            EditorPlanetAssetUtils.CompressTextures(_surface, EditableGO);
        }
        
        public override void OnComponentDestroy()
        {
            // Can't call base.OnComponentDestroy() here because it deletes materials before textures.
            EditorPlanetAssetUtils.DestroyMeshes(EditableGO);
            EditorPlanetAssetUtils.DestroyTextures(EditableGO);
            PrefabManager.DestroyChildrenSharedMaterials(EditableGO);
        }

        private const int previewTextureResolution = 64, previewMeshResolution = 64;

        private void ListOrChildUpdated()
        {
            objectUpdated = true;
            if (!_surface.UpdateTerrain) return;

            EditorPlanetAssetUtils.ReinitializeTexturesIfNeeded(EditableGO, previewTextureResolution);
            PlanetSurfaceGenerator.GeneratePlanetTerrain(_surface, EditableGO, previewMeshResolution, previewTextureResolution - 1);
            ((ScriptableComponentListWithPrefabEditor)_baseEditor).UpdatePreviewImage();
        }
    }
}