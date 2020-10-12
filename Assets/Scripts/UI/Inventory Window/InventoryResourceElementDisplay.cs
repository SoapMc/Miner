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
    public class InventoryResourceElementDisplay : Selectable, IDescriptableElement
    {
        [SerializeField] private GameEvent _descriptInventoryElement = null;
        [SerializeField] private Image _icon = null;
        [SerializeField] private TextMeshProUGUI _name = null;
        [SerializeField] private TextMeshProUGUI _amount = null;
        private CargoTable.Element _element;

        public string Name => _element.Type.Name;

        public string Description
        {
            get
            {
                return "Amount: " + _element.Amount.ToString() + "\nMass: " + (_element.Amount * _element.Type.Mass).ToString() + " kg";
            }
        }

        public void Initialize(CargoTable.Element element)
        {
            _element = element;
            _icon.sprite = _element.Type.Icon;
            _name.text = _element.Type.Name;
            _amount.text = _element.Amount.ToString();
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            _descriptInventoryElement.Raise(new DescriptElementEA(this, GetComponent<RectTransform>()));
        }

        protected override void Awake()
        {
            base.Awake();
        }
    }
}