using System.Collections;
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
        [SerializeField] private Color _warningColor;
        [SerializeField] private Color _failureColor;

        private Dictionary<TriggerStatusPanelEA.ESymbol, Coroutine> _coroutines = new Dictionary<TriggerStatusPanelEA.ESymbol, Coroutine>();

        private IEnumerator TriggerIcon(TriggerStatusPanelEA.ESymbol symbol, Image icon, float duration, TriggerStatusPanelEA.EMode mode)
        {

            if(mode == TriggerStatusPanelEA.EMode.Failure)
            {
                icon.color = _failureColor;
            }
            else
            {
                icon.color = _warningColor;
            }

            if (duration > 0)
            {
                float elapsedTime = 0f;
                while (elapsedTime < duration)
                {
                    icon.gameObject.SetActive(!icon.gameObject.activeSelf);
                    yield return new WaitForSeconds(_blinkPeriod);
                    elapsedTime += _blinkPeriod;
                }
            }
            else
            {
                while (true)
                {
                    icon.gameObject.SetActive(!icon.gameObject.activeSelf);
                    yield return new WaitForSeconds(_blinkPeriod);
                }
            }
            
            icon.gameObject.SetActive(false);
            _coroutines[symbol] = null;
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
                    if(_coroutines[icon.Symbol] == null)
                        _coroutines[icon.Symbol] = StartCoroutine(TriggerIcon(icon.Symbol, GetImage(icon.Symbol), icon.Time, icon.Mode));
                }

                foreach(var symbol in tsp.DisableIcons)
                {
                    if (_coroutines[symbol] != null)
                    {
                        StopCoroutine(_coroutines[symbol]);
                        GetImage(symbol).gameObject.SetActive(false);
                    }
                    _coroutines[symbol] = null;
                }
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        private void Start()
        {
            foreach(TriggerStatusPanelEA.ESymbol symbol in Enum.GetValues(typeof(TriggerStatusPanelEA.ESymbol)))
            {
                _coroutines.Add(symbol, null);
            }
            DisableAll();
        }

        private void OnDestroy()
        {
            foreach (var elem in _coroutines)
            {
                if(elem.Value != null)
                    StopCoroutine(elem.Value);
            }
        }
    }
}