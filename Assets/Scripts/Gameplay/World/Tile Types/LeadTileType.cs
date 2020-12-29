using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "World/Tile Types/Lead Tile Type")]
    public class LeadTileType : TileType
    {
        [SerializeField] private IntReference _playerRadiation = null;
        [SerializeField] private int _radiationChange = -1;

        public override void OnResourceAddedToCargo()
        {
            _playerRadiation.Value += _radiationChange;
        }

        public override void OnResourceRemovedFromCargo()
        {
            _playerRadiation.Value -= _radiationChange;
        }
    }
}