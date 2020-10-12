using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Miner.UI
{
    [RequireComponent(typeof(Bar))]
    public class PowerIndicator : MonoBehaviour
    {
        [SerializeField] private FloatReference _playerPower = null;
        [SerializeField] private FloatReference _playerMaxPower = null;
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

        private void OnValueChanged(float oldValue, float newValue)
        {
            if (_playerMaxPower > 0)
                _bar.Value = _playerPower / _playerMaxPower;
            else
                _bar.Value = 1f;

            _brightText.text = _playerPower.Value.ToString("0.0") + " / " + (int)_playerMaxPower.Value;
            _blackText.text = _brightText.text;
        }

        private void Awake()
        {
            _bar = GetComponent<Bar>();
            _indicationEffect = new LowLevelIndicatorEffect(this);
            _playerPower.ValueChanged += OnValueChanged;
            _playerMaxPower.ValueChanged += OnValueChanged;
            OnValueChanged(_playerPower, _playerPower);
        }

        private void OnDestroy()
        {
            _playerPower.ValueChanged -= OnValueChanged;
            _playerMaxPower.ValueChanged -= OnValueChanged;
        }
    }
}