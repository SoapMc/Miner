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

        public void OnResourceGathered(EventArgs args)
        {
            if (args is ResourcesGatheredEA rg)
            {
                _wholeText = string.Empty;
                foreach(var gatheredResource in rg.Resources)
                {
                    _wholeText += "+" + gatheredResource.Amount.ToString() + " " + gatheredResource.Type.Name + " (" + gatheredResource.Type.Value + " $)\n";
                }

                Show();
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        public void OnResourceLost(EventArgs args)
        {
            if (args is ResourcesLostEA rl)
            {
                _wholeText = string.Empty;
                foreach (var lostResource in rl.Resources)
                {
                    if (!lostResource.Type.IsFuel)
                        _wholeText += lostResource.Amount.ToString() + " " + lostResource.Type.Name + " LOST\n";
                    else
                        _wholeText += lostResource.Type.Mass.ToString() + " L of fuel LOST\n";
                }

                Show();
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        public void OnUpdateInfrastructureData(EventArgs args)
        {
            if(args is UpdateInfrastructureEA ui)
            {
                if (ui.FuelSupplyChange > 0)
                {
                    _wholeText += ui.FuelSupplyChange.ToString() + " L of fuel added to fuel supplies\n";
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
            _wholeText = "Cargo is full!\n";
            Show();
        }

        public void OnUseItem(EventArgs args)
        {
            if(args is UseItemEA ui)
            {
                _wholeText = ui.Item.Name + " has been used\n";
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