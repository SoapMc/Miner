using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Miner.Management.Events;
using Miner.Gameplay;
using System;

namespace Miner.UI
{
    public class InventoryPowerFlowFactorElement : MonoBehaviour, IDescriptableElement, ISelectHandler
    {
        [SerializeField] private GameEvent _descriptInventoryElement = null;
        [SerializeField] private TextMeshProUGUI _sourceName = null;
        private PowerFlowFactor _powerFlowFactor = null;
        private string _name;
        private string _description;

        public string Name => _name;
        public string Description => _description;

        public void Initialize(PowerFlowFactor factor)
        {
            _name = factor.Name;
            _description = factor.Description;
            _sourceName.text = factor.Name;
            _powerFlowFactor = factor;
        }

        public void OnSelect(BaseEventData eventData)
        {
            _descriptInventoryElement.Raise(new DescriptElementEA(this, GetComponent<RectTransform>()));
        }
    }
}