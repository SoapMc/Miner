using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using Miner.Management;
using System;
using Miner.Management.Exceptions;

namespace Miner.Gameplay
{
    public class HullController : MonoBehaviour
    {
        [SerializeField] private GameEvent _cameraShake = null;
        [SerializeField] private GameEvent _killPlayer = null;
        [SerializeField] private GameEvent _damagePlayer = null;
        [SerializeField] private GameEvent _signalizeLowHull = null;
        [SerializeField] private GameEvent _turnOffLowHullSignalization = null;
        [SerializeField] private IntReference _playerHull = null;
        [SerializeField] private IntReference _playerMaxHull = null;
        [SerializeField] private DamageType _kineticDamageType = null;
        [SerializeField] private Vector2Reference _currentSpeed = null;
        [SerializeField] private IntReference _resistanceToHit = null;
        [SerializeField] private FloatReference _permaDamageThreshold = null;
        [SerializeField] private Vector2IntReference _gridPosition = null;
        [SerializeField, Range(0f, 0.5f)] private float _lowHullSignalizationLevel = 0.2f;
        [SerializeField, Range(0f, 0.5f)] private float _cameraShakeAmplitude = 0.2f;
        private Vector2 _previousSpeed = Vector2.zero;

        private void CheckForHit()
        {
            float sqrSpeedChange = Vector2.SqrMagnitude(_previousSpeed - _currentSpeed.Value);
            if (sqrSpeedChange > _resistanceToHit && _gridPosition.Value.y < 0)
            {
                int damage = Mathf.CeilToInt(0.0002f * Mathf.Pow(5 * sqrSpeedChange, 1.2f));    //damage from ground
                DamagePlayerEA dp = new DamagePlayerEA(damage, _kineticDamageType);
                if (damage >= _playerMaxHull * _permaDamageThreshold)
                {
                    dp.DealPermaDamage(
                        new Tuple<EPartType, float>(EPartType.Hull, 0.01f), 
                        new Tuple<EPartType, float>(EPartType.FuelTank, 0.01f)
                        );
                }
                _damagePlayer.Raise(dp);
            }
            _previousSpeed = _currentSpeed.Value;
        }

        public void OnDamagePlayer(EventArgs args)
        {
            if (args is DamagePlayerEA pd)
            {
                if (pd.Damage > 0)
                {
                    _playerHull.Value -= pd.Damage;
                    _playerHull.Value = Mathf.Clamp(_playerHull.Value, 0, _playerMaxHull.Value);
                    _cameraShake.Raise(new CameraShakeEA(_cameraShakeAmplitude));
                    
                    if (_playerHull.Value < _lowHullSignalizationLevel * _playerMaxHull.Value)
                        _signalizeLowHull.Raise();

                    if (_playerHull.Value <= 0f)
                        _killPlayer.Raise(new KillPlayerEA(KillPlayerEA.ESource.Damage));
                }
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

        public void OnRepairPlayer(EventArgs args)
        {
            if (args is RepairPlayerEA pr)
            {
                if (pr.Repair > 0)
                {
                    _playerHull.Value += pr.Repair;
                    _playerHull.Value = Mathf.Clamp(_playerHull.Value, 0, _playerMaxHull.Value);
                    if (_playerHull.Value >= _lowHullSignalizationLevel * _playerMaxHull.Value)
                        _turnOffLowHullSignalization.Raise();
                }
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

        public void OnPlayerRespawned()
        {
            _turnOffLowHullSignalization.Raise();
        }

        private void Update()
        {
            CheckForHit();
            
        }
    }
}