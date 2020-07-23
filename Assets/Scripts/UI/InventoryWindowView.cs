using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Miner.Gameplay;
using TMPro;

namespace Miner.UI
{
    public class InventoryWindowView : MonoBehaviour
    {
        [SerializeField] private Transform _resourceLayout = null;
        [SerializeField] private Transform _equipmentLayout = null;
        [SerializeField] private Transform _itemLayout = null;

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
        [SerializeField] private UsableItemElementDisplay _usableItemElementPrefab = null;
        [SerializeField] private InventoryEquipmentElement _equipmentElementPrefab = null;

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
            LoadPart(_playerEquipment.Hull, _playerHull.Value.ToString() + "/" + _playerMaxHull.Value.ToString());
            LoadPart(_playerEquipment.Engine, (_playerEnginePower.Value / 1000).ToString() + " HP");
            LoadPart(_playerEquipment.FuelTank, _playerFuel.Value.ToString("0.0") + "/" + _playerMaxFuel.Value.ToString());
            LoadPart(_playerEquipment.Drill, _playerDrillSharpness.Value.ToString());
            LoadPart(_playerEquipment.Cooling, _playerEffectiveCooling.Value.ToString());
            LoadPart(_playerEquipment.Battery, _playerAvailableCells.Value.ToString());
            LoadPart(_playerEquipment.Cargo, _playerCargoVolume.Value.ToString() + " kg");
        }

        private void LoadPart(ReferencePart referencePart, string statusText)
        {
            if (referencePart != null)
            {
                InventoryEquipmentElement iee = Instantiate(_equipmentElementPrefab, _equipmentLayout);
                iee.Initialize(referencePart, statusText);
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

        public void Open()
        {
            LoadCargo();
            LoadEquipment();
            LoadItems();
            gameObject.SetActive(true);
        }

        public void Close()
        {
            UnloadCargo();
            UnloadEquipment();
            UnloadItems();
            gameObject.SetActive(false);
        }
    }
}