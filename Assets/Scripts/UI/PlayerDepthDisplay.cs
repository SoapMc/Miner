using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Miner.Management;

namespace Miner.UI
{
    public class PlayerDepthDisplay : MonoBehaviour
    {
        [SerializeField] private Vector2Reference _playerPosition = null;
        [SerializeField] private TextMeshProUGUI _depthText = null;

        private void OnValueChanged(Vector2 oldVal, Vector2 newVal)
        {
            _depthText.text = (-(int)(newVal.y * GameRules.Instance.RealDimensionOfTile)).ToString() + " m";
        }

        private void Start()
        {
            _playerPosition.ValueChanged += OnValueChanged;
        }

        private void OnDestroy()
        {
            _playerPosition.ValueChanged -= OnValueChanged;
        }
    }
}