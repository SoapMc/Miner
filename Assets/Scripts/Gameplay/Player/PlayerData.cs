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
        public int Hull;
        public float Fuel;
        public float Power;
        public int MaxAchievedDepth;
        public int MaxAchievedLayerNumber;
        public List<EquipmentElementSaveData> Equipment = new List<EquipmentElementSaveData>();
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

        [System.Serializable]
        public class EquipmentElementSaveData
        {
            public EPartType Type;
            public int Id;
            public float Durability;
        }
    }
}