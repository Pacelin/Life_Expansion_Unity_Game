using System.IO;
using UnityEditor;
using UnityEngine;

namespace UniOwl.Celestials.Editor
{
    public static class SaveUtility
    {
        public static void SaveMeshInPlace(MeshFilter mf, string folderPath)
        {
            Mesh mesh = mf.sharedMesh;
            SaveMesh(mesh, mesh.name, false, folderPath);
        }

        public static void SaveMeshNewInstanceItem(MeshFilter mf, string folderPath)
        {
            Mesh mesh = mf.sharedMesh;
            SaveMesh(mesh, mesh.name, true, folderPath);
        }

        public static Mesh SaveMesh(Mesh mesh, string name, bool makeNewInstance, string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath)) return null;

            var path = Path.Combine(folderPath, name);
            var filePath = AssetDatabase.GenerateUniqueAssetPath(path);

            Mesh meshToSave = makeNewInstance ? Object.Instantiate(mesh) : mesh;

            MeshUtility.Optimize(meshToSave);

            AssetDatabase.CreateAsset(meshToSave, filePath);
            AssetDatabase.SaveAssets();

            return meshToSave;
        }
        
        public static void SaveTexture(Texture2D texture, string name, string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath)) return;

            var path = Path.Combine(folderPath, name);
            var filePath = AssetDatabase.GenerateUniqueAssetPath(path);

            AssetDatabase.CreateAsset(texture, filePath);
            AssetDatabase.SaveAssets();
        }
        
        public static void SaveMaterial(Material material, string name, string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath)) return;

            var path = Path.Combine(folderPath, name);
            var filePath = AssetDatabase.GenerateUniqueAssetPath(path);

            AssetDatabase.CreateAsset(material, filePath);
            AssetDatabase.SaveAssets();
        }
    }
}
