using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;

namespace Miner.Gameplay
{
    public abstract class NaturalDisaster : ScriptableObject
    {
        [SerializeField] protected GameEvent _disasterEvent = null;

        public abstract void Execute();
    }
}