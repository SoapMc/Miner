using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using System.Linq;

namespace Miner.FX
{
    public class AmbientLightLayerController
    {
        private AmbientLightController _ambientLightController = null;
        private List<AmbientLight> _lightings = new List<AmbientLight>();
        private AmbientLight _defaultAmbientLight = null;
        private Color _currentColor = Color.white;
        private Coroutine _changeLightCoroutine = null;
        private AmbientLight _currentlySetLight = null;

        public Color CurrentColor => _currentColor;

        public AmbientLightLayerController(AmbientLightController ambientLightController, AmbientLight defaultAmbientLight)
        {
            _ambientLightController = ambientLightController;
            _defaultAmbientLight = defaultAmbientLight;
            _currentlySetLight = _defaultAmbientLight;
            _currentColor = _defaultAmbientLight.UpdateLightColor();
        }

        public void ChangeLighting(ChangeAmbientLightEA.AmbientLightSetting ambientLightSetting)
        {
            if(ambientLightSetting.LightToAdd != null)
            {
                if (ambientLightSetting.ChangeMode == ChangeAmbientLightEA.EChangeMode.Override)
                    _lightings.Clear();
                if(!_lightings.Exists(x => x == ambientLightSetting.LightToAdd))
                    _lightings.Add(ambientLightSetting.LightToAdd);
            }

            if(ambientLightSetting.LightToRemove != null)
            {
                _lightings.Remove(ambientLightSetting.LightToRemove);
            }

            _lightings = _lightings.OrderByDescending(x => x.Priority).ToList();

            AmbientLight target = null;
            if (_lightings.Count > 0)
                target = _lightings.First();
            else
                target = _defaultAmbientLight;

            if(_changeLightCoroutine != null)
            {
                if(_currentlySetLight != target)
                {
                    _ambientLightController.StopCoroutine(_changeLightCoroutine);
                    _changeLightCoroutine = _ambientLightController.StartCoroutine(ChangeLightFluently(_currentColor, target));
                }
            }
            else
            {
                _changeLightCoroutine = _ambientLightController.StartCoroutine(ChangeLightFluently(_currentColor, target));
            }
        }

        private IEnumerator ChangeLightFluently(Color startingColor, AmbientLight next)
        {
            float lerpCoeff = 0f;
            _currentlySetLight = next;
            while (lerpCoeff < 1f)
            {
                lerpCoeff += Time.unscaledDeltaTime;
                _currentColor = Color.Lerp(startingColor, next.UpdateLightColor(), lerpCoeff);
                yield return null;
            }
            _changeLightCoroutine = null;
        }

        public void UpdateColor()
        {
            if(_changeLightCoroutine == null)
                _currentColor = _currentlySetLight.UpdateLightColor();
        }
    }
}