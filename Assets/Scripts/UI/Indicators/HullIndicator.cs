using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Miner.UI
{
    [RequireComponent(typeof(Bar))]
    public class HullIndicator : MonoBehaviour
    {
        [SerializeField] private IntReference _playerHull = null;
        [SerializeField] private IntReference _playerMaxHull = null;
        [SerializeField] private TextMeshProUGUI _brightText = null;
        [SerializeField] private TextMeshProUGUI _blackText = null;
        private Bar _bar = null;
        private IIndicationEffect _indicationEffect = null;

        public void TurnOnLowLevelSignalization()
        {
            _indicationEffect.TurnOn();
        }

        public void TurnOffLowLevelSignalization()
        {
            _indicationEffect.TurnOff();
        }

        private void OnValueChanged(int oldValue, int newValue)
        {
            if (_playerMaxHull > 0)
                _bar.Value = _playerHull / (float)_playerMaxHull;
            else
                _bar.Value = 1f;
            _brightText.text = _playerHull.Value + " / " + _playerMaxHull.Value;
            _blackText.text = _brightText.text;
        }

        private void Awake()
        {
            _bar = GetComponent<Bar>();
            _indicationEffect = new LowLevelIndicatorEffect(this);
            _playerHull.ValueChanged += OnValueChanged;
            _playerMaxHull.ValueChanged += OnValueChanged;
            OnValueChanged(_playerHull, _playerHull);
        }

        private void OnDestroy()
        {
            _indicationEffect = null;
            _playerHull.ValueChanged -= OnValueChanged;
            _playerMaxHull.ValueChanged -= OnValueChanged;
        }
    }
}