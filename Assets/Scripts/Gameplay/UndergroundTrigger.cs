using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    public class UndergroundTrigger : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D _undergroundDisable = null;
        [SerializeField] private BoxCollider2D _undergroundEnable = null;
        [SerializeField] private float _hysteresis = 3f;

        public void Initialize(int undergroundDepth, int worldWidth)
        {
            _undergroundDisable.size = new Vector2(worldWidth, 1);
            _undergroundDisable.transform.position = new Vector3(worldWidth / 2f, undergroundDepth, 0);
            _undergroundEnable.size = new Vector2(worldWidth, 1);
            _undergroundEnable.transform.position = new Vector3(worldWidth / 2f, undergroundDepth - _hysteresis, 0);
        }
    }
}