﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using UnityEngine.Events;
using Miner.Management;

namespace Miner.Gameplay
{
    public class PlayerStatusController : MonoBehaviour
    {
        [SerializeField] private FloatReference _externalTemperature = null;
        [SerializeField] private FloatReference _internalTemperature = null;
        [SerializeField] private FloatReference _effectiveCooling = null;
        [SerializeField] private FloatReference _surfaceTemperature = null;
        [SerializeField] private IntReference _thermalInsulation = null;
        [SerializeField] private Vector2Reference _currentSpeed = null;
        [SerializeField] private IntReference _resistanceToHit = null;
        [SerializeField] private EquipmentTable _equipment = null;
        [SerializeField] private FloatReference _heatFlow = null;
        [Header("Events")]
        [SerializeField] private GameEvent _triggerStatusPanel = null;
        [SerializeField] private GameEvent _updatePlayerData = null;

        private Vector2 _previousSpeed = Vector2.zero;
        private float _elapsedTime = 0f;
        private float _lastEngineTemperatureCheck = 0f;
        private float _lastBatteryTemperatureCheck = 0f;

        private void CalculateTemperatureFlow()
        {
            _heatFlow.Value = (_externalTemperature.Value - _effectiveCooling.Value - _internalTemperature.Value) * Time.deltaTime / (float)(_thermalInsulation.Value + 1);
            _internalTemperature.Value += _heatFlow.Value; 
        }

        private void CheckForHit()
        {
            float sqrSpeedChange = Vector2.SqrMagnitude(_previousSpeed - _currentSpeed.Value);
            if (sqrSpeedChange > _resistanceToHit)
            {
                UpdatePlayerDataEA upd = new UpdatePlayerDataEA() { HullChange = -GameRules.Instance.CalculateDamageFromGroundHit(5 * sqrSpeedChange) };
                if (_equipment.Hull != null)
                {
                    if (Mathf.Abs(upd.HullChange) >= _equipment.Hull.MaxHull * _equipment.Hull.PermaDamageThreshold)
                        upd.HullPermaDamage = 1;
                }
                _updatePlayerData.Raise(upd);
            }
            _previousSpeed = _currentSpeed.Value;
        }

        public void OnCargoFull()
        {
            TriggerStatusPanelEA tsp = new TriggerStatusPanelEA();
            tsp.EnableIcons.Add(new TriggerStatusPanelEA.Element() { Symbol = TriggerStatusPanelEA.ESymbol.Cargo, Mode = TriggerStatusPanelEA.EMode.Warning, Time = 4f });
            _triggerStatusPanel.Raise(tsp);
        }

        private IEnumerator CheckStatus()
        {
            while (true)
            {
                if (_equipment.Engine != null)
                {
                    if (_internalTemperature.Value > _equipment.Engine.ThermalVulnerability && (_elapsedTime - _lastEngineTemperatureCheck) > 5f)
                    {
                        TriggerStatusPanelEA tsp = new TriggerStatusPanelEA();
                        tsp.EnableIcons.Add(new TriggerStatusPanelEA.Element() { Symbol = TriggerStatusPanelEA.ESymbol.Temperature, Mode = TriggerStatusPanelEA.EMode.Warning, Time = 4f });
                        tsp.EnableIcons.Add(new TriggerStatusPanelEA.Element() { Symbol = TriggerStatusPanelEA.ESymbol.Engine, Mode = TriggerStatusPanelEA.EMode.Warning, Time = 4f });
                        _triggerStatusPanel.Raise(tsp);
                        _lastEngineTemperatureCheck = _elapsedTime;
                    }
                }

                if (_equipment.Battery != null)
                {
                    if (_internalTemperature.Value > _equipment.Battery.ThermalVulnerability)
                    {
                        TriggerStatusPanelEA tsp = new TriggerStatusPanelEA();
                        tsp.EnableIcons.Add(new TriggerStatusPanelEA.Element() { Symbol = TriggerStatusPanelEA.ESymbol.Temperature, Mode = TriggerStatusPanelEA.EMode.Warning});
                        tsp.EnableIcons.Add(new TriggerStatusPanelEA.Element() { Symbol = TriggerStatusPanelEA.ESymbol.Battery, Mode = TriggerStatusPanelEA.EMode.Warning});
                        _triggerStatusPanel.Raise(tsp);
                    }
                    else
                    {
                        TriggerStatusPanelEA tsp = new TriggerStatusPanelEA();
                        tsp.DisableIcons.Add(TriggerStatusPanelEA.ESymbol.Battery);
                        _triggerStatusPanel.Raise(tsp);
                    }
                }

                yield return new WaitForSeconds(1f);
            }
        }

        private void Update()
        {
            CalculateTemperatureFlow();
            CheckForHit();
            _elapsedTime += Time.deltaTime;
        }

        private void Start()
        {
            StartCoroutine(CheckStatus());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}