using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using TMPro;

namespace Miner.FX
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextAppearingEffect : MonoBehaviour
    {
        public Action AppearingFinished;
        public Action DisappearingFinished;
        [Range(5f, 200f), SerializeField] private float _rateOfAppearing = 50;
        [Range(5f, 200f), SerializeField] private float _rateOfDisappearing = 100;
        private TextMeshProUGUI _target = null;
        private StringBuilder _sb = new StringBuilder(255);
        private string _wholeText = string.Empty;
        private Coroutine _coroutine = null;

        public void Show(string text)
        {
            _coroutine = StartCoroutine(TriggerAppearing(text));
        }

        public void Hide()
        {
            if(_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
            _coroutine = StartCoroutine(TriggerDisappearing());
        }

        private IEnumerator TriggerAppearing(string wholeText)
        {
            _wholeText = wholeText;
            while (_sb.Length < _wholeText.Length)
            {
                _sb.Append(_wholeText[_sb.Length]);
                _target.text = _sb.ToString();
                yield return new WaitForSecondsRealtime(1 / _rateOfAppearing);
            }
            AppearingFinished?.Invoke();
        }

        private IEnumerator TriggerDisappearing()
        {
            while (_sb.Length > 0)
            {
                _sb.Remove(_sb.Length - 1, 1);
                _target.text = _sb.ToString();
                yield return new WaitForSecondsRealtime(1 / _rateOfDisappearing);
            }
            DisappearingFinished?.Invoke();
        }

        private void Awake()
        {
            _target = GetComponent<TextMeshProUGUI>();
        }
    }
}