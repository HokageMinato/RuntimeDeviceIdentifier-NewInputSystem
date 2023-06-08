using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

namespace InputManagement
{
    public class DeviceIdentityHandler : MonoBehaviour
    {

        #region PRIVATE_SERIALIZED_VARS
        #endregion


        #region PRIVATE_VARS
        private InputActionAsset inputActionAsset;
        private Dictionary<InputControl, InputBinding> inputControlEffectivePathCache = new();
        private static Dictionary<int, SUPPORTED_DEVICE_TYPE> gamepadDevicesDictCache = new();
        private static Dictionary<int, SUPPORTED_DEVICE_TYPE> keyboardDevicesDictCache = new();
        private static Dictionary<int, SUPPORTED_DEVICE_TYPE> touchScreenDeviceDictCache = new();


        private static List<Action> onSwitchedToGamepad = new();
        private static List<Action> onSwtichedToKeyboard = new();
        private static List<Action> onSwtichedToTouchpad = new();
        #endregion


        #region PRIVATE_PROPERTIES
        #endregion


        #region PUBLIC_VARS
        #endregion


        #region PUBLIC_PROPERTIES
        #endregion


        #region UNITY_CALLBAKCS

        void OnEnable()
        {
            ToggleInputMethodChangeLister(true);
        }

        void OnDisable()
        {
            ToggleInputMethodChangeLister(false);
        }
        #endregion

        #region CONSTRUCTOR
        public void Init(InputActionAsset inputActionAsset)
        {
            this.inputActionAsset = inputActionAsset;
        }
        #endregion

        #region PRIVATE_METHODS
        private void ToggleInputMethodChangeLister(bool register)
        {
            if (register)
            {
                InputSystem.onActionChange += InputSystem_onActionChange;
                InputSystem.onDeviceChange += InputSystem_onDeviceChange;
            }
            else
            {
                InputSystem.onActionChange -= InputSystem_onActionChange;
                InputSystem.onDeviceChange -= InputSystem_onDeviceChange;
            }
        }

        private void InputSystem_onDeviceChange(InputDevice arg1, InputDeviceChange arg2)
        {
            CheckForCacheInvalidation(arg1,arg2);
        }

        private void InputSystem_onActionChange(object actionObject, InputActionChange actionStatus)
        {
            InputAction action = (actionObject as InputAction);
            InputControl control = action?.activeControl;
            InputDevice currentDevice = control?.device;

            if (currentDevice == null || action == null)
                return;

            //most effective method without noise
            bool isValidControl;
            InputBinding binding;
            
            isValidControl = MatchInputControlPathFast(control, out binding);

            if (isValidControl)
            {
                PerformDeviceSchemeSwitch(action, currentDevice, binding);
                return;
            }

            if (MatchInputControlPathFull(control, out binding))
            {
                AddToInputControlBindingPathCache(control, binding);
                TryMapUnresolvedInput(currentDevice, binding);
                PerformDeviceSchemeSwitch(action, currentDevice, binding);
            }
        }

        private bool MatchInputControlPathFast(InputControl control, out InputBinding bindingResult)
        {
            bool res = inputControlEffectivePathCache.ContainsKey(control);
            bindingResult = (res) ? inputControlEffectivePathCache[control] : default;
            return res;
        }

        private static void PerformDeviceSchemeSwitch(InputAction action, InputDevice currentDevice, InputBinding binding)
        {
            //Debug.Log($"Binding {binding.effectivePath}, {binding.name} , {currentDevice.name} ,did:{currentDevice.deviceId} , {action.activeControl}");

            if (IsGamepad(currentDevice))
            {
                Debug.Log("GAMEPAD");
                InvokeActions(onSwitchedToGamepad);
                return;
            }

            if (IsKeyboard(currentDevice))
            {
                Debug.Log("Keyboard");
                InvokeActions(onSwtichedToKeyboard);
                return;
            }

            if (IsTouchPad(currentDevice))
            {
                Debug.Log("TouchPad");
                InvokeActions(onSwtichedToTouchpad);
                return;
            }

            
        }

        private bool MatchInputControlPathFull(InputControl control, out InputBinding bindingResult)
        {
            foreach (var binding in inputActionAsset.bindings)
            {
                if (InputControlPath.Matches(binding.effectivePath, control))
                {
                    bindingResult = binding;
                    return true;
                }
            }

            bindingResult = default;
            return false;
        }

        private void AddToInputControlBindingPathCache(InputControl control, InputBinding binding)
        {
            Debug.Log($"Added to Cache{control.device.name}");
            inputControlEffectivePathCache.Add(control, binding);
        }

        private static void TryMapUnresolvedInput(InputDevice currentDevice, InputBinding binding)
        {
            if (TryMapAsGamepad(currentDevice, binding, gamepadDevicesDictCache))
                return;
            if (TryMapAsKeyboard(currentDevice, binding, keyboardDevicesDictCache))
                return;
            if (TryMapAsTouchpad(currentDevice, binding, touchScreenDeviceDictCache))
                return;
        }

