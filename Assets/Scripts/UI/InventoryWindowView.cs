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
        [SerializeField] private Transform _itemLayout = null;
        [SerializeField] private TextMeshProUGUI _hullName = null;
        [SerializeField] private TextMeshProUGUI _hullValue = null;
        [SerializeField] private TextMeshProUGUI _fuelTankName = null;
        [SerializeField] private TextMeshProUGUI _fuelTankValue = null;
        [SerializeField] private TextMeshProUGUI _engineName = null;
        [SerializeField] private TextMeshProUGUI _engineValue = null;
        [SerializeField] private TextMeshProUGUI _coolingName = null;
        [SerializeField] private TextMeshProUGUI _coolingValue = null;
        [SerializeField] private TextMeshProUGUI _drillName = null;
        [SerializeField] private TextMeshProUGUI _drillValue = null;
        [SerializeField] private TextMeshProUGUI _batteryName = null;
        [SerializeField] private TextMeshProUGUI _batteryValue = null;
        [SerializeField] private TextMeshProUGUI _cargoName = null;
        [SerializeField] private TextMeshProUGUI _cargoValue = null;
        [SerializeField] private PlayerInputSheet _input = null;

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


        [SerializeField] private UnityEvent _onKeyDown = null;
        [SerializeField] private UnityEvent _onKeyUp = null;

        public void LoadItems()
        {
            foreach(var item in _playerItems)
            {
                Instantiate(_usableItemElementPrefab, _itemLayout).Initialize(item);
            }
        }

        public void LoadCargo()
        {
            foreach (var elem in _playerCargo)
            {
                Instantiate(_resourceElementPrefab, _resourceLayout).Initialize(elem);
            }
        }

        public void LoadEquipment()
        {
            LoadPart(_playerEquipment.Hull, _hullName, _hullValue, _playerHull.Value.ToString() + "/" + _playerMaxHull.Value.ToString());
            LoadPart(_playerEquipment.Engine, _engineName, _engineValue, _playerEnginePower.Value.ToString() + " HP");
            LoadPart(_playerEquipment.FuelTank, _fuelTankName, _fuelTankValue, _playerFuel.Value.ToString("0.0") + "/" + _playerMaxFuel.Value.ToString());
            LoadPart(_playerEquipment.Drill, _drillName, _drillValue, _playerDrillSharpness.Value.ToString());
            LoadPart(_playerEquipment.Cooling, _coolingName, _coolingValue, _playerEffectiveCooling.Value.ToString());
            LoadPart(_playerEquipment.Battery, _batteryName, _batteryValue, _playerAvailableCells.Value.ToString());
            LoadPart(_playerEquipment.Cargo, _cargoName, _cargoValue, _playerCargoVolume.Value.ToString() + " kg");
        }

        private void LoadPart(ReferencePart referencePart, TextMeshProUGUI name, TextMeshProUGUI status, string statusText)
        {
            if (referencePart != null)
            {
                name.transform.parent.gameObject.SetActive(true);
                name.text = referencePart.Name;
                status.text = statusText;
                name.color = Color.Lerp(Color.red, Color.green, 2 * referencePart.Durability - 1);
                status.color = Color.Lerp(Color.red, Color.green, 2 * referencePart.Durability - 1);
            }
            else
            {
                name.transform.parent.gameObject.SetActive(false);
            }
        }
        
        public void UnloadItems()
        {
            foreach (Transform child in _itemLayout.transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void UnloadCargo()
        {
            foreach(Transform child in _resourceLayout.transform)
            {
                Destroy(child.gameObject);
            }
        }

        private void Start()
        {
            _onKeyUp.Invoke();
            _input.InventoryViewKeyPressed += _onKeyDown.Invoke;
            _input.InventoryViewKeyUp += _onKeyUp.Invoke;
        }

        private void OnDestroy()
        {
            _input.InventoryViewKeyPressed -= _onKeyDown.Invoke;
            _input.InventoryViewKeyUp -= _onKeyUp.Invoke;
        }
    }
}