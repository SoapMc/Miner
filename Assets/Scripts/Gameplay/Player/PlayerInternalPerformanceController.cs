using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using System;
using Miner.Management;
namespace Miner.Gameplay
{
    public class PlayerInternalPerformanceController : MonoBehaviour
    {
        [SerializeField] private EquipmentTable _equipment = null;
        [SerializeField] private GameEvent _showBriefInfo = null;

        public void OnDamagePlayer(EventArgs args)
        {
            if (args is DamagePlayerEA dp)
            {
                foreach(EPartType partType in System.Enum.GetValues(typeof(EPartType)))
                {
                    if (dp.PermaDamage.ContainsKey(partType))
                    {
                        Part damagedPart = _equipment.GetEquippedPart(partType);
                        if (damagedPart != null)
                        {
                            damagedPart.Durability -= dp.PermaDamage[partType];
                            _showBriefInfo.Raise(new ShowBriefInfoEA("Performance of " + damagedPart.Name + " has decreased by " + Mathf.RoundToInt(dp.PermaDamage[partType] * 100f) + " %", ShowBriefInfoEA.EType.Warning));
                        }
                    }
                }
            }
            else
                Log.Instance.WriteException(new InvalidEventArgsException());
        }

        public void OnRepairPlayer(EventArgs args)
        {
            if (args is RepairPlayerEA rp)
            {
                foreach (EPartType partType in System.Enum.GetValues(typeof(EPartType)))
                {
                    if (rp.PermaRepair.ContainsKey(partType))
                    {
                        Part repairedPart = _equipment.GetEquippedPart(partType);
                        if (repairedPart != null)
                        {
                            repairedPart.Durability = Mathf.Clamp01(repairedPart.Durability + rp.PermaRepair[partType]);
                            _showBriefInfo.Raise(new ShowBriefInfoEA("Performance of " + repairedPart.Name + " has increased by " + Mathf.RoundToInt(rp.PermaRepair[partType] * 100f) + " %", ShowBriefInfoEA.EType.Info));
                        }
                    }
                }
            }
            else
                Log.Instance.WriteException(new InvalidEventArgsException());
        }

        public void OnPlayerKilled()
        {
            foreach (EPartType partType in System.Enum.GetValues(typeof(EPartType)))
            {
                Part damagedPart = _equipment.GetEquippedPart(partType);
                if (damagedPart != null)
                {
                    damagedPart.Durability -= UnityEngine.Random.Range(0.01f, 0.1f);
                }
            }
        }
    }
}