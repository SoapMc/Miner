using UnityEngine;
using Miner.Management.Events;

namespace Miner.FX
{
    public abstract class SoundEffect : ScriptableObject
    {
        [SerializeField] protected GameEvent _playSound = null;

        public abstract AudioClip Sound { get; }
        public abstract float Volume { get; }
        public abstract float Pitch { get; }
        public virtual bool Loop => false;

        public abstract void Play(AudioSource target = null);
        public void Play() => Play(null);

        public static implicit operator AudioClip(SoundEffect sfx) => sfx.Sound;
    }
}