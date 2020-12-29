using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;

namespace Miner.Gameplay
{
    public class FuelController : MonoBehaviour
    {
        [SerializeField] private GameEvent _killPlayer = null;
        [SerializeField] private GameEvent _signalizeLowFuel = null;
        [SerializeField] private GameEvent _turnOffLowFuelSignalization = null;
        [SerializeField] private GameEvent _changeAlarms = null;
        [SerializeField] private FloatReference _playerFuel = null;
        [SerializeField] private FloatReference _playerMaxFuel = null;
        [SerializeField] private FloatReference _playerFuelUsage = null;
        [SerializeField, Range(0f, 0.5f)] private float _lowFuelSignalizationLevel = 0.2f;
        private Coroutine _checkStatusCoroutine = null;
        private bool _lowFuel = false;

        private void Update()
        {
            _playerFuel.Value -= _playerFuelUsage.Value * Time.deltaTime;
            if (_playerFuel.Value < 0f)
                _killPlayer.Raise(new KillPlayerEA(KillPlayerEA.ESource.FuelEnded));
        }

        private IEnumerator CheckStatus()
        {
            yield return new WaitForSeconds(1f);
            while (true)
            {
                if(_playerFuel.Value <= _lowFuelSignalizationLevel * _playerMaxFuel.Value)
                {
                    if (_lowFuel == false)
                    {
                        _signalizeLowFuel.Raise();
                        _lowFuel = true;
                        _changeAlarms.Raise(new ChangeAlarmsEA() { AddedAlarm = UI.AlarmDisplay.EAlarmType.FuelLow });
                    }
                }
                else if(_lowFuel == true)
                {
                    _turnOffLowFuelSignalization.Raise();
                    _lowFuel = false;
                    _changeAlarms.Raise(new ChangeAlarmsEA() { RemovedAlarm = UI.AlarmDisplay.EAlarmType.FuelLow });
                }
                
                yield return new WaitForSeconds(1f);
            }
        }

        private void OnEnable()
        {
            _checkStatusCoroutine = StartCoroutine(CheckStatus());
        }

        private void OnDisable()
        {
            if(_checkStatusCoroutine != null)
                StopCoroutine(_checkStatusCoroutine);
        }
    }
}