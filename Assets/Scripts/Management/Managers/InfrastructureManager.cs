using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using Miner.Gameplay;
using System;

namespace Miner.Management
{
    public class InfrastructureManager : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private GameEvent _infrastructureLoaded = null;
        [SerializeField] private GameEvent _newInfrastructureCreated = null;
        [SerializeField] private GameEvent _infrastructureReset = null;

        [Header("Resources")]
        [SerializeField] private FloatReference _fuelSupply = null;

        public void ResetState()
        {
            _fuelSupply.Value = 0;
            _infrastructureReset.Raise();
        }

        public void CreateNewInfrastructure()
        {
            _fuelSupply.Value = 5000;
            _newInfrastructureCreated.Raise();
        }

        public InfrastructureData RetrieveSerializableData()
        {
            return new InfrastructureData()
            {
                FuelSupply = _fuelSupply
            };
        }

        public void Load(InfrastructureData data)
        {
            if (data.FuelSupply < 0)
            {
                Log.Instance.WriteException(new InvalidSaveStateException());
            }
            _fuelSupply.Value = data.FuelSupply;
            _infrastructureLoaded.Raise();
        }

        public void OnUpdateInfrastructureDate(EventArgs args)
        {
            if(args is UpdateInfrastructureEA ui)
            {
                _fuelSupply.Value += ui.FuelSupplyChange;
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }
    }
}