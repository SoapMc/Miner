using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using UnityEngine.Events;
using Miner.Management;
using Miner.FX;

namespace Miner.Gameplay
{
    public class PlayerStatusController : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 2f)] private float _updateTime = 1f;
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
        [SerializeField] private Vector2IntReference _gridPosition = null;
        
        [SerializeField] private DamageType _damageTypeFromHit = null;

        [Header("Events")]
        [SerializeField] private GameEvent _triggerStatusPanel = null;
        [SerializeField] private GameEvent _updatePlayerData = null;
        [SerializeField] private GameEvent _playerDamaged = null;

        [Header("Signalization")]
        [SerializeField, Range(0f, 1f)] private float _fuelSignalizationThreshold = 0.2f;

        private Vector2 _previousSpeed = Vector2.zero;
        
        private Coroutine _checkStatus = null;
        private bool _engineOverheated = false;
        private float _engineOverheatTime = 0f;
        private bool _batteryOverheated = false;

        private void CalculateTemperatureFlow()
        {
            _heatFlow.Value = (_externalTemperature.Value - _effectiveCooling.Value - _internalTemperature.Value) * Time.deltaTime / (float)(_thermalInsulation.Value + 1);
            if (_internalTemperature.Value < 0f && _heatFlow.Value < 0f)
                return;
            _internalTemperature.Value += _heatFlow.Value;
        }

        private void CheckForHit()
        {
            float sqrSpeedChange = Vector2.SqrMagnitude(_previousSpeed - _currentSpeed.Value);
            if (sqrSpeedChange > _resistanceToHit && _gridPosition.Value.y < 0)
            {
                UpdatePlayerDataEA upd = new UpdatePlayerDataEA();
                int damage = GameRules.Instance.CalculateDamageFromGroundHit(5 * sqrSpeedChange);
                if (_equipment.Hull != null)
                {
                    if (damage >= _equipment.Hull.MaxHull * _equipment.Hull.PermaDamageThreshold)
                    {
                        TriggerStatusPanelEA tsp = new TriggerStatusPanelEA();
                        tsp.EnableIcons.Add(new TriggerStatusPanelEA.Element() { Symbol = TriggerStatusPanelEA.ESymbol.Fuel, Mode = TriggerStatusPanelEA.EMode.Failure, Time = 3f });
                        upd.HullPermaDamage = 1;
                        upd.FuelTankPermaDamage = 1;
                        _triggerStatusPanel.Raise(tsp);
                    }
                    _playerDamaged.Raise(new PlayerDamagedEA(damage, _damageTypeFromHit));
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
                TriggerStatusPanelEA tsp = new TriggerStatusPanelEA();
                if (_equipment.Engine != null)
                {
                    if (_internalTemperature.Value > _equipment.Engine.ThermalVulnerability)
                    {
                        _engineOverheated = true;
                        _engineOverheatTime += _updateTime;
                        if (_engineOverheatTime > 15f)
                        {
                            _updatePlayerData.Raise(new UpdatePlayerDataEA() { EnginePermaDamage = 1 });
                            tsp.EnableIcons.Add(new TriggerStatusPanelEA.Element() { Symbol = TriggerStatusPanelEA.ESymbol.Engine, Mode = TriggerStatusPanelEA.EMode.Failure, Time = 3f } );
                            _engineOverheatTime = 0f;
                        }
                    }
                    else
                    {
                        _engineOverheated = false;
                        _engineOverheatTime = 0f;
                    }

                }

                if (_equipment.Battery != null)
                {
                    if (_internalTemperature.Value > _equipment.Battery.ThermalVulnerability)
                        _batteryOverheated = true;
                    else
                        _batteryOverheated = false;
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

                if(_engineOverheated || _batteryOverheated)
                    tsp.EnableIcons.Add(new TriggerStatusPanelEA.Element() { Symbol = TriggerStatusPanelEA.ESymbol.Temperature, Mode = TriggerStatusPanelEA.EMode.Warning });
                else
                    tsp.DisableIcons.Add(TriggerStatusPanelEA.ESymbol.Temperature);

                if (!tsp.IsEmpty())
                    _triggerStatusPanel.Raise(tsp);
                yield return new WaitForSeconds(_updateTime);
            }
        }

        private void Start()
        {
            _checkStatus = StartCoroutine(CheckStatus());
        }

        private void Update()
        {
            CalculateTemperatureFlow();
            CheckForHit();
        }

        private void OnDestroy()
        {
            StopCoroutine(_checkStatus);
        }
    }
}