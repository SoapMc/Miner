using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;

namespace Miner.Gameplay
{
    public class TimeController : MonoBehaviour
    {
        [SerializeField] private DayTable _dayTable = null;
        [SerializeField] private GameEvent _hourElapsed = null;
        [SerializeField] private GameEvent _minuteElapsed = null;
        [SerializeField] private GameEvent _dayElapsed = null;
        [SerializeField] private GameEvent _dayBegan = null;
        [SerializeField] private GameEvent _nightBegan = null;
        [SerializeField] private IntReference _timeOfDay = null;
        [SerializeField] private IntReference _elapsedDays = null;
        [SerializeField, Range(0, 20)] private int _timeMultiplier = 1;
        private float _second = 0f;

        public void Update()
        {
            _second += Time.deltaTime * _timeMultiplier;
            if(_second >= 1f)
            {
                _timeOfDay.Value++;
                _second -= 1f;
                _minuteElapsed.Raise();
                if (_timeOfDay % 60 == 0)
                {
                    _hourElapsed.Raise();
                    if (_timeOfDay.Value / 60 == 24)
                    {
                        _elapsedDays.Value++;
                        _dayElapsed.Raise();
                        _timeOfDay.Value = 0;
                    }
                    if(_timeOfDay / 60 == Mathf.CeilToInt((_dayTable.Morning.maxValue - (_dayTable.Morning.maxValue - _dayTable.Morning.minValue) * 0.5f) / 60f))
                    {
                        _dayBegan.Raise();
                    }
                    else if(_timeOfDay / 60 == Mathf.CeilToInt((_dayTable.Evening.maxValue - (_dayTable.Evening.maxValue - _dayTable.Evening.minValue) * 0.5f) / 60f))
                    {
                        _nightBegan.Raise();
                    }
                }
            }
        }

        public void OnNewWorldCreated()
        {
            _timeOfDay.Value = 0;
            if (!_dayTable.Day.IsInRange(_timeOfDay))
            {
                _nightBegan.Raise();
            }
            else
                _dayBegan.Raise();
        }

        public void OnWorldLoaded()
        {
            if (!_dayTable.Day.IsInRange(_timeOfDay))
            {
                _nightBegan.Raise();
            }
            else
                _dayBegan.Raise();
        }

        public void OnWorldReset()
        {
            _timeOfDay.Value = 0;
        }
    }
}