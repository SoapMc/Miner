using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Miner.Gameplay
{
    [System.Serializable]
    public class PlayerData
    {
        public int Money;
        public int MaxHull;
        public int Hull;
        public float MaxFuel;
        public float Fuel;
        public int HullPartId = -1;
        public int FuelTankPartId = -1;
        public int EnginePartId = -1;
        public int DrillPartId = -1;
        public int CargoPartId = -1;
        public int CoolingPartId = -1;
        public int BatteryPartId = -1;
        public float HullDurability = 1f;
        public float FuelTankDurability = 1f;
        public float EngineDurability = 1f;
        public float DrillDurability = 1f;
        public float CoolingDurability = 1f;
        public float CargoDurability = 1f;
        public float BatteryDurability = 1f;
        public List<UsableItemSaveData> UsableItems = new List<UsableItemSaveData>();
        public List<CargoElementSaveData> CargoElements = new List<CargoElementSaveData>();

        [System.Serializable]
        public class UsableItemSaveData
        {
            public int Id;
            public int Amount;

            public UsableItemSaveData(int id, int amount)
            {
                Id = id;
                Amount = amount;
            }
        }

        [System.Serializable]
        public class CargoElementSaveData
        {
            public int Id;
            public int Amount;

            public CargoElementSaveData(int id, int amount)
            {
                Id = id;
                Amount = amount;
            }
        }
    }
}