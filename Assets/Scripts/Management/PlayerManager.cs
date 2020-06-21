using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using Miner.Management;
using System;
using Random = UnityEngine.Random;
using Miner.Gameplay;
using Miner.Management.Exceptions;
using UnityEngine.Tilemaps;


namespace Miner.Management
{
    [System.Serializable]
    public class PlayerManager : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private GameEvent _updatePlayerUI = null;

        [Header("Resources")]
        [SerializeField] private IntReference _money = null;
        [SerializeField] private IntReference _maxHull = null;
        [SerializeField] private IntReference _hull = null;
        [SerializeField] private FloatReference _maxFuel = null;
        [SerializeField] private FloatReference _fuel = null;
        [SerializeField] private FloatReference _effectiveCooling = null;
        [SerializeField] private IntReference _enginePower = null;
        [SerializeField] private FloatReference _drillSharpness = null;
        [SerializeField] private FloatReference _fuelUsage = null;
        [SerializeField] private IntReference _availableCells = null;
        [SerializeField] private IntReference _cargoVolume = null;
        [SerializeField] private CargoTable _cargo = null;
        [SerializeField] private EquipmentTable _equipment = null;
        [SerializeField] private UsableItemTable _usableItems = null;
        [SerializeField] private IntReference _resistanceToHit = null;
        [SerializeField] private IntReference _thermalInsulation = null;

        [Header("Initial resources")]
        [SerializeField] private int _initialMoney = 0;
        [SerializeField] private HullReferencePart _initialHull = null;
        [SerializeField] private BatteryReferencePart _initialBattery = null;
        [SerializeField] private EngineReferencePart _initialEngine = null;
        [SerializeField] private DrillReferencePart _initialDrill = null;
        [SerializeField] private CoolingReferencePart _initialCooling = null;
        [SerializeField] private CargoReferencePart _initialCargo = null;
        [SerializeField] private FuelTankReferencePart _initialFuelTank = null;

        private bool _suppressEvents = false;

        public int Money => _money;
        public int MaxHull => _maxHull;
        public int Hull => _hull;
        public float MaxFuel => _maxFuel;
        public float Fuel => _fuel;
        public CargoTable Cargo => _cargo;
        public EquipmentTable Equipment => _equipment;

        
        public void ResetCharacter()
        {
            _money.Value = _initialMoney;
            Equip(_initialHull);
            Equip(_initialEngine);
            Equip(_initialFuelTank);
            _fuel.Value = _maxFuel;
            Equip(_initialDrill);
            Equip(_initialCooling);
            Equip(_initialCargo);
            Equip(_initialBattery);
            _usableItems.Clear();
        }

        private void Equip(ReferencePart part, float durability = 1f)
        {
            if (part == null) return;
            part.Durability = durability;
            switch(part)
            {
                case HullReferencePart hull:
                    _equipment.Hull = hull;
                    _maxHull.Value = (int)(hull.MaxHull * part.Durability);
                    _hull.Value = _maxHull.Value;
                    _resistanceToHit.Value = (int)(hull.ResistanceToHit * part.Durability);
                    _thermalInsulation.Value = hull.ThermalInsulation;
                    break;
                case EngineReferencePart engine:
                    _equipment.Engine = engine;
                    _enginePower.Value = (int)(engine.Power * part.Durability);
                    _fuelUsage.Value = engine.FuelUsage;
                    break;
                case FuelTankReferencePart fuelTank:
                    _equipment.FuelTank = fuelTank;
                    _maxFuel.Value = fuelTank.Volume;
                    if(_fuel > _maxFuel)
                    {
                        _fuel.Value = _maxFuel;
                    }
                    break;
                case DrillReferencePart drill:
                    _equipment.Drill = drill;
                    _drillSharpness.Value = drill.Sharpness;
                    break;
                case CargoReferencePart cargo:
                    _equipment.Cargo = cargo;
                    _cargoVolume.Value = cargo.Volume;
                    break;
                case BatteryReferencePart battery:
                    _equipment.Battery = battery;
                    _availableCells.Value = battery.AvailableCells;
                    break;
                case CoolingReferencePart cooling:
                    _equipment.Cooling = cooling;
                    
                    break;
            }
        }
        

        public PlayerData RetrieveSerializableData()
        {
            return new PlayerData()
            {
                Money = _money,
                MaxHull = _maxHull,
                Hull = _hull,
                MaxFuel = _maxFuel,
                Fuel = _fuel
            };
        }

        public void Load(PlayerData data)
        {
            if (data.Money < 0 || data.MaxHull <= 0 || data.MaxFuel <= 0 || data.Hull < 0 || data.Fuel < 0)
            {
                Debug.LogException(new InvalidSaveStateException());
                throw new InvalidSaveStateException();
            }

            _money.Value = data.Money;
            _maxHull.Value = data.MaxHull;
            _hull.Value = data.Hull;
            _maxFuel.Value = data.MaxFuel;
            _fuel.Value = data.Fuel;

            //_updatePlayerStatsUI.Raise(new UpdatePlayerStatsUIEventArgs(0, GameManager.Instance.Player, GameManager.Instance.Player, GameManager.Instance.Player));
        }

        public void OnUpdatePlayerData(EventArgs args)
        {
            if(args is UpdatePlayerDataEA upd)
            {
                _money.Value += upd.MoneyChange;

                _maxHull.Value += upd.MaxHullChange;
                _hull.Value += upd.HullChange;
                if(_hull > _maxHull)
                {
                    _hull.Value = _maxHull;
                }

                _fuel.Value += upd.FuelChange;
                _maxFuel.Value += upd.MaxFuelChange;
                if(_fuel > _maxFuel)
                {
                    _fuel.Value = _maxFuel;
                }

                foreach(var addElem in upd.AddCargoChange)
                {
                    _cargo.Add(addElem);
                }

                foreach (var removeElem in upd.RemoveCargoChange)
                {
                    _cargo.Remove(removeElem);
                }

                foreach(var newPart in upd.EquipmentChange)
                {
                    Equip(newPart);
                }

                foreach(var addUsableItem in upd.AddUsableItemsChange)
                {
                    _usableItems.Add(addUsableItem);
                }

                foreach(var removeUsableItem in upd.RemoveUsableItemsChange)
                {
                    _usableItems.Remove(removeUsableItem);
                }
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

    }
}