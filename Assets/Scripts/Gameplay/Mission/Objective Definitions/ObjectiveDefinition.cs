using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    public abstract class ObjectiveDefinition : ScriptableObject
    {
        [SerializeField] private EMissionObjectiveType _objectiveType = default;

        public EMissionObjectiveType ObjectiveType => _objectiveType;

        public abstract Objective CreateObjective();
        public abstract Objective LoadObjective(ObjectiveData data);
    }
}