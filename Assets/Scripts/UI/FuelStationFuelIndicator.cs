using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Miner.UI
{
    public class FuelStationFuelIndicator : MonoBehaviour
    {
        [SerializeField] private FloatReference _playerFuel = null;
        [SerializeField] private FloatReference _playerMaxFuel = null;
        [SerializeField] private TextMeshProUGUI _text = null;
        private Slider _slider = null;
        
        private void OnValueChanged(float oldValue, float newValue)
        {
            if (_playerMaxFuel > 0)
                _slider.value = _playerFuel / _playerMaxFuel;
            else
                _slider.value = 1f;

            _text.text = _playerFuel.Value.ToString("0.0") + " / " + (int)_playerMaxFuel.Value;
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