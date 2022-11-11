//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Scripts/InputScripts/PlayerInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""6f641add-9ca4-4be1-bca9-21f9355014c2"",
            ""actions"": [
                {
                    ""name"": ""OnLeftClick"",
                    ""type"": ""Button"",
                    ""id"": ""a424da8a-b975-4c9f-b255-5621dd30594c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""OnRightClick"",
                    ""type"": ""Button"",
                    ""id"": ""a3d616d3-58a4-4f30-a4df-11ced216e3df"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""OnMiddleClick"",
                    ""type"": ""Button"",
                    ""id"": ""dade0cca-a3cc-440e-9638-4aa6549a1ff0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MousePos"",
                    ""type"": ""Value"",
                    ""id"": ""13aba83b-86b6-4050-b838-14a450b02671"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5d99450b-7a95-4474-9f86-d785079a7a4a"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OnLeftClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""02cbe92c-cb5b-4a37-9588-ac86c9848191"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OnRightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fe094ada-43bd-4867-a665-2ef0dcddfa1b"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OnMiddleClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f31503ae-928a-4e83-89b4-dde826a46682"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePos"",
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
        m_Player_OnLeftClick = m_Player.FindAction("OnLeftClick", throwIfNotFound: true);
        m_Player_OnRightClick = m_Player.FindAction("OnRightClick", throwIfNotFound: true);
        m_Player_OnMiddleClick = m_Player.FindAction("OnMiddleClick", throwIfNotFound: true);
        m_Player_MousePos = m_Player.FindAction("MousePos", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_OnLeftClick;
    private readonly InputAction m_Player_OnRightClick;
    private readonly InputAction m_Player_OnMiddleClick;
    private readonly InputAction m_Player_MousePos;
    public struct PlayerActions
    {
        private @PlayerInputActions m_Wrapper;
        public PlayerActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @OnLeftClick => m_Wrapper.m_Player_OnLeftClick;
        public InputAction @OnRightClick => m_Wrapper.m_Player_OnRightClick;
        public InputAction @OnMiddleClick => m_Wrapper.m_Player_OnMiddleClick;
        public InputAction @MousePos => m_Wrapper.m_Player_MousePos;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @OnLeftClick.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOnLeftClick;
                @OnLeftClick.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOnLeftClick;
                @OnLeftClick.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOnLeftClick;
                @OnRightClick.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOnRightClick;
                @OnRightClick.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOnRightClick;
                @OnRightClick.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOnRightClick;
                @OnMiddleClick.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOnMiddleClick;
                @OnMiddleClick.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOnMiddleClick;
                @OnMiddleClick.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOnMiddleClick;
                @MousePos.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMousePos;
                @MousePos.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMousePos;
                @MousePos.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMousePos;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @OnLeftClick.started += instance.OnOnLeftClick;
                @OnLeftClick.performed += instance.OnOnLeftClick;
                @OnLeftClick.canceled += instance.OnOnLeftClick;
                @OnRightClick.started += instance.OnOnRightClick;
                @OnRightClick.performed += instance.OnOnRightClick;
                @OnRightClick.canceled += instance.OnOnRightClick;
                @OnMiddleClick.started += instance.OnOnMiddleClick;
                @OnMiddleClick.performed += instance.OnOnMiddleClick;
                @OnMiddleClick.canceled += instance.OnOnMiddleClick;
                @MousePos.started += instance.OnMousePos;
                @MousePos.performed += instance.OnMousePos;
                @MousePos.canceled += instance.OnMousePos;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnOnLeftClick(InputAction.CallbackContext context);
        void OnOnRightClick(InputAction.CallbackContext context);
        void OnOnMiddleClick(InputAction.CallbackContext context);
        void OnMousePos(InputAction.CallbackContext context);
    }
}
