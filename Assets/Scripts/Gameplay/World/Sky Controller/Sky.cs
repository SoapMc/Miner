using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Miner.FX
{
    public abstract class Sky : MonoBehaviour
    {
        [SerializeField] private string _name = string.Empty;
        [SerializeField] private int _priority = 0;
        private SpriteRenderer[] _componentSprites = null;
        private Color _color = Color.white;

        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                for (int i = 0; i < _componentSprites.Length; ++i)
                {
                    _componentSprites[i].color = _color;
                }
            }
        }

        public string Name => _name;
        public int Priority => _priority;

        public abstract void OnMinuteElapsed(int oldVal, int timeOfDay);

        private void Awake()
        {
            _componentSprites = GetComponentsInChildren<SpriteRenderer>();
        }
    }
}