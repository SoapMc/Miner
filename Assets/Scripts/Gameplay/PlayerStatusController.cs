using System.Collections;
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
        [SerializeField] private FloatReference _fuel = null;

        [Header("Events")]
        [SerializeField] private GameEvent _triggerStatusPanel = null;
        [SerializeField] private GameEvent _updatePlayerData = null;
        [SerializeField] private GameEvent _cameraShake = null;

        [Header("Signalization")]
        [SerializeField, Range(0f, 1f)] private float _fuelSignalizationThreshold = 0.2f;

        private Vector2 _previousSpeed = Vector2.zero;
        private float _cameraShakeAmplitude = 0.2f;
        private Coroutine _checkStatus = null;

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
                _cameraShake.Raise(new CameraShakeEA(_cameraShakeAmplitude));
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
                TriggerStatusPanelEA tsp = new TriggerStatusPanelEA();
                if (_equipment.Engine != null)
                {
                    if (_internalTemperature.Value > _equipment.Engine.ThermalVulnerability)
                    {
                        tsp.EnableIcons.Add(new TriggerStatusPanelEA.Element() { Symbol = TriggerStatusPanelEA.ESymbol.Temperature, Mode = TriggerStatusPanelEA.EMode.Warning, Time = 1f });
                        tsp.EnableIcons.Add(new TriggerStatusPanelEA.Element() { Symbol = TriggerStatusPanelEA.ESymbol.Engine, Mode = TriggerStatusPanelEA.EMode.Warning});
                    }
                }

                if (_equipment.Battery != null)
                {
                    if (_internalTemperature.Value > _equipment.Battery.ThermalVulnerability)
                    {
                        
                        tsp.EnableIcons.Add(new TriggerStatusPanelEA.Element() { Symbol = TriggerStatusPanelEA.ESymbol.Temperature, Mode = TriggerStatusPanelEA.EMode.Warning, Time = 1f });
                        tsp.EnableIcons.Add(new TriggerStatusPanelEA.Element() { Symbol = TriggerStatusPanelEA.ESymbol.Battery, Mode = TriggerStatusPanelEA.EMode.Warning});
                    }
                    else
                    {
                        tsp.DisableIcons.Add(TriggerStatusPanelEA.ESymbol.Battery);
                    }
                }

                if(_equipment.FuelTank != null)
                {
                    if (_equipment.FuelTank.Volume != 0f)
                    {
                        if (_fuel.Value / _equipment.FuelTank.Volume < _fuelSignalizationThreshold)
                        {
                            tsp.EnableIcons.Add(new TriggerStatusPanelEA.Element() { Symbol = TriggerStatusPanelEA.ESymbol.Fuel, Mode = TriggerStatusPanelEA.EMode.Warning });
                        }
                        else
                        {
                            tsp.DisableIcons.Add(TriggerStatusPanelEA.ESymbol.Fuel);
                        }
                    }
                }
                if(!tsp.IsEmpty())
                    _triggerStatusPanel.Raise(tsp);
                yield return new WaitForSeconds(1f);
            }
        }

        private void Update()
        {
            CalculateTemperatureFlow();
            CheckForHit();
            if(_checkStatus == null)
                _checkStatus = StartCoroutine(CheckStatus());
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}