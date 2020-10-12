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
    public class InventoryConfigurablePartElement : MonoBehaviour, IDescriptableElement, ISelectHandler
    {
        [SerializeField] private GameEvent _descriptInventoryElement = null;
        [SerializeField] private Image _icon = null;
        [SerializeField] private TextMeshProUGUI _sourcePartName = null;
        [SerializeField] private TextMeshProUGUI _effectText = null;
        [SerializeField] private Toggle _toggle = null;
        private Part _part = null;

        public string Name
        {
            get
            {
                if (_part.IsConfigurable())
                    return _part.GetConfigurableComponent().ConfigurationName;
                else
                    return string.Empty;
            }
        }

        public string Description
        {
            get
            {
                if (_part.IsConfigurable())
                    return _part.GetConfigurableComponent().ConfigurationDescription;
                else
                    return string.Empty;
            }
        }

        public void Initialize(Part part)
        {
            if (part.IsConfigurable())
            {
                _part = part;
                _icon.sprite = _part.Sprite;
                _sourcePartName.text = _part.Name;
                _effectText.text = _part.GetConfigurableComponent().ConfigurationName;
                bool partEnabled = _part.Enabled;
                _toggle.isOn = partEnabled;
            }
            else
                throw new ArgumentException("The given part isn't configurable! [in InventoryConfigurablePartElement.Initialize]");

        }

        public void OnSelect(BaseEventData eventData)
        {
            _descriptInventoryElement.Raise(new DescriptElementEA(this, GetComponent<RectTransform>()));
        }

        public void OnToggleConfiguration()
        {
            bool toggled = !_toggle.isOn;
            _toggle.isOn = toggled;
            _part.Enabled = toggled;
        }
    }
}