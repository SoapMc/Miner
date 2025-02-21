﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Miner.Gameplay;
using Miner.Management.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Miner.FX;

namespace Miner.UI
{
    public class FuelStationWindow : Window
    {
        [SerializeField] private FloatReference _fuelSupply = null;
        [SerializeField] private FloatReference _playerFuel = null;
        [SerializeField] private FloatReference _playerMaxFuel = null;
        [SerializeField] private IntReference _playerMoney = null;
        [SerializeField] private TextMeshProUGUI _price = null;
        [SerializeField] private TextMeshProUGUI _fuelSupplyText = null;
        [SerializeField] private TextMeshProUGUI _fullRefillCostText = null;
        
        [Header("Events")]
        [SerializeField] private GameEvent _updatePlayerData = null;

        private int _fuelPrice;
        private int _fullRefillCost;

        public void RefillFull()
        {
            if (_playerMoney >= _fullRefillCost)
            {
                SellFuelToPlayer(_playerMaxFuel.Value - _playerFuel.Value, _fullRefillCost);
            }
            else
            {
                float boughtFuel = _playerMoney.Value / (float)_fuelPrice;
                _fuelSupply.Value -= boughtFuel;
                SellFuelToPlayer(boughtFuel, Mathf.CeilToInt(-boughtFuel * _fuelPrice));
            }
            CalculateFullRefillCost();
        }

        public void Refill_10percent()
        {
            float missingFuel = _playerMaxFuel.Value - _playerFuel.Value;

            if (missingFuel >= (_playerMaxFuel.Value * 0.1f))
            {
                int refillCost = Mathf.CeilToInt(_playerMaxFuel.Value * 0.1f * _fuelPrice);
                if (_playerMoney >= refillCost)
                {
                    SellFuelToPlayer(_playerMaxFuel.Value * 0.1f, refillCost);
                }
                else
                {
                    SellFuelToPlayer(_playerMoney / (float)_fuelPrice, _playerMoney);
                }
            }
            else
            {
                int refillCost = Mathf.CeilToInt(missingFuel * _fuelPrice);
                if (_playerMoney >= refillCost)
                {
                    SellFuelToPlayer(_playerMaxFuel.Value - _playerFuel.Value, refillCost);
                }
                else
                {
                    SellFuelToPlayer(_playerMoney / (float)_fuelPrice, _playerMoney);
                }
            }
            CalculateFullRefillCost();
        }

        public void Refill_1percent()
        {
            float missingFuel = _playerMaxFuel.Value - _playerFuel.Value;

            if (missingFuel >= _playerMaxFuel.Value * 0.01f)
            {
                int refillCost = Mathf.CeilToInt(_playerMaxFuel.Value * 0.01f * _fuelPrice);
                if (_playerMoney >= refillCost)
                {
                    SellFuelToPlayer(_playerMaxFuel.Value * 0.01f, refillCost);
                }
                else
                {
                    SellFuelToPlayer(_playerMoney / (float)_fuelPrice, _playerMoney);
                }
            }
            else
            {
                int refillCost = Mathf.CeilToInt((_playerMaxFuel.Value - _playerFuel.Value) * _fuelPrice);
                if (_playerMoney >= refillCost)
                {
                    SellFuelToPlayer(_playerMaxFuel.Value - _playerFuel.Value, refillCost);
                }
                else
                {
                    SellFuelToPlayer(_playerMoney / (float)_fuelPrice, _playerMoney.Value);
                }
            }
            
        }

        private void SellFuelToPlayer(float amount, int price)
        {
            if (amount == 0) return;
            _updatePlayerData.Raise(new UpdatePlayerDataEA()
            {
                MoneyChange = -price,
                FuelChange = amount
            });
            _fuelSupply.Value -= amount;
            RefreshUI();
        }

        public void RefreshUI()
        {
            CalculateFuelPrice();
            CalculateFullRefillCost();
            _fuelSupplyText.text = ((int)_fuelSupply).ToString() + " L";
            _price.text = _fuelPrice.ToString() + " $/ L";
            _fullRefillCostText.text = _fullRefillCost.ToString() + " $";
        }

        private void CalculateFuelPrice()
        {
            _fuelPrice = 10 - (int)_fuelSupply / 1000;
            _fuelPrice = Mathf.Clamp(_fuelPrice, 1, 10);
        }

        private void CalculateFullRefillCost()
        {
            _fullRefillCost = Mathf.CeilToInt((_playerMaxFuel.Value - _playerFuel.Value) * _fuelPrice);
        }

        private void Start()
        {
            RefreshUI();
        }
    }
}