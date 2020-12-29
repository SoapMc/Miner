using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using Miner.Management;

namespace Miner.Gameplay
{
    public class RadiationController : MonoBehaviour
    {

        [SerializeField] private IntReference _playerRadiation = null;
        [SerializeField] private IntReference _playerRadiationTolerance = null;
        [SerializeField] private GameEvent _changeAlarms = null;
        [SerializeField] private GameEvent _damagePlayer = null;
        [SerializeField, Range(1f, 30f)] private float _radiationDamageInterval = 15f;
        private Coroutine _dealInternalDamageOnTimeCoroutine = null;

        private void OnPlayerRadiationChanged(int oldVal, int newVal)
        {
            if (newVal > _playerRadiationTolerance)
            {
                _changeAlarms.Raise(new ChangeAlarmsEA() { AddedAlarmImportance = UI.AlarmDisplay.EAlarmImportance.Critical, AddedAlarm = UI.AlarmDisplay.EAlarmType.RadiationHigh });
                if(_dealInternalDamageOnTimeCoroutine == null)
                {
                    _dealInternalDamageOnTimeCoroutine = StartCoroutine(DealInternalDamageOnTime());
                }
            }
            else if(newVal == _playerRadiationTolerance)
            {
                _changeAlarms.Raise(new ChangeAlarmsEA() { AddedAlarmImportance = UI.AlarmDisplay.EAlarmImportance.Warning, AddedAlarm = UI.AlarmDisplay.EAlarmType.RadiationHigh });
            }
            else
            {
                _changeAlarms.Raise(new ChangeAlarmsEA() { RemovedAlarm = UI.AlarmDisplay.EAlarmType.RadiationHigh });
                if (_dealInternalDamageOnTimeCoroutine != null)
                {
                    StopCoroutine(_dealInternalDamageOnTimeCoroutine);
                    _dealInternalDamageOnTimeCoroutine = null;
                }
            }
        }

        private IEnumerator DealInternalDamageOnTime()
        {
            while(true)
            {
                DamagePlayerEA dp = new DamagePlayerEA();
                EPartType[] includedParts = { EPartType.Battery, EPartType.Cooling, EPartType.Engine };
                dp.PermaDamage.Add(includedParts[UnityEngine.Random.Range(0, includedParts.Length)], 0.01f);
                _damagePlayer.Raise(dp);
                yield return new WaitForSeconds(_radiationDamageInterval);
            }
        }

        private void Start()
        {
            _playerRadiation.ValueChanged += OnPlayerRadiationChanged;
            OnPlayerRadiationChanged(_playerRadiation.Value, _playerRadiation.Value);
        }

        private void OnDestroy()
        {
            _playerRadiation.ValueChanged -= OnPlayerRadiationChanged;
        }
    }
}