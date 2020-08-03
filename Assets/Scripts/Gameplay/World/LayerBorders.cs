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
        [SerializeField] private bool _initializedInEditor = false;
        private int _layerNumber;

        public void Initialize(int layerNumber, int minimumLayerDepth, int depth, int width)
        {
            if (!_initializedInEditor)
            {
                _layerNumber = layerNumber;
                BoxCollider2D collider = GetComponent<BoxCollider2D>();
                collider.size = new Vector2(width, depth);
                collider.transform.position = new Vector2(collider.size.x / 2f, -collider.size.y / 2f + minimumLayerDepth + 1);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject != null)
            {
                if(collision.gameObject.GetComponent<PlayerController>() != null)
                {
                    if (_playerLayer.Value != _layerNumber)
                        _playerCameToLayer.Raise(new PlayerCameToLayerEA(_layerNumber));
                }
            }
        }
    }
}