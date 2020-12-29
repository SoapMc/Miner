using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Exceptions;
using Miner.Management.Events;
using System;

namespace Miner.Management
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource _SFXPlayer = null;
        [SerializeField, Range(0.1f, 1f)] private float _transitionRate = 0.5f;
        [SerializeField, Range(0f, 1f)] private float _volume = 0.5f;

        private List<LinkedAudioSource> _musicPlayers = new List<LinkedAudioSource>();
        private LinkedAudioSource _currentlyUsedPlayer = null;
        private AudioClip _currentlyPlayedMusic = null;
        private Coroutine _musicDownTransitionCoroutine = null;
        private Coroutine _musicUpTransitionCoroutine = null;

        public void OnPlaySound(EventArgs args)
        {
            if(args is PlaySoundEA ps)
            {
                AudioClip sound = ps.SFX.Sound;
                _SFXPlayer.pitch = ps.SFX.Pitch;
                if (sound != null)
                {

                    if (ps.Target == null)
                        _SFXPlayer.PlayOneShot(sound, ps.SFX.Volume);
                    else
                    {
                        if (!ps.SFX.Loop)
                            ps.Target.PlayOneShot(sound, ps.SFX.Volume);
                        else
                        {
                            ps.Target.clip = sound;
                            ps.Target.volume = ps.SFX.Volume;
                            ps.Target.pitch = ps.SFX.Pitch;
                            ps.Target.Play();
                        }
                    }
                    
                }
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        public void OnPlayMusic(EventArgs args)
        {
            if (args is PlayMusicEA pm)
            {
                if (_currentlyPlayedMusic != pm.Music)
                {
                    if(_musicDownTransitionCoroutine != null)
                    {
                        StopCoroutine(_musicDownTransitionCoroutine);
                        _musicDownTransitionCoroutine = null;
                    }
                    if(_musicUpTransitionCoroutine != null)
                    {
                        StopCoroutine(_musicUpTransitionCoroutine);
                        _musicUpTransitionCoroutine = null;
                    }
                    _currentlyPlayedMusic = pm.Music;
                    _musicDownTransitionCoroutine = StartCoroutine(VolumeDown(_currentlyUsedPlayer, pm.Music));
                }
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        private IEnumerator VolumeDown(LinkedAudioSource musicPlayer, AudioClip musicClip)
        {
            _musicUpTransitionCoroutine = StartCoroutine(VolumeUp(musicPlayer.Next, musicClip));
            do
            {
                musicPlayer.Source.volume -= Time.fixedDeltaTime * _transitionRate;
                yield return null;
            }
            while(musicPlayer.Source.volume > 0f);
            musicPlayer.Source.Stop();
            _musicDownTransitionCoroutine = null;
        }

        private IEnumerator VolumeUp(LinkedAudioSource musicPlayer, AudioClip musicClip)
        {
            musicPlayer.Source.clip = musicClip;
            musicPlayer.Source.Play();
            do
            {
                musicPlayer.Source.volume += Time.fixedDeltaTime * _transitionRate;
                yield return null;
            }
            while (musicPlayer.Source.volume < _volume);
            _currentlyUsedPlayer = musicPlayer;
            _musicUpTransitionCoroutine = null;
        }

        private void Awake()
        {
            AudioSource[] audioSources = GetComponents<AudioSource>();
            for(int i = 0; i < audioSources.Length; ++i)
            {
                _musicPlayers.Add(new LinkedAudioSource(audioSources[i]));
            }

            int i_next;
            for(int i = 0; i < _musicPlayers.Count; ++i)
            {
                _musicPlayers[i].Source.loop = true;
                i_next = i + 1;
                if (i_next >= _musicPlayers.Count)
                    i_next = 0;
                _musicPlayers[i].Next = _musicPlayers[i_next];
            }

            if (_musicPlayers.Count == 0)
                throw new InvalidSettingException("Cannot find any music player");
            _currentlyUsedPlayer = _musicPlayers[0];
        }

        private class LinkedAudioSource
        {
            public readonly AudioSource Source = null;
            public LinkedAudioSource Next = null;

            public LinkedAudioSource(AudioSource audioSource)
            {
                Source = audioSource;
            }
        }
    }
}