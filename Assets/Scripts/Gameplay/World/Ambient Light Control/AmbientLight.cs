using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Miner.FX
{
    public abstract class AmbientLight : ScriptableObject
    {

        [SerializeField] protected int _priority = 0;

        public int Priority => _priority;

        public abstract Color UpdateLightColor();
        public virtual void OnSelected() { }
    }
}