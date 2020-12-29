using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{

    [CreateAssetMenu(menuName = "Equipment/Power Flow Factor List")]
    public class PowerFlowFactorList : ScriptableObject, IEnumerable<PowerFlowFactor>
    {
        private Dictionary<string, PowerFlowFactor> _elements = new Dictionary<string, PowerFlowFactor>();

        public int Count => _elements.Count;

        public bool IsReadOnly => false;

        public Dictionary<string, PowerFlowFactor>.ValueCollection Values => _elements.Values;

        public void Add(PowerFlowFactor item)
        {
            if(!_elements.ContainsKey(item.Name))
                _elements.Add(item.Name, item);
        }

        public void Clear()
        {
            _elements.Clear();
        }

        public bool Contains(string name)
        {
            return _elements.ContainsKey(name);
        }

        public IEnumerator GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        public bool Remove(string name)
        {
            return _elements.Remove(name);
        }

        IEnumerator<PowerFlowFactor> IEnumerable<PowerFlowFactor>.GetEnumerator()
        {
            return _elements.Values.GetEnumerator();
        }

        public PowerFlowFactor this[string key]
        {
            get => _elements[key];
        }
    }
}