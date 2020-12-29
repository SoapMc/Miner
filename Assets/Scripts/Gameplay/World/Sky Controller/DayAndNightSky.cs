using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Gameplay;

namespace Miner.FX
{
    public class DayAndNightSky : Sky
    {
        [SerializeField] private DayTable _dayTable = null;
        [SerializeField] private DefaultSky _daySky = null;
        [SerializeField] private DefaultSky _nightSky = null;
        private Color _transparent = new Color(1f, 1f, 1f, 0f);

        public override void OnMinuteElapsed(int oldVal, int timeOfDay)
        {
            if (_dayTable.Day.IsInRange(timeOfDay))
            {
                _daySky.Color = Color.white;
                _nightSky.Color = _transparent;
            }
            else if (_dayTable.Morning.IsInRange(timeOfDay))
            {
                float lerpCoeff = (timeOfDay - _dayTable.Morning.minValue) / (float)(_dayTable.Morning.maxValue - _dayTable.Morning.minValue);
                _daySky.Color = Color.Lerp(_transparent, Color.white, lerpCoeff);
                _nightSky.Color = Color.Lerp(Color.white, _transparent, lerpCoeff);
            }
            else if (_dayTable.Evening.IsInRange(timeOfDay))
            {
                float lerpCoeff = (timeOfDay - _dayTable.Evening.minValue) / (float)(_dayTable.Evening.maxValue - _dayTable.Evening.minValue);
                _daySky.Color = Color.Lerp(Color.white, _transparent, lerpCoeff);
                _nightSky.Color = Color.Lerp(_transparent, Color.white, lerpCoeff);
            }
            else
            {
                _daySky.Color = _transparent;
                _nightSky.Color = Color.white;
            }
        }
    }
}