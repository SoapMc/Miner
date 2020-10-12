using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;

namespace Miner.Gameplay
{
    public abstract class NaturalDisaster : ScriptableObject
    {
        [SerializeField] protected GameEvent _disasterEvent = null;
        [SerializeField] protected EHappeningType _happeningType = default;
        protected float _time = 0f;

        public float Time => _time;
        public EHappeningType HappeningType => _happeningType;

        public abstract void Execute();
        public abstract void End();

        public enum EHappeningType
        {
            Occasional,
            Constant
        }
    }
}