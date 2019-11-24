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
                    ""name"": ""Dash"",
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
                },
                {
                    ""name"": ""Spell"",
                    ""type"": ""Button"",
                    ""id"": ""f9db9f7d-0c51-4c6e-b166-eda98397c3ff"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Change"",
                    ""type"": ""Button"",
                    ""id"": ""e004cec4-7ef8-4080-a11e-d7b169ed8ad9"",
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
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""LightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f7f27e88-6194-4198-8b9a-4f606246c4f7"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""LightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""76fb7561-8293-4cde-bd5f-ee53ccd95f3f"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""HeavyAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b4a659e9-a42b-4e49-bce3-f4052b12d642"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""HeavyAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c22942ad-ea95-4c38-9036-b2d30484c9c7"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""56f2a046-4ebc-4b81-98f5-98ab68214bb8"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""20cd8a39-818c-4848-b256-12824ebfa284"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2072452b-7317-41a2-8b0f-4ad82129c43a"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""629a2db8-286c-418a-9a4e-281f19f7f76d"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""PlayerMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""41d8d529-a3e6-41bb-9be8-9aabf154498c"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerMove"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""46c32fa8-093a-4f7e-b871-381fd150ff2b"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""PlayerMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""96917d52-26e6-4af7-828c-6917d26f5c23"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""PlayerMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""243f16c7-3890-40fa-82e1-e337f9f87723"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""PlayerMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""ca4084fd-1c09-4bfd-a99c-58571f4736c6"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""PlayerMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""42971e89-713b-44ce-a4d8-43c9f25e90ca"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""CameraMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cc81ec19-9f5a-488e-b077-6108b82c4dc7"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": ""ScaleVector2(x=0.022,y=0.0198)"",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""CameraMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c99c949d-cf97-4014-b6e1-76a14756b4cd"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""LockOn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a3b8e472-d217-460c-b9a5-1a722cfe43cf"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""LockOn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f8f2fb49-f71f-4897-b55c-847ac9463deb"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Spell"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7f435eb8-f1ed-41bf-88ef-2e5e0be6c883"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Spell"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dd5aceb5-96c3-4efe-bcb5-4c31e836dc62"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Change"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""48617c68-0129-441f-b811-3e9ef9256744"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Change"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard and Mouse"",
            ""bindingGroup"": ""Keyboard and Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_LightAttack = m_Gameplay.FindAction("LightAttack", throwIfNotFound: true);
        m_Gameplay_HeavyAttack = m_Gameplay.FindAction("HeavyAttack", throwIfNotFound: true);
        m_Gameplay_Jump = m_Gameplay.FindAction("Jump", throwIfNotFound: true);
        m_Gameplay_Dash = m_Gameplay.FindAction("Dash", throwIfNotFound: true);
        m_Gameplay_PlayerMove = m_Gameplay.FindAction("PlayerMove", throwIfNotFound: true);
        m_Gameplay_CameraMove = m_Gameplay.FindAction("CameraMove", throwIfNotFound: true);
        m_Gameplay_LockOn = m_Gameplay.FindAction("LockOn", throwIfNotFound: true);
        m_Gameplay_Spell = m_Gameplay.FindAction("Spell", throwIfNotFound: true);
        m_Gameplay_Change = m_Gameplay.FindAction("Change", throwIfNotFound: true);
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
    private readonly InputAction m_Gameplay_Dash;
    private readonly InputAction m_Gameplay_PlayerMove;
    private readonly InputAction m_Gameplay_CameraMove;
    private readonly InputAction m_Gameplay_LockOn;
    private readonly InputAction m_Gameplay_Spell;
    private readonly InputAction m_Gameplay_Change;
    public struct GameplayActions
    {
        private PlayerControls m_Wrapper;
        public GameplayActions(PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @LightAttack => m_Wrapper.m_Gameplay_LightAttack;
        public InputAction @HeavyAttack => m_Wrapper.m_Gameplay_HeavyAttack;
        public InputAction @Jump => m_Wrapper.m_Gameplay_Jump;
        public InputAction @Dash => m_Wrapper.m_Gameplay_Dash;
        public InputAction @PlayerMove => m_Wrapper.m_Gameplay_PlayerMove;
        public InputAction @CameraMove => m_Wrapper.m_Gameplay_CameraMove;
        public InputAction @LockOn => m_Wrapper.m_Gameplay_LockOn;
        public InputAction @Spell => m_Wrapper.m_Gameplay_Spell;
        public InputAction @Change => m_Wrapper.m_Gameplay_Change;
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
                Dash.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                Dash.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                Dash.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                PlayerMove.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPlayerMove;
                PlayerMove.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPlayerMove;
                PlayerMove.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPlayerMove;
                CameraMove.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCameraMove;
                CameraMove.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCameraMove;
                CameraMove.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCameraMove;
                LockOn.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLockOn;
                LockOn.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLockOn;
                LockOn.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLockOn;
                Spell.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSpell;
                Spell.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSpell;
                Spell.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSpell;
                Change.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnChange;
                Change.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnChange;
                Change.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnChange;
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
                Dash.started += instance.OnDash;
                Dash.performed += instance.OnDash;
                Dash.canceled += instance.OnDash;
                PlayerMove.started += instance.OnPlayerMove;
                PlayerMove.performed += instance.OnPlayerMove;
                PlayerMove.canceled += instance.OnPlayerMove;
                CameraMove.started += instance.OnCameraMove;
                CameraMove.performed += instance.OnCameraMove;
                CameraMove.canceled += instance.OnCameraMove;
                LockOn.started += instance.OnLockOn;
                LockOn.performed += instance.OnLockOn;
                LockOn.canceled += instance.OnLockOn;
                Spell.started += instance.OnSpell;
                Spell.performed += instance.OnSpell;
                Spell.canceled += instance.OnSpell;
                Change.started += instance.OnChange;
                Change.performed += instance.OnChange;
                Change.canceled += instance.OnChange;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);
    private int m_KeyboardandMouseSchemeIndex = -1;
    public InputControlScheme KeyboardandMouseScheme
    {
        get
        {
            if (m_KeyboardandMouseSchemeIndex == -1) m_KeyboardandMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard and Mouse");
            return asset.controlSchemes[m_KeyboardandMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IGameplayActions
    {
        void OnLightAttack(InputAction.CallbackContext context);
        void OnHeavyAttack(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnPlayerMove(InputAction.CallbackContext context);
        void OnCameraMove(InputAction.CallbackContext context);
        void OnLockOn(InputAction.CallbackContext context);
        void OnSpell(InputAction.CallbackContext context);
        void OnChange(InputAction.CallbackContext context);
    }
}
