using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Ground Layer")]
    public class GroundLayer : ScriptableObject
    {
        public int Depth = 0;
        [Range(0f, 1f)] public float ProbabilityOfEmptySpaces = 0.01f;
        public Color _backgroundColor = Color.white;
        public List<TileType> DefaultTiles = new List<TileType>();
        public List<Element> Resources = new List<Element>();

        [System.Serializable]
        public class Element
        {
            public TileType Type;
            [Range(0f, 1f)] public float Probability;
        }
    }
}