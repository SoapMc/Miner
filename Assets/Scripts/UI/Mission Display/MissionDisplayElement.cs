using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using Miner.Gameplay;
using UnityEngine.UI;

namespace Miner.UI
{
    public class MissionDisplayElement : MonoBehaviour, IDisposable
    {
        [SerializeField] protected TextMeshProUGUI _description = null;
        [SerializeField] protected RectTransform _maskRectTransform = null;
        [SerializeField] protected RectTransform _imageRectTransform = null;
        [SerializeField, Range(2f, 10f)] protected float _appearingRate = 5f;
        protected Vector2 _size;
        protected Coroutine _appearingCoroutine = null;
        protected Mission _mission = null;
        protected bool _disposed = false;

        public Mission Mission => _mission;
        public Vector2 Size => _size;

        public void Initialize(Mission mission)
        {
            _mission = mission;
            _mission.MissionUpdated += Refresh;
            Refresh(mission);

            if (_appearingCoroutine != null)
                StopCoroutine(_appearingCoroutine);
            _appearingCoroutine = StartCoroutine(TriggerAppearingCoroutine());
        }

        private void Refresh(Mission mission)
        {
            if(!_disposed)
                _description.text = "<size=18>" + mission.Name + "</size>\n" + mission.ShortDescription;
        }

        private IEnumerator TriggerAppearingCoroutine()
        {
            _maskRectTransform.gameObject.SetActive(true);
            _maskRectTransform.sizeDelta = Vector2.zero;
            yield return null;
            RectTransform descriptionRect = _description.GetComponent<RectTransform>();
            _maskRectTransform.sizeDelta = new Vector2(descriptionRect.rect.width, descriptionRect.rect.height);
            _size = _maskRectTransform.sizeDelta;
            _maskRectTransform.gameObject.SetActive(true);
            float lerpCoeff = 0f;
            while (lerpCoeff < 1f)
            {
                lerpCoeff += (1.1f - lerpCoeff) * _appearingRate * Time.unscaledDeltaTime;
                _maskRectTransform.sizeDelta = new Vector2(Mathf.Lerp(0, _size.x, lerpCoeff), Mathf.Lerp(0, _size.y, lerpCoeff));
                yield return null;
            }
        }

        public void Dispose()
        {
            _mission.MissionUpdated -= Refresh;
            _disposed = true;
        }

        private void Awake()
        {
            _size = _maskRectTransform.sizeDelta;
            _maskRectTransform.gameObject.SetActive(false);
        }
    }
}