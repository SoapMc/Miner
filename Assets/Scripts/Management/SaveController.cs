using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using System;
using Miner.Management.Exceptions;

namespace Miner.Management
{
    public class SaveController : MonoBehaviour
    {
        [SerializeField] private GameEvent _showBriefInfo = null;

        public void OnPlayerCameToLayer(EventArgs args)
        {
            if(args is PlayerCameToLayerEA pctl)
            {
                if(pctl.GroundLayer.AreaType == Gameplay.GroundLayer.EAreaType.Surface && pctl.GroundLayer.LayerNumber == 0)
                {
                    GameManager.Instance.SaveToFile();
                    _showBriefInfo.Raise(new ShowBriefInfoEA("The game has been saved"));
                }
            }
            else
            {
                GameManager.Instance.Log.WriteException(new InvalidEventArgsException());
            }
        }
    }
}