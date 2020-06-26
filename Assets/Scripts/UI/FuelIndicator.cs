using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Miner.UI
{
    [RequireComponent(typeof(Slider))]
    public class FuelIndicator : MonoBehaviour
    {
        [SerializeField] private FloatReference _playerFuel = null;
        [SerializeField] private FloatReference _playerMaxFuel = null;
        private Slider _slider = null;

        private void OnValueChanged(float oldValue, float newValue)
        {
            if (_playerMaxFuel > 0)
                _slider.value = _playerFuel / _playerMaxFuel;
            else
                _slider.value = 1f;
        }

        private void Awake()
        {
            _slider = GetComponent<Slider>();
            _playerFuel.ValueChanged += OnValueChanged;
            OnValueChanged(_playerFuel, _playerFuel);
        }

        private void OnDestroy()
        {
            _playerFuel.ValueChanged -= OnValueChanged;
        }
    }
}