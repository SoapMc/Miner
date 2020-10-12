using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Miner.Management.Events;
using Miner.Management.Exceptions;

namespace Miner.Gameplay
{
    public class RadiationController : MonoBehaviour
    {
        [SerializeField] private IntReference _playerRadiationTolerance = null;
        [SerializeField] private GameEvent _changeAlarms = null;

        public void OnPlayerRadiationChanged(EventArgs args)
        {
            if(args is PlayerRadiationChangedEA prc)
            {
                if(prc.Radiation > _playerRadiationTolerance)
                {
                    _changeAlarms.Raise(new ChangeAlarmsEA() { AddedAlarm = UI.AlarmDisplay.EAlarmType.RadiationHigh });
                }
                else
                {
                    _changeAlarms.Raise(new ChangeAlarmsEA() { RemovedAlarm = UI.AlarmDisplay.EAlarmType.RadiationHigh });
                }
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }
    }
}