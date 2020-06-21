using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Miner.UI
{
    public class PlayerMoneyDisplay : MonoBehaviour
    {
        [SerializeField] private IntReference _money = null;
        private TextMeshProUGUI _text = null;

        private void OnValueChanged(int oldValue, int newValue)
        {
            _text.text = newValue + " $";
        }

        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _money.ValueChanged += OnValueChanged;
            OnValueChanged(_money.Value, _money.Value);
        }

        private void OnDestroy()
        {
            _money.ValueChanged -= OnValueChanged;
        }
    }
}