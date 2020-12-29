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
using UnityEngine.InputSystem;

namespace Miner.UI
{
    public class InventoryWindowView : Window
    {
        [SerializeField] private Transform _resourceLayout = null;
        [SerializeField] private Transform _equipmentLayout = null;
        [SerializeField] private Transform _itemLayout = null;
        [SerializeField] private Transform _configurablePartsLayout = null;

        [Header("Resources")]
        [SerializeField] private IntReference _playerCargoVolume = null;
        [SerializeField] private EquipmentTable _playerEquipment = null;
        [SerializeField] private CargoTable _playerCargo = null;
        [SerializeField] private UsableItemTable _playerItems = null;
        [SerializeField] private InventoryResourceElementDisplay _resourceElementPrefab = null;
        [SerializeField] private InventoryUsableItemElement _usableItemElementPrefab = null;
        [SerializeField] private InventoryEquipmentElement _equipmentElementPrefab = null;
        [SerializeField] private InventoryConfigurableModuleElement _configurableModuleElementPrefab = null;
        [SerializeField] private InventoryPowerFlowFactorElement _powerFlowFactorElementPrefab = null;
        [SerializeField] private PowerFlowFactorList _playerPowerFlowFactorList = null;

        public void SelectObjectAfterRemovingResourceFromCargo(Selectable destroyedSelectable)
        {
            Selectable s = null;
            if(_resourceLayout.childCount > 0)
            {
                foreach(Transform child in _resourceLayout)
                {
                    if (child.GetComponent<Selectable>() != destroyedSelectable)
                    {
                        s = child.GetComponent<Selectable>();
                        break;
                    }
                }
            }
            if (s == null)
            {
                foreach (Transform child in _itemLayout)
                {
                    s = child.GetComponent<Selectable>();
                    if (s != destroyedSelectable)
                        break;
                }
            }
            if (s == null && _equipmentLayout.childCount > 0)
            {
                foreach (Transform child in _equipmentLayout)
                {
                    s = child.GetComponent<Selectable>();
                    if (s != destroyedSelectable)
                        break;
                }
            }

            if (s != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(s.gameObject);
                s.OnSelect(null);
            }
        }

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
                Instantiate(_resourceElementPrefab, _resourceLayout).Initialize(elem, this);
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

        private void LoadConfigurableModule(Part part)
        {
            if (part != null)
            {
                for(int i = 0; i < part.ConfigurableModules.Count; ++i)
                {
                    InventoryConfigurableModuleElement icpe = Instantiate(_configurableModuleElementPrefab, _configurablePartsLayout);
                    icpe.Initialize(part, part.ConfigurableModules[i]);
                }
            }
        }
        
        private void LoadConfigurableModules()
        {
            foreach(EPartType partType in System.Enum.GetValues(typeof(EPartType)))
            {
                LoadConfigurableModule(_playerEquipment.GetEquippedPart(partType));
            }
        }

        private void LoadPowerFactors()
        {
            foreach(PowerFlowFactor factor in _playerPowerFlowFactorList.Values)
            {
                InventoryPowerFlowFactorElement ipffe = Instantiate(_powerFlowFactorElementPrefab, _configurablePartsLayout);
                ipffe.Initialize(factor);
            }
        }

        private void UnloadAllElements()
        {
            foreach (Transform child in _itemLayout.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (Transform child in _resourceLayout.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (Transform child in _equipmentLayout)
            {
                Destroy(child.gameObject);
            }

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
            LoadConfigurableModules();
            LoadPowerFactors();

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
            UnloadAllElements();
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
            _controls = new Management.Controls();
            _appearingEffect = GetComponent<IAppearingEffect>();
            _appearingEffect.AppearingFinished += OnAppearingFinished;
            _appearingEffect.DisappearingFinished += OnDisappearingFinished;
            _controls.Player.Inventory.performed += CloseWindow;
            _controls.Enable();
            Load();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Unload();
        }

        protected override void CloseWindow(InputAction.CallbackContext context)
        {
            _controls.Player.Inventory.performed -= CloseWindow;
            _appearingEffect.TriggerDisappearing();
        }
    }
}