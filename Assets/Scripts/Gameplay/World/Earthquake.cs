using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Natural Disasters/Earthquake")]
    public class Earthquake : NaturalDisaster
    {
        [SerializeField, MinMaxFloatRange(3f, 20f)] private RangedFloat _time = new RangedFloat(5f, 10f);
        [SerializeField, MinMaxIntRange(5, 30)] private RangedInt _intensity = new RangedInt(8, 12);
        [SerializeField] private Vector2Reference _playerPosition = null;
        [SerializeField] private EarthquakeRockThrower _earthquakeRockThrowerPrefab = null;

        public float Time => Random.Range(_time.minValue, _time.maxValue);
        public int Intensity => Random.Range(_intensity.minValue, _intensity.maxValue);

        public override void Execute()
        {
            EarthquakeRockThrower thrower = Instantiate(_earthquakeRockThrowerPrefab);
            int intensity = Intensity;
            float duration = Time;
            thrower.Initialize(intensity, duration);
            _disasterEvent.Raise(new EarthquakeEA(duration, intensity));
        }
    }
}