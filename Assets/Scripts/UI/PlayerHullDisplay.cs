using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Miner.UI
{
    public class PlayerHullDisplay : MonoBehaviour
    {
        [SerializeField] private IntReference _currentHull = null;
        [SerializeField] private IntReference _maxHull = null;
        private TextMeshProUGUI _text = null;

        private void OnValueChanged(int oldValue, int newValue)
        {
            _text.text = _currentHull.Value + " / " + _maxHull.Value;
        }

        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _currentHull.ValueChanged += OnValueChanged;
            _maxHull.ValueChanged += OnValueChanged;
            OnValueChanged(_currentHull.Value, _currentHull.Value);
        }

        private void OnDestroy()
        {
            _currentHull.ValueChanged -= OnValueChanged;
            _maxHull.ValueChanged -= OnValueChanged;
        }
    }
}