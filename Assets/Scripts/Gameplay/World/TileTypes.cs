using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "World/Tile Types")]
    public class TileTypes : ScriptableObject, ICollection<TileType>
    {
        [SerializeField] private List<TileType> _content = new List<TileType>();

        public int Count => _content.Count;

        public bool IsReadOnly => false;

        public void Add(TileType item)
        {
            _content.Add(item);
        }

        public void Clear()
        {
            _content.Clear();
        }

        public bool Contains(TileType item)
        {
            return _content.Contains(item);
        }

        public void CopyTo(TileType[] array, int arrayIndex)
        {
            _content.CopyTo(array, arrayIndex);
        }

        public IEnumerator<TileType> GetEnumerator()
        {
            return _content.GetEnumerator();
        }

        public bool Remove(TileType item)
        {
            return _content.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _content.GetEnumerator();
        }

        public TileType GetTileType(int id)
        {
            return _content.FirstOrDefault(x => x.Id == id);
        }
    }
}