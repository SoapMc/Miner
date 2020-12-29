using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using Miner.Gameplay;
using System.Linq;

using RemoveSkyEA = Miner.Management.Events.ChangeSkyEA;

namespace Miner.FX
{
    public class SkyController : MonoBehaviour
    {
        [SerializeField] private IntReference _timeOfDay = null;
        [SerializeField] private Sky _dayAndNightSkyPrefab = null;

        private List<Sky> _skies = new List<Sky>();
        private List<string> _skiesToRemove = new List<string>();
        private Color _transparent = new Color(1f, 1f, 1f, 0f);
        private Coroutine _changeSkyCoroutine = null;
        
        public void OnChangeSky(EventArgs args)
        {
            if(args is ChangeSkyEA cs)
            {
                if(cs.Sky == null)
                {
                    Management.Log.Instance.WriteException(new ArgumentNullException());
                    return;
                }

                if(!_skies.Exists(x => x.Name == cs.Sky.Name))
                {
                    Sky newSky = Instantiate(cs.Sky, transform);
                    newSky.Color = _transparent;
                    _skies.Add(newSky);
                }

                SelectProperSky();
            }
            else
            {
                Management.Log.Instance.WriteException(new InvalidEventArgsException(args.ToString()));
            }
        }

        public void OnRemoveSky(EventArgs args)
        {
            if (args is RemoveSkyEA cs)
            {
                if (cs.Sky == null)
                {
                    Management.Log.Instance.WriteException(new ArgumentNullException());
                    return;
                }

                if(cs.Sky.Name == _dayAndNightSkyPrefab.Name)
                {
                    Management.Log.Instance.Write(_dayAndNightSkyPrefab.Name + " cannot be removed from " + GetType().ToString());
                    return;
                }

                if (_skies.Exists(x => x.Name == cs.Sky.Name) && !_skiesToRemove.Exists(x => x == cs.Sky.Name))
                {
                    _skiesToRemove.Add(cs.Sky.Name);
                }

                SelectProperSky();
            }
            else
            {
                Management.Log.Instance.WriteException(new InvalidEventArgsException(args.ToString()));
            }
        }

        public void OnPlayerReset()
        {
            if(_changeSkyCoroutine != null)
            {
                StopCoroutine(_changeSkyCoroutine);
                _changeSkyCoroutine = null;
            }

            for (int i = _skies.Count - 1; i >= 0; --i)
            {
                if (_skies[i].Name != _dayAndNightSkyPrefab.Name)
                {
                    _skiesToRemove.Remove(_skies[i].Name);
                    _timeOfDay.ValueChanged -= _skies[i].OnMinuteElapsed;
                    Destroy(_skies[i].gameObject);
                    _skies.Remove(_skies[i]);
                }
            }
        }

        public void OnPlayerInstantiated()
        {
            AddSkyImmediately(_dayAndNightSkyPrefab);
        }

        private void AddSkyImmediately(Sky sky)
        {
            if (!_skies.Exists(x => x.Name == sky.Name))
            {
                Sky instantiatedSky = Instantiate(sky, transform);
                _timeOfDay.ValueChanged += instantiatedSky.OnMinuteElapsed;
                _skies.Add(instantiatedSky);
            }
            SelectProperSky();
        }

        private void SelectProperSky()
        {
            Sky currentlySelectedSky = _skies.FirstOrDefault();
            _skies = _skies.OrderByDescending(x => x.Priority).ToList();
            Sky nextSelectedSky = currentlySelectedSky;

            for(int i = 0; i < _skies.Count; ++i)
            {
                if (IsSkyToRemove(_skies[i].Name)) continue;
                nextSelectedSky = _skies[i];
                break;
            }

            if (currentlySelectedSky != null && nextSelectedSky != null && currentlySelectedSky != nextSelectedSky && _changeSkyCoroutine == null)
            {
                _timeOfDay.ValueChanged -= currentlySelectedSky.OnMinuteElapsed;
                _timeOfDay.ValueChanged += nextSelectedSky.OnMinuteElapsed;
                _changeSkyCoroutine = StartCoroutine(ChangeSkyFluently(currentlySelectedSky, nextSelectedSky));
            }
        }

        private bool IsSkyToRemove(string name)
        {
            for(int i = 0; i < _skiesToRemove.Count; ++i)
            {
                if (name == _skiesToRemove[i])
                    return true;
            }
            return false;
        }

        #region COROUTINES
        private IEnumerator ChangeSkyFluently(Sky previousSky, Sky nextSky)
        {
            float lerpCoeff = 0f;
            while (lerpCoeff < 1f)
            {
                lerpCoeff += Time.deltaTime;
                nextSky.Color = Color.Lerp(_transparent, Color.white, lerpCoeff);
                previousSky.Color = Color.Lerp(Color.white, _transparent, lerpCoeff);
                yield return null;
            }

            for (int i = _skies.Count - 1; i >= 0; --i)
            {
                if (IsSkyToRemove(_skies[i].Name) && _skies[i] != nextSky)
                {
                    _skiesToRemove.Remove(_skies[i].Name);
                    _timeOfDay.ValueChanged -= _skies[i].OnMinuteElapsed;
                    Destroy(_skies[i].gameObject);
                    _skies.Remove(_skies[i]);
                }
            }

            _changeSkyCoroutine = null;
            SelectProperSky();
        }
        #endregion

        #region UNITY CALLBACKS

        private void OnDestroy()
        {
            if(_changeSkyCoroutine != null)
                StopCoroutine(_changeSkyCoroutine);
        }
        #endregion
    }
}