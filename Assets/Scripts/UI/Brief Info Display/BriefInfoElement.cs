using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using System;

namespace Miner.UI
{

    public class BriefInfoElement : MonoBehaviour
    {
        private readonly static int TEXT_CAPACITY = 48;

        public Action<BriefInfoElement> DisappearingFinished;
        [SerializeField, Range(1f, 100f)] private float _rateOfAppearing = 50f;
        [SerializeField, Range(1f, 10f)] private float _timeOfDisplayingFullText = 5f;
        private TextMeshProUGUI _message = null;
        private string _textToShow = string.Empty;
        private StringBuilder _stringBuilder = null;
        private Coroutine _appearingCoroutine = null;
        private Coroutine _movingCoroutine = null;
        private Vector2 _distanceFromTargetPosition = Vector2.zero;

        private IEnumerator TriggerAppearing()
        {
            //_textAppearingSound.Play(_audioSource);
            while (_stringBuilder.Length < _textToShow.Length)
            {
                _stringBuilder.Append(_textToShow[_stringBuilder.Length]);
                _message.text = _stringBuilder.ToString();
                yield return new WaitForSecondsRealtime(1 / _rateOfAppearing);
            }
            //_audioSource.Stop();
            yield return new WaitForSeconds(_timeOfDisplayingFullText);
            _appearingCoroutine = StartCoroutine(TriggerDisappearing());
        }

        private IEnumerator TriggerDisappearing()
        {
            while (_message.color.a > 0.1f)
            {
                _message.color *= 0.9f;
                yield return null;
            }
            _appearingCoroutine = null;
            DisappearingFinished?.Invoke(this);
        }

        public void Initialize(string message, Color color)
        {
            _message = GetComponent<TextMeshProUGUI>();
            _message.text = string.Empty;
            _textToShow = message;
            _message.color = color;
            _stringBuilder = new StringBuilder(TEXT_CAPACITY);
            _appearingCoroutine = StartCoroutine(TriggerAppearing());
        }

        public void RequestTranslation(Vector2 translation)
        {
            if (_movingCoroutine != null)
            {
                StopCoroutine(_movingCoroutine);
                _distanceFromTargetPosition += translation;
            }
            else
            {
                _distanceFromTargetPosition = translation;
            }
            _movingCoroutine = StartCoroutine(MoveToPosition((Vector2)transform.localPosition + _distanceFromTargetPosition));
        }

        private IEnumerator MoveToPosition(Vector2 localPosition)
        {
            
            float lerpCoeff = 0f;
            while(lerpCoeff < 1f)
            {
                Vector2 currentPosition = transform.localPosition;
                transform.localPosition = Vector2.Lerp(transform.localPosition, localPosition, lerpCoeff);
                _distanceFromTargetPosition -= (Vector2)transform.localPosition - currentPosition;
                lerpCoeff += 2 * Time.unscaledDeltaTime;
                yield return null;
            }
        }
    }

}