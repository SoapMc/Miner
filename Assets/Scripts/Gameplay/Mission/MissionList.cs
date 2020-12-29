using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "World/Mission List")]
    public class MissionList : ScriptableObject, ICollection<Mission>
    {
        [System.NonSerialized] private List<Mission> _content = new List<Mission>();

        public List<Mission> Content => _content;

        public int Count => _content.Count;

        public bool IsReadOnly => false;

        public void Add(Mission item)
        {
            _content.Add(item);
        }

        public void Clear()
        {
            _content.Clear();
        }

        public bool Contains(Mission item)
        {
            return _content.Contains(item);
        }

        public void CopyTo(Mission[] array, int arrayIndex)
        {
            _content.CopyTo(array, arrayIndex);
        }

        public Mission Find(System.Predicate<Mission> predicate)
        {
            return _content.Find(predicate);
        }

        public IEnumerator<Mission> GetEnumerator()
        {
            return _content.GetEnumerator();
        }

        public bool Remove(Mission item)
        {
            return _content.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _content.GetEnumerator();
        }

        public Mission this[int index]
        {
            get => _content[index];
        }

        private void OnDestroy()
        {
            foreach(var mission in _content)
            {
                mission.Dispose();
            }
            _content.Clear();
        }
    }
}