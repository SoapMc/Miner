using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using System.Linq;
using UnityEngine.UI;
using MissionAcceptedEA = Miner.Management.Events.AcceptMissionEA;
using MissionCancelledEA = Miner.Management.Events.CancelMissionEA;
using ActiveMissionLoadedEA = Miner.Management.Events.AcceptMissionEA;
using Miner.Management;

namespace Miner.UI
{
    public class MissionDisplay : MonoBehaviour, IResettableHUDComponent
    {
        [SerializeField] private MissionDisplayElement _missionDisplayElementPrefab = null;
        [SerializeField] private Vector2 _spacing = Vector2.zero;
        private List<MissionDisplayElement> _elements = new List<MissionDisplayElement>();

        public void OnActiveMissionLoaded(EventArgs args)
        {
            if (args is ActiveMissionLoadedEA aml)
            {
                MissionDisplayElement mde = Instantiate(_missionDisplayElementPrefab, transform);
                mde.Initialize(aml.Mission);
                float yLocalPosition = 0;
                for (int i = 0; i < _elements.Count; ++i)
                {
                    yLocalPosition += _elements[i].Size.y;
                }
                mde.transform.localPosition = new Vector3(0, -yLocalPosition, 0);
                _elements.Add(mde);
            }
            else
            {
                Management.Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

        public void OnMissionAccepted(EventArgs args)
        {
            if(args is MissionAcceptedEA ma)
            {
                MissionDisplayElement mde = Instantiate(_missionDisplayElementPrefab, transform);
                mde.Initialize(ma.Mission);
                float yLocalPosition = 0;
                for (int i = 0; i < _elements.Count; ++i)
                {
                    yLocalPosition += _elements[i].Size.y;
                }
                mde.transform.localPosition = new Vector3(0, -yLocalPosition, 0);
                _elements.Add(mde);
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

        public void OnMissionCancelled(EventArgs args)
        {
            if (args is MissionCancelledEA mc)
            {
                MissionDisplayElement mde = _elements.FirstOrDefault(x => x.Mission == mc.Mission);
                int index = _elements.FindIndex(0, x => x == mde);
                for(;index < _elements.Count; ++index)
                {
                    _elements[index].transform.Translate(new Vector3(0, mde.Size.y));
                }

                if (mde != null)
                {
                    mde.Dispose();
                    _elements.Remove(mde);
                    Destroy(mde.gameObject);
                }
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

        public void OnMissionCompleted(EventArgs args)
        {
            if (args is MissionCompletedEA mc)
            {
                MissionDisplayElement mde = _elements.FirstOrDefault(x => x.Mission == mc.Mission);
                int index = _elements.FindIndex(0, x => x == mde);
                for (; index < _elements.Count; ++index)
                {
                    _elements[index].transform.Translate(new Vector3(0, mde.Size.y));
                }
                if (mde != null)
                {
                    mde.Dispose();
                    _elements.Remove(mde);
                    Destroy(mde.gameObject);
                }
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

        public void OnMissionFailed(EventArgs args)
        {
            if (args is MissionFailedEA mf)
            {
                MissionDisplayElement mde = _elements.FirstOrDefault(x => x.Mission == mf.Mission);
                int index = _elements.FindIndex(0, x => x == mde);
                for (; index < _elements.Count; ++index)
                {
                    _elements[index].transform.Translate(new Vector3(0, mde.Size.y));
                }
                if (mde != null)
                {
                    mde.Dispose();
                    _elements.Remove(mde);
                    Destroy(mde.gameObject);
                }
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

        public void ResetComponent()
        {
            for(int i = _elements.Count - 1; i >= 0; i--)
            {
                _elements[i].Dispose();
                Destroy(_elements[i].gameObject);
            }
            _elements.Clear();
        }
    }
}