using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    public class GroundLayerController : MonoBehaviour
    {
        [SerializeField] private LayerTrigger _topLayerTrigger = null;
        [SerializeField] private LayerTrigger _bottomLayerTrigger = null;

        public void Initialize(int depth, int worldWidth, int layerNumber)
        {
            _topLayerTrigger.Initialize(layerNumber, worldWidth);
            _bottomLayerTrigger.Initialize(layerNumber, worldWidth);
        }

    }
}