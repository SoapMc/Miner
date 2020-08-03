using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Miner.Management.Events;
using System.Linq;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Cargo Table")]
    public class CargoTable : ScriptableObject, IEnumerable<CargoTable.Element>, ISerializationCallbackReceiver
    {
        [Header("Events")]
        [SerializeField] private GameEvent _resourcesGathered = null;
        [SerializeField] private GameEvent _cargoFull = null;

        [Header("Resources")]
        [SerializeField] private IntReference _maxMass = null;
        [SerializeField] private IntReference _mass = null;
        [SerializeField] private List<TileType> _ignoredTileTypes = new List<TileType>();

        private List<Element> _content = new List<Element>();

        public int Count => _content.Count;
        public bool IsReadOnly => false;
        public bool IsAcceptedType(Element item) => _ignoredTileTypes.TrueForAll(x => x != item.Type);
        public int MaxMass
        {
            get => _maxMass;
            set => _maxMass.Value = value;
        }

        public void Add(List<Element> items)
        {
            List<Element> acceptedResources = (from res in items where IsAcceptedType(res) select res).OrderByDescending(x => x.Amount * x.Type.Value).ToList();
            List<Element> addedResources = new List<Element>();

            foreach (var item in acceptedResources)
            {
                int amount = item.Amount;
                while (amount * item.Type.Mass > (_maxMass - _mass))
                {
                    amount--;
                }

                if (amount > 0)
                {
                    Element containedResourceType = _content.FirstOrDefault(x => item.Type == x.Type);
                    if(containedResourceType != null)
                    {
                        containedResourceType.Amount += amount;
                        _mass.Value += amount * item.Type.Mass;
                    }
                    else
                    {
                        _mass.Value += amount * item.Type.Mass;
                        _content.Add(new Element() { Type = item.Type, Amount = amount });
                    }
                    addedResources.Add(new Element() { Type = item.Type, Amount = amount });
                }
                else
                {
                    _cargoFull.Raise();
                    return;
                }
            }
            if(acceptedResources.Count > 0)
                _resourcesGathered.Raise(new ResourcesGatheredEA(addedResources));
        }

        public void Clear()
        {
            Remove(_content);
            _content.Clear();
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

        public bool Remove(List<Element> items)
        {
            foreach (var resourceToRemove in items.ToList())
            {
                if (resourceToRemove.Amount > 0)
                {
                    Element containedResourceType = _content.FirstOrDefault(x => resourceToRemove.Type == x.Type);
                    if (containedResourceType != null)
                    {
                        if (containedResourceType.Amount >= resourceToRemove.Amount)
                        {
                            int amount = resourceToRemove.Amount;
                            containedResourceType.Amount -= resourceToRemove.Amount;
                            _mass.Value -= amount * resourceToRemove.Type.Mass;
                            if (containedResourceType.Amount == 0)
                                _content.Remove(containedResourceType);
                        }
                        else
                            return false;
                    }
                    else
                        return false;
                }
            }
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _content.GetEnumerator();
        }

        public List<Element> Get() => _content;

        public class Element
        {
            public TileType Type;
            public int Amount;
        }

    }
}