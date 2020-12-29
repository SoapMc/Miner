using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Miner.Management;

namespace Miner.UI
{
    public class InputHelpPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textPrefab = null;
        private Dictionary<EInputType, TextMeshProUGUI> _texts = new Dictionary<EInputType, TextMeshProUGUI>();

        public void UpdatePanel(Dictionary<EInputType, string> newElements)
        {
            foreach(var existingText in _texts)
            {
                if (!newElements.ContainsKey(existingText.Key))
                {
                    Destroy(_texts[existingText.Key].gameObject);
                    _texts.Remove(existingText.Key);
                }
            }

            foreach(var txt in newElements)
            {
                if(!_texts.ContainsKey(txt.Key))
                {
                    TextMeshProUGUI newText = Instantiate(_textPrefab, transform);
                    newText.text = "[" + GameManager.Instance.Input.GetBindingNameOfInputType(txt.Key) + "] " +  txt.Value;
                    _texts.Add(txt.Key, newText);
                }
                else
                {
                    _texts[txt.Key].text = "[" + GameManager.Instance.Input.GetBindingNameOfInputType(txt.Key) + "] " + txt.Value;
                }
            }
        }
    }
}