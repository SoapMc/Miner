using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using System;
using Miner.Management;

namespace Miner.Gameplay
{
    public abstract class Objective : IDisposable
    {
        public event Action ObjectiveUpdated;
        protected ObjectiveData _data;
        protected bool _isCompleted = false;
        protected bool _isFailed = false;

        public ObjectiveData Data => _data;
        public bool IsCompleted => _isCompleted;
        public bool IsFailed => _isFailed;

        public Objective(ObjectiveData data)
        {
            _data = data;
        }

        protected void InvokeObjectiveUpdated()
        {
            ObjectiveUpdated?.Invoke();
        }

        public abstract string GetDescription();
        public abstract string GetShortDescription();
        public abstract void Reset();

        public virtual void Dispose() { }
    }

    
}