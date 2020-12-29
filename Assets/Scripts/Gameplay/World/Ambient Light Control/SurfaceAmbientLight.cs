using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Gameplay;

namespace Miner.FX
{
    [CreateAssetMenu(menuName = "World/Ambient Lights/Surface Ambient Light")]
    public class SurfaceAmbientLight : AmbientLight
    {
        [SerializeField] private Color _nightColor = Color.white;
        [SerializeField] private Color _dayColor = Color.white;
        [SerializeField] private IntReference _timeOfDay = null;
        [SerializeField] private DayTable _dayTable = null;

        public override Color UpdateLightColor()
        {
            if (_dayTable.Day.IsInRange(_timeOfDay))
            {
                return _dayColor;
            }
            else if (_dayTable.Morning.IsInRange(_timeOfDay))
            {
                float lerpCoeff = (_timeOfDay - _dayTable.Morning.minValue) / (float)(_dayTable.Morning.maxValue - _dayTable.Morning.minValue);
                return Color.Lerp(_nightColor, _dayColor, lerpCoeff);
            }
            else if (_dayTable.Evening.IsInRange(_timeOfDay))
            {
                float lerpCoeff = (_timeOfDay - _dayTable.Evening.minValue) / (float)(_dayTable.Evening.maxValue - _dayTable.Evening.minValue);
                return Color.Lerp(_dayColor, _nightColor, lerpCoeff);
            }
            else
            {
                return _nightColor;
            }
        }
    }
}