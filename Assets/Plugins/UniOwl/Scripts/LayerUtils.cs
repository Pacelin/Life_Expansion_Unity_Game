using UnityEngine;

namespace UniOwl
{
    public static class LayerUtils
    {
        public static int GetLayerId(this LayerMask mask)
        {
            int layerNumber = -1;
            int layer = mask.value;
            while (layer > 0)
            {
                layer >>= 1;
                layerNumber++;
            }

            return layerNumber;
        }
    }
}
