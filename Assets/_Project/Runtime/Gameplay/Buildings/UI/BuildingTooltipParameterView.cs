using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Gameplay.Buildings.UI
{
    public class BuildingTooltipParameterView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _caption;
        [SerializeField] private TextMeshProUGUI _iconText;
        [SerializeField] private Image _iconImage;
        [SerializeField] private SerializedDictionary<EParameterOpinion, Color> _opinionColors;
        [SerializeField] private Sprite[] _arrowsSprites;

        public void SetCaption(string caption) => _caption.text = caption;

        public void SetIcon(int value, EParameterOpinion opinion)
        {
            _iconImage.gameObject.SetActive(false);
            _iconText.gameObject.SetActive(true);
            _iconText.text = value > 0 ? ("+" + value) : value.ToString();
            _iconText.color = _opinionColors[opinion];
        }

        public void SetIcon(int index, int value, EParameterOpinion opinion)
        {
            _iconText.gameObject.SetActive(false);
            _iconImage.gameObject.SetActive(true);
            _iconImage.sprite = _arrowsSprites[index];
            _iconImage.transform.rotation = Quaternion.Euler(0, 0, value > 0 ? 0 : 180);
            _iconImage.color = _opinionColors[opinion];
        }
    }
}