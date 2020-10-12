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

namespace Miner.Management
{
    [System.Serializable]
    public class PlayerManager : MonoBehaviour, IEquipmentOwner
    {
        [Header("Events")]
        [SerializeField] private GameEvent _playerLoaded = null;
        [SerializeField] private GameEvent _playerDestroyed = null;
        [SerializeField] private GameEvent _updatePlayerData = null;
        [SerializeField] private GameEvent _playerDied = null;
        [SerializeField] private GameEvent _chooseUsableItem = null;
        [SerializeField] private GameEvent _updateInfrastructureData = null;
        [SerializeField] private GameEvent _cargoFull = null;
        [SerializeField] private GameEvent _useItem = null;
        [SerializeField] private GameEvent _resourcesLost = null;
        [SerializeField] private GameEvent _cameraShake = null;
        [SerializeField] private GameEvent _respawnPlayer = null;
        [SerializeField] private GameEvent _playerRespawned = null;
        [SerializeField] private GameEvent _playerRadiationChanged = null;

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
        [SerializeField] private FloatReference _power = null;
        [SerializeField] private FloatReference _powerUsage = null;
        [SerializeField] private FloatReference _maxPower = null;
        [SerializeField] private IntReference _radiation = null;
        [SerializeField] private IntReference _radiationTolerance = null;
        [SerializeField] private Vector2IntReference _playerGridPosition = null;
        [SerializeField] private IntReference _playerMaxAchievedDepth = null;

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

        public void ResetCharacter()
        {
            if (_player != null)
                Destroy(_player.gameObject);
            _player = Instantiate(_playerPrefab);
            _equipment.Clear();
            _money.Value = _initialMoney;
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
            _power.Value = 0f;
            _maxPower.Value = 0f;
            _radiation.Value = 0;
            _radiationTolerance.Value = 0;
            Equip(ReferencePart.CreatePart(_initialHull));
            Equip(ReferencePart.CreatePart(_initialEngine));
            Equip(ReferencePart.CreatePart(_initialFuelTank));
            _fuel.Value = _maxFuel;
            Equip(ReferencePart.CreatePart(_initialDrill));
            Equip(ReferencePart.CreatePart(_initialCooling));
            Equip(ReferencePart.CreatePart(_initialCargo));
            
            Equip(ReferencePart.CreatePart(_initialBattery));
            _cargo.Clear();
            _usableItems.Clear();
            _playerGridPosition.ValueChanged += OnPlayerGridPostionChanged;

            _playerLoaded.Raise(new PlayerLoadedEA(_player.gameObject));
        }

        private void Equip(Part part)
        {
            if (part == null) return;
            Part previousPart = _equipment.GetEquippedPart(part.Type);
            if (previousPart != null)
                previousPart.Unequip();
            _equipment.SetEquippedPart(part);
            part.Equip(this);
        }
        
        public PlayerData RetrieveSerializableData()
        {
            PlayerData pd = new PlayerData()
            {
                Money = _money,
                Hull = _hull,
                Fuel = _fuel,
                Power = _power,
                MaxAchievedDepth = _playerMaxAchievedDepth
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
                Debug.LogException(new InvalidSaveStateException());
                throw new InvalidSaveStateException();
            }

            ResetCharacter();
            foreach (var equipmentElement in data.Equipment)
            {
                Equip(_partList.GetReferencePart(equipmentElement.Id).CreatePart(equipmentElement.Durability));
            }

            _money.Value = data.Money;
            _hull.Value = data.Hull;
            _fuel.Value = data.Fuel;
            _power.Value = data.Power;
            _playerMaxAchievedDepth.Value = data.MaxAchievedDepth;

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
            _playerGridPosition.ValueChanged -= OnPlayerGridPostionChanged;
            _playerDestroyed.Raise();
            if (_player != null)
                Destroy(_player.gameObject);
        }

        #region COROUTINES

        private IEnumerator RestorePlayerAfterSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            _respawnPlayer.Raise();
            _playerRespawned.Raise();
        }

        #endregion

        #region EVENT RESPONSES

        public void OnChangePlayerRadiation(EventArgs args)
        {
            if(args is ChangePlayerRadiationEA cpr)
            {
                _radiation.Value += cpr.RadiationChange;
                _playerRadiationChanged.Raise(new PlayerRadiationChangedEA(_radiation.Value));
            }
            else
            {
                throw new InvalidEventArgsException();
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

                foreach (var addUsableItem in upd.AddUsableItemsChange)
                {
                    _usableItems.Add(addUsableItem);
                    _chooseUsableItem.Raise(new ChooseUsableItemEA(_usableItems[_chosenUsableItemIndex].Item));
                }

                foreach (var removeUsableItem in upd.RemoveUsableItemsChange)
                {
                    _usableItems.Remove(removeUsableItem);
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
                if (item.UsedPower > _power) return; //not enough power
                _power.Value -= item.UsedPower;
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
            if(args is DamagePlayerEA pd)
            {
                if (pd.Damage > 0)
                {
                    _hull.Value -= pd.Damage;
                    _hull.Value = Mathf.Clamp(_hull.Value, 0, _maxHull.Value);
                    _cameraShake.Raise(new CameraShakeEA(_cameraShakeAmplitude));
                    _hitSound.Play();

                    foreach(var perma in pd.PermaDamage)
                    {
                        Part part = _equipment.GetEquippedPart(perma.Key);
                        if(part != null)
                            part.Durability -= perma.Value / 100f;
                    }

                    if (_hull.Value <= 0)
                    {
                        OnKillPlayer();
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

        public void OnKillPlayer()
        {
            foreach (EPartType partType in Enum.GetValues(typeof(EPartType)))
            {
                Part part = _equipment.GetEquippedPart(partType);
                if (part != null)
                    part.Durability -= Random.Range(1, 10)/100f;
            }

            _playerDied.Raise();
            StartCoroutine(RestorePlayerAfterSeconds(5f));
        }

        public void OnPlayerGridPostionChanged(Vector2Int old, Vector2Int newVal)
        {
            int depth = (int)Mathf.Abs(newVal.y * GameRules.Instance.RealDimensionOfTile);  //in meters
            if (depth > _playerMaxAchievedDepth.Value)
                _playerMaxAchievedDepth.Value = depth;
        }
        #endregion

    }
}