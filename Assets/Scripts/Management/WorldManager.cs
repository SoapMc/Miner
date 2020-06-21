using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using Miner.Management;
using UnityEngine.Events;
using Miner.Gameplay;

namespace Miner.Management
{
    public class WorldManager : MonoBehaviour
    {
        [SerializeField] private WorldController _worldPrefab = null;
        private WorldController _worldController = null;

        public void ResetState()
        {
            _worldController = Instantiate(_worldPrefab, Vector3.zero, Quaternion.identity);
        }
    }
}