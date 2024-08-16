using UnityEngine;

namespace UniOwl.Graphics
{
    public class ShaderGraphUnscaledTimer : MonoBehaviour
    {
        private const string UNSCALED_TIME = "_UnscaledTime";
        private static readonly int UNSCALED_TIME_ID = Shader.PropertyToID(UNSCALED_TIME);
        
        [SerializeField] private Material material;

        private void OnDestroy()
        {
            material.SetFloat(UNSCALED_TIME_ID, 0f);
        }

        private void Update()
        {
            material.SetFloat(UNSCALED_TIME_ID, Time.unscaledTime);
        }
    }
}
