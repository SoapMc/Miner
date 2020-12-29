using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;

namespace Miner.Gameplay
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class LayerBorders : MonoBehaviour
    {
        [SerializeField] private GameEvent _playerCameToLayer = null;
        [SerializeField] private IntReference _playerLayer = null;
        [SerializeField] private GroundLayer _layer;

        public void Initialize(GroundLayer layer)
        {
            _layer = layer;
        }

        public void Initialize(GroundLayer layer, int minimumLayerDepth, int width)
        {
            _layer = layer;
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            collider.size = new Vector2(width, layer.Depth);
            collider.transform.position = new Vector2(collider.size.x / 2f, -collider.size.y / 2f + minimumLayerDepth + 1);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject != null)
            {
                if(collision.gameObject.GetComponent<PlayerController>() != null)
                {
                    if (_playerLayer.Value != _layer.LayerNumber)
                        _playerCameToLayer.Raise(new PlayerCameToLayerEA(_layer));
                }
            }
        }
    }
}