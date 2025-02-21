﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using System;
using Miner.Management.Exceptions;
using Miner.Management;
using System.Linq;

namespace Miner.Gameplay
{
    public class PowerController : MonoBehaviour
    {
        [SerializeField] private GameEvent _signalizeLowPower = null;
        [SerializeField] private GameEvent _turnOffLowPowerSignalization = null;
        [SerializeField] private GameEvent _changeAlarms = null;
        [SerializeField] private GameEvent _changePowerFlowFactors = null;

        [SerializeField] private FloatReference _playerPower = null;
        [SerializeField] private FloatReference _playerMaxPower = null;
        [SerializeField] private FloatReference _playerPowerUsage = null;
        [SerializeField] private PowerFlowFactorList _powerFlowFactorList = null;
        [SerializeField, Range(0f, 0.5f)] private float _lowPowerSignalizationLevel = 0.2f;
        [SerializeField] private EquipmentTable _playerEquipment = null;
        [SerializeField, Range(0f, 50f)] private float _chargingPowerOnSurface = 10f;
        private GroundLayer.EAreaType _currentlyVisitedAreaType = GroundLayer.EAreaType.Underground;
        private bool _lowPower = false;
        

        public void OnPlayerCameToLayer(EventArgs args)
        {
            if(args is PlayerCameToLayerEA pctl)
            {
                if(pctl.GroundLayer.AreaType == GroundLayer.EAreaType.Surface)
                {
                    if (_currentlyVisitedAreaType == GroundLayer.EAreaType.Underground)  //player came to surface
                    {
                        ChangePowerFlowFactorsEA cpff = new ChangePowerFlowFactorsEA();
                        cpff.AddFactor(new PowerFlowFactor("Surface Charging", "Nearby star is charging your battery", _chargingPowerOnSurface));
                        _changePowerFlowFactors.Raise(cpff);
                    }
                }
                else
                {
                    if(_currentlyVisitedAreaType == GroundLayer.EAreaType.Surface)  //player came to underground
                    {
                        ChangePowerFlowFactorsEA cpff = new ChangePowerFlowFactorsEA();
                        cpff.RemoveFactor("Surface Charging");
                        _changePowerFlowFactors.Raise(cpff);
                    }
                }
                _currentlyVisitedAreaType = pctl.GroundLayer.AreaType;
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        public void OnChangePowerFlowFactors(EventArgs args)
        {

            if (args is ChangePowerFlowFactorsEA cpff)
            {
                foreach (var factor in cpff.AddedPowerFlowFactors)
                {
                    _powerFlowFactorList.Add(factor);
                    _playerPowerUsage.Value -= factor.PowerFlow;
                }

                foreach (var factor in cpff.RemovedPowerFlowFactors)
                {
                    _playerPowerUsage.Value += _powerFlowFactorList[factor].PowerFlow;
                    _powerFlowFactorList.Remove(factor);
                }
            }
            else
            {
                GameManager.Instance.Log.WriteException(new InvalidEventArgsException());
            }
        }


        private void DisableAllPowerLoads()
        {
            foreach(EPartType partType in Enum.GetValues(typeof(EPartType)))
            {
                Part part = _playerEquipment.GetEquippedPart(partType);
                if (part != null)
                {
                    part.DisableAllConfigurableLoads();
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