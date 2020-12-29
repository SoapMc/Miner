using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    public class PowerFlowFactor
    {
        private string _name;
        private string _description;
        private float _powerFlow;

        public string Name => _name;
        public string Description => _description;
        public float PowerFlow => _powerFlow;

        public PowerFlowFactor(string name, string description, float powerFlow)
        {
            _name = name;
            _description = description;
            _powerFlow = powerFlow;
        }
    }
}