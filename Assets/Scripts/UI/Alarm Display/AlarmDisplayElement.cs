using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Miner.UI
{
    public class AlarmDisplayElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text = null;
        [SerializeField] private Image _image = null;
        [SerializeField, Range(0.1f, 1f)] private float _blinkPeriod = 0.5f;
        private AlarmDisplay.EAlarmImportance _importance = default;
        private float _elapsedTime = 0f;

        public AlarmDisplay.EAlarmImportance Importance => _importance;
        public string Text => _text.text;

        public void Initialize(string text, AlarmDisplay.EAlarmImportance importance, Color color)
        {
            _text.text = text;
            _importance = importance;
            _text.color = color;
            _image.color = color;
            transform.localScale = Vector3.one;
        }

        public void Update()
        {
            if (Importance == AlarmDisplay.EAlarmImportance.Critical)
            {
                _elapsedTime += Time.deltaTime;
                if (_elapsedTime >= _blinkPeriod)
                {
                    if (transform.localScale.x == 0)
                    {
                        transform.localScale = Vector3.one;
                    }
                    else
                    {
                        transform.localScale = Vector3.zero;
                    }
                    _elapsedTime = 0f;
                }
            }
        }
    }
}