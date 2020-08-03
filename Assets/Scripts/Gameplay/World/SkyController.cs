using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Miner.Management.Events;
using Miner.Management.Exceptions;

namespace Miner.Gameplay
{
    public class SkyController : MonoBehaviour
    {
        [SerializeField] private IntReference _timeOfDay = null;
        [SerializeField] private DayTable _dayTable = null;
        [SerializeField] private SpriteRenderer _daySky = null;
        [SerializeField] private SpriteRenderer _nightSky = null;
        
        private bool _enabled = true;
        private Color _transparent = new Color(1f, 1f, 1f, 0f);

        private void MinuteElapsed(int oldVal, int timeOfDay)
        {
            if (!_enabled) return;
            
            if(_dayTable.Day.IsInRange(timeOfDay))
            {
                _nightSky.color = _transparent;
                _daySky.color = Color.white;
            }
            else if(_dayTable.Morning.IsInRange(timeOfDay))
            {
                float lerpCoeff = (timeOfDay - _dayTable.Morning.minValue) / (float)(_dayTable.Morning.maxValue - _dayTable.Morning.minValue);
                _nightSky.color = Color.Lerp(Color.white, _transparent, lerpCoeff);
                _daySky.color = Color.Lerp(_transparent, Color.white, lerpCoeff);
            }
            else if(_dayTable.Evening.IsInRange(timeOfDay))
            {
                float lerpCoeff = (timeOfDay - _dayTable.Evening.minValue) / (float)(_dayTable.Evening.maxValue - _dayTable.Evening.minValue);
                _daySky.color = Color.Lerp(Color.white, _transparent, lerpCoeff);
                _nightSky.color = Color.Lerp(_transparent, Color.white, lerpCoeff);
            }
            else
            {
                _daySky.color *= _transparent;
                _nightSky.color *= Color.white;
            }
        }

        private void OnPlayerCameToLayer(EventArgs args)
        {
            if (args is PlayerCameToLayerEA pctl)
            {
                if (pctl.LayerNumber > 0)
                {
                    _enabled = false;
                }
                else
                    _enabled = true;
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        private void Awake()
        {
            _timeOfDay.ValueChanged += MinuteElapsed;
        }

        private void Start()
        {
            MinuteElapsed(0, _timeOfDay.Value);
        }

        private void OnDestroy()
        {
            _timeOfDay.ValueChanged -= MinuteElapsed;
        }

    }
}