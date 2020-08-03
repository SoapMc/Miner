using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using Miner.Management;
using System;
using Random = UnityEngine.Random;
using Miner.Gameplay;
using Miner.Management.Exceptions;
using UnityEngine.Tilemaps;
using System.Linq;
using Miner.FX;

namespace Miner.Management
{
    [System.Serializable]
    public class PlayerManager : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private GameEvent _playerLoaded = null;
        [SerializeField] private GameEvent _playerDestroyed = null;
        [SerializeField] private GameEvent _updatePlayerData = null;
        [SerializeField] private GameEvent _playerDead = null;
        [SerializeField] private GameEvent _chooseUsableItem = null;
        [SerializeField] private GameEvent _updateInfrastructureData = null;
        [SerializeField] private GameEvent _cargoFull = null;
        [SerializeField] private GameEvent _useItem = null;
        [SerializeField] private GameEvent _resourcesLost = null;
        [SerializeField] private GameEvent _cameraShake = null;

        [Header("Resources")]
        [SerializeField] private IntReference _layer = null;
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
        [SerializeField] private IntReference _cargoMaxMass = null;
        [SerializeField] private CargoTable _cargo = null;
        [SerializeField] private EquipmentTable _equipment = null;
        [SerializeField] private UsableItemTable _usableItems = null;
        [SerializeField] private IntReference _resistanceToHit = null;
        [SerializeField] private IntReference _thermalInsulation = null;
        [SerializeField] private FloatReference _chanceForLoseResource = null;
        [SerializeField] private ReferencePartList _partList = null;
        [SerializeField] private UsableItemList _usableItemList = null;
        [SerializeField] private TileTypes _tileTypes = null;
        [SerializeField] private GameObject _playerPrefab = null;
        [SerializeField] private SoundEffect _hitSound = null;

        [Header("Initial resources")]
        [SerializeField] private int _initialMoney = 0;
        [SerializeField] private HullReferencePart _initialHull = null;
        [SerializeField] private BatteryReferencePart _initialBattery = null;
        [SerializeField] private EngineReferencePart _initialEngine = null;
        [SerializeField] private DrillReferencePart _initialDrill = null;
        [SerializeField] private CoolingReferencePart _initialCooling = null;
        [SerializeField] private CargoReferencePart _initialCargo = null;
        [SerializeField] private FuelTankReferencePart _initialFuelTank = null;

        private GameObject _player = null;
        private int _chosenUsableItemIndex = 0;
        private float _cameraShakeAmplitude = 0.2f;

        public int Money => _money;
        public int MaxHull => _maxHull;
        public int Hull => _hull;
        public float MaxFuel => _maxFuel;
        public float Fuel => _fuel;
        public CargoTable Cargo => _cargo;
        public EquipmentTable Equipment => _equipment;

        
        public void ResetCharacter()
        {
            if (_player != null)
                Destroy(_player.gameObject);
            _player = Instantiate(_playerPrefab);
            _equipment.Clear();
            _money.Value = _initialMoney;
            Equip(_initialHull);
            Equip(_initialEngine);
            Equip(_initialFuelTank);
            _fuel.Value = _maxFuel;
            Equip(_initialDrill);
            Equip(_initialCooling);
            Equip(_initialCargo);
            Equip(_initialBattery);
            _cargo.Clear();
            _usableItems.Clear();

            _playerLoaded.Raise(new PlayerLoadedEA(_player.gameObject));
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
                    _enginePower.Value = (int)(engine.Power * part.Durability * 1000);
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
                    _cargoMaxMass.Value = cargo.MaxMass;
                    _chanceForLoseResource.Value = cargo.ChanceForLoseResource();
                    break;
                case BatteryReferencePart battery:
                    _equipment.Battery = battery;
                    _availableCells.Value = battery.AvailableCells;
                    break;
                case CoolingReferencePart cooling:
                    _equipment.Cooling = cooling;
                    _effectiveCooling.Value = cooling.EffectiveCooling;
                    break;
                default:
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
                pd.FuelTankPartId = _equipment.FuelTank.Id;
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
            if (data.Money < 0 || data.MaxHull <= 0 || data.MaxFuel <= 0)
            {
                Debug.LogException(new InvalidSaveStateException());
                throw new InvalidSaveStateException();
            }

            ResetCharacter();
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
            List<CargoTable.Element> resources = new List<CargoTable.Element>(data.CargoElements.Count);
            foreach(var cargoElem in data.CargoElements)
            {
                resources.Add(new CargoTable.Element() { Type = _tileTypes.GetTileType(cargoElem.Id), Amount = cargoElem.Amount });
            }
            _cargo.Add(resources);
        }

