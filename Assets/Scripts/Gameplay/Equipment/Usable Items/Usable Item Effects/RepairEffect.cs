using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Usable Item Effects/Repair Effect")]
    public class RepairEffect : UsableItemEffect
    {
        [Header("Events")]
        [SerializeField] private GameEvent _updatePlayerData = null;

        [Header("Data")]
        [SerializeField] private int _repairValue = 25;

        public override void Execute()
        {
            _soundOnUse.Play();
            UpdatePlayerDataEA upd = new UpdatePlayerDataEA() { HullChange = _repairValue };
            _updatePlayerData.Raise(upd);
        }
    }
}