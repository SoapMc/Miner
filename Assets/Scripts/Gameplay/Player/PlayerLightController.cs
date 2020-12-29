using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using Miner.Management;

namespace Miner.Gameplay
{
    public class PlayerLightController : MonoBehaviour
    {
        [SerializeField] private Light _lightPoint = null;
        private List<string> _reasonsToLight = new List<string>();

        private const string REASON_NIGHT = "Night";
        private const string REASON_UNDERGROUND = "Underground";

        public void OnDayBegan()
        {
            RemoveReason(REASON_NIGHT);
        }

        public void OnNightBegan()
        {
            AddReason(REASON_NIGHT);
        }

        public void OnPlayerCameToLayer(EventArgs args)
        {
            if (args is PlayerCameToLayerEA pctl)
            {
                if (pctl.GroundLayer.AreaType == GroundLayer.EAreaType.Surface)
                    RemoveReason(REASON_UNDERGROUND);
                else if (pctl.GroundLayer.AreaType == GroundLayer.EAreaType.Underground)
                    AddReason(REASON_UNDERGROUND);
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

        private void AddReason(string name)
        {
            if (!_reasonsToLight.Exists(x => x == name))
                _reasonsToLight.Add(name);

            SetLight();
        }

        private void RemoveReason(string name)
        {
            _reasonsToLight.Remove(name);

            SetLight();
        }

        private void SetLight()
        {
            if (_reasonsToLight.Count > 0)
                _lightPoint.gameObject.SetActive(true);
            else
                _lightPoint.gameObject.SetActive(false);
        }

        private void Awake()
        {
            SetLight();
        }
    }
}