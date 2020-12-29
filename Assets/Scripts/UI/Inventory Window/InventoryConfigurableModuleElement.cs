using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Miner.Management.Events;
using Miner.Gameplay;
using System;
using Miner.Management;

namespace Miner.UI
{
    public class InventoryConfigurableModuleElement : MonoBehaviour, IDescriptableElement, ISelectHandler
    {
        [SerializeField] private GameEvent _descriptInventoryElement = null;
        [SerializeField] private Image _icon = null;
        [SerializeField] private TextMeshProUGUI _sourcePartName = null;
        [SerializeField] private TextMeshProUGUI _effectText = null;
        [SerializeField] private Toggle _toggle = null;
        private ConfigurableModule _configurableModule = null;
        private Part _sourcePart = null;

        public string Name => _configurableModule.Name;
        public string Description => _configurableModule.Description;

        public void Initialize(Part sourcePart, ConfigurableModule configurableModule) 
        {
            if (sourcePart == null || configurableModule == null)
            {
                GameManager.Instance.Log.WriteException(new ArgumentException());
                return;
            }
            _sourcePart = sourcePart;
            if (configurableModule != null)
            {
                _configurableModule = configurableModule;
                _icon.sprite = sourcePart.Sprite;
                _sourcePartName.text = sourcePart.Name;
                _effectText.text = _configurableModule.Name;
                bool partEnabled = _configurableModule.Enabled;
                _toggle.isOn = partEnabled;
            } 
        }

        public void OnSelect(BaseEventData eventData)
        {
            _descriptInventoryElement.Raise(new DescriptElementEA(this, GetComponent<RectTransform>()));
        }

        public void OnToggleConfiguration()
        {
            bool toggled = !_toggle.isOn;
            _toggle.isOn = toggled;
            if (toggled == true)
                _sourcePart.EnableConfigurableModule(_configurableModule);
            else
                _sourcePart.DisableConfigurableModule(_configurableModule);
        }
    }
}