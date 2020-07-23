using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using System;
using Miner.Management.Events;
using Miner.Management.Exceptions;

namespace Miner.UI
{
    public class PlayerAttachedInfoDisplay : MonoBehaviour
    {
        [Range(0.5f, 5f), SerializeField] private float _timeOfDisplayingFullText = 2f;
        [Range(5f, 100f), SerializeField] private float _rateOfAppearing = 10;
        [Range(5f, 100f), SerializeField] private float _rateOfDisappearing = 50;

        private TextMeshProUGUI _text = null;
        const int MAX_LENGTH = 48;
        private StringBuilder _sb = new StringBuilder(MAX_LENGTH);
        private string _wholeText = string.Empty;
        private Coroutine _coroutine = null;

        private IEnumerator TriggerAppearing()
        {
            while(_sb.Length < _wholeText.Length)
            {
                _sb.Append(_wholeText[_sb.Length]);
                _text.text = _sb.ToString();
                yield return new WaitForSeconds(1 / _rateOfAppearing);
            }
            _wholeText = string.Empty;

            yield return new WaitForSeconds(_timeOfDisplayingFullText);

            _coroutine = StartCoroutine(TriggerDisappearing());
        }

        private IEnumerator TriggerDisappearing()
        {
            while (_sb.Length > 0)
            {
                _sb.Remove(_sb.Length - 1, 1);
                _text.text = _sb.ToString();
                yield return new WaitForSeconds(1 / _rateOfDisappearing);
            }

            if (_wholeText != string.Empty)
            {
                _coroutine = StartCoroutine(TriggerAppearing());
            }
            else
                _coroutine = null;
        }

        public void OnAddResourceToCargo(EventArgs args)
        {
            if (args is AddResourceToCargoEA artc)
            {
                if (artc.Resource.Type.ShowBriefInfoOnDig)
                {

                    if(!artc.Resource.Type.IsFuel)
                        _wholeText = "+" + artc.Resource.Amount.ToString() + " " + artc.Resource.Type.Name + " (" + artc.Resource.Type.Value + " $)";
                    else
                        _wholeText = artc.Resource.Type.Mass.ToString() + " l of fuel added to fuel supplies";

                    if (artc.IsLost)
                        _wholeText += " LOST!";

                    Show();
                }
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        public void OnCargoFull()
        {
            _wholeText = "Cargo is full!";
            Show();
        }

        public void OnUseItem(EventArgs args)
        {
            if(args is UseItemEA ui)
            {
                _wholeText = ui.Item.Name + " has been used";
                Show();
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        private void Show()
        {
            if (_coroutine == null)
            {
                _coroutine = StartCoroutine(TriggerAppearing());
            }
            else
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
                _coroutine = StartCoroutine(TriggerDisappearing());
            }
        }

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _text.text = string.Empty;
        }
    }
}