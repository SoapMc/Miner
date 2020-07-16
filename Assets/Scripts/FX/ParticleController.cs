using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Miner.Management.Events;
using Miner.Management.Exceptions;

namespace Miner.FX
{
    public class ParticleController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _groundParticles = null;
        [SerializeField] private ParticleSystem _groundDust = null;

        private Grid _worldGrid = null;
        private Coroutine _groundParticlesFollowing = null;

        private IEnumerator FollowPlayer(Vector3 playerPosition)
        {
            while (true)
            {
                _groundParticles.transform.position = playerPosition;
                yield return null;
            }
        }

        #region EVENT RESPONSES
        public void OnLeadToDigPlace(EventArgs args)
        {
            if (args is LeadToDigPlaceEA ltdp)
            {
                if (_groundParticlesFollowing == null)
                {
                    ParticleSystem.EmissionModule emission = _groundParticles.emission;
                    emission.enabled = true;
                    emission = _groundDust.emission;
                    emission.enabled = true;
                    _groundParticlesFollowing = StartCoroutine(FollowPlayer(ltdp.PlayerPosition));
                    _groundDust.transform.position = _worldGrid.GetCellCenterWorld((Vector3Int)ltdp.Place);
                }
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }


        public void OnDigComplete()
        {
            if (_groundParticlesFollowing != null)
            {
                StopCoroutine(_groundParticlesFollowing);
                _groundParticlesFollowing = null;
            }
            ParticleSystem.EmissionModule emission = _groundParticles.emission;
            emission.enabled = false;
            emission = _groundDust.emission;
            emission.enabled = false;
        }

        public void OnWorldLoaded(EventArgs args)
        {
            if (args is WorldLoadedEA wl)
            {
                _worldGrid = wl.WorldGrid;
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        public void  OnCreateParticle(EventArgs args)
        {
            if(args is CreateParticleEA cp)
            {
                if (cp.CoordinateType == CreateParticleEA.ECoordinateType.World)
                    Instantiate(cp.Particle.gameObject, cp.Position, Quaternion.identity);
                else if (cp.CoordinateType == CreateParticleEA.ECoordinateType.Grid)
                    Instantiate(cp.Particle.gameObject, _worldGrid.GetCellCenterWorld(new Vector3Int((int)cp.Position.x, (int)cp.Position.y, 0)), Quaternion.identity);
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }
        #endregion

    }
}