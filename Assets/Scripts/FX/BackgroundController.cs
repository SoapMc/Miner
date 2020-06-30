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
            _topDepth = _bottomDepth = _layers.First().Depth;
        }

        private void Update()
        {
            _backgroundMaterial.SetFloat(_offsetXName, -_playerPosition.Value.x * 0.1f);
            _backgroundMaterial.SetFloat(_offsetYName, _playerPosition.Value.y * 0.1f);
            Color c = Color.Lerp(_layers[_currentLayerNumber]._backgroundColor, _layers[_nextLayerNumber]._backgroundColor, Mathf.Clamp01(Mathf.Abs((_playerPosition.Value.y - _topDepth)/(_bottomDepth - _topDepth))));
            _backgroundRenderer.color = new Color(c.r, c.g, c.b, _backgroundRenderer.color.a);
        }

        public void OnEnableUnderground()
        {
            if (_disablingBackground != null)
            {
                StopCoroutine(_disablingBackground);
                _disablingBackground = null;
            }
            _enablingBackground = StartCoroutine(EnableUnderground());
        }

        public void OnDisableUnderground()
        {
            if(_enablingBackground != null)
            {
                StopCoroutine(_enablingBackground);
                _enablingBackground = null;
            }

            _disablingBackground = StartCoroutine(DisableUnderground());
        }

        private IEnumerator EnableUnderground()
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

        private IEnumerator DisableUnderground()
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

        public void OnTriggerTopLayer(EventArgs args)
        {
            if(args is LayerTriggerEA lt)
            {
                _currentLayerNumber = Mathf.Clamp(lt.LayerNumber - 1, 0, int.MaxValue);
                if (_layers.Count > _currentLayerNumber + 1)
                    _nextLayerNumber = _currentLayerNumber + 1;
                else
                    _nextLayerNumber = _currentLayerNumber;
                _topDepth = CalculateTopDepth(_currentLayerNumber);
                _bottomDepth = _topDepth + _layers[_currentLayerNumber].Depth;
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        private float CalculateTopDepth(int layerNumber)
        {
            if (layerNumber - 1 < 0) return 0f;
            if (layerNumber >= _layers.Count) throw new ArgumentOutOfRangeException();

            float result = 0f;
            for(int i = 0; i < layerNumber - 1; ++i)
            {
                result += _layers[i].Depth;
            }
            return result;
        }
    }
}