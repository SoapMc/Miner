using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

namespace Miner.Gameplay
{
    [RequireComponent(typeof(Tilemap))]
    public class GroundLayerController : MonoBehaviour
    {
        [SerializeField] private LayerTrigger _topLayerTrigger = null;
        [SerializeField] private LayerTrigger _bottomLayerTrigger = null;
        [SerializeField] private Tilemap _tilemap = null;

        [Tooltip("Manipulate with this value when triggering of any layer is visible on main camera.")]
        [SerializeField] private int _triggerOffset = 2;
        public Tilemap Tilemap => _tilemap;



        public void Initialize(GroundLayer layer, int minimumLayerDepth, int worldWidth, float cameraSize)
        {
            if (_topLayerTrigger != null)
            {
                _topLayerTrigger.Initialize(layer.LayerNumber, worldWidth, layer.PlayMusic);
                _topLayerTrigger.transform.Translate(new Vector3(0, minimumLayerDepth - cameraSize - _triggerOffset, 0));
            }
            if (_bottomLayerTrigger != null)
            {
                _bottomLayerTrigger.Initialize(layer.LayerNumber, worldWidth, layer.PlayMusic);
                _bottomLayerTrigger.transform.Translate(new Vector3(0, minimumLayerDepth - layer.Depth + cameraSize + _triggerOffset, 0));
            }
        }
    }
}