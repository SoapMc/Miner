using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Ground Layer List")]
    public class GroundLayerList : ScriptableObject, ICollection<GroundLayer>
    { 
        [SerializeField] private List<GroundLayer> _layers = new List<GroundLayer>();

        public int Count => _layers.Count;

        public bool IsReadOnly => false;

        public void Add(GroundLayer item)
        {
            _layers.Add(item);
        }

        public void Clear()
        {
            _layers.Clear();
        }

        public bool Contains(GroundLayer item)
        {
            return _layers.Contains(item);
        }

        public void CopyTo(GroundLayer[] array, int arrayIndex)
        {
            _layers.CopyTo(array, arrayIndex);
        }

        public IEnumerator<GroundLayer> GetEnumerator()
        {
            return _layers.GetEnumerator();
        }

        public bool Remove(GroundLayer item)
        {
            return _layers.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _layers.GetEnumerator();
        }

        public GroundLayer this[int index]
        {
            get => _layers[index];
        }
    }
}