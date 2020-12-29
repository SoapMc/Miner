using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Gameplay;
using UnityEngine.UI;
using TMPro;

namespace Miner.UI
{
    public class ChosenUsableItem : MonoBehaviour
    {
        [SerializeField] private Image _image = null;
        [SerializeField] private TextMeshProUGUI _amountText = null;

        private UsableItem _item = null;
        private int _amount = 0;

        public UsableItem Item => _item;
        public int Amount => _amount;

        public void Initialize(UsableItem item, int amount)
        {
            _item = item;
            _amount = amount;
            _image.sprite = item.Sprite;
            _amountText.text = _amount.ToString();
        }

        public void UpdateElement(int newAmount)
        {
            _amount = newAmount;
            _amount = Mathf.Clamp(_amount, 0, int.MaxValue);
            _amountText.text = _amount.ToString();
        }
    }
}