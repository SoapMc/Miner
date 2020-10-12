using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using TMPro;

namespace Miner.UI
{
    public class UniversalDescriptor : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI _description = null;
        [SerializeField] protected RectTransform _maskRectTransform = null;
        [SerializeField] protected RectTransform _imageRectTransform = null;
        [SerializeField, Range(2f, 10f)] protected float _appearingRate = 5f;
        protected Vector2 _size;
        protected Coroutine _appearingCoroutine = null;

        public virtual void OnDescriptElement(EventArgs args)
        {
            if(args is DescriptElementEA die)
            {
                _description.text = "<size=18>" + die.DescriptableElement.Name + "</size>\n" + die.DescriptableElement.Description;
                Rect rect = die.RectTransform.rect;
                transform.position = (Vector2)die.RectTransform.position + rect.position;
                if (_appearingCoroutine != null)
                    StopCoroutine(_appearingCoroutine);
                _appearingCoroutine = StartCoroutine(TriggerAppearingCoroutine());
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        protected IEnumerator TriggerAppearingCoroutine()
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

        private void Awake()
        {
            _size = _maskRectTransform.sizeDelta;
            _maskRectTransform.gameObject.SetActive(false);
        }
    }
}