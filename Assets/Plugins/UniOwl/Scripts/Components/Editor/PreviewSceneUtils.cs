using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.SceneManagement;

namespace UniOwl.Components.Editor
{
    public static class PreviewSceneUtils
    {
        private static readonly GraphicsFormat s_graphicsFormat = GraphicsFormatUtility.GetGraphicsFormat(
            RenderTextureFormat.ARGBFloat,
            RenderTextureReadWrite.Default);

        public static Scene CreatePreviewScene()
        {
            Scene previewScene = EditorSceneManager.NewPreviewScene();

            return previewScene;
        }
        
        public static GameObject LoadPrefabIntoPreviewScene(Scene previewScene, GameObject prefab)
        {
            string path = AssetDatabase.GetAssetPath(prefab);
            PrefabUtility.LoadPrefabContentsIntoPreviewScene(path, previewScene);

            GameObject editableGO = previewScene.GetRootGameObjects()[0];

            return editableGO;
        }

        public static void UnloadPrefabAndClosePreviewScene(Scene previewScene, GameObject prefab)
        {
            string path = AssetDatabase.GetAssetPath(prefab);
            
            GameObject editableGO = previewScene.GetRootGameObjects()[0];
            
            PrefabUtility.SaveAsPrefabAsset(editableGO, path);
            PrefabUtility.UnloadPrefabContents(editableGO);
            EditorSceneManager.ClosePreviewScene(previewScene);
        }
        
        public static Camera CreatePreviewCameraAndAddToScene(Scene previewScene)
        {
            var go = new GameObject("Preview Camera");
            var camera = go.AddComponent<Camera>();

            camera.cameraType = CameraType.Preview;
            camera.aspect = 1f;
            camera.backgroundColor = Color.black;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.scene = previewScene;
            camera.targetTexture = new RenderTexture(
                width: 256,
                height: 256,
                colorFormat: s_graphicsFormat,
                depthStencilFormat: GraphicsFormat.D24_UNorm,
                mipCount: 1);

            go.transform.position = Vector3.back * 10f + Vector3.up;
            go.transform.LookAt(Vector3.zero);
            SceneManager.MoveGameObjectToScene(go, previewScene);
            
            return camera;
        }

        public static Light CreatePreviewLightAndAddToScene(Scene previewScene)
        {
            var go = new GameObject("Preview Light");
            var light = go.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1f;
            light.shadows = LightShadows.None;
            light.color = Color.white;

            go.transform.forward = Vector3.Normalize(Vector3.right + Vector3.down + Vector3.forward);
            SceneManager.MoveGameObjectToScene(go, previewScene);

            return light;
        }

        public static void AddAdditionalPrefabsToScene(Scene previewScene, params GameObject[] prefabs)
        {
            if (prefabs == null)
                return;
            
            foreach (GameObject go in prefabs)
            {
                if (!go)
                    continue;
                var instance = Object.Instantiate(go);
                SceneManager.MoveGameObjectToScene(instance, previewScene);
            }
        }

        public static Texture2D CreatePreviewTexture()
        {
            var tex = new Texture2D(
                width: 256,
                height: 256,
                s_graphicsFormat,
                mipCount: 1,
                TextureCreationFlags.DontInitializePixels);

            return tex;
        }

        public static void RenderPreviewScene(Camera camera, Texture2D target)
        {
            camera.Render();
            Graphics.CopyTexture(camera.targetTexture, target);
        }
    }
}