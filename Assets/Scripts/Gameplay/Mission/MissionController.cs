using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Miner.UI;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using Miner.Management;
using MissionAcceptedEA = Miner.Management.Events.AcceptMissionEA;
using MissionCancelledEA = Miner.Management.Events.CancelMissionEA;
using ActiveMissionLoadedEA = Miner.Management.Events.AcceptMissionEA;
using System.Linq;

//using MissionFailedEA = Miner.Management.Events.CancelMissionEA;
//using MissionCompletedEA = Miner.Management.Events.CompleteMissionEA;

namespace Miner.Gameplay
{
    public class MissionController : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private GameEvent _missionAccepted = null;
        [SerializeField] private GameEvent _missionCancelled = null;
        [SerializeField] private GameEvent _missionFailed = null;
        [SerializeField] private GameEvent _missionCompleted = null;
        [SerializeField] private GameEvent _activeMissionLoaded = null;

        [Header("Resources")]
        [SerializeField] private MissionList _availableMissions = null;
        [SerializeField] private MissionList _playerActiveMissions = null;
        [SerializeField] private IntReference _playerMaxAchievedLayer = null;
        [SerializeField] private IntReference _playerMoney = null;
        [SerializeField] private Message _message_NewMissions = null;
        [SerializeField] private Message _message_MissionCompleted = null;
        [SerializeField] private Message _message_MissionFailed = null;
        [SerializeField] private List<ObjectiveDefinition> _objectiveDefinitions = new List<ObjectiveDefinition>();

        public void Initialize()
        {
            OnDayElapsed();
        }

        public void ResetState()
        {
            foreach (var mission in _playerActiveMissions)
            {
                mission.MissionUpdated -= OnMissionUpdated;
            }

            _availableMissions.Clear();
            _playerActiveMissions.Clear();
        }

        public void Load(MissionControllerData missionControllerData)
        {
            foreach (Mission mission in missionControllerData.AvailableMissions)
            {
                mission.LoadObjectives(LoadObjective);
                _availableMissions.Add(mission);
            }

            StartCoroutine(WaitOneFrameAndLoadActiveMissions(missionControllerData));
        }

        public MissionControllerData RetrieveSerializableData()
        {
            return new MissionControllerData() { AvailableMissions = _availableMissions.Content, PlayerActiveMissions = _playerActiveMissions.Content };
        }

        public void OnAcceptMission(EventArgs args)
        {
            if(args is AcceptMissionEA am)
            {
                _availableMissions.Remove(am.Mission);
                am.Mission.Status = EMissionStatus.Active;
                _playerActiveMissions.Add(am.Mission);
                am.Mission.Reset();
                am.Mission.MissionUpdated += OnMissionUpdated;
                _missionAccepted.Raise(new MissionAcceptedEA(am.Mission));
            }
            else
            {
                GameManager.Instance.Log.WriteException(new InvalidEventArgsException());
            }
        }

        public void OnCancelMission(EventArgs args)
        {
            if (args is CancelMissionEA cm)
            {
                _playerActiveMissions.Remove(cm.Mission);
                cm.Mission.Status = EMissionStatus.Available;
                _availableMissions.Add(cm.Mission);
                cm.Mission.MissionUpdated -= OnMissionUpdated;
                cm.Mission.Reset();
                _missionCancelled.Raise(new MissionCancelledEA(cm.Mission));
            }
            else
            {
                GameManager.Instance.Log.WriteException(new InvalidEventArgsException());
            }
        }

        public void OnDayElapsed()
        {
            Mission m = GenerateMission();
            if(m == null)
            {
                Management.Log.Instance.WriteException(new ArgumentNullException());
                return;
            }

            if (m.Verified)
            {
                if (_message_NewMissions != null)
                    _message_NewMissions.Show();
                _availableMissions.Add(m);
            }
        }

        private void OnMissionUpdated(Mission mission)
        {
            if(mission.IsCompleted())
            {
                mission.Status = EMissionStatus.Finished;
                _missionCompleted.Raise(new MissionCompletedEA(mission));
                _message_MissionCompleted.OverrideMessage("Mission \'" + mission.Name + "\' has been completed. You received " + mission.Reward + " $.");
                _message_MissionCompleted.Show();
                _playerMoney.Value += mission.Reward;
                _playerActiveMissions.Remove(mission);
                mission.MissionUpdated -= OnMissionUpdated;
                mission.Reset();
                mission.Dispose();
            }
            else if(mission.IsFailed())
            { 
                mission.Status = EMissionStatus.Finished;
                _missionFailed.Raise(new MissionFailedEA(mission));
                _message_MissionFailed.OverrideMessage("Mission \'" + mission.Name + "\' has been failed. You haven't received any reward.");
                _message_MissionFailed.Show();
                _playerActiveMissions.Remove(mission);
                mission.MissionUpdated -= OnMissionUpdated;
                mission.Reset();
                mission.Dispose();
            }
        }

        public Mission GenerateMission()
        {
            Mission mission = new Mission();
            var objectiveTypes = Enum.GetValues(typeof(EMissionObjectiveType));
            EMissionObjectiveType objectiveType = (EMissionObjectiveType)objectiveTypes.GetValue(UnityEngine.Random.Range(0, objectiveTypes.Length));
            ObjectiveDefinition objectiveDefinition = _objectiveDefinitions.FirstOrDefault(x => x.ObjectiveType == objectiveType);
            if (objectiveDefinition == null)
            {
                Log.Instance.WriteException(new InvalidSettingException());
                return null;
            }
            Objective objective = objectiveDefinition.CreateObjective();
            if(objective != null)
                mission.AddObjective(objective);

            mission.Reward = Mathf.RoundToInt(0.8f * Mathf.Pow(200 + Random.Range(0.8f, 1.2f) * 500 * _playerMaxAchievedLayer.Value, 1 + (_playerMaxAchievedLayer.Value/20f))/100f) * 100;
            
            return mission;
        }

        public Objective LoadObjective(ObjectiveData objectiveData)
        {
            ObjectiveDefinition objectiveDefinition = _objectiveDefinitions.FirstOrDefault(x => x.ObjectiveType == objectiveData.Type);
            if (objectiveDefinition != null)
                return objectiveDefinition.LoadObjective(objectiveData);
            return null;
        }

        public IEnumerator WaitOneFrameAndLoadActiveMissions(MissionControllerData missionControllerData)
        {
            yield return null;

            foreach (Mission mission in missionControllerData.PlayerActiveMissions)
            {
                mission.LoadObjectives(LoadObjective);
                _playerActiveMissions.Add(mission);
                mission.MissionUpdated += OnMissionUpdated;
                _activeMissionLoaded.Raise(new ActiveMissionLoadedEA(mission));
            }
        }

        [System.Serializable]
        public class MissionControllerData
        {
            public List<Mission> AvailableMissions = new List<Mission>();
            public List<Mission> PlayerActiveMissions = new List<Mission>();
        }
    }
}