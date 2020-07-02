﻿using System.Collections;
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
        private Transform _playerTransform = null;
        private Coroutine _groundParticlesFollowing = null;
        
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
                    _groundParticlesFollowing = StartCoroutine(FollowPlayer(ltdp.PlayerTransform.position));
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
                Instantiate(cp.Particle.gameObject, cp.Position, Quaternion.identity);
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        private IEnumerator FollowPlayer(Vector3 playerPosition)
        {
            while (true)
            {
                _groundParticles.transform.position = playerPosition;
                yield return null;
            }
        }
    }
}