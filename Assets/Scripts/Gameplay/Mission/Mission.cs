using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Miner.Management;

namespace Miner.Gameplay
{
    [System.Serializable]
    public class Mission : IDisposable
    {
        public event Action<Mission> MissionUpdated;
        [SerializeField] private List<ObjectiveData> _data = new List<ObjectiveData>();
        [SerializeField] private int _reward = 0;
        [SerializeField] private EMissionStatus _status = EMissionStatus.Available;
        private List<Objective> _objectives = new List<Objective>();

        public string Name
        {
            get
            {
                try
                {
                    return _data[0].Name;
                }
                catch(IndexOutOfRangeException ioor)
                {
                    Management.Log.Instance.WriteException(ioor);
                    return "No objectives";
                }
            }
        }
        public string Description
        {
            get
            {
                string result = string.Empty;
                for(int i = 0; i < _objectives.Count; ++i)
                {
                    result += _objectives[i].GetDescription() + "\n";
                }
                result += "\nReward: " + _reward + "$";

                return result;
            }
        }
        public int Reward { get => _reward; set => _reward = value; }
        public string ShortDescription
        {
            get
            {
                string result = string.Empty;
                for (int i = 0; i < _objectives.Count; ++i)
                {
                    result += _objectives[i].GetShortDescription() + "\n";
                }
                result += "\nReward: " + _reward + "$";

                return result;
            }
        }
        public EMissionStatus Status { get => _status; set => _status = value; }
        public bool Verified
        {
            get
            {
                if (_objectives.Count > 0) return true;
                return false;
            }
        }

        public void AddObjective(Objective objective)
        {
            _data.Add(objective.Data);
            _objectives.Add(objective);
            objective.ObjectiveUpdated += OnObjectiveUpdated;
        }

        public void RemoveObjective(Objective objective)
        {
            if (objective == null) Log.Instance.WriteException(new ArgumentNullException());
            ObjectiveData od = _data.FirstOrDefault(x => objective.Data == x);
            if (od != null)
            {
                _data.Remove(od);
            }
            objective.ObjectiveUpdated -= OnObjectiveUpdated;
            _objectives.Remove(objective);
        }

        public void LoadObjectives(Func<ObjectiveData, Objective> loadingFunction)
        {
            for(int i = 0; i < _data.Count; ++i)
            {
                Objective o = loadingFunction(_data[i]);
                if (o != null)
                {
                    _objectives.Add(o);
                    o.ObjectiveUpdated += OnObjectiveUpdated;
                }
            }
        }

        public void Reset()
        {
            for (int i = 0; i < _objectives.Count; ++i)
            {
                _objectives[i].Reset();
            }
        }

        private void OnObjectiveUpdated()
        {
            MissionUpdated?.Invoke(this);
        }

        public bool IsCompleted()
        {
            for(int i = 0; i < _objectives.Count; ++i)
            {
                if (!_objectives[i].IsCompleted)
                    return false;
            }
            return true;
        }

        public bool IsFailed()
        {
            for (int i = 0; i < _objectives.Count; ++i)
            {
                if (_objectives[i].IsFailed)
                {
                    return true;
                }
            }

            return false;
        }

        public void Dispose()
        {
            for(int i = _objectives.Count - 1; i >= 0; --i)
            {
                RemoveObjective(_objectives[i]);
            }
        }

        
    }
}