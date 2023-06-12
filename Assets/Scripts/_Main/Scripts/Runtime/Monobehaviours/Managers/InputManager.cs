using UnityEngine;
using UnityEngine.InputSystem;
using CommonDS;
using System.Collections.Generic;
using System;

namespace InputManagement
{
    public class InputManager : Singleton<InputManager>
    {
        #region PRIVATE_SERIALIZED_VARS
        [SerializeField] private InputActionAsset inputActionAsset;
        [SerializeField] private DeviceIdentityHandler deviceIdentityHandler;
        #endregion

        #region PRIVATE_VARS
        private InputActionMap activeInputMap = null;
        private List<Action> onSwitchedToGamepad = new();
        private List<Action> onSwtichedToKeyboard = new();
        private List<Action> onSwtichedToTouchpad = new();
        #endregion


        #region PRIVATE_PROPERTIES
        #endregion


        #region PUBLIC_VARS
        #endregion


        #region PUBLIC_PROPERTIES
        public InputActionAsset InputActionAsset { get { return inputActionAsset; } }
        #endregion


        #region UNITY_CALLBAKCS

        public override void Awake()
        {
            base.Awake();
            InitializeDeviceIdentityHandler();
            SwitchMapTo(Constants.InputConstants.Player); //TODO Handle this every UI change.
        }


        #endregion

        #region PRIVATE_METHODS
        private InputActionMap FindActionMap(string inputMapName)
        {
            return inputActionAsset.FindActionMap(inputMapName, true);
        }

        private void InitializeDeviceIdentityHandler()
        {
            deviceIdentityHandler.Init(inputActionAsset,
                               () => { Debug.Log($"<color=red> Im Gamepad </color>");  Invokee(onSwitchedToGamepad); },
                               () => { Debug.Log($"<color=red> Im keyboard </color>"); Invokee(onSwtichedToKeyboard); },
                               () => { Debug.Log($"<color=red> Im Touchpad </color>"); Invokee(onSwtichedToTouchpad); });
        }

        private void Invokee(List<Action> actions)
        {
            for (int i = 0; i < actions.Count; i++)
                actions[i].Invoke();
        }
        #endregion


        #region PUBLIC_METHODS
        public void SwitchMapTo(string inputMapName)
        {
            activeInputMap?.Disable();
            activeInputMap = FindActionMap(inputMapName);
            activeInputMap.Enable();
        }


        public void RegisterOnSwitchedToGamepad(Action action)
        {
            onSwitchedToGamepad.Add(action);
        }

        public void RegisterOnSwitchedToKeyboard(Action action)
        {
            onSwtichedToKeyboard.Add(action);
        }

        public void RegisterOnSwitchedToTouch(Action action)
        {
            onSwtichedToTouchpad.Add(action);
        }

        public void DeregisterOnSwitchedToGamepad(Action action)
        {
            onSwitchedToGamepad.Remove(action);
        }

        public void DeregisterOnSwitchedToKeyboard(Action action)
        {
            onSwtichedToKeyboard.Remove(action);
        }

        public void DeregisterOnSwitchedToTouch(Action action)
        {
            onSwtichedToTouchpad.Remove(action);
        }
        #endregion


        #region EVENT_CALLBACKS
        #endregion

        #region COROUTINES
        #endregion

        #region INNERCLASS_DEFINITIONS
        #endregion

    }

}