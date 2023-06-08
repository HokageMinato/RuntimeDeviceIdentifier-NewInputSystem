using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputManagement;

namespace InputManagement
{
    public class InitialSettingsManager : MonoBehaviour
    {

        #region PRIVATE_SERIALIZED_VARS
        #endregion


        #region PRIVATE_VARS
        #endregion


        #region PRIVATE_PROPERTIES
        #endregion


        #region PUBLIC_VARS
        #endregion


        #region PUBLIC_PROPERTIES
        #endregion


        #region UNITY_CALLBAKCS

        void Awake()
        {
            Init();
        }
        #endregion


        #region CONSTRUCTOR
        private void Init()
        {
            SetMaxFramerate();
            SetScreenAutoLock();
            CreateAndConnectTouchScreenDevice();
        }

        private void CreateAndConnectTouchScreenDevice()
        {
           DeviceConnector.CreateTouchScreenGamepad();
        }

        private void SetScreenAutoLock()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        private static void SetMaxFramerate()
        {
            Application.targetFrameRate = 60;
        }
        #endregion


        #region PRIVATE_METHODS
        #endregion


        #region PUBLIC_METHODS
        #endregion


        #region EVENT_CALLBACKS
        #endregion


        #region COROUTINES
        #endregion


        #region INNERCLASS_DEFINITIONS
        #endregion


        #region EDITOR_ACCESSORS_OR_HELPERS
#if UNITY_EDITOR

#endif
        #endregion


    }
}
