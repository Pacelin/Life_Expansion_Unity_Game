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
            CreateAsset(mesh, mesh.name, folderPath);
        }

        public static void SaveMeshNewInstanceItem(MeshFilter mf, string folderPath)
        {
            Mesh mesh = mf.sharedMesh;
            Mesh meshToSave = Object.Instantiate(mesh);
            CreateAsset(meshToSave, mesh.name, folderPath);
        }

        public static void CreateAsset(Object asset, string name, string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath)) return;

            var path = Path.Combine(folderPath, name);
            var filePath = AssetDatabase.GenerateUniqueAssetPath(path);

            AssetDatabase.CreateAsset(asset, filePath);
        }
    }
}
