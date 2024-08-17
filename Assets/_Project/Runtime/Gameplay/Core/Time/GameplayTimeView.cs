using System;
using TMPro;
using UnityEngine;

namespace Runtime.Gameplay.Core
{
    public class GameplayTimeView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _timeText;
        [SerializeField] private string _timeFormat;

        public void SetTime(DateTime time)
        {
            _timeText.text = string.Format(_timeFormat, time);
        }
    }
}