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
    public class PartGridElement : Button, IDescriptableElement
    {
        [SerializeField] private GameEvent _descriptOffer = null;
        [SerializeField] private GameEvent _hideDescription = null;

        private Image _image = null;
        private PartListGrid _parent = null;
        private ReferencePart _part = null;
        private EOfferState _state = default;
        private int _row = 0;

        public EOfferState CurrentState => _state;
        public ReferencePart ReferencePart => _part;
        public int Row => _row;

        public string Name => _part.Name;

        public string Description => string.Join("\n", _part.GetOfferDescription());

        public void Initialize(PartListGrid parent, ReferencePart part, int row)
        {
            _parent = parent;
            _part = part;
            _image.sprite = part.Sprite;
            _row = row;
        }

        
        public override void OnSelect(BaseEventData eventData)
        {
            if (interactable == false) return;
            base.OnSelect(eventData);
            _parent.MoveViewRequest(transform.localPosition);
            _descriptOffer.Raise(new DescriptOfferEA(this, null, _state));
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            if (interactable == false) return;
            base.OnDeselect(eventData);
            _hideDescription.Raise();
        }

        public void Buy()
        {
            if (_state == EOfferState.Available || _state == EOfferState.Bought)
                _parent.BuyPart(this);
            
        }

        public void Refresh(EOfferState state)
        {
            _state = state;
            switch (state)
            {
                case EOfferState.Available:
                    colors = new ColorBlock() {
                        normalColor = Color.green,
                        highlightedColor = colors.highlightedColor,
                        pressedColor = colors.pressedColor,
                        selectedColor = colors.selectedColor,
                        fadeDuration = colors.fadeDuration,
                        colorMultiplier = colors.colorMultiplier
                    };
                    break;
                case EOfferState.Bought:
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
                case EOfferState.Unavailable:
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

    }
}