using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using Miner.FX;
using RemoveSkyEA = Miner.Management.Events.ChangeSkyEA;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "World/Natural Disasters/Burning Sky")]
    public class BurningSky : NaturalDisaster
    {
        [SerializeField, MinMaxFloatRange(40f, 120f)] private RangedFloat _timeRange = new RangedFloat(40f, 80f);
        [SerializeField] private Sky _burningSkyPrefab = null;
        [SerializeField] private AmbientLight _ambientLight = null;
        [SerializeField] private GameEvent _changeSky = null;
        [SerializeField] private GameEvent _removeSky = null;
        [SerializeField] private GameEvent _changeAmbientLight = null;

        public override void Execute()
        {
            _time = Random.Range(_timeRange.minValue, _timeRange.maxValue);
            _changeSky.Raise(new ChangeSkyEA(_burningSkyPrefab));
            _changeAmbientLight.Raise(new ChangeAmbientLightEA() { SurfaceLighting = new ChangeAmbientLightEA.AmbientLightSetting() { LightToAdd = _ambientLight, ChangeMode = ChangeAmbientLightEA.EChangeMode.Stack } });
        }

        public override void End()
        {
            _removeSky.Raise(new RemoveSkyEA(_burningSkyPrefab));
            _changeAmbientLight.Raise(new ChangeAmbientLightEA() { SurfaceLighting = new ChangeAmbientLightEA.AmbientLightSetting() { LightToRemove = _ambientLight} });
        }
    }
}