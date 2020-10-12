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
using System;

namespace Miner.UI
{
    public class ServiceWindow : Window
    {
        [SerializeField] private GameEvent _updatePlayerData = null;
        [SerializeField] private GameEvent _playerRepaired = null;

        [SerializeField] private IntReference _playerMoney = null;
        [SerializeField] private UsableItemList _usableItems = null;
        [SerializeField] private UsableItemTable _playerUsableItems = null;
        [SerializeField] private EquipmentTable _playerEquipment = null;
        [SerializeField] private Transform _shopLayout = null;
        [SerializeField] private ServiceUsableItemOffer _usableItemOfferPrefab = null;
        [SerializeField] private Selectable _firstSelectedElement = null;
        [SerializeField] private TextMeshProUGUI _repairPriceText = null;
        [SerializeField] private IntReference _playerHull = null;
        [SerializeField] private IntReference _playerMaxHull = null;
        [SerializeField] private SoundEffect _buySound = null;
        [SerializeField] private SoundEffect _repairSound = null;

        private int _repairCostPerPoint = 25;
        private List<ServiceUsableItemOffer> _offers;

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
                RefreshOffers();
                return 1;
            }
            return 0;
        }

        private void RefreshOffers()
        {
            for (int i = 0; i < _offers.Count; ++i)
            {
                _offers[i].UpdateOffer(_playerMoney.Value);
            }
        }

        private void LoadUsableItemOffers()
        {
            _offers = new List<ServiceUsableItemOffer>(_usableItems.Count);
            foreach(var item in _usableItems)
            {
                ServiceUsableItemOffer uio = Instantiate(_usableItemOfferPrefab, _shopLayout);
                int playerAmount = 0;
                UsableItemTable.Element elem = _playerUsableItems.FirstOrDefault(x => x.Item.Id == item.Id);
                if (elem != null)
                    playerAmount = elem.Amount;

                uio.Initialize(this, item, playerAmount);
                _offers.Add(uio);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            LoadUsableItemOffers();
            CalculateRepairCost();
            RefreshOffers();
        }
    }
}