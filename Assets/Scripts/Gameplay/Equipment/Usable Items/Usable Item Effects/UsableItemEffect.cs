using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    public abstract class UsableItemEffect : ScriptableObject
    {
        virtual public string Description() { return string.Empty; }

        public abstract void Execute();
    }
}