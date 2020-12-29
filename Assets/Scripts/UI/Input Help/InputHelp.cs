using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management;
using System.Linq;
using Miner.Management.Events;
using System;

namespace Miner.UI
{
    [CreateAssetMenu(menuName = "UI/InputHelp")]
    public class InputHelp : ScriptableObject
    {
        [SerializeField] private GameEvent _showInputHelp = null;
        [SerializeField] private GameEvent _hideInputHelp = null;
        [SerializeField] private List<InputHelpField> _elements = new List<InputHelpField>();
        [NonSerialized] private Dictionary<EInputType, string> _elementsForEvent = new Dictionary<EInputType, string>();

        public void Show()
        {
            if (_showInputHelp != null)
            {
                _showInputHelp.Raise(new ShowInputHelpEA(_elementsForEvent));
            }
            else
                Management.Log.Instance.WriteException(new ArgumentNullException());
        }

        public void Hide()
        {
            if (_hideInputHelp != null)
                _hideInputHelp.Raise();
            else
                Management.Log.Instance.WriteException(new ArgumentNullException());
        }

        private void OnEnable()
        {
            for (int i = 0; i < _elements.Count; ++i)
            {
                if(!_elementsForEvent.ContainsKey(_elements[i].InputType))
                    _elementsForEvent.Add(_elements[i].InputType, _elements[i].CommandName);
            }
        }
    }
}