using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;

namespace Miner.Gameplay
{
    public class PlayerStatusController : MonoBehaviour
    {
        [SerializeField] private FloatReference _externalTemperature = null;
        [SerializeField] private FloatReference _internalTemperature = null;
        [SerializeField] private FloatReference _effectiveCooling = null;
        [SerializeField] private FloatReference _surfaceTemperature = null;

        [Header("Events")]
        [SerializeField] private GameEvent _triggerStatusPanel = null;

        private void CalculateTemperatureFlow()
        {
            if (_internalTemperature.Value > _externalTemperature) return;

            _internalTemperature.Value += (_externalTemperature.Value - _effectiveCooling.Value) * Time.deltaTime * 0.1f;
        }

        private void Update()
        {
            CalculateTemperatureFlow();
        }

        private void Start()
        {
            TriggerStatusPanelEA tsp = new TriggerStatusPanelEA();
            tsp.EnableIcons.Add(new TriggerStatusPanelEA.Element() { Symbol = TriggerStatusPanelEA.ESymbol.Temperature, Mode = TriggerStatusPanelEA.EMode.Warning, Time = 5f });
            _triggerStatusPanel.Raise(tsp);
        }
    }
}