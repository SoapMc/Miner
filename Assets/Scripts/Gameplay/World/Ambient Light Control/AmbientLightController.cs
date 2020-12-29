using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using Miner.Management;
using System.Linq;
using Miner.Gameplay;

namespace Miner.FX
{
    public class AmbientLightController : MonoBehaviour
    {
        [SerializeField] private AmbientLight _defaultAmbientLight = null;
        private AmbientLightLayerController _surfaceLightingController = null;
        private AmbientLightLayerController _undergroundLightingController = null;
        private GroundLayer _visitedGroundLayer = null;
        private Coroutine _changeControllerCoroutine = null;
        private float _lerpCoefficient = 0f;

        public void OnPlayerCameToLayer(EventArgs args)
        {
            if (args is PlayerCameToLayerEA pctl)
            {
                if (_visitedGroundLayer != null)
                {
                    if (_visitedGroundLayer.AreaType == GroundLayer.EAreaType.Surface)
                        _surfaceLightingController.ChangeLighting(new ChangeAmbientLightEA.AmbientLightSetting { LightToRemove = _visitedGroundLayer.AmbientLight });
                    else if (_visitedGroundLayer.AreaType == GroundLayer.EAreaType.Underground)
                        _undergroundLightingController.ChangeLighting(new ChangeAmbientLightEA.AmbientLightSetting { LightToRemove = _visitedGroundLayer.AmbientLight });
                }

                _visitedGroundLayer = pctl.GroundLayer;

                if (_visitedGroundLayer != null)
                {
                    if (_visitedGroundLayer.AreaType == GroundLayer.EAreaType.Surface)
                    {
                        _surfaceLightingController.ChangeLighting(new ChangeAmbientLightEA.AmbientLightSetting { LightToAdd = pctl.GroundLayer.AmbientLight, ChangeMode = ChangeAmbientLightEA.EChangeMode.Stack });
                        if (_changeControllerCoroutine != null)
                            StopCoroutine(_changeControllerCoroutine);
                        _changeControllerCoroutine = StartCoroutine(ChangeToSurface());
                    }
                    else if (_visitedGroundLayer.AreaType == GroundLayer.EAreaType.Underground)
                    {
                        _undergroundLightingController.ChangeLighting(new ChangeAmbientLightEA.AmbientLightSetting { LightToAdd = pctl.GroundLayer.AmbientLight, ChangeMode = ChangeAmbientLightEA.EChangeMode.Stack });
                        if (_changeControllerCoroutine != null)
                            StopCoroutine(_changeControllerCoroutine);
                        _changeControllerCoroutine = StartCoroutine(ChangeToUnderground());
                    }
                }
                
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

        public void OnChangeAmbientLight(EventArgs args)
        {
            if(args is ChangeAmbientLightEA cal)
            {
                if (cal.SurfaceLighting != null)
                    _surfaceLightingController.ChangeLighting(cal.SurfaceLighting);

                if (cal.UndergroundLighting != null)
                    _undergroundLightingController.ChangeLighting(cal.UndergroundLighting);
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

        #region COROUTINES
        private IEnumerator ChangeToSurface()
        {
            while (_lerpCoefficient > 0f)
            {
                _lerpCoefficient -= Time.unscaledDeltaTime;
                yield return null;
            }
            _lerpCoefficient = 0f;
            _changeControllerCoroutine = null;
        }

        private IEnumerator ChangeToUnderground()
        {
            while (_lerpCoefficient < 1f)
            {
                _lerpCoefficient += Time.unscaledDeltaTime;
                yield return null;
            }
            _lerpCoefficient = 1f;
            _changeControllerCoroutine = null;
        }
        #endregion

        #region UNITY CALLBACKS

        private void Awake()
        {
            _surfaceLightingController = new AmbientLightLayerController(this, _defaultAmbientLight);
            _undergroundLightingController = new AmbientLightLayerController(this, _defaultAmbientLight);
        }

        private void Update()
        {
            _surfaceLightingController.UpdateColor();
            _undergroundLightingController.UpdateColor();
            RenderSettings.ambientLight = Color.Lerp(_surfaceLightingController.CurrentColor, _undergroundLightingController.CurrentColor, _lerpCoefficient);
        }
        #endregion
    }
}