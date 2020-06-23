using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Miner.Management.Events;

namespace Miner.Gameplay
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class LayerTrigger : MonoBehaviour
    {
        [SerializeField] private GameEvent _triggerTopLayer = null;
        [SerializeField] private GameEvent _triggerBottomLayer = null;
        [SerializeField] private bool _topLayerActivity = false;
        [SerializeField] private bool _bottomLayerActivity = false;
        private BoxCollider2D _collider = null;
        private int _layerNumber = 0;
        private int _triggerThickness = 5;

        public void Initialize(int layerNumber, int worldWidth)
        {
            _collider.size = new Vector2(worldWidth, _triggerThickness);
            transform.position = new Vector3(worldWidth / 2f, 0, 0);
            _layerNumber = layerNumber;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.GetComponent<PlayerController>() != null)
            {
               _triggerTopLayer.Raise(new LayerTriggerEA(_layerNumber, _topLayerActivity));
               _triggerBottomLayer.Raise(new LayerTriggerEA(_layerNumber, _bottomLayerActivity));
            }
        }

        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
        }
    }
}