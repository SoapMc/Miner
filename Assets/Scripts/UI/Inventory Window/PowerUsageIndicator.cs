using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Miner.UI
{
    public class PowerUsageIndicator : MonoBehaviour
    {
        [SerializeField] private Image _fill = null;
        [SerializeField] private FloatReference _playerPowerUsage = null;
        [SerializeField] private TextMeshProUGUI _brightText = null;
        [SerializeField] private TextMeshProUGUI _blackText = null;
        [SerializeField] private Color _positiveBalance = Color.blue;
        [SerializeField] private Color _negativeBalance = Color.red;
        [SerializeField] private float _powerUsageScale = 1f;
        private Bar _bar = null;

        private void OnPowerUsageChanged(float old, float newVal)
        {
            _bar.Value = Mathf.Abs(_playerPowerUsage.Value / _powerUsageScale);
            _brightText.text = (-_playerPowerUsage.Value).ToString("0.00") + " MJ/s";
            _blackText.text = _brightText.text;

            if (_playerPowerUsage.Value <= 0f)
            {
                _fill.color = _positiveBalance;
                _brightText.color = _positiveBalance;
            }
            else
            {
                _fill.color = _negativeBalance;
                _brightText.color = _negativeBalance;
            }
        }

        private void Awake()
        {
            _bar = GetComponent<Bar>();
        }

        private void OnEnable()
        {
            _playerPowerUsage.ValueChanged += OnPowerUsageChanged;
            OnPowerUsageChanged(_playerPowerUsage.Value, _playerPowerUsage.Value);
        }

        private void OnDisable()
        {
            _playerPowerUsage.ValueChanged -= OnPowerUsageChanged;
        }
    }
}