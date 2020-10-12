using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Miner.Gameplay;
using TMPro;
using Miner.FX;
using UnityEngine.EventSystems;
using Miner.Management.Events;
using UnityEngine.UI;


namespace Miner.UI
{
    public class InventoryWindowView : Window
    {
        [SerializeField] private Transform _resourceLayout = null;
        [SerializeField] private Transform _equipmentLayout = null;
        [SerializeField] private Transform _itemLayout = null;
        [SerializeField] private Transform _configurablePartsLayout = null;

        [Header("Resources")]
        [SerializeField] private IntReference _playerMaxHull = null;
        [SerializeField] private IntReference _playerHull = null;
        [SerializeField] private FloatReference _playerMaxFuel = null;
        [SerializeField] private FloatReference _playerFuel = null;
        [SerializeField] private FloatReference _playerEffectiveCooling = null;
        [SerializeField] private IntReference _playerEnginePower = null;
        [SerializeField] private FloatReference _playerDrillSharpness = null;
        [SerializeField] private FloatReference _playerFuelUsage = null;
        [SerializeField] private IntReference _playerAvailableCells = null;
        [SerializeField] private IntReference _playerCargoVolume = null;
        [SerializeField] private EquipmentTable _playerEquipment = null;
        [SerializeField] private CargoTable _playerCargo = null;
        [SerializeField] private UsableItemTable _playerItems = null;
        [SerializeField] private InventoryResourceElementDisplay _resourceElementPrefab = null;
        [SerializeField] private InventoryUsableItemElement _usableItemElementPrefab = null;
        [SerializeField] private InventoryEquipmentElement _equipmentElementPrefab = null;
        [SerializeField] private InventoryConfigurablePartElement _configurablePartElementPrefab = null;

        private void LoadItems()
        {
            foreach(var item in _playerItems)
            {
                Instantiate(_usableItemElementPrefab, _itemLayout).Initialize(item);
            }
        }

        private void LoadCargo()
        {
            foreach (var elem in _playerCargo)
            {
                Instantiate(_resourceElementPrefab, _resourceLayout).Initialize(elem);
            }
            
        }

        private void LoadEquipment()
        {
            foreach(EPartType partType in System.Enum.GetValues(typeof(EPartType)))
            {
                LoadPart(_playerEquipment.GetEquippedPart(partType));
            }
        }

        private void LoadPart(Part referencePart)
        {
            if (referencePart != null)
            {
                InventoryEquipmentElement iee = Instantiate(_equipmentElementPrefab, _equipmentLayout);
                iee.Initialize(referencePart);
            }
        }

        private void LoadConfigurablePart(Part configurablePart)
        {
            if (configurablePart != null)
            {
                if (configurablePart.IsConfigurable())
                {
                    InventoryConfigurablePartElement icpe = Instantiate(_configurablePartElementPrefab, _configurablePartsLayout);
                    icpe.Initialize(configurablePart);
                }
            }
        }
        
        private void LoadConfigurableParts()
        {
            foreach(EPartType partType in System.Enum.GetValues(typeof(EPartType)))
            {
                LoadConfigurablePart(_playerEquipment.GetEquippedPart(partType));
            }
        }

        private void UnloadItems()
        {
            foreach (Transform child in _itemLayout.transform)
            {
                Destroy(child.gameObject);
            }
        }

        private void UnloadCargo()
        {
            foreach(Transform child in _resourceLayout.transform)
            {
                Destroy(child.gameObject);
            }
        }

        private void UnloadEquipment()
        {
            foreach(Transform child in _equipmentLayout)
            {
                Destroy(child.gameObject);
            }
        }

        private void UnloadConfigurableParts()
        {
            foreach (Transform child in _configurablePartsLayout)
            {
                Destroy(child.gameObject);
            }
        }

        private void Load()
        {
            _appearingEffect.TriggerAppearing();
            LoadCargo();
            LoadEquipment();
            LoadItems();
            LoadConfigurableParts();
            if (EventSystem.current.alreadySelecting == false)
            {
                if (_configurablePartsLayout.childCount > 0)
                {
                    _firstSelectedObject = _configurablePartsLayout.GetChild(0).GetComponent<Selectable>();
                }
                else if (_resourceLayout.childCount > 0)
                {
                    _firstSelectedObject = _resourceLayout.GetChild(0).GetComponent<Selectable>();
                }
                else if (_itemLayout.childCount > 0)
                {
                    _firstSelectedObject = _itemLayout.GetChild(0).GetComponent<Selectable>();
                }
                else if (_equipmentLayout.childCount > 0)
                {
                    _firstSelectedObject = _equipmentLayout.GetChild(0).GetComponent<Selectable>();
                }
            }
            SelectFirstObject();
        }

        private void Unload()
        {
            UnloadCargo();
            UnloadEquipment();
            UnloadItems();
            UnloadConfigurableParts();
            gameObject.SetActive(false);
        }

        protected override void OnAppearingFinished()
        {
            
        }

        protected override void OnDisappearingFinished()
        {
            Unload();
            base.OnDisappearingFinished();
        }

        protected override void Awake()
        {
            _appearingEffect = GetComponent<IAppearingEffect>();
            _appearingEffect.AppearingFinished += OnAppearingFinished;
            _appearingEffect.DisappearingFinished += OnDisappearingFinished;
            _input.InventoryViewKeyPressed += CloseWindow;
            Load();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Unload();
        }

        protected override void CloseWindow()
        {
            _input.InventoryViewKeyPressed -= CloseWindow;
            _appearingEffect.TriggerDisappearing();
        }
    }
}