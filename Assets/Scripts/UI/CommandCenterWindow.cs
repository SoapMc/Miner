using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Gameplay;
using System;

namespace Miner.UI
{
    public class CommandCenterWindow : Window
    {
        [SerializeField] private EquipmentTable _playerEquipment = null;
        [SerializeField] private IntReference _playerMaxAchievedDepth = null;
        
        public void OnDayElapsed()
        {
            GenerateMission(CalculateDifficulty());
        }

        private void GenerateMission(int difficulty)
        {
            //create new mission
        }

        private int CalculateDifficulty()
        {
            int difficulty = 0;
            foreach(EPartType type in Enum.GetValues(typeof(EPartType)))
            {
                Part part = _playerEquipment.GetEquippedPart(type);
                if (part != null)
                    difficulty += (int)(part.Cost * 0.01f);
            }
            difficulty += Mathf.Abs(_playerMaxAchievedDepth.Value);

            return difficulty;
        }
    }
}