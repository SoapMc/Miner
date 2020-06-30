using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.FX
{
    public class BackgroundController : MonoBehaviour
    {
        [SerializeField] private Vector2Reference _playerPosition = null;
        [SerializeField] private IntReference _undergroundDepth = null;
        [SerializeField] private Material _backgroundMaterial = null;
        [SerializeField] private SpriteRenderer _backgroundRenderer = null;
        [SerializeField, Range(0.1f, 5f)] private float _rateOfChange = 1f;

        private string _offsetXName = "_OffsetX";
        private string _offsetYName = "_OffsetY";
        private Coroutine _enablingBackground = null;
        private Coroutine _disablingBackground = null;
        private float _alpha = 0f;

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
    }
}