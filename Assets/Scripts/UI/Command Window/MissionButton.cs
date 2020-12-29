using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Gameplay;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Miner.FX;
using TMPro;
using Miner.Management.Events;

namespace Miner.UI
{
    public class MissionButton : MonoBehaviour, IDescriptableElement, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private TextMeshProUGUI _text = null;
        [SerializeField] private FX.UIStandardStyleSheet _selectedStyle = null;
        [SerializeField] private GameEvent _descriptElement = null;
        private CommandCenterWindow _commandCenterWindow = null;
        private ListScrolling _listScrolling = null;
        private Mission _mission;
        private FX.UIStandardStyleSheet _defaultStyle = null;

        private FX.UIStandardStyleSheet StyleSheet
        {
            set
            {
                Graphic[] background = gameObject.GetComponentsInChildrenWithTag<Graphic>("UI Background Styled");
                foreach (var graphic in background)
                    graphic.color = value.BackgroundColor;
                Graphic[] foreground = gameObject.GetComponentsInChildrenWithTag<Graphic>("UI Foreground Styled");
                foreach (var graphic in foreground)
                    graphic.color = value.ForegroundColor;
            }
        }

        public string Name => _mission.Name;

        public string Description => _mission.Description;

        public void Initialize(CommandCenterWindow commandCenterWindow, ListScrolling listScrolling, Mission mission, UIStandardStyleSheet style)
        {
            _commandCenterWindow = commandCenterWindow;
            _listScrolling = listScrolling;
            _mission = mission;
            _text.text = _mission.Name;
            _defaultStyle = style;
            StyleSheet = style;
        }

        public void UpdateStyle(FX.UIStandardStyleSheet style)
        {
            _defaultStyle = style;
        }

        public void OnClick()
        {
            if (_mission.Status == EMissionStatus.Available)
                _commandCenterWindow.AcceptMission(this, _mission);
            else if (_mission.Status == EMissionStatus.Active)
                _commandCenterWindow.CancelMission(this, _mission);
        }

        public void OnSelect(BaseEventData eventData)
        {
            StyleSheet = _selectedStyle;
            _listScrolling.MoveViewRequest(transform.localPosition);
            _descriptElement.Raise(new DescriptElementEA(this, null));
        }

        public void OnDeselect(BaseEventData eventData)
        {
            StyleSheet = _defaultStyle;
        }

    }
}