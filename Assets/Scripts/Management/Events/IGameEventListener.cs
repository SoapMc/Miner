using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Miner.Management.Events
{
    public interface IGameEventListener
    {
        void OnEventRaised(EventArgs args);
    }
}