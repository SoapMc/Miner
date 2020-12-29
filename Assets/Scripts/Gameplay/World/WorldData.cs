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
        public WorldController.WorldControllerData WorldControllerData = new WorldController.WorldControllerData();
        public MissionController.MissionControllerData MissionControllerData = new MissionController.MissionControllerData();
    }
}