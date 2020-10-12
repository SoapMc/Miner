using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Miner.UI
{
    public class Bar : MonoBehaviour
    {
        [SerializeField] private RectTransform _mask = null;
        private Vector2 _initialMaskSize;
        private float _value = 0f;

        public float Value
        {
            get => _value;
            set
            {
                _mask.sizeDelta = new Vector2(_initialMaskSize.x * Mathf.Clamp01(value), _initialMaskSize.y);
            }
        }

        private void Awake()
        {
            _initialMaskSize = _mask.sizeDelta;
        }
    }
}