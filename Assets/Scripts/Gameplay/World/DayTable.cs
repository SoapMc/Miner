using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Exceptions;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Day Table")]
    public class DayTable : ScriptableObject
    {
        [SerializeField, MinMaxIntRange(0, 800)] private RangedInt _morning = new RangedInt(240, 480);
        [SerializeField, MinMaxIntRange(200, 1200)] private RangedInt _day = new RangedInt(480, 1200);
        [SerializeField, MinMaxIntRange(800, 1440)] private RangedInt _evening = new RangedInt(1200, 1320);
        //rest of the time is night

        public RangedInt Morning => _morning;
        public RangedInt Day => _day;
        public RangedInt Evening => _evening;

        private void OnEnable()
        {
            //validate setting
            if (_day.minValue < _morning.maxValue || _day.minValue < _morning.minValue || _day.maxValue < _morning.maxValue || _day.maxValue < _morning.minValue)
                throw new InvalidSettingException();
            if(_evening.minValue < _day.maxValue || _evening.minValue < _day.minValue || _evening.maxValue < _day.maxValue  || _evening.maxValue < _day.minValue)
                throw new InvalidSettingException();

        }
    }
}