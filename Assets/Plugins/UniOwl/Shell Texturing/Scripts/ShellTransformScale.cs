using UnityEngine;

namespace UniOwl.Rendering
{
    public struct ShellTransformScale : IShellTransform
    {
        public void Apply(ShellSurface surface, MeshRenderer shell, int index)
        {
            float scaleAmount = (index + 1) * surface.ShellDistance + 1f;
            shell.transform.localScale *= scaleAmount;
        }
    }
}