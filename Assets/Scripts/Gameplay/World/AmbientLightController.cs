using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Miner.Management.Events;
using Miner.Management.Exceptions;

namespace Miner.Gameplay
{
    public class AmbientLightController : MonoBehaviour
    {
        [SerializeField] private Vector2Reference _playerPosition = null;
        [SerializeField] private GroundLayerList _layers = null;
        [SerializeField] private IntReference _timeOfDay = null;
        [SerializeField] private DayTable _dayTable = null;
        [SerializeField] private Color _dayAmbientLight = Color.white;
        [SerializeField] private Color _nightAmbientLight = Color.black;
        private bool _enabledSurfaceLighting = true;
        private int _currentLayerNumber = 0;
        private int _nextLayerNumber = 0;
        private float _topDepth = 0;
        private float _bottomDepth = 0;
        private Coroutine _changeLightCoroutine = null;

        private IEnumerator ChangeLightFluentlyToSurface(Color startColor, Color endColor)
        {
            float lerpCoeff = 0f;
            while(lerpCoeff < 1f)
            {
                lerpCoeff += Time.unscaledDeltaTime;
                RenderSettings.ambientLight = Color.Lerp(startColor, CalculateAmbientLightColorOnSurface(_timeOfDay), lerpCoeff);
                yield return null;
            }
            _timeOfDay.ValueChanged += MinuteElapsed;
            _changeLightCoroutine = null;
        }

        private IEnumerator ChangeLightFluentlyToUnderground(Color startColor, Color endColor)
        {
            float lerpCoeff = 0f;
            while (lerpCoeff < 1f)
            {
                lerpCoeff += Time.unscaledDeltaTime;
                RenderSettings.ambientLight = Color.Lerp(startColor, CalculateAmbientLightColorInUnderground(_playerPosition.Value.y), lerpCoeff);
                yield return null;
            }
            _playerPosition.ValueChanged += OnPlayerPositionChanged;
            _changeLightCoroutine = null;
        }

        private void MinuteElapsed(int oldVal, int timeOfDay)
        {
            RenderSettings.ambientLight = CalculateAmbientLightColorOnSurface(_timeOfDay);
        }

        private Color CalculateAmbientLightColorOnSurface(int timeOfDay)
        {
            if (_dayTable.Day.IsInRange(timeOfDay))
            {
                return _dayAmbientLight;
            }
            else if (_dayTable.Morning.IsInRange(timeOfDay))
            {
                float lerpCoeff = (timeOfDay - _dayTable.Morning.minValue) / (float)(_dayTable.Morning.maxValue - _dayTable.Morning.minValue);
                return Color.Lerp(_nightAmbientLight, _dayAmbientLight, lerpCoeff);
            }
            else if (_dayTable.Evening.IsInRange(timeOfDay))
            {
                float lerpCoeff = (timeOfDay - _dayTable.Evening.minValue) / (float)(_dayTable.Evening.maxValue - _dayTable.Evening.minValue);
                return Color.Lerp(_dayAmbientLight, _nightAmbientLight, lerpCoeff);
            }
            else
            {
                return _nightAmbientLight;
            }
        }

        private Color CalculateAmbientLightColorInUnderground(float playerYPosition)
        {
            return Color.Lerp(_layers[_currentLayerNumber].AmbientLightColor, _layers[_nextLayerNumber].AmbientLightColor, Mathf.Clamp01((Mathf.Abs(_playerPosition.Value.y) - _topDepth) / (_bottomDepth - _topDepth)));
        }

        private void OnPlayerPositionChanged(Vector2 oldVal, Vector2 newVal)
        {
            RenderSettings.ambientLight = CalculateAmbientLightColorInUnderground(newVal.y);
        }

        public void OnPlayerCameToLayer(EventArgs args)
        {
            if (args is PlayerCameToLayerEA pctl)
            {
                _currentLayerNumber = Mathf.Clamp(pctl.LayerNumber, 0, _layers.Count - 1);
                _nextLayerNumber = _currentLayerNumber + 1;
                _nextLayerNumber = Mathf.Clamp(_nextLayerNumber, 0, _layers.Count - 1);
                _topDepth = CalculateTopDepth(_currentLayerNumber);
                _bottomDepth = _topDepth + _layers[_currentLayerNumber].Depth;

                if (pctl.LayerNumber > 0)
                {
                    if (_enabledSurfaceLighting == true)
                    {
                        _timeOfDay.ValueChanged -= MinuteElapsed;
                        if (_changeLightCoroutine != null)
                            StopCoroutine(_changeLightCoroutine);
                        _changeLightCoroutine = StartCoroutine(ChangeLightFluentlyToUnderground(RenderSettings.ambientLight, _layers[pctl.LayerNumber].AmbientLightColor));
                    }
                    _enabledSurfaceLighting = false;
                }
                else
                {
                    if (_enabledSurfaceLighting == false)
                    {
                        _playerPosition.ValueChanged -= OnPlayerPositionChanged;
                        if (_changeLightCoroutine != null)
                            StopCoroutine(_changeLightCoroutine);
                        _changeLightCoroutine = StartCoroutine(ChangeLightFluentlyToSurface(RenderSettings.ambientLight, CalculateAmbientLightColorOnSurface(_timeOfDay.Value)));
                    }
                    _enabledSurfaceLighting = true;
                }
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        private float CalculateTopDepth(int layerNumber)
        {
            if (layerNumber >= _layers.Count) throw new ArgumentOutOfRangeException();

            float result = 0f;
            for (int i = 0; i < layerNumber; ++i)
            {
                result += _layers[i].Depth;
            }
            return result;
        }

        private void Start()
        {
            MinuteElapsed(0, _timeOfDay.Value);
        }

        private void OnDestroy()
        {
            _timeOfDay.ValueChanged -= MinuteElapsed;
            _playerPosition.ValueChanged -= OnPlayerPositionChanged;
            if (_changeLightCoroutine != null)
            {
                StopCoroutine(_changeLightCoroutine);
                _changeLightCoroutine = null;
            }
        }
    }
}