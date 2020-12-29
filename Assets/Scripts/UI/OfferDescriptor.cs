using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using Miner.FX;
using UnityEngine.UI;

namespace Miner.UI
{
    public class OfferDescriptor : UniversalDescriptor
    {
        [SerializeField] private UIStandardStyleSheet _availableStyle = null;
        [SerializeField] private UIStandardStyleSheet _unavailableStyle = null;
        [SerializeField] private UIStandardStyleSheet _boughtStyle = null;

        public FX.UIStandardStyleSheet StyleSheet
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

        public override void OnDescriptElement(EventArgs args)
        {
            if (args is DescriptOfferEA doe)
            {
                _description.text = "<size=18>" + doe.DescriptableElement.Name + "</size>\n" + doe.DescriptableElement.Description;
                if (doe.RectTransform != null)
                {
                    Rect rect = doe.RectTransform.rect;
                    transform.position = (Vector2)doe.RectTransform.position + rect.position;
                }
                if (_appearingCoroutine != null)
                    StopCoroutine(_appearingCoroutine);
                _appearingCoroutine = StartCoroutine(TriggerAppearingCoroutine());
                AdjustToType(doe.State);
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        private void AdjustToType(EOfferState state)
        {
            switch (state)
            {
                case EOfferState.Available:
                    StyleSheet = _availableStyle;
                    break;
                case EOfferState.Bought:
                    StyleSheet = _boughtStyle;
                    break;
                case EOfferState.Locked:
                    StyleSheet = _unavailableStyle;
                    break;
                case EOfferState.Unavailable:
                    StyleSheet = _unavailableStyle;
                    break;
                default:
                    break;
            }
        }
    }
}