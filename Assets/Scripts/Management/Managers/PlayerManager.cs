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
using System.Collections;
using NewPlayerCreatedEA = Miner.Management.Events.PlayerLoadedEA;
using PlayerEquipmentChangedEA = Miner.Management.Events.ChangeEquipmentEA;

namespace Miner.Management
{

    public class PlayerManager : MonoBehaviour, IEquipmentOwner
    {
        [Header("Events")]
        [SerializeField] private GameEvent _playerLoaded = null;
        [SerializeField] private GameEvent _newPlayerCreated = null;
        [SerializeField] private GameEvent _playerReset = null;
        [SerializeField] private GameEvent _playerDied = null;
        [SerializeField] private GameEvent _chooseUsableItem = null;
        [SerializeField] private GameEvent _updateInfrastructureData = null;
        [SerializeField] private GameEvent _useItem = null;
        [SerializeField] private GameEvent _resourcesLost = null;
        [SerializeField] private GameEvent _respawnPlayer = null;
        [SerializeField] private GameEvent _playerEquipmentChanged = null;
        [SerializeField] private GameEvent _changeUsableItems = null;
        [SerializeField] private GameEvent _showBriefInfo = null;
        [SerializeField] private GameEvent _playerInstantiated = null;

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
        [SerializeField] private FloatReference _power = null;
        [SerializeField] private FloatReference _powerUsage = null;
        [SerializeField] private FloatReference _maxPower = null;
        [SerializeField] private IntReference _radiation = null;
        [SerializeField] private IntReference _radiationTolerance = null;
        [SerializeField] private Vector2IntReference _playerGridPosition = null;
        [SerializeField] private IntReference _maxAchievedDepth = null;
        [SerializeField] private IntReference _maxAchievedLayerNumber = null;
        [SerializeField] private FloatReference _hullPermaDamageThreshold = null;

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

        public int Money => _money;
        public CargoTable Cargo => _cargo;
        public EquipmentTable Equipment => _equipment;
        public float Power { get => _power; set => _power.Value = value; }
        public int MaxHull { get => _maxHull; set => _maxHull.Value = value; }
        public int Hull { get => _hull; set => _hull.Value = value; }
        public float MaxFuel { get => _maxFuel; set => _maxFuel.Value = value; }
        public float Fuel { get => _fuel; set => _fuel.Value = value; }
        public float EffectiveCooling { get => _effectiveCooling; set => _effectiveCooling.Value = value; }
        public int EnginePower { get => _enginePower; set => _enginePower.Value = value; }
        public float FuelUsage { get => _fuelUsage; set => _fuelUsage.Value = value; }
        public float DrillSharpness { get => _drillSharpness; set => _drillSharpness.Value = value; }
        public int CargoMaxMass { get => _cargoMaxMass; set => _cargoMaxMass.Value = value; }
        public int ResistanceToHit { get => _resistanceToHit; set => _resistanceToHit.Value = value; }
        public int ThermalInsulation { get => _thermalInsulation; set => _thermalInsulation.Value = value; }
        public float ChanceForLoseResource { get => _chanceForLoseResource; set => _chanceForLoseResource.Value = value; }
        public float PowerUsage { get => _powerUsage; set => _powerUsage.Value = value; }
        public float MaxPower { get => _maxPower; set => _maxPower.Value = value; }
        public int RadiationTolerance { get => _radiationTolerance; set => _radiationTolerance.Value = value; }
        public float HullPermaDamageThreshold { get => _hullPermaDamageThreshold; set => _hullPermaDamageThreshold.Value = value; }


