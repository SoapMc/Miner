using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    public abstract class UsableItemEffect : ScriptableObject
    {
        [TextArea, SerializeField] private string _description = null;

        public abstract void Execute();
    }
}