using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Miner.Management.Events;

namespace Miner.FX
{
    [CreateAssetMenu(menuName = "Audio/Looping Sound Effect")]
    public class LoopingSoundEffect : SoundEffect
    {
        [SerializeField] private AudioClip _sound = null;
        [SerializeField, Range(0f, 2f)] private float _volume = 1f;
        [SerializeField, Range(0.3f, 2f)] private float _pitch = 1f;

        public override AudioClip Sound => _sound;
        public override float Volume => _volume;
        public override float Pitch => _pitch;
        public override bool Loop => true;

        public override void Play(AudioSource target)
        {
            if (_playSound != null)
                _playSound.Raise(new PlaySoundEA(this, target));
            else
                Debug.LogError("PlaySound is null!");
        }
    }
}
