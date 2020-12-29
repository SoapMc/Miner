using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;

namespace Miner.Gameplay
{
    public class LavaBehaviour : MonoBehaviour, IDigCompletedHandler, ITemperatureOffsetSource
    {
        private static float _externalTemperatureIncreasing = 100f;
        [SerializeField] private GameEvent _damagePlayer = null;
        [SerializeField] private DamageType _damageType = null;
        [SerializeField, MinMaxIntRange(10, 200)] private RangedInt _damageRange = new RangedInt(40, 80);

        public float GetTemperatureOffset(float sqrDistance)
        {
            float result = Mathf.Clamp(_externalTemperatureIncreasing - 2*sqrDistance, 0, _externalTemperatureIncreasing);
            return result;
        }

        public void OnDigCompleted()
        {
            _damagePlayer.Raise(new DamagePlayerEA(Random.Range(_damageRange.minValue, _damageRange.maxValue), _damageType));
        }
    }
}