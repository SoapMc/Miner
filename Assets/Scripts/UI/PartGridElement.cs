using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Gameplay;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Miner.Management.Events;

namespace Miner.UI
{
    [RequireComponent(typeof(Button)), RequireComponent(typeof(Image))]
    public class PartGridElement : MonoBehaviour, ISelectHandler
    {

        private Button _button = null;
        private Image _image = null;
        private PartListGrid _parent = null;
        private ReferencePart _part = null;
        private State _state = default;
        private int _row = 0;

        public State CurrentState => _state;
        public ReferencePart ReferencePart => _part;
        public int Row => _row;

        public void Initialize(PartListGrid parent, ReferencePart part, int row, State state = default)
        {
            _parent = parent;
            _part = part;
            _image.sprite = part.Sprite;
            Refresh(state);
        }

        public void OnSelect(BaseEventData eventData)
        {
            _parent.MoveViewRequest(transform.localPosition);
            _parent.ShowDescription(this);
        }

        public void Buy()
        {
            if (_state == State.Available)
                _parent.BuyPart(this);
        }

        public void Refresh(State state)
        {
            _state = state;
            switch (state)
            {
                case State.Available:
                    _button.colors = new ColorBlock() {
                        normalColor = Color.green,
                        highlightedColor = _button.colors.highlightedColor,
                        pressedColor = _button.colors.pressedColor,
                        selectedColor = _button.colors.selectedColor,
                        fadeDuration = _button.colors.fadeDuration,
                        colorMultiplier = _button.colors.colorMultiplier
                    };
                    break;
                case State.Bought:
                    _button.colors = new ColorBlock()
                    {
                        normalColor = Color.yellow,
                        highlightedColor = _button.colors.highlightedColor,
                        pressedColor = _button.colors.pressedColor,
                        selectedColor = _button.colors.selectedColor,
                        fadeDuration = _button.colors.fadeDuration,
                        colorMultiplier = _button.colors.colorMultiplier
                    };
                    break;
                case State.Unavailable:
                    _button.colors = new ColorBlock()
                    {
                        normalColor = Color.red,
                        highlightedColor = _button.colors.highlightedColor,
                        pressedColor = _button.colors.pressedColor,
                        selectedColor = _button.colors.selectedColor,
                        fadeDuration = _button.colors.fadeDuration,
                        colorMultiplier = _button.colors.colorMultiplier
                    };
                    break;
            }

        }

        private void Awake()
        {
            _button = GetComponent<Button>();
            _image = GetComponent<Image>();
        }

        public enum State
        {
            Available,
            Bought,
            Unavailable
        }
    }
}