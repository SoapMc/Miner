using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Miner.Gameplay;
using UnityEngine.UI;
using Miner.Management.Events;
using UnityEngine.EventSystems;

namespace Miner.UI
{
    public class InventoryResourceElementDisplay : Button, IDescriptableElement
    {
        [SerializeField] private GameEvent _tryChangePlayerResources = null;
        [SerializeField] private GameEvent _descriptInventoryElement = null;
        [SerializeField] private Image _icon = null;
        [SerializeField] private TextMeshProUGUI _name = null;
        [SerializeField] private TextMeshProUGUI _amount = null;
        private CargoTable.Element _element;
        private InventoryWindowView _inventoryWindow = null;

        public string Name => _element.Type.Name;

        public string Description
        {
            get
            {
                return "Amount: " + _element.Amount.ToString() + "\nMass: " + (_element.Amount * _element.Type.Mass).ToString() + " kg\nPress CONFIRM button to throw away";
            }
        }

        public void Initialize(CargoTable.Element element, InventoryWindowView inventoryWindow)
        {
            _element = element;
            _icon.sprite = _element.Type.Icon;
            _name.text = _element.Type.Name;
            _amount.text = _element.Amount.ToString();
            _inventoryWindow = inventoryWindow;
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            _descriptInventoryElement.Raise(new DescriptElementEA(this, GetComponent<RectTransform>()));
        }

        public void RemoveSingleResourceFromCargo()
        {
            _tryChangePlayerResources.Raise(new TryChangeResourcesInPlayerCargoEA() { ResourcesToRemove = new List<CargoTable.Element>() { new CargoTable.Element() { Type = _element.Type, Amount = 1 } } });
            _amount.text = _element.Amount.ToString();
            if (_element.Amount <= 0)
            {
                Destroy(gameObject);
                _inventoryWindow.SelectObjectAfterRemovingResourceFromCargo(GetComponent<Selectable>());
                
            }
        }

        protected override void Awake()
        {
            base.Awake();
        }
    }
}