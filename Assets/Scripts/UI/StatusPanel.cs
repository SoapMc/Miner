﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Miner.Management.Events;
using Miner.Management.Exceptions;

namespace Miner.UI
{
    public class StatusPanel : MonoBehaviour
    {
        [SerializeField] private Image _engineIcon = null;
        [SerializeField] private Image _temperatureIcon = null;
        [SerializeField] private Image _fuelIcon = null;
        [SerializeField] private Image _cargoIcon = null;
        [SerializeField] private Image _drillIcon = null;
        [SerializeField] private Image _batteryIcon = null;
        [SerializeField] private float _blinkPeriod = 0.3f;

        private Dictionary<TriggerStatusPanelEA.ESymbol, Coroutine> _coroutines = new Dictionary<TriggerStatusPanelEA.ESymbol, Coroutine>();

        private IEnumerator TriggerIcon(Image icon, float duration, TriggerStatusPanelEA.EMode mode)
        {
            float elapsedTime = 0f;
            if (duration > 0)
            {
                while (elapsedTime < duration)
                {
                    icon.gameObject.SetActive(!icon.gameObject.activeSelf);
                    yield return new WaitForSeconds(_blinkPeriod);
                    elapsedTime += _blinkPeriod;
                }
            }
            else
            {
                while(true)
                {
                    icon.gameObject.SetActive(!icon.gameObject.activeSelf);
                    yield return new WaitForSeconds(_blinkPeriod);
                }
            }
            icon.gameObject.SetActive(false);
        }

        private Image GetImage(TriggerStatusPanelEA.ESymbol symbol)
        {
            switch(symbol)
            {
                case TriggerStatusPanelEA.ESymbol.Engine:
                    return _engineIcon;
                case TriggerStatusPanelEA.ESymbol.Temperature:
                    return _temperatureIcon;
                case TriggerStatusPanelEA.ESymbol.Fuel:
                    return _fuelIcon;
                case TriggerStatusPanelEA.ESymbol.Cargo:
                    return _cargoIcon;
                case TriggerStatusPanelEA.ESymbol.Drill:
                    return _drillIcon;
                case TriggerStatusPanelEA.ESymbol.Battery:
                    return _batteryIcon;
                default:
                    Debug.LogError("Cannot find status icon image (" + symbol.ToString() + ")");
                    return null;
            }
        }

        private void DisableAll()
        {
            TriggerStatusPanelEA.ESymbol[] symbols = (TriggerStatusPanelEA.ESymbol[])Enum.GetValues(typeof(TriggerStatusPanelEA.ESymbol));
            foreach (TriggerStatusPanelEA.ESymbol symbol in symbols)
            {
                GetImage(symbol).gameObject.SetActive(false);
            }
        }

        public void OnTriggerStatusPanel(EventArgs args)
        {
            if(args is TriggerStatusPanelEA tsp)
            {
                foreach(var icon in tsp.EnableIcons)
                {
                    _coroutines[icon.Symbol] = StartCoroutine(TriggerIcon(GetImage(icon.Symbol), icon.Time, icon.Mode));
                }

                foreach(var symbol in tsp.DisableIcons)
                {
                    StopCoroutine(_coroutines[symbol]);
                }
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        private void Awake()
        {
            DisableAll();
        }

        private void OnDestroy()
        {
            foreach (var elem in _coroutines)
            {
                StopCoroutine(elem.Value);
            }
        }
    }
}