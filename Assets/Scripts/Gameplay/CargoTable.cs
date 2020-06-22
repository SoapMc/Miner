using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Miner.Management.Events;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Cargo Table")]
    public class CargoTable : ScriptableObject, ICollection<CargoTable.Element>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<TileType> _ignoredTileTypes = new List<TileType>();
        private List<Element> _content = new List<Element>();

        public int Count => _content.Count;

        public bool IsReadOnly => false;

        public void Add(Element item)
        {
            if (!_ignoredTileTypes.TrueForAll(x => x != item.Type)) return;

            foreach (var elem in _content)
            {
                if(item.Type == elem.Type)
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
                if (item.Type == elem.Type)
                {
                    if (elem.Amount >= item.Amount)
                    {
                        elem.Amount -= item.Amount;
                        if(elem.Amount == 0)
                            _content.Remove(elem);
                        return true;
                    }
                    else
                    {
                        elem.Amount = 0;
                        _content.Remove(elem);
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

        public class Element
        {
            public TileType Type;
            public int Amount;
        }

    }
}