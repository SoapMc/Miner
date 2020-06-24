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
        [Header("Resources")]
        [SerializeField] private FloatReference _fuelSupply = null;

        private bool _suppressEvents = false;

        public void ResetState()
        {
            _fuelSupply.Value = 5000;
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
                Debug.LogException(new InvalidSaveStateException());
                throw new InvalidSaveStateException();
            }
            _fuelSupply.Value = data.FuelSupply;
        }

        public void OnUpdateInfrastructureDate(EventArgs args)
        {
            if(args is UpdateInfrastructureEA ui)
            {
                _fuelSupply.Value += ui.FuelSupplyChange;
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }
    }
}