using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Miner.Management.Events;
using System;

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
        private Action _playMusicAction = null;

        public void Initialize(int layerNumber, int worldWidth, Action playMusicAction = null)
        {
            if(_collider == null)
                _collider = GetComponent<BoxCollider2D>();

            _collider.size = new Vector2(worldWidth, _triggerThickness);
            transform.position = new Vector3(worldWidth / 2f, 0, 0);
            _layerNumber = layerNumber;
            _playMusicAction = playMusicAction;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.GetComponent<PlayerController>() != null)
            {
               _triggerTopLayer.Raise(new LayerTriggerEA(_layerNumber, _topLayerActivity));
               _triggerBottomLayer.Raise(new LayerTriggerEA(_layerNumber, _bottomLayerActivity));
               _playMusicAction?.Invoke();
            }
        }
    }
}