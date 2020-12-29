using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem;

namespace Miner.Management
{
    public class InputManager : MonoBehaviour
    {
        private Controls _controls = null;
        private bool _usingKeyboard = true;

        public string GetBindingNameOfInputType(EInputType inputType)
        {
            switch(inputType)
            {
                case EInputType.Cancel:
                    return GetNameOfBinding(_controls.Player.Cancel.bindings);
                case EInputType.Confirm:
                    return GetNameOfBinding(_controls.Player.Confirm.bindings);
                case EInputType.Inventory:
                    return GetNameOfBinding(_controls.Player.Inventory.bindings);
                case EInputType.Movement:
                    return GetNameOfBindingForComposedInput(_controls.Player.Movement.bindings);
                case EInputType.Next:
                    return GetNameOfBinding(_controls.Player.Next.bindings);
                case EInputType.Previous:
                    return GetNameOfBinding(_controls.Player.Previous.bindings);
                case EInputType.Use:
                    return GetNameOfBinding(_controls.Player.Use.bindings);
                default:
                    Management.Log.Instance.WriteException(new ArgumentException());
                    return string.Empty;
            }
        }

        private string GetNameOfBindingForComposedInput(ReadOnlyArray<InputBinding> bindingArray)
        {
            string bindingName = string.Empty;
            int amount = 0;
            foreach (var binding in bindingArray)
            {
                if (!binding.isPartOfComposite)
                {
                    if (amount == 0)
                        bindingName += binding.name;
                    else
                        bindingName += "/" + binding.name;
                    amount++;
                }    
            }
            return bindingName;
        }

        private string GetNameOfBinding(ReadOnlyArray<InputBinding> bindingArray)
        {
            string bindingName = string.Empty;
            int amount = 0;
            foreach (var binding in bindingArray)
            {
                if (amount == 0)
                    bindingName += binding.path;
                else
                    bindingName += "/" + binding.path;
                amount++;
            }
            return bindingName;
        }

        private void Awake()
        {
            _controls = new Controls();
        }
    }
}