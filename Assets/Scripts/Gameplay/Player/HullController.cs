using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using Miner.Management;
using System;

namespace Miner.Gameplay
{
    public class HullController : MonoBehaviour
    {
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
        [SerializeField] private EquipmentTable _playerEquipment = null;
        [SerializeField, Range(0f, 0.5f)] private float _lowHullSignalizationLevel = 0.2f;
        private bool _lowHull = false;
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
                        new Tuple<EPartType, int>(EPartType.Hull, 1), 
                        new Tuple<EPartType, int>(EPartType.FuelTank, 1)
                        );
                }
                _damagePlayer.Raise(dp);
            }
            _previousSpeed = _currentSpeed.Value;
        }

        private void Update()
        {
            CheckForHit();
            if (_playerHull.Value < 0f)
                _killPlayer.Raise(new KillPlayerEA(KillPlayerEA.ESource.FuelEnded));
        }
    }
}