using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.FX;

namespace Miner.Gameplay
{
    public abstract class UsableItemEffect : ScriptableObject
    {
        [SerializeField] protected SoundEffect _soundOnUse = null;

        public abstract void Execute();
    }
}