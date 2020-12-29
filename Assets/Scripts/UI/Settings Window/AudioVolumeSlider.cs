using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Miner.Management.Exceptions;
using Miner.Management;

namespace Miner.UI
{
    [RequireComponent(typeof(Slider))]
    public class AudioVolumeSlider : MonoBehaviour
    {

        [SerializeField] private AudioMixer _audioMixer = null;
        [SerializeField] private string _volumeParameterName = string.Empty;
        private Slider _slider = null;

        private void Start()
        {
            _slider = GetComponent<Slider>();
            if (_audioMixer.GetFloat(_volumeParameterName, out float volume))
                _slider.SetValueWithoutNotify(Mathf.Pow(10, volume/20f));
            else
                Log.Instance.WriteException(new InvalidSettingException());
        }

        public void OnValueChanged(float volume)
        {
            _audioMixer.SetFloat(_volumeParameterName, Mathf.Log10(volume) * 20);
        }
    }
}