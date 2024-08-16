using UnityEngine;

namespace UniOwl.Rendering
{
    public interface IShellTransform
    {
        void Apply(ShellSurface surface, MeshRenderer shell, int index);
    }
}