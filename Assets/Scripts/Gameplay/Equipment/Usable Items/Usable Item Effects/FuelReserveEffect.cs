using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Usable Item Effects/Fuel Reserve Effect")]
    public class FuelReserveEffect : UsableItemEffect
    {
        [Header("Events")]
        [SerializeField] private GameEvent _updatePlayerData = null;

        [Header("Data")]
        [SerializeField] private float _reserveVolume = 25;

        public override void Execute()
        {
            _soundOnUse.Play();
            UpdatePlayerDataEA upd = new UpdatePlayerDataEA() { FuelChange = _reserveVolume };
            _updatePlayerData.Raise(upd);
        }
    }
}