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
        private static string KeyBoard = "keyboard";
        private static string GamePad = "gamepad";
        private static string TouchscreenPad = "touchscreenpad";
        private static string TouchScreen = "touchscreen";
        private static string Mousee = "mouse";


        private static InputActionAsset myInputActionAsset;
        private static Dictionary<InputControl, InputBinding> inputControlEffectivePathCache = new();
        private static List<Dictionary<int, int>> deviceClassificationDicts = new();


        private static int presentDevice = (int)DEVICE_CLASSIFICATION.TOUCHSCREEN;
        private static Action OnSwitchedToGamepad;
        private static Action OnSwtichedToKeyboard;
        private static Action OnSwtichedToTouch;

        #endregion


        #region PRIVATE_PROPERTIES
        private static Dictionary<int, int> GamepadDevicesDictCache => deviceClassificationDicts[(int)DEVICE_CLASSIFICATION.GAMEPAD];
        private static Dictionary<int, int> KeyboardDevicesDictCache => deviceClassificationDicts[(int)DEVICE_CLASSIFICATION.KEYBOARD];
        private static Dictionary<int, int> TouchScreenDeviceDictCache => deviceClassificationDicts[(int)DEVICE_CLASSIFICATION.TOUCHSCREEN];

        /// <summary>
        /// Add your dictCache property here or simply access according to the indexed Enumerated Value of DEVICECLASSIFICATION.
        /// </summary>
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

            GenerateDeviceRepresentationDict();
        }

        private void GenerateDeviceRepresentationDict()
        {
            string[] values = Enum.GetNames(typeof(DEVICE_CLASSIFICATION));
            for (int i = 0; i < values.Length; i++)
            {
                var deviceCacheDict = new Dictionary<int, int>();
                deviceClassificationDicts.Add(deviceCacheDict);
            }

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

            //PrintControlData(control);
            //PrintDeviceUsages(currentDevice);

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
            if ((IsTouchPad(currentDevice)) && presentDevice != TOUCHSCREEN)
            {
                Debug.Log($"present: {DEVICE_CLASSIFICATION.TOUCHSCREEN} prev {(DEVICE_CLASSIFICATION)presentDevice}");
                presentDevice = TOUCHSCREEN;
                InvokeActions(OnSwtichedToTouch);
                return;
            }

            if (IsGamepad(currentDevice) && presentDevice != GAMEPAD)
            {
                Debug.Log($"present {DEVICE_CLASSIFICATION.GAMEPAD} prev {(DEVICE_CLASSIFICATION)presentDevice}");
                presentDevice = GAMEPAD;
                InvokeActions(OnSwitchedToGamepad);
                return;
            }

            if (IsKeyboard(currentDevice) && presentDevice != KEYBOARD)
            {
                Debug.Log($"present {DEVICE_CLASSIFICATION.KEYBOARD}  prev {(DEVICE_CLASSIFICATION)presentDevice}");
                presentDevice = KEYBOARD;
                InvokeActions(OnSwtichedToKeyboard);
                return;
            }

            ///Extend your representations here.
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
            if (TryMapAsGamepad(currentDevice, binding))
            {
                Debug.Log("Mapped as gamepad");
                return;
            }

            if (TryMapAsKeyboard(currentDevice, binding))
            {
                Debug.Log("Mapped as keyboard");
                return;
            }

            if (TryMapAsTouchpad(currentDevice, binding))
            {
                Debug.Log("Mapped as touchpad");
                return;
            }
        }

        private static bool TryMapAsGamepad(InputDevice device, InputBinding binding)
        {
            if (!IsEmulatedOnScreen(device))
            {
                return TryResolveDevice(deviceName: GamePad, mapToDevice: DEVICE_CLASSIFICATION.GAMEPAD, device, binding, GamepadDevicesDictCache);
            }

            return false;
        }

        private static bool TryMapAsKeyboard(InputDevice device, InputBinding binding)
        {
            bool res = TryResolveDevice(deviceName: KeyBoard, mapToDevice: DEVICE_CLASSIFICATION.KEYBOARD, device, binding, KeyboardDevicesDictCache);
            Debug.Log($"{IsEmulatedOnScreen(device)} -^^^- {res}");

            if (!IsEmulatedOnScreen(device))
            {
                return res;
            }

            return false;
        }

        private static bool TryMapAsTouchpad(InputDevice device, InputBinding binding)
        {
            ///This is where actual remapping occurs
            ///Make sure to provice propernames and the device they are mapped to.
            ///Based on this the cache will be generated.

            if (!IsEmulatedOnScreen(device))
            {
                if (TryResolveDevice(deviceName: TouchscreenPad, mapToDevice: DEVICE_CLASSIFICATION.TOUCHSCREEN, device, binding, TouchScreenDeviceDictCache))
                    return true;

                if (TryResolveDevice(deviceName: TouchScreen, mapToDevice: DEVICE_CLASSIFICATION.TOUCHSCREEN, device, binding, TouchScreenDeviceDictCache))
                    return true;

                if (TryResolveDevice(deviceName: Mousee, mapToDevice: DEVICE_CLASSIFICATION.TOUCHSCREEN, device, binding, TouchScreenDeviceDictCache))
                    return true;
            }

            if (IsEmulatedOnScreen(device))
            {
                Debug.Log($"{device.name} MAPPING AS EMULATED");

                if (TryResolveDevice(deviceName: GamePad, mapToDevice: DEVICE_CLASSIFICATION.TOUCHSCREEN, device, binding, TouchScreenDeviceDictCache))
                    return true;

                if (TryResolveDevice(deviceName: KeyBoard, mapToDevice: DEVICE_CLASSIFICATION.TOUCHSCREEN, device, binding, TouchScreenDeviceDictCache))
                    return true;
            }

            return false;
        }


        private static bool IsEmulatedOnScreen(InputDevice device)
        {
            foreach (var usages in device.usages)
                if (usages.ToString().Contains(OnScreenTag))
                    return true;

            return false;
        }

        private static bool TryResolveDevice(string deviceName, DEVICE_CLASSIFICATION mapToDevice, InputDevice device, InputBinding binding, Dictionary<int, int> dictCache)
        {
            string loweredEffectivePath = binding.effectivePath.ToLower();
            string loweredDeviceName = deviceName;
            int deviceId = device.deviceId;
            bool resolveSuccesful = false;

            if (loweredEffectivePath.Contains(loweredDeviceName) && !dictCache.ContainsKey(deviceId))
            {
                dictCache.Add(deviceId, (int)mapToDevice);
                resolveSuccesful = true;
            }

            Debug.Log($"CONTAINER {loweredEffectivePath} SUPPDEVID {loweredDeviceName} CLASSTYP {device.GetType()} RESOLVED AS {((resolveSuccesful) ? mapToDevice : "Unsuccesful")} CacheStatus: PATHCONTAINSCHECK {loweredEffectivePath.Contains(loweredDeviceName)}   DICTCONTAINTSCHECK {dictCache.ContainsKey(deviceId)}");
            return resolveSuccesful;
        }

        private static bool IsGamepad(InputDevice currentDevice)
        {
            return IsInDeviceIdCache(currentDevice, GamepadDevicesDictCache);
        }

        private static bool IsKeyboard(InputDevice currentDevice)
        {
            return IsInDeviceIdCache(currentDevice, KeyboardDevicesDictCache);
        }

        private static bool IsTouchPad(InputDevice currentDevice)
        {
            return IsInDeviceIdCache(currentDevice, TouchScreenDeviceDictCache);
        }

        private static bool IsInDeviceIdCache(InputDevice device, Dictionary<int, int> dictCache)
        {
            return dictCache.ContainsKey(device.deviceId);
        }

        private static void InvokeActions(Action invokees)
        {
            Debug.Log($"INVOKEDD");
            invokees?.Invoke();
        }

        private void InvalidateCache()
        {
            inputControlEffectivePathCache.Clear();
            GamepadDevicesDictCache.Clear();
            KeyboardDevicesDictCache.Clear();
            TouchScreenDeviceDictCache.Clear();
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

        private const int TOUCHSCREEN = (int)DEVICE_CLASSIFICATION.TOUCHSCREEN;
        private const int GAMEPAD = (int)DEVICE_CLASSIFICATION.GAMEPAD;
        private const int KEYBOARD = (int)DEVICE_CLASSIFICATION.KEYBOARD;

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
