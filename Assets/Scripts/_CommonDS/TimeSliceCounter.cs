using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonDS
{
    [Serializable]
    public class TimeSliceCounter 
    {

        #region PRIVATE_SERIALIZED_VARS
        [SerializeField][Range(64f, 600f)] private float resultClearTimeInMilliSeconds;
        #endregion


        #region PRIVATE_VARS
        private float lastSaveTime = 1;
        #endregion


        #region PRIVATE_PROPERTIES
        private static float GetTimeInMilliseconds()
        {
         //   Debug.Log($"time{Time.time}----utime{Time.unscaledTime}----dt{Time.deltaTime}----usdt{Time.unscaledDeltaTime}");

            return Time.time * 1000;
        }
        #endregion


        #region PUBLIC_VARS
        #endregion


        #region PUBLIC_PROPERTIES
        //public void UpdateLastQueriedTime()
        //{
        //    lastSaveTime = GetTimeInMilliseconds();
        //}

        public bool HasTimeSliceExpired()
        {
            float currentTime = GetTimeInMilliseconds();
            float interval = (currentTime - lastSaveTime);
            bool res = interval > resultClearTimeInMilliSeconds;

            if (res)
                lastSaveTime = currentTime;

            return res;
        }
        #endregion


        #region UNITY_CALLBAKCS

        void Awake()
        {

        }

        void OnEnable()
        {

        }

        void Start()
        {

        }

        void Update()
        {

        }

        void OnDisable()
        {

        }

        void OnDestroy()
        {

        }

        #endregion

        #region CONSTRUCTOR
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
