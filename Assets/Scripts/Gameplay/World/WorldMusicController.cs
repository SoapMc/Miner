using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using System;
using System.Linq;

namespace Miner.Gameplay
{
    public class WorldMusicController : MonoBehaviour
    {
        [SerializeField] private GameEvent _playMusic = null;

        public void OnPlayerCameToLayer(EventArgs args)
        {
            if(args is PlayerCameToLayerEA pctl)
            {
                if(pctl.GroundLayer.Music.Count > 0)
                    _playMusic.Raise(new PlayMusicEA(pctl.GroundLayer.Music.First()));
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }
    }
}