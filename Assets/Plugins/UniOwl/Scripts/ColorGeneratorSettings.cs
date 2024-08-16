using UnityEngine;

namespace UniOwl
{
    [CreateAssetMenu(menuName = "Game/Color Generator", fileName = "SO_ColorGenerator")]
    public class ColorGeneratorSettings : ScriptableObject
    {
        [MinMaxSlider(0f, 1f)]
        [SerializeField] private Vector2 minMaxHue;
        [MinMaxSlider(0f, 1f)]
        [SerializeField] private Vector2 minMaxSaturation;
        [MinMaxSlider(0f, 1f)]
        [SerializeField] private Vector2 minMaxValue;

        public Color GetColor()
        {
            float hue = Random.Range(minMaxHue.x, minMaxHue.y);
            float sat = Random.Range(minMaxSaturation.x, minMaxSaturation.y);
            float val = Random.Range(minMaxValue.x, minMaxValue.y);

            return Color.HSVToRGB(hue, sat, val);
        }
    }
}