using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Miner.FX
{
    public class WindowAppearingEffect : MonoBehaviour, IAppearingEffect
    {
        public event Action AppearingFinished;
        public event Action DisappearingFinished;
        [SerializeField] private RectTransform _titlebarMask = null;
        [SerializeField] private RectTransform _contentMask = null;
        [SerializeField, Range(2f, 10f)] private float _appearingRate = 5f;
        private float _contentSizeDeltaY;
        private Vector2 _windowSize;
        private Coroutine _triggeringCoroutine = null;
        private float _lerpCoeff = 0f;

        public void TriggerAppearing()
        {
            if (_triggeringCoroutine != null)
                StopCoroutine(_triggeringCoroutine);
            _triggeringCoroutine = StartCoroutine(TriggerAppearingCoroutine());
        }

        public void TriggerDisappearing()
        {
            if (_triggeringCoroutine != null)
                StopCoroutine(_triggeringCoroutine);
            _triggeringCoroutine = StartCoroutine(TriggerDisappearingCoroutine());
        }

        private IEnumerator TriggerAppearingCoroutine()
        {
            _titlebarMask.sizeDelta = new Vector2(-_windowSize.x, _titlebarMask.sizeDelta.y);
            _contentMask.sizeDelta = new Vector2(0, _titlebarMask.sizeDelta.y);
            
            while (_lerpCoeff < 1f)
            {
                _lerpCoeff += (1.1f - _lerpCoeff) * _appearingRate * Time.unscaledDeltaTime;
                _titlebarMask.sizeDelta = new Vector2(Mathf.Lerp(-_windowSize.x, 0, _lerpCoeff), _titlebarMask.sizeDelta.y);
                _contentMask.sizeDelta = new Vector2(_titlebarMask.sizeDelta.x, Mathf.Lerp(_titlebarMask.sizeDelta.y, _contentSizeDeltaY, _lerpCoeff));
                yield return null;
            }
            _lerpCoeff = 1f;
            AppearingFinished?.Invoke();
            _triggeringCoroutine = null;
        }

        private IEnumerator TriggerDisappearingCoroutine()
        {
            while (_lerpCoeff > 0f)
            {
                _lerpCoeff -= (1.1f - _lerpCoeff) * _appearingRate * Time.unscaledDeltaTime;
                _titlebarMask.sizeDelta = new Vector2(Mathf.Lerp(-_windowSize.x, 0, _lerpCoeff), _titlebarMask.sizeDelta.y);
                _contentMask.sizeDelta = new Vector2(_titlebarMask.sizeDelta.x, Mathf.Lerp(_titlebarMask.sizeDelta.y, _contentSizeDeltaY, _lerpCoeff));
                yield return null;
            }
            _lerpCoeff = 0f;
            DisappearingFinished?.Invoke();
            _triggeringCoroutine = null;
        }

        private void Awake()
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            _windowSize = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
            _contentSizeDeltaY = -_windowSize.y - _titlebarMask.sizeDelta.y;
            TriggerAppearing();
        }

        private void OnDestroy()
        {
            if (_triggeringCoroutine != null)
                StopCoroutine(_triggeringCoroutine);
        }

    }
}