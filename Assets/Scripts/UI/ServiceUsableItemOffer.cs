using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Miner.Gameplay;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Miner.Management.Events;
using Miner.FX;

namespace Miner.UI
{
    public class ServiceUsableItemOffer : Button, IDescriptableElement
    {
        [SerializeField] private GameEvent _descriptOffer = null;
        [SerializeField] private Image _icon = null;
        [SerializeField] private UIStandardStyleSheet _unavailableStyleSheet = null;
        [SerializeField] private UIStandardStyleSheet _availableStyleSheet = null;
        private ServiceWindow _serviceWindow = null;
        private UsableItem _element = null;
        private RectTransform _rectTransform = null;
        private int _playerAmount = 0;
        private DescriptOfferEA.EState _offerState = DescriptOfferEA.EState.Available;

        public string Name => _element.Name;

        public string Description
        {
            get
            {
                return "Onboard: " + _playerAmount.ToString() + "\nCost: " + _element.Cost.ToString() + " $\n" + _element.ShortDescription;
            }
        }

        public void Initialize(ServiceWindow serviceWindow, UsableItem element, int playerAmount)
        {
            _serviceWindow = serviceWindow;
            _element = element;
            _icon.sprite = _element.Sprite;
            _playerAmount = playerAmount;
        }

        public void UpdateOffer(int playerMoney)
        {
            if (playerMoney >= _element.Cost)
            {
                _offerState = DescriptOfferEA.EState.Available;
                _icon.color = _availableStyleSheet.ForegroundColor;
            }
            else
            {
                _offerState = DescriptOfferEA.EState.Unavailable;
                _icon.color = _unavailableStyleSheet.ForegroundColor;
            }
        }

        public void Buy()
        {
            int addAmount = _serviceWindow.BuyItem(_element);
            _playerAmount += addAmount;
            if(addAmount > 0)
                OnSelect(null);
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            _descriptOffer.Raise(new DescriptOfferEA(this, _rectTransform, _offerState));
        }

        protected override void Awake()
        {
            base.Awake();
            _rectTransform = GetComponent<RectTransform>();
        }
    }
}