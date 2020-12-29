using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Miner.Management.Events;
using Miner.Management;

namespace Miner.Gameplay
{
    public class GatherResourcesObjective : Objective, IGameEventListener
    {
        private GameEvent _playerResourcesGathered = null;
        private TileType _tileType = null;

        public GatherResourcesObjective(ObjectiveData data, TileType tileType, GameEvent playerResourcesGathered) : base(data)
        {
            _tileType = tileType;
            _playerResourcesGathered = playerResourcesGathered;
            _playerResourcesGathered.RegisterEvent(this);
        }

        public override string GetDescription()
        {
            return "Collect " + _data.GetFieldValue("RequiredResourceAmount") + " " + _tileType.Name;
        }

        public override string GetShortDescription()
        {
            return "Collect " + _data.GetFieldValue("RequiredResourceAmount") + " " + _tileType.Name + " \nCollected: " + _data.GetFieldValue("CollectedResourceAmount");
        }

        public void OnEventRaised(EventArgs args)
        {
            if (args is ResourcesGatheredEA rg)
            {
                int requiredResourceId = _data.GetFieldValue("RequiredResourceId");
                for (int i = 0; i < rg.Resources.Count; ++i)
                {
                    if (rg.Resources[i].Type.Id == requiredResourceId)
                    {
                        int newAmount = _data.GetFieldValue("CollectedResourceAmount");
                        newAmount += rg.Resources[i].Amount;
                        _data.SetFieldValue("CollectedResourceAmount", newAmount);
                        if (_data.GetFieldValue("CollectedResourceAmount") >= _data.GetFieldValue("RequiredResourceAmount"))
                        {
                            _isCompleted = true;
                        }
                        InvokeObjectiveUpdated();
                    }
                }
            }
            else
            {
                Log.Instance.WriteException(new Management.Exceptions.InvalidEventArgsException());
            }
        }

        public override void Reset()
        {
            _data.SetFieldValue("CollectedResourceAmount", 0);
        }

        public override void Dispose()
        {
            _playerResourcesGathered.UnregisterEvent(this);
        }
    }
}