using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Gameplay;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using System;

namespace Miner.UI
{
    public class AlarmDisplay : MonoBehaviour
    {
        [SerializeField] private AlarmDisplayElement _alarmElementPrefab = null;
        [SerializeField] private Vector2 _spacing = new Vector2(0, 10);
        [SerializeField] private Color _criticalImportanceColor = Color.red;
        [SerializeField] private Color _warningImportanceColor = Color.yellow;
        private Dictionary<EAlarmType, AlarmDisplayElement> _elements = new Dictionary<EAlarmType, AlarmDisplayElement>();
        private Vector2 _elementSize;

        private void Awake()
        {
            _elementSize = _alarmElementPrefab.GetComponent<RectTransform>().sizeDelta;
        }

        public string Translate(EAlarmType type)
        {
            switch(type)
            {
                case EAlarmType.FuelLow:
                    return "FUEL LOW";
                case EAlarmType.PowerLow:
                    return "POWER LOW";
                case EAlarmType.RadiationHigh:
                    return "RAD HIGH";
                case EAlarmType.TemperatureHigh:
                    return "TEMP HIGH";
                default:
                    return string.Empty;
            }
        }

        public Color GetAlarmColor(EAlarmImportance importance)
        {
            switch(importance)
            {
                case EAlarmImportance.Critical:
                    return _criticalImportanceColor;
                case EAlarmImportance.Warning:
                    return _warningImportanceColor;
                default:
                    return Color.white;
            }
        }

        public void OnChangeAlarms(EventArgs args)
        {
            if(args is ChangeAlarmsEA ca)
            {
                if (ca.AddedAlarm != null)
                {
                    if(!_elements.ContainsKey(ca.AddedAlarm.Value))
                    {
                        AlarmDisplayElement ade = Instantiate(_alarmElementPrefab, transform);
                        ade.transform.localPosition = new Vector3(0, _elements.Count * (_elementSize.y + _spacing.y), 0);
                        ade.Initialize(Translate(ca.AddedAlarm.Value), ca.AddedAlarmImportance, GetAlarmColor(ca.AddedAlarmImportance));
                        _elements.Add(ca.AddedAlarm.Value, ade);
                    }
                    else if(_elements.ContainsKey(ca.AddedAlarm.Value) && _elements[ca.AddedAlarm.Value].Importance == EAlarmImportance.Warning && ca.AddedAlarmImportance == EAlarmImportance.Critical)
                    {
                        _elements[ca.AddedAlarm.Value].Initialize(Translate(ca.AddedAlarm.Value), ca.AddedAlarmImportance, GetAlarmColor(ca.AddedAlarmImportance));
                    }


                }
                if (ca.RemovedAlarm != null)
                {
                    if (_elements.ContainsKey(ca.RemovedAlarm.Value))
                    {
                        Destroy(_elements[ca.RemovedAlarm.Value].gameObject);
                        _elements.Remove(ca.RemovedAlarm.Value);

                        {
                            int i = 0;
                            foreach (var elem in _elements)
                            {
                                elem.Value.transform.localPosition = new Vector3(0, i * (_elementSize.y + _spacing.y), 0);
                                ++i;
                            }
                        }
                    }
                }
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        public void OnPlayerUnloaded()
        {
            foreach(var elem in _elements)
            {
                Destroy(elem.Value.gameObject);
            }
            _elements.Clear();
        }

        public enum EAlarmType
        {
            FuelLow,
            RadiationHigh,
            TemperatureHigh,
            PowerLow
        }

        public enum EAlarmImportance
        {
            Warning,
            Critical
        }
    }
}