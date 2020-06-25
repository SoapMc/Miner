using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Miner.Management.Events;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Usable Item Table")]
    public class UsableItemTable : ScriptableObject, ICollection<UsableItemTable.Element>, ISerializationCallbackReceiver
    {

        private List<Element> _content = new List<Element>();

        public int Count => _content.Count;

        public bool IsReadOnly => false;

        public void Add(Element item)
        {
            foreach (var elem in _content)
            {
                if (item.Item.Id == elem.Item.Id)
                {
                    elem.Amount += item.Amount;
                    return;
                }
            }
            _content.Add(item);
        }

        public void Clear()
        {
            _content.Clear();
        }

        public bool Contains(Element item)
        {
            return _content.Contains(item);
        }

        public void CopyTo(Element[] array, int arrayIndex)
        {
            _content.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Element> GetEnumerator()
        {
            return _content.GetEnumerator();
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            _content = new List<Element>();
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {

        }

        public bool Remove(Element item)
        {
            foreach (var elem in _content)
            {
                if (item.Item.Id == elem.Item.Id)
                {
                    if (elem.Amount >= item.Amount)
                    {
                        elem.Amount -= item.Amount;
                        return true;
                    }
                    else
                    {
                        elem.Amount = 0;
                        return false;
                    }
                }
            }
            return _content.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _content.GetEnumerator();
        }

        public Element this[int index]
        {
            get => _content[index];
        }

        public class Element
        {
            public UsableItem Item;
            public int Amount;
        }

    }
}