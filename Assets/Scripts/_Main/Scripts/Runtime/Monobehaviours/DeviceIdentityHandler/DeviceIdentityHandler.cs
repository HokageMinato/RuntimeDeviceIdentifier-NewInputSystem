using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputManagement
{
    public class DeviceIdentityHandler : MonoBehaviour
    {

        #region PRIVATE_SERIALIZED_VARS
        #endregion


        #region PRIVATE_VARS
        private static bool IsClassifierReady = false;
        private static string OnScreenTag = "OnScreen";


        private static InputActionAsset myInputActionAsset;
        private static Dictionary<InputControl, InputBinding> inputControlEffectivePathCache = new();

        private static DeviceClassification presentDevice;
        private static Action OnSwitchedToGamepad;
        private static Action OnSwtichedToKeyboard;
        private static Action OnSwtichedToTouch;

        #endregion


        #region PRIVATE_PROPERTIES
        private static List<DeviceClassification> deviceClassifications = new List<DeviceClassification>();
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

        public void Init(InputActionAsset inputActionAsset, Action onSwitchedToGamepad, Action onSwtichedToKeyboard, Action onSwtichedToTouchpad)
        {
            myInputActionAsset = inputActionAsset;
            OnSwitchedToGamepad = onSwitchedToGamepad;
            OnSwtichedToKeyboard = onSwtichedToKeyboard;
            OnSwtichedToTouch = onSwtichedToTouchpad;

            GenerateDeviceClassifications();
        }

        private void GenerateDeviceClassifications()
        {

            deviceClassifications.Add(new DeviceClassification(
                                     (int)DEVICE_CLASSIFICATION.TOUCHSCREEN,
                                     DEVICE_CLASSIFICATION.TOUCHSCREEN.ToString(),
                                     new string[] {"touchscreenpad","touchscreen", "mouse"},
                                     new string[] {"gamepad","keyboard"},OnSwtichedToTouch));

            deviceClassifications.Add(new DeviceClassification(
                                     (int)DEVICE_CLASSIFICATION.GAMEPAD,
                                     DEVICE_CLASSIFICATION.GAMEPAD.ToString(),
                                     new string[] {"gamepad"},null, OnSwitchedToGamepad));
            
            
            deviceClassifications.Add(new DeviceClassification(
                                     (int)DEVICE_CLASSIFICATION.KEYBOARD,
                                     DEVICE_CLASSIFICATION.KEYBOARD.ToString(),
                                     new string[] {"keyboard"},null, OnSwtichedToKeyboard));


            presentDevice = deviceClassifications[0];

            IsClassifierReady = true;
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


        private void InputSystem_onActionChange(object actionObject, InputActionChange actionStatus)
        {
            if (actionStatus != InputActionChange.ActionPerformed || !IsClassifierReady)
                return;

            DetectHardware(actionObject);
        }


        private void InputSystem_onDeviceChange(InputDevice device, InputDeviceChange deviceChangeType)
        {
            bool isCacheInvalidationRequried = (deviceChangeType == InputDeviceChange.UsageChanged ||
                                    deviceChangeType == InputDeviceChange.ConfigurationChanged ||
                                    deviceChangeType == InputDeviceChange.Disconnected);

            if (isCacheInvalidationRequried)
            {
                InvalidateCache();
                Debug.Log($"<color=red>Input Cache Invalidated, reason {device} : {deviceChangeType}</color>");
            }

        }


        private void DetectHardware(object actionObject)
        {
            InputAction action = (actionObject as InputAction);
            InputControl control = action?.activeControl;
            InputDevice currentDevice = control?.device;

            if (currentDevice == null || action == null)
                return;

            InputBinding binding;

            if (MatchInputControlPathFast(control, out binding))
            {
                PerformDeviceSchemeSwitch(currentDevice);
                return;
            }

            if (MatchInputControlPathFull(control, out binding))
            {
                AddToInputControlBindingPathCache(control, binding);
                TryMapUnresolvedInput(currentDevice, binding);
                PerformDeviceSchemeSwitch(currentDevice);
            }
        }

        private bool MatchInputControlPathFast(InputControl control, out InputBinding bindingResult)
        {
            bool res = inputControlEffectivePathCache.ContainsKey(control);
            bindingResult = (res) ? inputControlEffectivePathCache[control] : default;
            return res;
        }

        private static void PerformDeviceSchemeSwitch(InputDevice currentDevice)
        {
            for (int i = 0; i < deviceClassifications.Count; i++)
            {
                DeviceClassification deviceClassification = deviceClassifications[i];

                if (deviceClassification.FastMatch(currentDevice) && 
                    presentDevice.DeviceClassifierId != deviceClassification.DeviceClassifierId)
                {
                    Debug.Log($"PRESENT {presentDevice.DeviceClassifierName} -- new {deviceClassification.DeviceClassifierName}");
                    presentDevice = deviceClassification;
                    presentDevice.InvokeOnSwitchedTo();
                    return;
                }
            }

        }

        private bool MatchInputControlPathFull(InputControl control, out InputBinding bindingResult)
        {
            foreach (var binding in myInputActionAsset.bindings)
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
            Debug.Log($"Added to Cache {control.device.name}");
            inputControlEffectivePathCache.Add(control, binding);
        }

        private static void TryMapUnresolvedInput(InputDevice currentDevice, InputBinding binding)
        {
            for (int i = 0; i < deviceClassifications.Count; i++)
            {
                DeviceClassification classification = deviceClassifications[i];
                if (classification.FullMatch(currentDevice,binding,IsEmulatedOnScreen(currentDevice))) 
                    Debug.Log($"Mapped as {classification.DeviceClassifierName}");
            }
        }


        private static bool IsEmulatedOnScreen(InputDevice device)
        {
            foreach (var usages in device.usages)
                if (usages.ToString().Contains(OnScreenTag))
                    return true;

            return false;
        }


        private void InvalidateCache()
        {
            inputControlEffectivePathCache.Clear();
            for (int i = 0; i < deviceClassifications.Count; i++)
                deviceClassifications[i].InValidate();
        }
        #endregion

        #region PUBLIC_METHODS

        #endregion


        #region EVENT_CALLBACKS
        #endregion


        #region COROUTINES
        #endregion


        #region UI_CALLBACKS
        #endregion


        #region INNERCLASS_DEFINITIONS
        /// <summary>
        /// DEFINE YOUR CLASSIFICATIONS HERE.
        /// THESE DENOTE THE REPRESENTATION
        /// YOU WANT INSTEAD OF PHYSICAL/VIRUTAL/EMULATED DEVICES PRESENTED BY UNITY.
        ///
        /// Constants declaration is optional, but since the comparision occurs every input dispatch,
        /// it is preferrable.
        /// </summary>

        //private const int TOUCHSCREEN = (int)DEVICE_CLASSIFICATION.TOUCHSCREEN;
        //private const int GAMEPAD = (int)DEVICE_CLASSIFICATION.GAMEPAD;
        //private const int KEYBOARD = (int)DEVICE_CLASSIFICATION.KEYBOARD;

        public enum DEVICE_CLASSIFICATION
        {
            TOUCHSCREEN = 0,
            GAMEPAD = 1,
            KEYBOARD = 2,
        }
        #endregion


        #region EDITOR_ACCESSORS_OR_HELPERS

        private static void PrintDeviceUsages(InputDevice currentDevice)
        {
            string all = $"DEVICE DATA: ";
            foreach (var item in InputSystem.devices)
            {
                string st = $"{item.name}:\n";
                foreach (var usages in item.usages)
                {
                    st += $" {usages} \n";
                }
                all += $"{st} \n ----------";
            }

            string stt = $"{currentDevice.name}:";
            foreach (var usages in currentDevice.usages)
            {
                stt += $"{usages} \n";
            }


            Debug.Log($"All {all} -- {currentDevice.name} -- Current:{stt}");
        }

        private static void PrintControlData(InputControl control)
        {
            Debug.Log("<color=red>----------------------------------------------------------------------------------------------------------------------------</color>");
            Debug.Log($"fIdx{Time.frameCount} CONTROL {control.name} -- {control.device.name} -- {control.device.displayName}");
        }

        #if UNITY_EDITOR
        #endif
        #endregion


    }
}
