// GENERATED AUTOMATICALLY FROM 'Assets/Input/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Miner.Management
{
    public class @Controls : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @Controls()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""c5853f6a-bada-44d2-b1e3-9364f83ffbb9"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""050a1bb5-f0af-4334-bf8c-04c5d5969019"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Inventory"",
                    ""type"": ""Button"",
                    ""id"": ""2e3e2263-9e82-4cfc-a83e-8960f170361d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""cb479314-36a2-4384-adcb-e048d3340665"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Use"",
                    ""type"": ""Button"",
                    ""id"": ""fd6ee74d-58d8-479f-b56b-fffd658a97b2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Previous"",
                    ""type"": ""Button"",
                    ""id"": ""1c6eb9ee-5c6a-40ed-8c23-90023b38a374"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Next"",
                    ""type"": ""Button"",
                    ""id"": ""58083b1e-84ac-433f-930f-693141af3d27"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Confirm"",
                    ""type"": ""Button"",
                    ""id"": ""dfc5a79e-fb34-46b2-a4e3-349a8dbb61f0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""da8c5b4e-e0c7-450e-b160-693e66a08122"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""e2ac9a4f-5c09-4e86-8b03-edc60bbf698a"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""c0a1de8c-b843-42dc-8c3a-d8fc7466bcd0"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""4edfa64c-ad96-4906-b59d-51018d77cdcd"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""83481dce-ea93-4cf1-9b32-b7451240a607"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrows"",
                    ""id"": ""37dc2cca-bf69-4534-9b49-051e072ba740"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""a3fedbce-5c65-45d1-9c9e-9ab144ee1597"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0bacadcc-6ef7-4a54-b127-aa3869ffa32c"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""986771a4-dc8e-4064-b304-60d339f833e2"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4f7a3a31-52e1-404f-abf2-24bba38694f0"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""LeftStick"",
                    ""id"": ""caf83c23-adf2-405a-b94d-e27b19655f7c"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""fab5362d-d705-4534-b1d1-7045c265c818"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""811747ee-84a8-4fa2-9bfd-62c029000125"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""723a6310-7488-48a3-868e-c15f742c2cc9"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1357f65b-4cbc-406e-8404-1cdaf0431e82"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""ae68332c-35b7-4bde-b058-5da0d2114504"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""97d930bc-8239-4a8f-aed3-1c04494a1b9a"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0b8b5d1f-254f-4354-8215-441fc2c33dd9"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Use"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c1bdbeb0-40ce-41f2-b727-b7bdeb87ed6f"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Previous"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""61032f79-64bf-4ad4-8ecf-7641148f0eb8"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Next"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""91072b11-b360-4229-a0c3-2f5dcc066ab7"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Player
            m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
            m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
            m_Player_Inventory = m_Player.FindAction("Inventory", throwIfNotFound: true);
            m_Player_Cancel = m_Player.FindAction("Cancel", throwIfNotFound: true);
            m_Player_Use = m_Player.FindAction("Use", throwIfNotFound: true);
            m_Player_Previous = m_Player.FindAction("Previous", throwIfNotFound: true);
            m_Player_Next = m_Player.FindAction("Next", throwIfNotFound: true);
            m_Player_Confirm = m_Player.FindAction("Confirm", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // Player
        private readonly InputActionMap m_Player;
        private IPlayerActions m_PlayerActionsCallbackInterface;
        private readonly InputAction m_Player_Movement;
        private readonly InputAction m_Player_Inventory;
        private readonly InputAction m_Player_Cancel;
        private readonly InputAction m_Player_Use;
        private readonly InputAction m_Player_Previous;
        private readonly InputAction m_Player_Next;
        private readonly InputAction m_Player_Confirm;
        public struct PlayerActions
        {
            private @Controls m_Wrapper;
            public PlayerActions(@Controls wrapper) { m_Wrapper = wrapper; }
            public InputAction @Movement => m_Wrapper.m_Player_Movement;
            public InputAction @Inventory => m_Wrapper.m_Player_Inventory;
            public InputAction @Cancel => m_Wrapper.m_Player_Cancel;
            public InputAction @Use => m_Wrapper.m_Player_Use;
            public InputAction @Previous => m_Wrapper.m_Player_Previous;
            public InputAction @Next => m_Wrapper.m_Player_Next;
            public InputAction @Confirm => m_Wrapper.m_Player_Confirm;
            public InputActionMap Get() { return m_Wrapper.m_Player; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
            public void SetCallbacks(IPlayerActions instance)
            {
                if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
                {
                    @Movement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                    @Movement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                    @Movement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                    @Inventory.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInventory;
                    @Inventory.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInventory;
                    @Inventory.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInventory;
                    @Cancel.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCancel;
                    @Cancel.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCancel;
                    @Cancel.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCancel;
                    @Use.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUse;
                    @Use.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUse;
                    @Use.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUse;
                    @Previous.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPrevious;
                    @Previous.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPrevious;
                    @Previous.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPrevious;
                    @Next.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNext;
                    @Next.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNext;
                    @Next.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNext;
                    @Confirm.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnConfirm;
                    @Confirm.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnConfirm;
                    @Confirm.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnConfirm;
                }
                m_Wrapper.m_PlayerActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Movement.started += instance.OnMovement;
                    @Movement.performed += instance.OnMovement;
                    @Movement.canceled += instance.OnMovement;
                    @Inventory.started += instance.OnInventory;
                    @Inventory.performed += instance.OnInventory;
                    @Inventory.canceled += instance.OnInventory;
                    @Cancel.started += instance.OnCancel;
                    @Cancel.performed += instance.OnCancel;
                    @Cancel.canceled += instance.OnCancel;
                    @Use.started += instance.OnUse;
                    @Use.performed += instance.OnUse;
                    @Use.canceled += instance.OnUse;
                    @Previous.started += instance.OnPrevious;
                    @Previous.performed += instance.OnPrevious;
                    @Previous.canceled += instance.OnPrevious;
                    @Next.started += instance.OnNext;
                    @Next.performed += instance.OnNext;
                    @Next.canceled += instance.OnNext;
                    @Confirm.started += instance.OnConfirm;
                    @Confirm.performed += instance.OnConfirm;
                    @Confirm.canceled += instance.OnConfirm;
                }
            }
        }
        public PlayerActions @Player => new PlayerActions(this);
        public interface IPlayerActions
        {
            void OnMovement(InputAction.CallbackContext context);
            void OnInventory(InputAction.CallbackContext context);
            void OnCancel(InputAction.CallbackContext context);
            void OnUse(InputAction.CallbackContext context);
            void OnPrevious(InputAction.CallbackContext context);
            void OnNext(InputAction.CallbackContext context);
            void OnConfirm(InputAction.CallbackContext context);
        }
    }
}