        private static bool TryMapAsGamepad(InputDevice device,InputBinding binding,Dictionary<int,SUPPORTED_DEVICE_TYPE> dictCache)
        {
            bool res = (ResolveDevice(SUPPORTED_DEVICE_TYPE.GAMEPAD, device, binding, dictCache));
            
            if(res)    
                Debug.Log($"Resolved AS {SUPPORTED_DEVICE_TYPE.GAMEPAD}");

            return res;
        }
        
        private static bool TryMapAsKeyboard(InputDevice device,InputBinding binding,Dictionary<int,SUPPORTED_DEVICE_TYPE> dictCache)
        {
            bool res = (ResolveDevice(SUPPORTED_DEVICE_TYPE.KEYBOARD, device, binding, dictCache));
            
            if(res)
               Debug.Log($"Resolved AS {SUPPORTED_DEVICE_TYPE.KEYBOARD}");
            
            return res;
        }

        private static bool TryMapAsTouchpad(InputDevice device, InputBinding binding, Dictionary<int, SUPPORTED_DEVICE_TYPE> dictCache)
        {
            bool res = ResolveDevice(SUPPORTED_DEVICE_TYPE.TOUCHSCREENPAD, device, binding, dictCache);
          
            if (res)
                Debug.Log($"Resolved As {SUPPORTED_DEVICE_TYPE.TOUCHSCREENPAD}");
            
            return res;
        }


        private static bool ResolveDevice(SUPPORTED_DEVICE_TYPE supportType,InputDevice device, InputBinding binding, Dictionary<int, SUPPORTED_DEVICE_TYPE> dictCache)
        {
            string loweredEffectivePath = binding.effectivePath.ToLower();
            string loweredSupportedDevId = supportType.ToString().ToLower();
            int deviceId = device.deviceId;

            //Debug.Log($"CONTAINER {loweredEffectivePath} SUPPDEV {loweredSupportedDevId}");

            if (loweredEffectivePath.Contains(loweredSupportedDevId) && !dictCache.ContainsKey(deviceId))
            {
                dictCache.Add(deviceId, supportType);
                return true;
            }
            return false;
        }

        private static bool IsGamepad(InputDevice currentDevice) 
        {
            return IsInDeviceIdCache(currentDevice, gamepadDevicesDictCache);
        }
        
        private static bool IsKeyboard(InputDevice currentDevice) 
        {
            return IsInDeviceIdCache(currentDevice, keyboardDevicesDictCache);
        }
        
        private static bool IsTouchPad(InputDevice currentDevice) 
        {
            return IsInDeviceIdCache(currentDevice, touchScreenDeviceDictCache);
        }

        private static bool IsInDeviceIdCache(InputDevice device,Dictionary<int,SUPPORTED_DEVICE_TYPE> dictCache) 
        {
            return dictCache.ContainsKey(device.deviceId);
        }

        private static void InvokeActions(List<Action> invokees) 
        {
            for (int i = 0; i < invokees.Count; i++)
                invokees[i]();
        }

        private void CheckForCacheInvalidation(InputDevice device,InputDeviceChange deviceChangeType)
        {
            bool invalidateCache = (deviceChangeType == InputDeviceChange.UsageChanged ||
                                    deviceChangeType == InputDeviceChange.ConfigurationChanged);

            if (invalidateCache)
            {
                inputControlEffectivePathCache.Clear();
                gamepadDevicesDictCache.Clear();
                keyboardDevicesDictCache.Clear();
                touchScreenDeviceDictCache.Clear();
                Debug.Log($"<color=red>Input Cache Invalidated, reason {device} : {deviceChangeType}</color>");
            }
        }
        #endregion

        #region PUBLIC_METHODS
        public static void RegisterOnSwitchedToGamepad(Action action) 
        { 
            onSwitchedToGamepad.Add(action);
        }
        
        public static void RegisterOnSwitchedToKeyboard(Action action) 
        {
            onSwtichedToKeyboard.Add(action);
        }
        
        public static void RegisterOnSwitchedToTouch(Action action) 
        {
            onSwtichedToTouchpad.Add(action);
        }
        
        public static void DeregisterOnSwitchedToGamepad(Action action) 
        { 
            onSwitchedToGamepad.Remove(action);
        }
        
        public static void DeregisterOnSwitchedToKeyboard(Action action) 
        {
            onSwtichedToKeyboard.Remove(action);
        }
        
        public static void DeregisterOnSwitchedToTouch(Action action) 
        {
            onSwtichedToTouchpad.Remove(action);
        }
        #endregion


        #region EVENT_CALLBACKS
        #endregion


        #region COROUTINES
        #endregion


        #region UI_CALLBACKS
        #endregion


        #region INNERCLASS_DEFINITIONS
        public enum SUPPORTED_DEVICE_TYPE 
        { 
            TOUCHSCREENPAD,
            GAMEPAD,
            KEYBOARD
        }
        #endregion


        #region EDITOR_ACCESSORS_OR_HELPERS
        #if UNITY_EDITOR
        #endif
        #endregion


    }
}
