using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "World/Natural Disasters/Earthquake")]
    public class Earthquake : NaturalDisaster
    {
        [SerializeField, MinMaxFloatRange(3f, 20f)] private RangedFloat _timeRange = new RangedFloat(5f, 10f);
        [SerializeField, MinMaxIntRange(5, 30)] private RangedInt _intensityRange = new RangedInt(8, 12);
        [SerializeField] private EarthquakeRockThrower _earthquakeRockThrowerPrefab = null;
        EarthquakeRockThrower _earthquakeRockThrowerInstance = null;
        private int _intensityForInstance;

        public override void End()
        {
            Destroy(_earthquakeRockThrowerInstance.gameObject);
        }

        public override void Execute()
        {
            _earthquakeRockThrowerInstance = Instantiate(_earthquakeRockThrowerPrefab);
            _intensityForInstance = Random.Range(_intensityRange.minValue, _intensityRange.maxValue);
            _time = Random.Range(_timeRange.minValue, _timeRange.maxValue);
            _earthquakeRockThrowerInstance.Initialize(_intensityForInstance);
            _disasterEvent.Raise(new EarthquakeEA(_time, _intensityForInstance));
        }
    }
}