using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Gameplay;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Miner.Management.Events;

namespace Miner.UI
{
    [RequireComponent(typeof(Image))]
    public class PartGridElement : Button
    {
        private Image _image = null;
        private PartListGrid _parent = null;
        private ReferencePart _part = null;
        private State _state = default;
        private int _row = 0;

        public State CurrentState => _state;
        public ReferencePart ReferencePart => _part;
        public int Row => _row;

        public void Initialize(PartListGrid parent, ReferencePart part, int row)
        {
            _parent = parent;
            _part = part;
            _image.sprite = part.Sprite;
            _row = row;
        }

        
        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
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
                    colors = new ColorBlock() {
                        normalColor = Color.green,
                        highlightedColor = colors.highlightedColor,
                        pressedColor = colors.pressedColor,
                        selectedColor = colors.selectedColor,
                        fadeDuration = colors.fadeDuration,
                        colorMultiplier = colors.colorMultiplier
                    };
                    break;
                case State.Bought:
                    colors = new ColorBlock()
                    {
                        normalColor = Color.yellow,
                        highlightedColor = colors.highlightedColor,
                        pressedColor = colors.pressedColor,
                        selectedColor =colors.selectedColor,
                        fadeDuration = colors.fadeDuration,
                        colorMultiplier = colors.colorMultiplier
                    };
                    break;
                case State.Unavailable:
                    colors = new ColorBlock()
                    {
                        normalColor = Color.red,
                        highlightedColor = colors.highlightedColor,
                        pressedColor = colors.pressedColor,
                        selectedColor = colors.selectedColor,
                        fadeDuration = colors.fadeDuration,
                        colorMultiplier = colors.colorMultiplier
                    };
                    break;
            }

        }

        protected override void Awake()
        {
            base.Awake();
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