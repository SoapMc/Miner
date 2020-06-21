using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Miner.UI
{
    public class PlayerFuelDisplay : MonoBehaviour
    {
        [SerializeField] private FloatReference _currentFuel = null;
        [SerializeField] private FloatReference _maxFuel = null;
        private TextMeshProUGUI _text = null;

        private void OnValueChanged(float oldValue, float newValue)
        {
            _text.text = newValue.ToString("0.0") + " / " + (int)_maxFuel.Value;
        }

        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _currentFuel.ValueChanged += OnValueChanged;
            OnValueChanged(_currentFuel.Value, _currentFuel.Value);
        }

        private void OnDestroy()
        {
            _currentFuel.ValueChanged -= OnValueChanged;
        }
    }
}