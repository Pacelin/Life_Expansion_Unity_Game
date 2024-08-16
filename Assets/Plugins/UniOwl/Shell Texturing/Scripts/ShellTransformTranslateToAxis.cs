using UnityEngine;

namespace UniOwl.Rendering
{
    public struct ShellTransformTranslateToAxis : IShellTransform
    {
        private Vector3 axis;
        
        public ShellTransformTranslateToAxis(Vector3 axis)
        {
            this.axis = axis;
        }

        public void Apply(ShellSurface surface, MeshRenderer shell, int index)
        {
            float moveAmount = (index + 1) * surface.ShellDistance;
            shell.transform.localPosition = axis * moveAmount;
        }
    }
}