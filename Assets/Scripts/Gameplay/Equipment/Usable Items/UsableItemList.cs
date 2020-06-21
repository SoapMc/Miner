using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Usable Item List")]
    public class UsableItemList : ScriptableObject, IEnumerable<UsableItem>
    {
        [SerializeField] private List<UsableItem> _items = new List<UsableItem>();

        public int Count => _items.Count;

        public IEnumerator<UsableItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}