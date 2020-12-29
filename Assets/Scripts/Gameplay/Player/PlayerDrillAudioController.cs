using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.FX
{
    public class PlayerDrillAudioController : MonoBehaviour
    {
        [SerializeField] private AudioSource _drillAudio = null;
        private Coroutine _delayAudioStopCoroutine = null;
        private bool _isDigging = false;

        public void OnAllowDig()
        {
            if(!_drillAudio.isPlaying)
                _drillAudio.Play();
            _isDigging = true;
        }

        public void OnDigCompleted()
        {
            if (_delayAudioStopCoroutine != null)
                StopCoroutine(_delayAudioStopCoroutine);
            
            _delayAudioStopCoroutine = StartCoroutine(DelayAudioStop());
            _isDigging = false;
        }

        private IEnumerator DelayAudioStop()
        {
            yield return new WaitForSeconds(0.3f);
            if (!_isDigging)
            {
                _drillAudio.Stop();
            }
            
            _delayAudioStopCoroutine = null;
        }
    }
}