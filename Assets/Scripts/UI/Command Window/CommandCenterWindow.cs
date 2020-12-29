using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Gameplay;
using System;
using Miner.FX;
using System.Linq;
using Miner.Management.Events;
using UnityEngine.UI;

namespace Miner.UI
{
    public class CommandCenterWindow : Window
    {
        [SerializeField] private GameEvent _acceptMission = null;
        [SerializeField] private GameEvent _cancelMission = null;
        [SerializeField] private MissionList _availableMissions = null;
        [SerializeField] private MissionList _playerActiveMissions = null;
        [SerializeField] private ListScrolling _listScrolling = null;
        [SerializeField] private MissionButton _missionButtonPrefab = null;
        [SerializeField] private UIStandardStyleSheet _availableStyle = null;
        [SerializeField] private UIStandardStyleSheet _activeStyle = null;
        [SerializeField] private UIStandardStyleSheet _finishedStyle = null;
        [SerializeField] private Vector2 _spacingBetweenButtons = Vector2.zero;

        public void AcceptMission(MissionButton button, Mission mission)
        {
            Mission acceptedMission = _availableMissions.Find(x => x == mission);
            if (acceptedMission != null)
            {
                _acceptMission.Raise(new AcceptMissionEA(acceptedMission));
                button.UpdateStyle(GetStyleForMission(mission));
            }
        }

        public void CancelMission(MissionButton button, Mission mission)
        {
            Mission cancelledMission = _playerActiveMissions.Find(x => x == mission);
            if (cancelledMission != null)
            {
                _cancelMission.Raise(new CancelMissionEA(cancelledMission));
                button.UpdateStyle(GetStyleForMission(mission));
            }
        }

        private UIStandardStyleSheet GetStyleForMission(Mission mission)
        {
            switch(mission.Status)
            {
                case EMissionStatus.Active:
                    return _activeStyle;
                case EMissionStatus.Available:
                    return _availableStyle;
                case EMissionStatus.Finished:
                    return _finishedStyle;
                default:
                    return _finishedStyle;
            }
        }

        protected override void Awake()
        {
            int row = 0;
            Vector2 elementSize = _missionButtonPrefab.GetComponent<RectTransform>().sizeDelta;
            foreach (var mission in _availableMissions)
            {
                MissionButton mb = Instantiate(_missionButtonPrefab, _listScrolling.transform);
                mb.transform.localPosition = new Vector2(0, -row * elementSize.y - (row + 1) * _spacingBetweenButtons.y);
                row++;
                mb.Initialize(this, _listScrolling, mission, GetStyleForMission(mission));
            }

            foreach (var mission in _playerActiveMissions)
            {
                MissionButton mb = Instantiate(_missionButtonPrefab, _listScrolling.transform);
                mb.transform.localPosition = new Vector2(0, -row * elementSize.y - (row + 1) * _spacingBetweenButtons.y);
                row++;
                mb.Initialize(this, _listScrolling, mission, GetStyleForMission(mission));
            }

            if (_listScrolling.transform.childCount > 0)
            {
                MissionButton firstButton = _listScrolling.transform.GetChild(0).GetComponent<MissionButton>();
                _firstSelectedObject = firstButton.GetComponent<Button>();
            }

            base.Awake();
        }

        protected override void OnAppearingFinished()
        {
            base.OnAppearingFinished();
            SelectFirstObject();
        }
    }
}