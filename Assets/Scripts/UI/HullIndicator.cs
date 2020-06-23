using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Miner.UI
{
    [RequireComponent(typeof(Slider))]
    public class HullIndicator : MonoBehaviour
    {
        [SerializeField] private IntReference _playerHull = null;
        [SerializeField] private IntReference _playerMaxHull = null;
        private Slider _slider = null;

        private void OnValueChanged(int oldValue, int newValue)
        {
            if (_playerMaxHull > 0)
                _slider.value = _playerHull / (float)_playerMaxHull;
            else
                _slider.value = 1f;
        }

        private void Awake()
        {
            _slider = GetComponent<Slider>();
            _playerHull.ValueChanged += OnValueChanged;
            _playerMaxHull.ValueChanged += OnValueChanged;
            OnValueChanged(_playerHull, _playerHull);
        }

        private void OnDestroy()
        {
            _playerHull.ValueChanged -= OnValueChanged;
            _playerMaxHull.ValueChanged -= OnValueChanged;
        }
    }
}