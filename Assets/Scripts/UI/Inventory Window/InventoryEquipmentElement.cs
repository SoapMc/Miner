using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Miner.Gameplay;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Miner.Management.Events;

namespace Miner.UI
{
    public class InventoryEquipmentElement : Selectable, IDescriptableElement
    {
        [SerializeField] private GameEvent _descriptInventoryElement = null;
        [SerializeField] private TextMeshProUGUI _durabilityText = null;
        [SerializeField] private TextMeshProUGUI _shortNameText = null;
        private Part _part = null;
        private RectTransform _rectTransform = null;

        public string Name => _part.Name;
        public string Description => string.Join("\n",_part.GetPerformanceDescription());

        public void Initialize(Part part)
        {
            _part = part ?? throw new ArgumentException("Part in InventoryEquipmentElement cannot be null");
            _durabilityText.color = Color.Lerp(Color.red, Color.green, 2 * part.Durability - 1);
            _shortNameText.color = _durabilityText.color;
            _durabilityText.text = ((int)(part.Durability * 100)).ToString();
            _shortNameText.text = part.Type.ToString().Substring(0, 1).ToUpper();
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