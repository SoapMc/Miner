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
        [SerializeField] private GameEvent _updatePlayerData = null;
        [SerializeField] private GameEvent _playerDead = null;
        [SerializeField] private GameEvent _chooseUsableItem = null;
        [SerializeField] private GameEvent _addResourceToCargo = null;

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
        [SerializeField] private IntReference _cargoMass = null;
        [SerializeField] private ReferencePartList _partList = null;
        [SerializeField] private UsableItemList _usableItemList = null;
        [SerializeField] private TileTypes _tileTypes = null;

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
        private int _chosenUsableItemIndex = 0;

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
            part.Durability = Mathf.Clamp(durability, 0.1f, 1f);
            switch(part)
            {
                case HullReferencePart hull:
                    _equipment.Hull = hull;
                    _maxHull.Value = (int)(hull.MaxHull * part.Durability);
                    if(durability == 1f)
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
            PlayerData pd = new PlayerData()
            {
                Money = _money,
                MaxHull = _maxHull,
                Hull = _hull,
                MaxFuel = _maxFuel,
                Fuel = _fuel
            };

            if(_equipment.Hull != null)
            {
                pd.HullPartId = _equipment.Hull.Id;
                pd.HullDurability = _equipment.Hull.Durability;
            }
            if (_equipment.FuelTank != null)
            {
                pd.FuelTankPartId = _equipment.Hull.Id;
                pd.FuelTankDurability = _equipment.FuelTank.Durability;
            }
            if (_equipment.Engine != null)
            {
                pd.EnginePartId = _equipment.Engine.Id;
                pd.EngineDurability = _equipment.Engine.Durability;
            }
            if (_equipment.Drill != null)
            {
                pd.DrillPartId = _equipment.Drill.Id;
                pd.DrillDurability = _equipment.Drill.Durability;
            }
            if (_equipment.Cooling != null)
            {
                pd.CoolingPartId = _equipment.Cooling.Id;
                pd.CoolingDurability = _equipment.Cooling.Durability;
            }
            if (_equipment.Cargo != null)
            {
                pd.CargoPartId = _equipment.Cargo.Id;
                pd.CargoDurability = _equipment.Cargo.Durability;
            }
            if (_equipment.Battery != null)
            {
                pd.BatteryPartId = _equipment.Battery.Id;
                pd.BatteryDurability = _equipment.Battery.Durability;
            }

            foreach(var usableItem in _usableItems)
            {
                pd.UsableItems.Add(new PlayerData.UsableItemSaveData(usableItem.Item.Id, usableItem.Amount));
            }
            foreach(var cargoElem in _cargo)
            {
                pd.CargoElements.Add(new PlayerData.CargoElementSaveData(cargoElem.Type.Id, cargoElem.Amount));
            }
            return pd;
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
            if(data.HullPartId != -1) Equip(_partList.GetPart(data.HullPartId), data.HullDurability);
            if (data.FuelTankPartId != -1) Equip(_partList.GetPart(data.FuelTankPartId), data.FuelTankDurability);
            if (data.EnginePartId != -1) Equip(_partList.GetPart(data.EnginePartId), data.EngineDurability);
            if (data.DrillPartId != -1) Equip(_partList.GetPart(data.DrillPartId), data.DrillDurability);
            if (data.CoolingPartId != -1) Equip(_partList.GetPart(data.CoolingPartId), data.CoolingDurability);
            if (data.CargoPartId != -1) Equip(_partList.GetPart(data.CargoPartId), data.CargoDurability);
            if (data.BatteryPartId != -1) Equip(_partList.GetPart(data.BatteryPartId), data.BatteryDurability);

            foreach(var usableItem in data.UsableItems)
            {
                _usableItems.Add(new UsableItemTable.Element() { Item = _usableItemList.GetItem(usableItem.Id), Amount = usableItem.Amount });
            }
            foreach(var cargoElem in data.CargoElements)
            {
                _cargo.Add(new CargoTable.Element() { Type = _tileTypes.GetTileType(cargoElem.Id), Amount = cargoElem.Amount });
            }
        }

        public void OnUpdatePlayerData(EventArgs args)
        {
            if (args is UpdatePlayerDataEA upd)
            {
                _money.Value += upd.MoneyChange;

                _maxHull.Value += upd.MaxHullChange;
                _hull.Value += upd.HullChange;
                _hull.Value = Mathf.Clamp(_hull.Value, 0, _maxHull.Value);

                _fuel.Value += upd.FuelChange;
                _maxFuel.Value += upd.MaxFuelChange;
                if (_fuel > _maxFuel)
                {
                    _fuel.Value = _maxFuel;
                }

                foreach (var addElem in upd.AddCargoChange)
                {
                    _cargo.Add(addElem);
                    _cargoMass.Value += addElem.Type.Mass;
                    _addResourceToCargo.Raise(new AddResourceToCargoEA(addElem));
                }

                foreach (var removeElem in upd.RemoveCargoChange)
                {
                    _cargo.Remove(removeElem);
                    _cargoMass.Value -= removeElem.Type.Mass;
                }

                foreach (var newPart in upd.EquipmentChange)
                {
                    Equip(newPart);
                }

                foreach (var addUsableItem in upd.AddUsableItemsChange)
                {
                    _usableItems.Add(addUsableItem);
                }

                foreach (var removeUsableItem in upd.RemoveUsableItemsChange)
                {
                    _usableItems.Remove(removeUsableItem);
                }

                DealPermaDamage(_equipment.Hull, upd.HullPermaDamage);
                DealPermaDamage(_equipment.FuelTank, upd.FuelTankPermaDamage);
                DealPermaDamage(_equipment.Engine, upd.EnginePermaDamage);
                DealPermaDamage(_equipment.Drill, upd.DrillPermaDamage);
                DealPermaDamage(_equipment.Cooling, upd.CoolingPermaDamage);
                DealPermaDamage(_equipment.Cargo, upd.CargoPermaDamage);
                DealPermaDamage(_equipment.Battery, upd.BatteryPermaDamage);

                if(_hull.Value <= 0)
                {
                    _playerDead.Raise();

                    DealPermaDamage(_equipment.Hull, Random.Range(0, 5));
                    DealPermaDamage(_equipment.FuelTank, Random.Range(0, 5));
                    DealPermaDamage(_equipment.Engine, Random.Range(0, 5));
                    DealPermaDamage(_equipment.Drill, Random.Range(0, 5));
                    DealPermaDamage(_equipment.Cooling, Random.Range(0, 5));
                    DealPermaDamage(_equipment.Cargo, Random.Range(0, 5));
                    DealPermaDamage(_equipment.Battery, Random.Range(0, 5));
                }

            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        public void OnRestoreGameAfterPlayerDestroyed()
        {
            _hull.Value = _maxHull.Value;
            _fuel.Value = _maxFuel.Value;

            _cargo.Clear();
        }

        public void OnChooseNextUsableItem()
        {
            if(_usableItems.Count > 0)
            {
                
                _chosenUsableItemIndex++;
                if (_chosenUsableItemIndex >= _usableItems.Count)
                    _chosenUsableItemIndex = 0;
                _chooseUsableItem.Raise(new ChooseUsableItemEA(_usableItems[_chosenUsableItemIndex].Item));
            }
        }

        public void OnChoosePreviousUsableItem()
        {
            if (_usableItems.Count > 0)
            {
                _chosenUsableItemIndex--;
                if (_chosenUsableItemIndex < 0)
                    _chosenUsableItemIndex = _usableItems.Count - 1;
                _chooseUsableItem.Raise(new ChooseUsableItemEA(_usableItems[_chosenUsableItemIndex].Item));
            }
        }

        public void OnUseItemRequest(EventArgs args)
        {
            if(args is UseItemRequestEA uir)
            {
                if (_chosenUsableItemIndex < _usableItems.Count && _chosenUsableItemIndex >= 0)
                {
                    Debug.Log("Use item : " + _usableItems[_chosenUsableItemIndex].Item.Name + " at position " + uir.GridPosition);
                    UpdatePlayerDataEA upd = new UpdatePlayerDataEA();
                    upd.RemoveUsableItemsChange.Add(new UsableItemTable.Element() { Item = _usableItems[_chosenUsableItemIndex].Item, Amount = 1 });
                    _updatePlayerData.Raise(upd);
                }
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        private void DealPermaDamage(ReferencePart part, int amount)
        {
            if (amount > 0)
            {
                if (part != null)
                {
                    part.Durability -= amount / 100f;
                    Equip(part, part.Durability);
                }
            }
        }

    }
}