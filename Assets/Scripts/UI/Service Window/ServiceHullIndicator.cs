using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Miner.UI
{
    [RequireComponent(typeof(Slider))]
    public class ServiceHullIndicator : MonoBehaviour
    {
        [SerializeField] private IntReference _playerHull = null;
        [SerializeField] private IntReference _playerMaxHull = null;
        [SerializeField] private TextMeshProUGUI _text = null;
        private Slider _slider = null;

        private void OnValueChanged(int oldValue, int newValue)
        {
            if (_playerMaxHull > 0)
                _slider.value = _playerHull / _playerMaxHull;
            else
                _slider.value = 1f;

            _text.text = _playerHull.Value.ToString() + " / " + _playerMaxHull.Value;
        }

        private void Awake()
        {
            _slider = GetComponent<Slider>();
            _playerHull.ValueChanged += OnValueChanged;
            OnValueChanged(_playerHull, _playerHull);
        }

        private void OnDestroy()
        {
            _playerHull.ValueChanged -= OnValueChanged;
        }
    }
}