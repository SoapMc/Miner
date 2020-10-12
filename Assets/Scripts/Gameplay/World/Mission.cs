using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    [System.Serializable]
    public class Mission
    {
        private List<Objective> _objectives;
        private int _timeLimit;
        private int _elapsedTime;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectives">List of objectives</param>
        /// <param name="timeLimit">Amount of time given to complete the mission (in minutes of in-game time)</param>
        public Mission(List<Objective> objectives, int timeLimit)
        {
            _objectives = objectives;
            _timeLimit = timeLimit;
            _timeLimit = Mathf.Clamp(_timeLimit, 0, 1440);
        }

        public Mission(params Objective[] objectives)
        {
            _objectives = new List<Objective>(objectives.Length);
            foreach(var obj in objectives)
            {
                _objectives.Add(obj);
            }
        }

        private bool IsCompleted()
        {
            for(int i = 0; i < _objectives.Count; ++i)
            {
                if (!_objectives[i].IsCompleted())
                    return false;
            }
            return true;
        }

        public abstract class Objective
        {
            public abstract bool IsCompleted();
        }

        public class GatherResourcesObjective : Objective
        {
            private int _resourceId;
            private int _requiredAmount;
            private int _alreadyGathered;

            public GatherResourcesObjective(int resourceId, int requiredAmount)
            {
                _resourceId = resourceId;
                _requiredAmount = requiredAmount;
                _alreadyGathered = 0;
            }

            public override bool IsCompleted()
            {
                if (_alreadyGathered >= _requiredAmount)
                    return true;
                return false;
            }
        }
    }
}