using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "World/Tile Types/Uranium Tile Type")]
    public class UraniumTileType : TileType
    {
        [SerializeField] private GameEvent _changePlayerRadiation = null;
        [SerializeField] private int _radiationChange = 1;

        public override void OnResourceAddedToCargo()
        {
            _changePlayerRadiation.Raise(new ChangePlayerRadiationEA(_radiationChange));
        }

        public override void OnResourceRemovedFromCargo()
        {
            _changePlayerRadiation.Raise(new ChangePlayerRadiationEA(-_radiationChange));
        }
    }
}