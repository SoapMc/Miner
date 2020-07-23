using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Miner.Management.Events;

namespace Miner.FX
{
    [CreateAssetMenu(menuName = "Audio/Varying Sound Effect")]
    public class VaryingSoundEffect : SoundEffect
    {
        [SerializeField] private List<AudioClip> _soundList = null;
        [SerializeField, MinMaxFloatRange(0f, 2f)] private RangedFloat _volumeRange = new RangedFloat() { minValue = 1f, maxValue = 1f };
        [SerializeField, MinMaxFloatRange(0.3f, 2f)] private RangedFloat _pitchRange = new RangedFloat() { minValue = 1f, maxValue = 1f };

        public List<AudioClip> SoundList => _soundList;
        public RangedFloat VolumeRange => _volumeRange;
        public RangedFloat PitchRange => _pitchRange;
        public override AudioClip Sound
        {
            get
            {
                try
                {
                    return _soundList[Random.Range(0, _soundList.Count)];
                }
                catch (IndexOutOfRangeException)
                {
                    return null;
                }
            }
        }
        public override float Volume => Random.Range(_volumeRange.minValue, _volumeRange.maxValue);
        public override float Pitch => Random.Range(_pitchRange.minValue, _pitchRange.maxValue);

        public override void Play(AudioSource target)
        {
            if (_playSound != null)
                _playSound.Raise(new PlaySoundEA(this, target));
            else
                Debug.LogError("PlaySound is null!");
        }
    }
}
