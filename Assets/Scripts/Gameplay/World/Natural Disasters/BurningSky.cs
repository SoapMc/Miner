using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "World/Natural Disasters/Burning Sky")]
    public class BurningSky : NaturalDisaster
    {
        [SerializeField, MinMaxFloatRange(40f, 120f)] private RangedFloat _timeRange = new RangedFloat(40f, 80f);
        [SerializeField] private GameObject _burningSkyPrefab = null;
        [SerializeField] private Color _surfaceAmbientColor = Color.red;
        [SerializeField] private GameEvent _overrideSky = null;
        [SerializeField] private GameEvent _overrideAmbientLight = null;

        public override void End()
        {
            _overrideSky.Raise(new OverrideSkyEA(null));
            _overrideAmbientLight.Raise(new OverrideAmbientLightEA(_surfaceAmbientColor, false, false));
        }

        public override void Execute()
        {
            _time = Random.Range(_timeRange.minValue, _timeRange.maxValue);
            _overrideSky.Raise(new OverrideSkyEA(_burningSkyPrefab));
            _overrideAmbientLight.Raise(new OverrideAmbientLightEA(_surfaceAmbientColor, true, false));
        }
    }
}