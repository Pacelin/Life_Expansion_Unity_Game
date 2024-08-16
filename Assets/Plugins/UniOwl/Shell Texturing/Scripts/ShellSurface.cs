using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniOwl.Rendering
{
    [ExecuteInEditMode]
    public class ShellSurface : MonoBehaviour
    {
        private static int AlphaThreshold = Shader.PropertyToID("Shell_Alpha_Threshold");
        
        private List<MeshRenderer> shells;

        private IShellTransform shellTransformType = new ShellTransformTranslateToAxis(Vector3.up);
        
        [SerializeField] private MeshRenderer shellPrefab;
        
        [SerializeField, Range(0, 32)] private int shellCount;
        [SerializeField] private float shellDistance;

        [SerializeField] private bool calculateFromCenter;

        public int ShellCount => shellCount;
        public float ShellDistance => shellDistance;

        private void Awake()
        {
            shells = new List<MeshRenderer>();
        }

        private void OnValidate()
        {
            UpdateShells();
        }

        private void UpdateShells()
        {
            UpdateShellTransformType();
            UpdateChildrenCount();
            UpdateChildren();
        }

        private void UpdateShellTransformType()
        {
            if (calculateFromCenter)
                shellTransformType = new ShellTransformScale();
            else
                shellTransformType = new ShellTransformTranslateToAxis(Vector3.up);
        }
        
        private void UpdateChildrenCount()
        {
            int childrenToDestroy = transform.childCount - 1 - shellCount;
            int childrenToSpawn = -childrenToDestroy;

            shells.Clear();
            for (int i = 1; i < transform.childCount; i++)
            {
                shells.Add(transform.GetChild(i).GetComponent<MeshRenderer>());
            }
            
            for (int i = 0; i < childrenToDestroy; i++)
            {
                DestroyImmediate(shells[0].gameObject);
                shells.RemoveAt(0);
            }

            for (int i = 0; i < childrenToSpawn; i++)
            {
                var shell = Instantiate(shellPrefab, transform);
                shell.sharedMaterial = new Material(shellPrefab.sharedMaterial);
                shells.Add(shell);
            }
        }

        private void UpdateChildren()
        {
            for (int i = 0; i < shellCount; i++)
            {
                shells[i].transform.localPosition = Vector3.zero;
                shells[i].transform.localScale = shellPrefab.transform.localScale;
                
                shellTransformType.Apply(this, shells[i], i);
                
                shells[i].sharedMaterial.SetFloat(AlphaThreshold, (float)i / (shellCount - 1));
            }
        }
    }
}
