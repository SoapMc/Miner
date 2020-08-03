using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Gameplay;
using Miner.Management.Events;
using TMPro;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using Miner.FX;

namespace Miner.UI
{
    public class ServiceWindow : MonoBehaviour
    {
        [SerializeField] private GameEvent _updatePlayerData = null;
        [SerializeField] private GameEvent _playerRepaired = null;

        [SerializeField] private IntReference _playerMoney = null;
        [SerializeField] private UsableItemList _usableItems = null;
        [SerializeField] private UsableItemTable _playerUsableItems = null;
        [SerializeField] private EquipmentTable _playerEquipment = null;
        [SerializeField] private GameEvent _closeWindow = null;
        [SerializeField] private Transform _performanceLayout = null;
        [SerializeField] private Transform _shopLayout = null;
        [SerializeField] private TextMeshProUGUI _preformanceInfoPrefab = null;
        [SerializeField] private UsableItemOffer _usableItemOfferPrefab = null;
        [SerializeField] private Selectable _firstSelectedElement = null;
        [SerializeField] private TextMeshProUGUI _repairPriceText = null;
        [SerializeField] private IntReference _playerHull = null;
        [SerializeField] private IntReference _playerMaxHull = null;
        [SerializeField] private PlayerInputSheet _input = null;
        [SerializeField] private SoundEffect _buySound = null;
        [SerializeField] private SoundEffect _repairSound = null;

        private int _repairCostPerPoint = 25;
        private List<UsableItemOffer> _offers;

        public void Repair()
        {
            if (_playerMaxHull.Value - _playerHull.Value <= 0) return;
            int repairCost = (_playerMaxHull.Value - _playerHull.Value) * _repairCostPerPoint;
            if(repairCost <= _playerMoney.Value)
            {
                _updatePlayerData.Raise(new UpdatePlayerDataEA() { MoneyChange = -repairCost });
                _playerRepaired.Raise(new PlayerRepairedEA(_playerMaxHull.Value - _playerHull.Value));
            }
            else
            {
                _updatePlayerData.Raise(new UpdatePlayerDataEA() { MoneyChange = -_playerMoney });
                _playerRepaired.Raise(new PlayerRepairedEA(Mathf.CeilToInt(_playerMoney.Value / (float)_repairCostPerPoint)));
            }
            _repairSound.Play();
            CalculateRepairCost();
        }

        public int CalculateRepairCost()
        {
            int repairCost = (_playerMaxHull.Value - _playerHull.Value) * _repairCostPerPoint;
            _repairPriceText.text = repairCost.ToString() + " $";
            return repairCost;
        }

        public int BuyItem(UsableItem item)
        {
            if(_playerMoney >= item.Cost)
            {
                UpdatePlayerDataEA upd = new UpdatePlayerDataEA();
                upd.AddUsableItemsChange.Add(new UsableItemTable.Element() { Item = item, Amount = 1 });
                upd.MoneyChange = -item.Cost;
                _updatePlayerData.Raise(upd);
                _buySound.Play();
                return 1;
            }
            return 0;
        }

        private void RefreshOffers()
        {
            foreach(var offer in _offers)
            {
                offer.RefreshOffer(_playerMoney);
            }
        }

        private void CloseWindow()
        {
            Time.timeScale = 1f;
            _closeWindow.Raise(new CloseWindowEA(gameObject));
            _input.CancelKeyPressed -= CloseWindow;
        }

        private void LoadUsableItemOffers()
        {
            _offers = new List<UsableItemOffer>(_usableItems.Count);
            foreach(var item in _usableItems)
            {
                UsableItemOffer uio = Instantiate(_usableItemOfferPrefab, _shopLayout);
                int playerAmount = 0;
                UsableItemTable.Element elem = _playerUsableItems.FirstOrDefault(x => x.Item.Id == item.Id);
                if (elem != null)
                    playerAmount = elem.Amount;

                uio.Initialize(this, item, playerAmount);
                uio.RefreshOffer(_playerMoney);
                _offers.Add(uio);
            }
        }


        private void LoadPerformanceInfo(ReferencePart part, string title)
        {
            if(part != null)
            {
                TextMeshProUGUI info = Instantiate(_preformanceInfoPrefab, _performanceLayout);
                StringBuilder sb = new StringBuilder(100);
                string[] performanceInfo = part.GetPerformanceDescription();
                sb.Append(title + "\n");
                for(int i = 0; i < performanceInfo.Length; ++i)
                {
                    sb.Append(performanceInfo[i] + "\n");
                }
                info.text = sb.ToString();
            }
        }

        private void LoadPerformanceInfos()
        {
            LoadPerformanceInfo(_playerEquipment.Hull, "<size=24>Hull</size>");
            LoadPerformanceInfo(_playerEquipment.FuelTank, "<size=24>Fuel Tank</size>");
            LoadPerformanceInfo(_playerEquipment.Engine, "<size=24>Engine</size>");
            LoadPerformanceInfo(_playerEquipment.Drill, "<size=24>Drill</size>");
            LoadPerformanceInfo(_playerEquipment.Cooling, "<size=24>Cooling</size>");
            LoadPerformanceInfo(_playerEquipment.Cargo, "<size=24>Cargo</size>");
            LoadPerformanceInfo(_playerEquipment.Battery, "<size=24>Battery</size>");
        }

        public void SetFocus(Selectable selectable)
        {
            selectable.Select();
            
        }

        private void Awake()
        {
            LoadPerformanceInfos();
            LoadUsableItemOffers();
            CalculateRepairCost();
        }

        private void Start()
        {
            Time.timeScale = 0f;
            _firstSelectedElement.Select();
            _input.CancelKeyPressed += CloseWindow;
        }
    }
}