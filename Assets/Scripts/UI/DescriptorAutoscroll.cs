using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Miner.UI
{
    public class DescriptorAutoscroll : MonoBehaviour
    {
        [Range(0.1f, 2f)] public float Velocity = 0.5f;
        [Range(0f, 10f)] public float Interval = 3f;

        private ScrollRect _scrollRect;
        private RectTransform _content = null;

        private IEnumerator ScrollDown()
        {
            do
            {
                yield return null;
                _scrollRect.verticalNormalizedPosition -= Velocity * Time.unscaledDeltaTime;
                if (_scrollRect.verticalNormalizedPosition < 0f)
                    _scrollRect.verticalNormalizedPosition = 0f;
            }
            while (_scrollRect.verticalNormalizedPosition > 0.001f);

            StopCoroutine(ScrollDown());
            StartCoroutine(WaitAndScrollUp());
        }

        private IEnumerator ScrollUp()
        {
            do
            {
                yield return null;
                _scrollRect.verticalNormalizedPosition += Velocity * Time.unscaledDeltaTime;
                if (_scrollRect.verticalNormalizedPosition > 1f)
                    _scrollRect.verticalNormalizedPosition = 1f;
            }
            while (_scrollRect.verticalNormalizedPosition < 0.999f);

            StopCoroutine(ScrollUp());
            StartCoroutine(WaitAndScrollDown());
        }

        private IEnumerator WaitAndScrollDown()
        {
            yield return new WaitForSeconds(Interval);
            StopCoroutine(WaitAndScrollDown());
            StartCoroutine(ScrollDown());
        }

        private IEnumerator WaitAndScrollUp()
        {
            yield return new WaitForSeconds(Interval);
            StopCoroutine(WaitAndScrollUp());
            StartCoroutine(ScrollUp());
        }

        public void OnShowPartDescription()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_content);
            
            _scrollRect.verticalNormalizedPosition = 1f;
            if(transform.parent.gameObject.activeSelf && gameObject.activeSelf)
                StartCoroutine(WaitAndScrollDown());

        }

        private void Awake()
        {
            _scrollRect = GetComponent<ScrollRect>();
            _content = _scrollRect.content;
            _scrollRect.verticalNormalizedPosition = 1f;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}