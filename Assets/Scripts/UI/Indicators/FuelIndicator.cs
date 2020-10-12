using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Miner.UI
{
    [RequireComponent(typeof(Bar))]
    public class FuelIndicator : MonoBehaviour
    {
        [SerializeField] private FloatReference _playerFuel = null;
        [SerializeField] private FloatReference _playerMaxFuel = null;
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
            if (_playerMaxFuel > 0)
                _bar.Value = _playerFuel / _playerMaxFuel;
            else
                _bar.Value = 1f;

            _brightText.text = _playerFuel.Value.ToString("0.0") + " / " + (int)_playerMaxFuel.Value;
            _blackText.text = _brightText.text;
        }

        private void Awake()
        {
            _bar = GetComponent<Bar>();
            _indicationEffect = new LowLevelIndicatorEffect(this);
            _playerFuel.ValueChanged += OnValueChanged;
            _playerMaxFuel.ValueChanged += OnValueChanged;
            OnValueChanged(_playerFuel, _playerFuel);
        }

        private void OnDestroy()
        {
            _playerFuel.ValueChanged -= OnValueChanged;
            _playerMaxFuel.ValueChanged -= OnValueChanged;
        }
    }
}