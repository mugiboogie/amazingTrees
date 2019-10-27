// GENERATED AUTOMATICALLY FROM 'Assets/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerControls : IInputActionCollection, IDisposable
{
    private InputActionAsset asset;
    public PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""d7480455-3f5a-495c-aaca-bd533ae42c6c"",
            ""actions"": [
                {
                    ""name"": ""LightAttack"",
                    ""type"": ""Button"",
                    ""id"": ""265e088d-1cb7-4243-b404-432cb6bfbecb"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""HeavyAttack"",
                    ""type"": ""Button"",
                    ""id"": ""42c7a3c6-0422-458c-bbc7-91d55cd160dd"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""79f40051-16d3-492b-8fba-cad88af765cd"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dodge"",
                    ""type"": ""Button"",
                    ""id"": ""f619c9ab-eb79-484f-849f-e385c8a13288"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PlayerMove"",
                    ""type"": ""Button"",
                    ""id"": ""c40a5918-4665-456f-b556-32f5d1a8cf9e"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraMove"",
                    ""type"": ""Button"",
                    ""id"": ""77c196c2-81b5-4db4-95ea-2e0a05db41a2"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LockOn"",
                    ""type"": ""Button"",
                    ""id"": ""c83d4651-27bb-4b6c-a959-9536cc7710c2"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4d3a5204-f6bc-43a8-a0c0-b438e01ba2ee"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""76fb7561-8293-4cde-bd5f-ee53ccd95f3f"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HeavyAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c22942ad-ea95-4c38-9036-b2d30484c9c7"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""20cd8a39-818c-4848-b256-12824ebfa284"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dodge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""629a2db8-286c-418a-9a4e-281f19f7f76d"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""42971e89-713b-44ce-a4d8-43c9f25e90ca"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c99c949d-cf97-4014-b6e1-76a14756b4cd"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LockOn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_LightAttack = m_Gameplay.FindAction("LightAttack", throwIfNotFound: true);
        m_Gameplay_HeavyAttack = m_Gameplay.FindAction("HeavyAttack", throwIfNotFound: true);
        m_Gameplay_Jump = m_Gameplay.FindAction("Jump", throwIfNotFound: true);
        m_Gameplay_Dodge = m_Gameplay.FindAction("Dodge", throwIfNotFound: true);
        m_Gameplay_PlayerMove = m_Gameplay.FindAction("PlayerMove", throwIfNotFound: true);
        m_Gameplay_CameraMove = m_Gameplay.FindAction("CameraMove", throwIfNotFound: true);
        m_Gameplay_LockOn = m_Gameplay.FindAction("LockOn", throwIfNotFound: true);
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

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_LightAttack;
    private readonly InputAction m_Gameplay_HeavyAttack;
    private readonly InputAction m_Gameplay_Jump;
    private readonly InputAction m_Gameplay_Dodge;
    private readonly InputAction m_Gameplay_PlayerMove;
    private readonly InputAction m_Gameplay_CameraMove;
    private readonly InputAction m_Gameplay_LockOn;
    public struct GameplayActions
    {
        private PlayerControls m_Wrapper;
        public GameplayActions(PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @LightAttack => m_Wrapper.m_Gameplay_LightAttack;
        public InputAction @HeavyAttack => m_Wrapper.m_Gameplay_HeavyAttack;
        public InputAction @Jump => m_Wrapper.m_Gameplay_Jump;
        public InputAction @Dodge => m_Wrapper.m_Gameplay_Dodge;
        public InputAction @PlayerMove => m_Wrapper.m_Gameplay_PlayerMove;
        public InputAction @CameraMove => m_Wrapper.m_Gameplay_CameraMove;
        public InputAction @LockOn => m_Wrapper.m_Gameplay_LockOn;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                LightAttack.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLightAttack;
                LightAttack.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLightAttack;
                LightAttack.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLightAttack;
                HeavyAttack.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnHeavyAttack;
                HeavyAttack.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnHeavyAttack;
                HeavyAttack.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnHeavyAttack;
                Jump.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                Jump.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                Jump.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                Dodge.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDodge;
                Dodge.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDodge;
                Dodge.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDodge;
                PlayerMove.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPlayerMove;
                PlayerMove.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPlayerMove;
                PlayerMove.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPlayerMove;
                CameraMove.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCameraMove;
                CameraMove.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCameraMove;
                CameraMove.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCameraMove;
                LockOn.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLockOn;
                LockOn.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLockOn;
                LockOn.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLockOn;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                LightAttack.started += instance.OnLightAttack;
                LightAttack.performed += instance.OnLightAttack;
                LightAttack.canceled += instance.OnLightAttack;
                HeavyAttack.started += instance.OnHeavyAttack;
                HeavyAttack.performed += instance.OnHeavyAttack;
                HeavyAttack.canceled += instance.OnHeavyAttack;
                Jump.started += instance.OnJump;
                Jump.performed += instance.OnJump;
                Jump.canceled += instance.OnJump;
                Dodge.started += instance.OnDodge;
                Dodge.performed += instance.OnDodge;
                Dodge.canceled += instance.OnDodge;
                PlayerMove.started += instance.OnPlayerMove;
                PlayerMove.performed += instance.OnPlayerMove;
                PlayerMove.canceled += instance.OnPlayerMove;
                CameraMove.started += instance.OnCameraMove;
                CameraMove.performed += instance.OnCameraMove;
                CameraMove.canceled += instance.OnCameraMove;
                LockOn.started += instance.OnLockOn;
                LockOn.performed += instance.OnLockOn;
                LockOn.canceled += instance.OnLockOn;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);
    public interface IGameplayActions
    {
        void OnLightAttack(InputAction.CallbackContext context);
        void OnHeavyAttack(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnDodge(InputAction.CallbackContext context);
        void OnPlayerMove(InputAction.CallbackContext context);
        void OnCameraMove(InputAction.CallbackContext context);
        void OnLockOn(InputAction.CallbackContext context);
    }
}