        public void Unload()
        {
            _playerDestroyed.Raise();
            if (_player != null)
                Destroy(_player.gameObject);
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

        #region EVENT RESPONSES

        public void OnUpdatePlayerData(EventArgs args)
        {
            if (args is UpdatePlayerDataEA upd)
            {
                _money.Value += upd.MoneyChange;
                _maxHull.Value += upd.MaxHullChange;
                _fuel.Value += upd.FuelChange;
                _fuel.Value = Mathf.Clamp(_fuel.Value, 0, _maxFuel.Value);
                _maxFuel.Value += upd.MaxFuelChange;
                if (_fuel > _maxFuel)
                {
                    _fuel.Value = _maxFuel;
                }

                foreach (var newPart in upd.EquipmentChange)
                {
                    Equip(newPart);
                }

                foreach (var addUsableItem in upd.AddUsableItemsChange)
                {
                    _usableItems.Add(addUsableItem);
                    _chooseUsableItem.Raise(new ChooseUsableItemEA(_usableItems[_chosenUsableItemIndex].Item));
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

        public void OnTryChangeResourcesToPlayerCargo(EventArgs args)
        {
            if (args is TryChangeResourcesInPlayerCargoEA tcr)
            {
                List<CargoTable.Element> addedResources = new List<CargoTable.Element>();
                List<CargoTable.Element> lostResources = new List<CargoTable.Element>();
                int fuelAddedToSupplies = 0;
                foreach (var res in tcr.ResourcesToAdd)
                {
                    float prob = Random.Range(0f, 1f);
                    if (prob <= _chanceForLoseResource)
                        lostResources.Add(res);
                    else
                    {
                        if (!res.Type.IsFuel)
                        {
                            addedResources.Add(res);  
                        }
                        else
                        {
                            fuelAddedToSupplies += res.Type.Mass;
                        }
                    }
                }
                _cargo.Add(addedResources);
                if (fuelAddedToSupplies > 0)
                    _updateInfrastructureData.Raise(new UpdateInfrastructureEA() { FuelSupplyChange = fuelAddedToSupplies });
                if (lostResources.Count > 0)
                    _resourcesLost.Raise(new ResourcesLostEA(lostResources));

                _cargo.Remove(tcr.ResourcesToRemove);
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        public void OnUseItemRequest()
        {
            if (_chosenUsableItemIndex < _usableItems.Count && _chosenUsableItemIndex >= 0)
            {
                bool changeUsableItemIndex = false;
                UsableItem item = _usableItems[_chosenUsableItemIndex].Item;
                if (_usableItems[_chosenUsableItemIndex].Amount == 1)   //last item used
                    changeUsableItemIndex = true;
                item.Execute();
                UpdatePlayerDataEA upd = new UpdatePlayerDataEA();
                upd.RemoveUsableItemsChange.Add(new UsableItemTable.Element() { Item = _usableItems[_chosenUsableItemIndex].Item, Amount = 1 });
                _updatePlayerData.Raise(upd);
                _useItem.Raise(new UseItemEA(item));
                if(changeUsableItemIndex && _usableItems.Count > 0)
                {
                    _chosenUsableItemIndex = 0;
                    _chooseUsableItem.Raise(new ChooseUsableItemEA(_usableItems[_chosenUsableItemIndex].Item));
                }
            }
        }

        public void OnPlayerCameToLayer(EventArgs args)
        {
            if (args is PlayerCameToLayerEA pctl)
            {
                _layer.Value = pctl.LayerNumber;
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        public void OnPlayerDamaged(EventArgs args)
        {
            if(args is PlayerDamagedEA pd)
            {
                if (pd.Damage > 0)
                {
                    _hull.Value -= pd.Damage;
                    _hull.Value = Mathf.Clamp(_hull.Value, 0, _maxHull.Value);
                    _cameraShake.Raise(new CameraShakeEA(_cameraShakeAmplitude));
                    _hitSound.Play();

                    if (_hull.Value <= 0)
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
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        public void OnPlayerRepaired(EventArgs args)
        {
            if(args is PlayerRepairedEA pr)
            {
                if(pr.Repair > 0)
                {
                    _hull.Value += pr.Repair;
                    _hull.Value = Mathf.Clamp(_hull.Value, 0, _maxHull.Value);
                }
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }
        #endregion

    }
}