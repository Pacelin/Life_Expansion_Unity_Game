using UnityEngine;

namespace Jamcelin.Runtime.Entities
{
    public class Entity
    {
        public Transform Transform => _kernel.Transform;
        public GameObject GameObject => _kernel.GameObject;
        
        private readonly EntityKernel _kernel;
        
        public Entity(EntityKernel kernel)
        {
            _kernel = kernel;
        }
    }
}