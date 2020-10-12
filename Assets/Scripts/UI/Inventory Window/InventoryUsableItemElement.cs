using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Miner.Gameplay;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Miner.Management.Events;

namespace Miner.UI
{
    public class InventoryUsableItemElement : Selectable, IDescriptableElement
    {
        [SerializeField] private GameEvent _descriptInventoryElement = null;
        [SerializeField] private Image _icon = null;
        [SerializeField] private TextMeshProUGUI _amount = null;
        private UsableItemTable.Element _element = null;
        private RectTransform _rectTransform = null;

        public string Name => _element.Item.Name;

        public string Description
        {
            get
            {
                return "Amount: " + _element.Amount.ToString() + "\n" + _element.Item.ShortDescription;
            }
        }

        public void Initialize(UsableItemTable.Element element)
        {
            _element = element;
            _icon.sprite = _element.Item.Sprite;
            _amount.text = _element.Amount.ToString();
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            _descriptInventoryElement.Raise(new DescriptElementEA(this, _rectTransform));
        }

        protected override void Awake()
        {
            base.Awake();
            _rectTransform = GetComponent<RectTransform>();
        }
    }
}