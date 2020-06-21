using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Miner.UI
{
    public class PlayerGridPositionDisplay : MonoBehaviour
    {
        [SerializeField] private Vector2IntReference _playerGridPosition = null;
        private TextMeshProUGUI _text = null;

        private void OnValueChanged(Vector2Int oldValue, Vector2Int newValue)
        {
            _text.text = "X: " + newValue.x + " / Y: " + newValue.y;
        }

        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _playerGridPosition.ValueChanged += OnValueChanged;
        }

        private void OnDestroy()
        {
            _playerGridPosition.ValueChanged -= OnValueChanged;
        }
    }
}