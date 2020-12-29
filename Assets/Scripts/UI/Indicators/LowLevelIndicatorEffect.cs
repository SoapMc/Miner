using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace Miner.UI
{
    public class LowLevelIndicatorEffect : IIndicationEffect
    {
        private MonoBehaviour _owner = null;
        private float _rateOfBlinking = 0.5f;
        private Color _signalizationColor = Color.red;
        private Color[] _colorsOfChildren;
        private Graphic[] _graphics;
        private Coroutine _indicateCoroutine = null;

        public void TurnOn()
        {
            if (_indicateCoroutine != null) return;
            _indicateCoroutine = _owner.StartCoroutine(IndicateLowLevel());
        }

        public void TurnOff()
        {
            if(_indicateCoroutine != null)
                _owner.StopCoroutine(_indicateCoroutine);
            RestoreDefaultColors();
        }

        private IEnumerator IndicateLowLevel()
        {
            while(true)
            {
                SetColors(_signalizationColor);
                yield return new WaitForSecondsRealtime(_rateOfBlinking);
                RestoreDefaultColors();
                yield return new WaitForSecondsRealtime(_rateOfBlinking);
            }
        }

        private void RestoreDefaultColors()
        {
            for(int i = 0; i < _graphics.Length; ++i)
            {
                _graphics[i].color = _colorsOfChildren[i];
            }
        }

        private void SetColors(Color col)
        {
            for (int i = 0; i < _graphics.Length; ++i)
            {
                _graphics[i].color *= col;
            }
        }

        public LowLevelIndicatorEffect(MonoBehaviour owner)
        {
            _owner = owner ?? throw new ArgumentException("Owner cannot be null.");
            _owner = owner;
            _graphics = _owner.GetComponentsInChildren<Graphic>();
            _colorsOfChildren = new Color[_graphics.Length];
            for(int i = 0; i < _graphics.Length; ++i)
            {
                _colorsOfChildren[i] = _graphics[i].color;
            }
        }
    }
}