        #region SAVING AND LOADING
        public void ResetState()
        {
            if (_player != null)
                Destroy(_player.gameObject);

            _chosenUsableItemIndex = 0;
            _money.Value = 0;
            _cargo.Clear();
            _equipment.Clear();
            _power.Value = 0f;
            _maxHull.Value = 0;
            _hull.Value = 0;
            _maxFuel.Value = 0f;
            _effectiveCooling.Value = 0f;
            _enginePower.Value = 0;
            _fuelUsage.Value = 0f;
            _drillSharpness.Value = 0f;
            _cargoMaxMass.Value = 0;
            _resistanceToHit.Value = 0;
            _thermalInsulation.Value = 0;
            _chanceForLoseResource.Value = 0f;
            _powerUsage.Value = 0f;
            _maxPower.Value = 0f;
            _radiation.Value = 0;
            _radiationTolerance.Value = 0;
            _hullPermaDamageThreshold.Value = 0f;
            _maxAchievedLayerNumber.Value = 0;
            _maxAchievedDepth.Value = 0;

            _usableItems.Clear();

            _playerReset.Raise();
        }
        public void CreateNewPlayer()
        {
            if (_player != null)
                Destroy(_player.gameObject);
            _player = Instantiate(_playerPrefab);
            _playerGridPosition.ValueChanged += OnPlayerGridPostionChanged;
            _chosenUsableItemIndex = 0;
            _money.Value = _initialMoney;
            Equip(ReferencePart.CreatePart(_initialHull));
            Equip(ReferencePart.CreatePart(_initialEngine));
            Equip(ReferencePart.CreatePart(_initialFuelTank));
            _fuel.Value = _maxFuel;
            Equip(ReferencePart.CreatePart(_initialDrill));
            Equip(ReferencePart.CreatePart(_initialCooling));
            Equip(ReferencePart.CreatePart(_initialCargo));
            Equip(ReferencePart.CreatePart(_initialBattery));

            _newPlayerCreated.Raise(new NewPlayerCreatedEA(_player));
            _playerInstantiated.Raise();
        }
        public PlayerData RetrieveSerializableData()
        {
            PlayerData pd = new PlayerData()
            {
                Money = _money,
                Hull = _hull,
                Fuel = _fuel,
                Power = _power,
                MaxAchievedDepth = _maxAchievedDepth,
                MaxAchievedLayerNumber = _maxAchievedLayerNumber
            };

            foreach(EPartType partType in Enum.GetValues(typeof(EPartType)))
            {
                Part part = _equipment.GetEquippedPart(partType);
                if(part != null)
                {
                    pd.Equipment.Add(new PlayerData.EquipmentElementSaveData() { Type = partType, Id = part.Id, Durability = part.Durability });
                }
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
            if (data.Money < 0 || data.Hull <= 0 || data.Fuel <= 0 || data.Power <= 0)
            {
                Log.Instance.WriteException(new InvalidSaveStateException());
            }

            ResetState();
            CreateNewPlayer();
            foreach (var equipmentElement in data.Equipment)
            {
                Equip(_partList.GetReferencePart(equipmentElement.Id).CreatePart(equipmentElement.Durability));
            }

            _money.Value = data.Money;
            _hull.Value = data.Hull;
            _fuel.Value = data.Fuel;
            _power.Value = data.Power;
            _maxAchievedDepth.Value = data.MaxAchievedDepth;
            _maxAchievedLayerNumber.Value = data.MaxAchievedLayerNumber;

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
            _playerLoaded.Raise(new PlayerLoadedEA(_player.gameObject));
            _playerInstantiated.Raise();
        }
        #endregion

        private void Equip(Part part)
        {
            if (part == null) return;
            Part previousPart = _equipment.GetEquippedPart(part.Type);
            if (previousPart != null)
                previousPart.Unequip();
            _equipment.SetEquippedPart(part);
            part.Equip(this);
            _playerEquipmentChanged.Raise();
        }

        #region COROUTINES

        private IEnumerator RestorePlayerAfterSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            _hull.Value = _maxHull.Value;
            _fuel.Value = _maxFuel.Value;
            _cargo.Clear();
            _respawnPlayer.Raise();
        }

        #endregion

        #region EVENT RESPONSES

        public void OnChangeUsableItems(EventArgs args)
        {
            if (args is ChangeUsableItemsEA cui)
            {

                foreach (var addedUsableItem in cui.AddedUsableItems)
                {
                    _usableItems.Add(addedUsableItem);
                    _chooseUsableItem.Raise(new ChooseUsableItemEA(_usableItems[_chosenUsableItemIndex].Item));
                }

                foreach (var removedUsableItem in cui.RemovedUsableItems)
                {
                    _usableItems.Remove(removedUsableItem);
                }
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

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
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

        public void OnChangeEquipment(EventArgs args)
        {
            if (args is ChangeEquipmentEA ce)
            {
                foreach (var newPart in ce.PartsToRemove)
                {
                    _equipment.UnequipPart(newPart.Type);
                }

                foreach (var newPart in ce.PartsToEquip)
                {
                    Equip(newPart);
                }
                _playerEquipmentChanged.Raise(ce);
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
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
                    {
                        lostResources.Add(res);
                        _showBriefInfo.Raise(new ShowBriefInfoEA(res.Amount.ToString() + " " + res.Type.Name + " lost!"));
                    }
                    else
                    {
                        if (!res.Type.IsFuel)
                        {
                            addedResources.Add(res);
                            _showBriefInfo.Raise(new ShowBriefInfoEA("+ " + res.Amount.ToString() + " " + res.Type.Name + " (" + (res.Amount * res.Type.Value) + " $)"));
                        }
                        else
                        {
                            fuelAddedToSupplies += res.Type.Mass * res.Amount;
                            _showBriefInfo.Raise(new ShowBriefInfoEA((res.Type.Mass * res.Amount).ToString() + " L of fuel added to fuel supplies"));
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
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

        public void OnUseItemRequest()
        {
            if (_chosenUsableItemIndex < _usableItems.Count && _chosenUsableItemIndex >= 0)
            {
                bool changeUsableItemIndex = false;
                UsableItem item = _usableItems[_chosenUsableItemIndex].Item;
                if (item.UsedPower > _power) return; //not enough power
                _power.Value -= item.UsedPower;
                if (_usableItems[_chosenUsableItemIndex].Amount == 1)   //last item used
                    changeUsableItemIndex = true;
                item.Execute();
                ChangeUsableItemsEA cui = new ChangeUsableItemsEA();
                cui.RemovedUsableItems.Add(new UsableItemTable.Element() { Item = _usableItems[_chosenUsableItemIndex].Item, Amount = 1 });
                _changeUsableItems.Raise(cui);
                _useItem.Raise(new UseItemEA(item));
                _showBriefInfo.Raise(new ShowBriefInfoEA(item.Name + " has been used"));
                if (changeUsableItemIndex && _usableItems.Count > 0)
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
                _layer.Value = pctl.GroundLayer.LayerNumber;
                if (_maxAchievedLayerNumber.Value < _layer.Value)
                    _maxAchievedLayerNumber.Value = _layer.Value;
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

        public void OnKillPlayer()
        {
            _playerDied.Raise();
            StartCoroutine(RestorePlayerAfterSeconds(5f));
        }

        public void OnPlayerGridPostionChanged(Vector2Int old, Vector2Int newVal)
        {
            if (newVal.y < 0)
            {
                int depth = Mathf.Abs(newVal.y);
                if (depth > _maxAchievedDepth.Value)
                    _maxAchievedDepth.Value = depth;
            }
        }

        
        #endregion

    }
}