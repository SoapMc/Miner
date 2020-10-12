using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Miner.Gameplay
{
    [System.Serializable]
    public class WorldData
    {
        public int TimeOfDay;
        public int ElapsedDays;
        public int Seed;
        [NonSerialized] public TilemapData TilemapData;
    }
}