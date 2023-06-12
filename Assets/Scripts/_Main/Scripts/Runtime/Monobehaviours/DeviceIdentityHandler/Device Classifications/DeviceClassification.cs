using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputManagement
{
    public class DeviceClassification
    {

        #region PRIVATE_SERIALIZED_VARS
        #endregion


        #region PRIVATE_VARS
        private int deviceClassifierId;
        private string deviceClassificationName;
        private string[] physicalDevicesToMap;
        private string[] emulatedDevicesToMap;
        private Action onSwitchedTo;        
       
        private HashSet<int> mappedDeviceCache = new HashSet<int>();
        #endregion


        #region PRIVATE_PROPERTIES
        #endregion


        #region PUBLIC_VARS
        #endregion


        #region PUBLIC_PROPERTIES
        public int DeviceClassifierId { get { return deviceClassifierId; } }
        public HashSet<int> MappedDeviceCache { get { return mappedDeviceCache; } }
        public string DeviceClassifierName { get { return deviceClassificationName; } }
        #endregion


        #region UNITY_CALLBAKCS
        #endregion


        #region CONSTRUCTOR
        public DeviceClassification(int classifierId,string classificationName,string[] physicalDevices, string[] emulatedDevices,Action onSwitchedTo) 
        {
            deviceClassificationName = classificationName;
            deviceClassifierId = classifierId;
            physicalDevicesToMap = physicalDevices;
            emulatedDevicesToMap = (emulatedDevices == null)?new string[0]:emulatedDevices;
            this.onSwitchedTo = onSwitchedTo;
        }
        #endregion


        #region PRIVATE_METHODS
        #endregion


        #region PUBLIC_METHODS
        public void InValidate() 
        {
            MappedDeviceCache.Clear();
        }

        public bool FastMatch(InputDevice actualDevice) 
        {
            return mappedDeviceCache.Contains(actualDevice.deviceId);
        }

        public bool FullMatch(InputDevice actualDevice,InputBinding inputBinding,bool IsEmulatedOnScreen) 
        {
            string[] devicesToBeResolved = (IsEmulatedOnScreen) ? emulatedDevicesToMap : physicalDevicesToMap;
            return ResolveDevices(devicesToBeResolved,actualDevice,inputBinding);
        }

        public void InvokeOnSwitchedTo() 
        {
            Debug.Log($"INVOKEED -> {deviceClassificationName}");
            onSwitchedTo();
        }

        private bool ResolveDevices(string[] devicesToBeMapped,InputDevice actualDevice,InputBinding inputBinding) 
        {
            for (int i = 0; i < devicesToBeMapped.Length; i++)
            {
                if (TryResolveDevice(devicesToBeMapped[i], actualDevice, inputBinding))
                    return true;
            }
            return false;
        }

        private bool TryResolveDevice(string deviceName, InputDevice device, InputBinding binding) 
        {
            string loweredEffectivePath = binding.effectivePath.ToLower();
            string loweredDeviceName = deviceName;
            int deviceId = device.deviceId;
            bool resolveSuccesful = false;

            if (loweredEffectivePath.Contains(loweredDeviceName) && !mappedDeviceCache.Contains(deviceId))
            {
                mappedDeviceCache.Add(deviceId);
                resolveSuccesful = true;
            }

            Debug.Log($"CONTAINER {loweredEffectivePath} SUPPDEVID {loweredDeviceName} CLASSTYP {device.GetType()} RESOLVED AS {((resolveSuccesful) ? this.deviceClassificationName : "Unsuccesful")} CacheStatus: PATHCONTAINSCHECK {loweredEffectivePath.Contains(loweredDeviceName)}   DICTCONTAINTSCHECK {mappedDeviceCache.Contains(deviceId)}");
            return resolveSuccesful;
        }
        #endregion


        #region EVENT_CALLBACKS
        #endregion


        #region COROUTINES
        #endregion

        #region UI_CALLBACKS
        #endregion

        #region INNERCLASS_DEFINITIONS
        #endregion


        #region EDITOR_ACCESSORS_OR_HELPERS
#if UNITY_EDITOR

#endif
        #endregion


    }
}
