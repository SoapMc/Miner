using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Gameplay;
using System;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using System.Linq;

namespace Miner.FX
{
    public class BackgroundController : MonoBehaviour
    {
        [SerializeField] private Vector2Reference _playerPosition = null;
        [SerializeField] private IntReference _undergroundDepth = null;
        [SerializeField] private Material _backgroundMaterial = null;
        [SerializeField] private SpriteRenderer _backgroundRenderer = null;
        [SerializeField, Range(0.1f, 5f)] private float _rateOfChange = 1f;
        [SerializeField] private GroundLayerList _layers = null;

        private string _offsetXName = "_OffsetX";
        private string _offsetYName = "_OffsetY";
        private Coroutine _enablingBackground = null;
        private Coroutine _disablingBackground = null;
        private float _alpha = 0f;
        private int _currentLayerNumber = 0;
        private int _nextLayerNumber = 0;
        private float _topDepth = 0;
        private float _bottomDepth = 0;
        private bool _backgroundEnabled = false;

        private void Awake()
        {
            if(_playerPosition.Value.y >= _undergroundDepth)
            {
                _alpha = 0f;
            }
            else
            {
                _alpha = 1f;
            }
            _backgroundRenderer.color = new Color(_backgroundRenderer.color.r, _backgroundRenderer.color.g, _backgroundRenderer.color.b, _alpha);
        }

        private void Update()
        {
            _backgroundMaterial.SetFloat(_offsetXName, -_playerPosition.Value.x * 0.1f);
            _backgroundMaterial.SetFloat(_offsetYName, _playerPosition.Value.y * 0.1f);
            Color c = Color.Lerp(_layers[_currentLayerNumber].BackgroundColor, _layers[_nextLayerNumber].BackgroundColor, Mathf.Clamp01((Mathf.Abs(_playerPosition.Value.y) - _topDepth) / (_bottomDepth - _topDepth)));
            _backgroundRenderer.color = new Color(c.r, c.g, c.b, _backgroundRenderer.color.a);
        }

        private void EnableUnderground()
        {
            if (_backgroundEnabled == true) return;
            _backgroundEnabled = true;
            if (_disablingBackground != null)
            {
                StopCoroutine(_disablingBackground);
                _disablingBackground = null;
            }
            _enablingBackground = StartCoroutine(EnableUndergroundBackground());
        }

        private void DisableUnderground()
        {
            if (_backgroundEnabled == false) return;
            _backgroundEnabled = false;
            if (_enablingBackground != null)
            {
                StopCoroutine(_enablingBackground);
                _enablingBackground = null;
            }

            _disablingBackground = StartCoroutine(DisableUndergroundBackground());
        }

        private IEnumerator EnableUndergroundBackground()
        {
            if(_enablingBackground == null)
            {
                while (_alpha < 0.999f)
                {
                    _backgroundRenderer.color = new Color(_backgroundRenderer.color.r, _backgroundRenderer.color.g, _backgroundRenderer.color.b, _alpha);
                    _alpha += _rateOfChange * Time.deltaTime;
                    yield return null;
                }
                _backgroundRenderer.color = new Color(_backgroundRenderer.color.r, _backgroundRenderer.color.g, _backgroundRenderer.color.b, 1f);
                _enablingBackground = null;
            }
        }

        private IEnumerator DisableUndergroundBackground()
        {
            if (_disablingBackground == null)
            {
                while (_alpha > 0.001f)
                {
                    _backgroundRenderer.color = new Color(_backgroundRenderer.color.r, _backgroundRenderer.color.g, _backgroundRenderer.color.b, _alpha);
                    _alpha -= _rateOfChange * Time.deltaTime;
                    yield return null;
                }
                _backgroundRenderer.color = new Color(_backgroundRenderer.color.r, _backgroundRenderer.color.g, _backgroundRenderer.color.b, 0f);
                _disablingBackground = null;
            }
        }

        public void OnPlayerCameToLayer(EventArgs args)
        {
            if(args is PlayerCameToLayerEA pctl)
            {
                _currentLayerNumber = Mathf.Clamp(pctl.LayerNumber, 0, _layers.Count - 1);
                _nextLayerNumber = _currentLayerNumber + 1;
                _nextLayerNumber = Mathf.Clamp(_nextLayerNumber, 0, _layers.Count - 1);
                _topDepth = CalculateTopDepth(_currentLayerNumber);
                _bottomDepth = _topDepth + _layers[_currentLayerNumber].Depth;

                if(pctl.LayerNumber == 0)
                {
                    DisableUnderground();
                }
                else
                {
                    EnableUnderground();
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
            for(int i = 0; i < layerNumber; ++i)
            {
                result += _layers[i].Depth;
            }
            return result;
        }
    }
}