using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Miner.Management.Events;

namespace Miner.FX
{
    public class MainMenuAppearingEffect : MonoBehaviour, IAppearingEffect
    {

        public event Action AppearingFinished;
        public event Action DisappearingFinished;
        [SerializeField] private RectTransform _panel = null;
        [SerializeField, Range(1f, 5f)] private float _appearingRate = 5f;
        private Coroutine _triggeringCoroutine = null;
        private float _lerpCoeff = 0f;
        private Vector2 _windowSize;

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
            _panel.sizeDelta = new Vector2(_panel.sizeDelta.x, 0);
            while (_lerpCoeff < 1f)
            {
                _lerpCoeff += _appearingRate * Time.unscaledDeltaTime;
                _panel.sizeDelta = new Vector2(_panel.sizeDelta.x, Mathf.Lerp(0, _windowSize.y, _lerpCoeff));
                yield return null;
            }
            _lerpCoeff = 1f;
            AppearingFinished?.Invoke();
            _triggeringCoroutine = null;
        }

        private IEnumerator TriggerDisappearingCoroutine()
        {
            _panel.sizeDelta = new Vector2(_panel.sizeDelta.x, _panel.sizeDelta.y);
            while (_lerpCoeff > 0f)
            {
                _lerpCoeff -= _appearingRate * Time.unscaledDeltaTime;
                _panel.sizeDelta = new Vector2(_panel.sizeDelta.x, Mathf.Lerp(0, _windowSize.y, _lerpCoeff));
                yield return null;
            }
            _lerpCoeff = 0f;
            DisappearingFinished?.Invoke();
            _triggeringCoroutine = null;
        }


        private void Awake()
        {
            _windowSize = new Vector2(_panel.rect.width, _panel.rect.height);
            TriggerAppearing();
        }

        private void OnDestroy()
        {
            if (_triggeringCoroutine != null)
                StopCoroutine(_triggeringCoroutine);
        }
    }
}