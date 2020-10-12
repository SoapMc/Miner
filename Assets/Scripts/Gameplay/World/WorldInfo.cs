using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "World/World Info")]
    public class WorldInfo : ScriptableObject
    {
        private Vector2Int _verticalBorders = Vector2Int.zero;
        private Vector2Int _horizontalBorders = Vector2Int.zero;
        private Vector2 _loadableGridHorizontalRange = Vector2Int.zero;
        private Vector2 _loadableGridVerticalRange = Vector2Int.zero;

        public Vector2Int VerticalBorders { get => _verticalBorders; set => _verticalBorders = value; }
        public Vector2Int HorizontalBorders { get => _horizontalBorders; set => _horizontalBorders = value; }
        public Vector2 LoadableGridHorizontalRange { get => _loadableGridHorizontalRange; set => _loadableGridHorizontalRange = value; }
        public Vector2 LoadableGridVerticalRange { get => _loadableGridVerticalRange; set => _loadableGridVerticalRange = value; }

    }
}