using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Miner.Management.Events;
using Miner.Management.Exceptions;

namespace Miner.Gameplay
{
    public class SkyController : MonoBehaviour
    {
        [SerializeField] private IntReference _timeOfDay = null;
        [SerializeField] private DayTable _dayTable = null;
        [SerializeField] private GameObject _daySky = null;
        [SerializeField] private GameObject _nightSky = null;

        private SpriteRenderer[] _daySkySprites = null;
        private SpriteRenderer[] _nightSkySprites = null;
        private GameObject _overridingSky = null;
        private bool _enabled = true;
        private Color _transparent = new Color(1f, 1f, 1f, 0f);
        private float _lerpCoeff = 0f;
        private Coroutine _overridingSkyCoroutine = null;

        private void MinuteElapsed(int oldVal, int timeOfDay)
        {
            if (!_enabled) return;
            if (_overridingSky != null) return;

            if(_dayTable.Day.IsInRange(timeOfDay))
            {
                SetSkyColor(_nightSkySprites, _transparent);
                SetSkyColor(_daySkySprites, Color.white);
            }
            else if(_dayTable.Morning.IsInRange(timeOfDay))
            {
                float lerpCoeff = (timeOfDay - _dayTable.Morning.minValue) / (float)(_dayTable.Morning.maxValue - _dayTable.Morning.minValue);
                SetSkyColor(_nightSkySprites, Color.Lerp(Color.white, _transparent, lerpCoeff));
                SetSkyColor(_daySkySprites, Color.Lerp(_transparent, Color.white, lerpCoeff));
            }
            else if(_dayTable.Evening.IsInRange(timeOfDay))
            {
                float lerpCoeff = (timeOfDay - _dayTable.Evening.minValue) / (float)(_dayTable.Evening.maxValue - _dayTable.Evening.minValue);
                SetSkyColor(_daySkySprites, Color.Lerp(Color.white, _transparent, lerpCoeff));
                SetSkyColor(_nightSkySprites, Color.Lerp(_transparent, Color.white, lerpCoeff));
            }
            else
            {
                SetSkyColor(_daySkySprites, _transparent);
                SetSkyColor(_nightSkySprites, Color.white);
            }
        }

        private void OnPlayerCameToLayer(EventArgs args)
        {
            if (args is PlayerCameToLayerEA pctl)
            {
                if (pctl.LayerNumber > 0)
                {
                    _enabled = false;
                }
                else
                    _enabled = true;
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        public void OnOverrideSky(EventArgs args)
        {
            if(args is OverrideSkyEA os)
            {
                if (_overridingSky == null)
                {
                    if (os.Sky != null)
                    {
                        _overridingSky = Instantiate(os.Sky, transform);
                        SpriteRenderer[] sprites = _overridingSky.GetComponentsInChildren<SpriteRenderer>();
                        for (int i = 0; i < sprites.Length; ++i)
                        {
                            sprites[i].color = _transparent;
                        }
                        _overridingSkyCoroutine = StartCoroutine(ChangeSkyFluently(sprites));
                    }
                }
                else if (_overridingSky != null)
                {
                    _overridingSkyCoroutine = StartCoroutine(RestoreDefaultSky(os.Sky));
                }
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        private void SetSkyColor(SpriteRenderer[] sprites, Color color)
        {
            foreach(var sprite in sprites)
            {
                sprite.color = color;
            }
        }

        private IEnumerator ChangeSkyFluently(SpriteRenderer[] skySprites)
        {
            while(_lerpCoeff < 1f)
            {
                _lerpCoeff += Time.deltaTime;
                Color c = Color.Lerp(_transparent, Color.white, _lerpCoeff);
                for (int i = 0; i < skySprites.Length; ++i)
                {
                    skySprites[i].color = c;
                }
                yield return null;
            }
            _lerpCoeff = 1f;
        }

        private IEnumerator RestoreDefaultSky(GameObject newSky = null)
        {
            SpriteRenderer[] skySprites = _overridingSky.GetComponentsInChildren<SpriteRenderer>();
            while (_lerpCoeff > 0f)
            {
                _lerpCoeff -= Time.deltaTime;
                Color c = Color.Lerp(_transparent, Color.white, _lerpCoeff);
                for (int i = 0; i < skySprites.Length; ++i)
                {
                    skySprites[i].color = c;
                }
                yield return null;
            }
            Destroy(_overridingSky);
            _overridingSky = null;
            _lerpCoeff = 0f;

            if (newSky != null)
            {
                skySprites = newSky.GetComponentsInChildren<SpriteRenderer>();
                yield return StartCoroutine(ChangeSkyFluently(skySprites));
            }
        }

        private void Awake()
        {
            _daySkySprites = _daySky.GetComponentsInChildren<SpriteRenderer>();
            _nightSkySprites = _nightSky.GetComponentsInChildren<SpriteRenderer>();
            _timeOfDay.ValueChanged += MinuteElapsed;
        }

        private void Start()
        {
            MinuteElapsed(0, _timeOfDay.Value);
        }

        private void OnDestroy()
        {
            _timeOfDay.ValueChanged -= MinuteElapsed;
            if(_overridingSkyCoroutine != null)
                StopCoroutine(_overridingSkyCoroutine);
        }

    }
}