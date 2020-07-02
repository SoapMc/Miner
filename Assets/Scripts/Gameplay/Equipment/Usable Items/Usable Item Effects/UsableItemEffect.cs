using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    public abstract class UsableItemEffect : ScriptableObject
    {
        public abstract void Execute();
    }
}