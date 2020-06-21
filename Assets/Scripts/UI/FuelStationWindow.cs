using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Miner.Gameplay;
using Miner.Management.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Miner.UI
{
    public class FuelStationWindow : MonoBehaviour
    {
        [SerializeField] private IntReference _fuelSupply = null;
        [SerializeField] private CargoTable _playerCargo = null;
        [SerializeField] private FloatReference _playerFuel = null;
        [SerializeField] private FloatReference _playerMaxFuel = null;
        [SerializeField] private IntReference _playerMoney = null;

        [SerializeField] private TextMeshProUGUI _price = null;
        [SerializeField] private TextMeshProUGUI _fuelSupplyText = null;
        [SerializeField] private TextMeshProUGUI _fullRefillCostText = null;
        [SerializeField] private Selectable _firstSelectedObject = null;

        [Header("Events")]
        [SerializeField] private GameEvent _updatePlayerData = null;
        [SerializeField] private GameEvent _closeWindow = null;

        private int _fuelPrice;
        private int _fullRefillCost;

        public void RefillFull()
        {
            if (_playerMoney >= _fullRefillCost)
            {
                _updatePlayerData.Raise(new UpdatePlayerDataEA() {
                    MoneyChange = -_fullRefillCost,
                    FuelChange = _playerMaxFuel.Value - _playerFuel.Value
                });
            }
            else
            {
                float boughtFuel = _playerMoney.Value / (float)_fuelPrice;
                _updatePlayerData.Raise(new UpdatePlayerDataEA()
                {
                    MoneyChange = Mathf.CeilToInt(-boughtFuel * _fuelPrice),
                    FuelChange = boughtFuel
                });
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
                    _updatePlayerData.Raise(new UpdatePlayerDataEA()
                    {
                        MoneyChange = -refillCost,
                        FuelChange = _playerMaxFuel.Value * 0.1f
                    });
                }
                else
                {
                    _updatePlayerData.Raise(new UpdatePlayerDataEA()
                    {
                        MoneyChange = -_playerMoney,
                        FuelChange = (_playerMoney / (float)_fuelPrice)
                    });
                }
            }
            else
            {
                int refillCost = Mathf.CeilToInt(missingFuel * _fuelPrice);
                if (_playerMoney >= refillCost)
                {
                    _updatePlayerData.Raise(new UpdatePlayerDataEA()
                    {
                        MoneyChange = -refillCost,
                        FuelChange = _playerMaxFuel.Value - _playerFuel.Value
                    });
                }
                else
                {
                    _updatePlayerData.Raise(new UpdatePlayerDataEA()
                    {
                        MoneyChange = -_playerMoney,
                        FuelChange = (_playerMoney / (float)_fuelPrice)
                    });
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
                    _updatePlayerData.Raise(new UpdatePlayerDataEA()
                    {
                        MoneyChange = -refillCost,
                        FuelChange = _playerMaxFuel.Value * 0.01f
                    });
                }
                else
                {
                    _updatePlayerData.Raise(new UpdatePlayerDataEA()
                    {
                        MoneyChange = -_playerMoney,
                        FuelChange = (_playerMoney / (float)_fuelPrice)
                    });
                }
            }
            else
            {
                int refillCost = Mathf.CeilToInt((_playerMaxFuel.Value - _playerFuel.Value) * _fuelPrice);
                if (_playerMoney >= refillCost)
                {
                    _updatePlayerData.Raise(new UpdatePlayerDataEA()
                    {
                        MoneyChange = -refillCost,
                        FuelChange = _playerMaxFuel.Value - _playerFuel.Value
                    });
                }
                else
                {
                    _updatePlayerData.Raise(new UpdatePlayerDataEA()
                    {
                        MoneyChange = -_playerMoney,
                        FuelChange = (_playerMoney / (float)_fuelPrice)
                    });
                }
            }
            CalculateFullRefillCost();
        }

        public void RefreshUI()
        {
            CalculateFuelPrice();
            CalculateFullRefillCost();
            _fuelSupplyText.text = _fuelSupply.Value.ToString() + " l";
        }

        public void CloseWindow()
        {
            _closeWindow.Raise(new CloseWindowEA(gameObject));
        }

        private void CalculateFuelPrice()
        {
            _fuelPrice = 10 - _fuelSupply / 1000;
            _fuelPrice = Mathf.Clamp(_fuelPrice, 1, 10);
            _price.text = _fuelPrice.ToString() + " $/ l";
        }

        private void CalculateFullRefillCost()
        {
            _fullRefillCost = Mathf.CeilToInt((_playerMaxFuel.Value - _playerFuel.Value) * _fuelPrice);
            _fullRefillCostText.text = _fullRefillCost.ToString() + " $";
        }

        private void Awake()
        {
            Time.timeScale = 0f;
            RefreshUI();
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_firstSelectedObject.gameObject);
            _firstSelectedObject.OnSelect(null);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                CloseWindow();
        }

        private void OnDestroy()
        {
            Time.timeScale = 1f;
        }
    }
}