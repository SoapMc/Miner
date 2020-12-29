using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.FX
{
    [CreateAssetMenu(menuName = "World/Ambient Lights/Standard Ambient Light")]
    public class StandardAmbientLight : AmbientLight
    {
        [SerializeField] private Color _color = Color.white;

        public override Color UpdateLightColor()
        {
            return _color;
        }
    }
}
