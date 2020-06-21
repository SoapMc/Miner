using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Miner.Gameplay;

namespace Miner.UI
{
    public class UsableItemOffer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name = null;
        [SerializeField] private TextMeshProUGUI _price = null;
        private UsableItem _usableItem = null;
        private ServiceWindow _serviceWindow = null;
        private int _amount;

        public void Initialize(ServiceWindow sw, UsableItem item, int playerAmount)
        {
            _serviceWindow = sw;
            _usableItem = item;
            _amount = playerAmount;
            RefreshText(playerAmount);
        }

        public void Buy()
        {
            int addAmount = _serviceWindow.BuyItem(_usableItem);
            _amount += addAmount;
            RefreshText(_amount);
        }

        public void RefreshOffer(IntReference playerMoney)
        {
            if(playerMoney >= _usableItem.Cost)
            {
                _name.color = Color.green;
            }
            else
            {
                _name.color = Color.red;
            }
        }

        private void RefreshText(int amount)
        {
            _name.text = _usableItem.Name + "<size=14> (onboard: " + amount.ToString() + ")</size>";
            _price.text = _usableItem.Cost.ToString() + " $";
        }
    }
}