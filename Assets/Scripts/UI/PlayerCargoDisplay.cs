using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Miner.Management;

namespace Miner.UI
{
    public class PlayerCargoDisplay : MonoBehaviour
    {
        [SerializeField] private IntReference _playerCargoMass = null;
        [SerializeField] private IntReference _playerCargoMaxMass = null;
        [SerializeField] private TextMeshProUGUI _cargoValueText = null;

        private void OnValueChanged(int oldVal, int newVal)
        {
            _cargoValueText.text = (Mathf.CeilToInt(_playerCargoMass.Value * 100 / (float)_playerCargoMaxMass.Value) ).ToString() + " % (" + _playerCargoMass.Value.ToString() + " kg)";
        }

        private void Start()
        {
            _playerCargoMass.ValueChanged += OnValueChanged;
            _playerCargoMaxMass.ValueChanged += OnValueChanged;
            OnValueChanged(0, 0);
        }

        private void OnDestroy()
        {
            _playerCargoMass.ValueChanged -= OnValueChanged;
            _playerCargoMaxMass.ValueChanged -= OnValueChanged;
        }
    }
}