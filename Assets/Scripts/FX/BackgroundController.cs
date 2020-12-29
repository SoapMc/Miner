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
        [SerializeField] private FloatReference _playerLayerRelativePosition = null;

        private string _offsetXName = "_OffsetX";
        private string _offsetYName = "_OffsetY";
        private float _alpha = 0f;
        private Coroutine _changeTransparencyCoroutine = null;
        private GroundLayer _currentLayer = null;
        private ECurrentState _state = ECurrentState.Idle;

        public void OnPlayerInstantiated()
        {
            if (_playerPosition.Value.y >= _undergroundDepth)
            {
                _alpha = 0f;
            }
            else
            {
                _alpha = 1f;
            }
            _backgroundRenderer.color = new Color(_backgroundRenderer.color.r, _backgroundRenderer.color.g, _backgroundRenderer.color.b, _alpha);
        }

        public void OnPlayerReset()
        {
            if(_changeTransparencyCoroutine != null)
            {
                StopCoroutine(_changeTransparencyCoroutine);
                _changeTransparencyCoroutine = null;
            }
            _alpha = 0f;
            _backgroundRenderer.color = new Color(_backgroundRenderer.color.r, _backgroundRenderer.color.g, _backgroundRenderer.color.b, _alpha);
            enabled = false;
        }

        public void OnPlayerCameToLayer(EventArgs args)
        {
            if(args is PlayerCameToLayerEA pctl)
            {
                if(!enabled) enabled = true;

                _currentLayer = pctl.GroundLayer;
                ChangeTransparency(pctl.GroundLayer.AreaType);
            }
            else
            {
                Management.Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

        private void ChangeTransparency(GroundLayer.EAreaType layerAreaType)
        {
            switch(layerAreaType)
            {
                case GroundLayer.EAreaType.Surface:
                    if(_state != ECurrentState.ToTransparent && _changeTransparencyCoroutine != null)
                    {
                        StopCoroutine(_changeTransparencyCoroutine);
                    }
                    _state = ECurrentState.ToTransparent;
                    _changeTransparencyCoroutine = StartCoroutine(ToTransparent());
                    break;
                case GroundLayer.EAreaType.Underground:
                    if (_state != ECurrentState.ToOpaque && _changeTransparencyCoroutine != null)
                    {
                        StopCoroutine(_changeTransparencyCoroutine);
                    }
                    _state = ECurrentState.ToOpaque;
                    _changeTransparencyCoroutine = StartCoroutine(ToOpaque());
                    break;
                default:
                    _state = ECurrentState.Idle;
                    break;
            }
        }

        #region COROUTINES
        private IEnumerator ToOpaque()
        {
            while (_alpha < 1f)
            {
                _alpha += Time.deltaTime * _rateOfChange;
                yield return null;
            }
            _alpha = 1f;
            _changeTransparencyCoroutine = null;
        }

        private IEnumerator ToTransparent()
        {
            while (_alpha > 0f)
            {
                _alpha -= Time.deltaTime * _rateOfChange;
                yield return null;
            }
            _alpha = 0f;
            _changeTransparencyCoroutine = null;
        }

        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            enabled = false;
        }

        private void Update()
        {
            _backgroundMaterial.SetFloat(_offsetXName, -_playerPosition.Value.x * 0.1f);
            _backgroundMaterial.SetFloat(_offsetYName, _playerPosition.Value.y * 0.1f);
            Color c = Color.Lerp(_currentLayer.BackgroundColorSet.TopColor, _currentLayer.BackgroundColorSet.BottomColor, _playerLayerRelativePosition.Value);
            _backgroundRenderer.color = new Color(c.r, c.g, c.b, _alpha);
        }
        #endregion

        private enum ECurrentState
        {
            Idle,
            ToOpaque,
            ToTransparent
        }
    }
}