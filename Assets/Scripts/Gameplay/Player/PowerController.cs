using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using System;
using Miner.Management.Exceptions;

namespace Miner.Gameplay
{
    public class PowerController : MonoBehaviour
    {
        [SerializeField] private GameEvent _signalizeLowPower = null;
        [SerializeField] private GameEvent _turnOffLowPowerSignalization = null;
        [SerializeField] private GameEvent _changeAlarms = null;
        [SerializeField] private FloatReference _playerPower = null;
        [SerializeField] private FloatReference _playerMaxPower = null;
        [SerializeField] private FloatReference _playerPowerUsage = null;
        [SerializeField, Range(0f, 0.5f)] private float _lowPowerSignalizationLevel = 0.2f;
        [SerializeField] private EquipmentTable _playerEquipment = null;
        [SerializeField, Range(0f, 50f)] private float _chargingPowerOnSurface = 10f;
        private bool _onSurface = false;
        private bool _lowPower = false;
        

        public void OnPlayerCameToLayer(EventArgs args)
        {
            if(args is PlayerCameToLayerEA pctl)
            {
                if(pctl.LayerNumber == 0)
                {
                    if (_onSurface == false)  //player came to surface
                    {
                        _playerPowerUsage.Value -= _chargingPowerOnSurface;
                    }
                    _onSurface = true;
                }
                else
                {
                    if(_onSurface)  //player came to underground
                    {
                        _playerPowerUsage.Value += _chargingPowerOnSurface;
                    }
                    _onSurface = false;
                }
               
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        private void DisableAllPowerLoads()
        {
            foreach(EPartType partType in Enum.GetValues(typeof(EPartType)))
            {
                Part part = _playerEquipment.GetEquippedPart(partType);
                if (part.IsConfigurable())
                {
                    if(part.GetConfigurableComponent().UsesPower == true)
                        part.Enabled = false;
                }
            }
        }

        private IEnumerator CheckStatus()
        {
            while (true)
            {
                if (_playerPower.Value <= _lowPowerSignalizationLevel * _playerMaxPower.Value)
                {
                    if (_lowPower == false)
                    {
                        _signalizeLowPower.Raise();
                        _lowPower = true;
                        _changeAlarms.Raise(new ChangeAlarmsEA() { AddedAlarm = UI.AlarmDisplay.EAlarmType.PowerLow });
                    }
                }
                else if (_lowPower == true)
                {
                    _turnOffLowPowerSignalization.Raise();
                    _lowPower = false;
                    _changeAlarms.Raise(new ChangeAlarmsEA() { RemovedAlarm = UI.AlarmDisplay.EAlarmType.PowerLow });
                }

                yield return new WaitForSeconds(1f);
            }
        }

        private void Start()
        {
            StartCoroutine(CheckStatus());
        }

        private void Update()
        {
            _playerPower.Value -= _playerPowerUsage.Value * Time.deltaTime;
            _playerPower.Value = Mathf.Clamp(_playerPower.Value, 0, _playerMaxPower.Value);
            if (_playerPower.Value <= 0.01f)
                DisableAllPowerLoads();
        }
    }
}