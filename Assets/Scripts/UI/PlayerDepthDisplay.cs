using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Miner.Management;

namespace Miner.UI
{
    public class PlayerDepthDisplay : MonoBehaviour
    {
        [SerializeField] private Vector2IntReference _playerGridPosition = null;
        [SerializeField] private TextMeshProUGUI _depthText = null;

        private void OnValueChanged(Vector2Int oldVal, Vector2Int newVal)
        {
            _depthText.text = (-(int)(newVal.y * GameRules.Instance.RealDimensionOfTile)).ToString() + " m";
        }

        private void Start()
        {
            _playerGridPosition.ValueChanged += OnValueChanged;
        }

        private void OnDisable()
        {
            _playerGridPosition.ValueChanged -= OnValueChanged;
        }
    }
}