using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.FX
{
    [CreateAssetMenu(menuName = "World/Ambient Lights/Underground Ambient Light")]
    public class UndergroundAmbientLight : AmbientLight
    {
        [SerializeField] private Color _topDepthColor = Color.white;
        [SerializeField] private Color _botDepthColor = Color.white;
        [SerializeField] private FloatReference _playerLayerRelativePosition = null;

        public override Color UpdateLightColor()
        {
            Color output = Color.Lerp(_topDepthColor, _botDepthColor, _playerLayerRelativePosition);
            return output;
        }
    }
}