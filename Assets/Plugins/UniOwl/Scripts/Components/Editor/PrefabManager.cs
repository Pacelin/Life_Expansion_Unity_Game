using System.IO;
using UnityEditor;
using UnityEngine;

namespace UniOwl.Components.Editor
{
    public static class PrefabManager
    {
        /// <summary>
        /// Makes prefab variant from prefabToAdd and adds it to the rootPrefab.
        /// </summary>
        /// <param name="rootPrefab">Persistent root prefab we want to edit.</param>
        /// <param name="prefabToAdd">Prefab to make variant of and add to root.</param>
        /// <returns>Reference to variant of prefabToAdd.</returns>
        public static GameObject AddPrefabToRootPrefabAsVariant(GameObject rootPrefab, GameObject prefabToAdd)
        {
            if (!EditorUtility.IsPersistent(rootPrefab))
                return null;

            string path = AssetDatabase.GetAssetPath(rootPrefab);
            
            using (var scope = new PrefabUtility.EditPrefabContentsScope(path))
            {
                CreatePrefabVariant(scope.prefabContentsRoot, prefabToAdd);
            }

            return rootPrefab.transform.GetChild(rootPrefab.transform.childCount - 1).gameObject;
        }

        public static GameObject CreatePrefabVariant(GameObject root, GameObject prefabToAdd)
        {
            var variant = (GameObject)PrefabUtility.InstantiatePrefab(prefabToAdd);
            variant.transform.SetParent(root.transform);
            variant.name = prefabToAdd.name;
            return variant;
        }

        /// <summary>
        /// Removes index-th child from rootPrefab and destroys it. 
        /// </summary>
        /// <param name="rootPrefab">Persistent root prefab we want to edit.</param>
        /// <param name="index">Index of the child being destroyed.</param>
        public static void RemoveChildObjectFromPrefab(GameObject rootPrefab, int index)
        {
            if (!EditorUtility.IsPersistent(rootPrefab))
                return;

            string path = AssetDatabase.GetAssetPath(rootPrefab);
            using var scope = new PrefabUtility.EditPrefabContentsScope(path);

            Object.DestroyImmediate(scope.prefabContentsRoot.transform.GetChild(index).gameObject);
        }
        
        /// <summary>
        /// Set index-th child from rootPrefab active or inactive. 
        /// </summary>
        /// <param name="rootPrefab">Persistent root prefab we want to edit.</param>
        /// <param name="childIndex">Index of the child being destroyed.</param>
        /// <param name="active">Value.</param>
        public static void SetChildObjectActive(GameObject rootPrefab, int childIndex, bool active)
        {
            if (!EditorUtility.IsPersistent(rootPrefab))
                return;

            if (rootPrefab.transform.GetChild(childIndex).gameObject.activeSelf == active)
                return;
            
            string path = AssetDatabase.GetAssetPath(rootPrefab);
            using var scope = new PrefabUtility.EditPrefabContentsScope(path);

            scope.prefabContentsRoot.transform.GetChild(childIndex).gameObject.SetActive(active);
        }
        
        /// <summary>
        /// Creates material variants from their source and assigns to rootPrefab.
        /// </summary>
        /// <param name="rootPrefab">Persistent root prefab we want to edit.</param>
        /// <param name="childIndex">Index of the child being modified.</param>
        public static void CreateMaterialVariantsFromOriginals(GameObject rootPrefab, int childIndex)
        {
            var path = AssetDatabase.GetAssetPath(rootPrefab);
            using var scope = new PrefabUtility.EditPrefabContentsScope(path);
            CreateMaterialVariantsFromOriginals(rootPrefab, scope.prefabContentsRoot, childIndex);
        }
        
        public static void CreateMaterialVariantsFromOriginals(GameObject rootPrefab, GameObject editableRoot, int childIndex)
        {
            var componentGO = editableRoot.transform.GetChild(childIndex);
            foreach (Renderer renderer in componentGO.GetComponentsInChildren<Renderer>())
            {
                var materials = renderer.sharedMaterials;

                for (int i = 0; i < materials.Length; i++)
                {
                    var materialVariant = new Material(materials[i])
                    {
                        parent = materials[i],
                        hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector,
                    };
                    AssetDatabase.AddObjectToAsset(materialVariant, rootPrefab);

                    materials[i] = materialVariant;
                }

                renderer.sharedMaterials = materials;
            }
        }

        public static void DestroyChildrenSharedMaterials(GameObject go)
        {
            foreach (var renderer in go.GetComponentsInChildren<Renderer>())
            {
                var materials = renderer.sharedMaterials;

                foreach (Material mat in materials)
                    Object.DestroyImmediate(mat, true);
            }
        }
        
        public static GameObject CreatePrefabVariant(string folderPath, string namePrefix, GameObject prefab)
        {
            string name = $"{namePrefix}_{prefab.name}_Variant.prefab";
            string path = Path.Combine(folderPath, name);
            path = AssetDatabase.GenerateUniqueAssetPath(path);
            
            var source = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            var root = PrefabUtility.SaveAsPrefabAsset(source, path);
            Object.DestroyImmediate(source);

            return root;
        }
    }
}