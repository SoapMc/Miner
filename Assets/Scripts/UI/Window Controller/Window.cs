﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.FX;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Miner.Management.Events;
using Miner.Gameplay;
using System.Linq;
using UnityEngine.InputSystem;

namespace Miner.UI
{
    public abstract class Window : MonoBehaviour
    {
        [SerializeField] protected GameEvent _closeWindow = null;
        [SerializeField] protected Selectable _firstSelectedObject = null;
        [SerializeField] protected InputHelp _inputHelp = null;
        protected Management.Controls _controls = null;
        protected IAppearingEffect _appearingEffect = null;
        protected bool _interactable = true;

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

        public bool Interactable
        {
            get => _interactable;
            set
            {
                if (_interactable != value)
                {
                    _interactable = value;
                    Selectable[] selectables = GetComponentsInChildren<Selectable>();
                    foreach(var sel in selectables)
                    {
                        sel.interactable = value;
                    }
                }
            }
        }

        public void SelectFirstObject()
        {
            if (_firstSelectedObject != null)
            {
                EventSystem.current.SetSelectedGameObject(_firstSelectedObject.gameObject);
                _firstSelectedObject.OnSelect(null);
            }  
        }

        public void GainFocus()
        {
            if (_inputHelp != null)
                _inputHelp.Show();
        }

        public void LoseFocus()
        {
            if (_inputHelp != null)
                _inputHelp.Hide();
        }

        protected virtual void CloseWindow(InputAction.CallbackContext context)
        {
            _controls.Player.Cancel.performed -= CloseWindow;
            if (_appearingEffect != null)
                _appearingEffect.TriggerDisappearing();
            else
                OnDisappearingFinished();
        }

        protected virtual void OnAppearingFinished()
        {
            SelectFirstObject();
        }

        protected virtual void OnDisappearingFinished()
        {
            _closeWindow.Raise(new CloseWindowEA(this));
        }

        protected virtual void Awake()
        {
            _controls = new Management.Controls();
            _appearingEffect = GetComponent<IAppearingEffect>();
            _appearingEffect.AppearingFinished += OnAppearingFinished;
            _appearingEffect.DisappearingFinished += OnDisappearingFinished;
            _controls.Player.Cancel.performed += CloseWindow;
            _controls.Enable();
        }

        protected virtual void OnDestroy()
        {
            if (_appearingEffect != null)
            {
                _appearingEffect.AppearingFinished -= OnAppearingFinished;
                _appearingEffect.DisappearingFinished -= OnDisappearingFinished;
            }
            if(_controls != null)
                _controls.Player.Cancel.performed -= CloseWindow;
        }

    }
}