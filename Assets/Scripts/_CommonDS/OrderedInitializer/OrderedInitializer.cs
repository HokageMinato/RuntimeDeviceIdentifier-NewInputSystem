using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonDS
{
    public class OrderedInitializer : MonoBehaviour
    {

        #region PRIVATE_SERIALIZED_VARS
        [SerializeField] private InitializationTarget[] initializationTargets;
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
            CheckForDirtyActiveState();
            StartCoroutine(LoadRoutine());
        }

        private void CheckForDirtyActiveState()
        {
            for (int i = 0; i < initializationTargets.Length; i++)
            {
                InitializationTarget target = initializationTargets[i];
                if (target.gameObject.activeInHierarchy) 
                    throw new Exception($"{target.gameObject.name} 'GameObject' is active while it shouldnt be, Make sure to disable all ordered init targets before entering play mode or unwanted behaviours may arise");
            }
        }
        #endregion


        #region PRIVATE_METHODS
        private void CheckForStuckWarning(ref int elapsedTimeInSeconds,InitializationTarget target) 
        {
            if (elapsedTimeInSeconds > 10) 
            {
                Debug.LogError($"More than 10 seconds spent waiting for {target.gameObject.name} initialization, Might wanna check it out");
            }
            else
            {
                elapsedTimeInSeconds++;
            }
        }
        #endregion


        #region PUBLIC_METHODS
        #endregion


        #region EVENT_CALLBACKS
        #endregion


        #region COROUTINES
        private IEnumerator LoadRoutine() 
        {
            WaitForSeconds unitSecondWaitTime = new WaitForSeconds(1);
            

            for (int i = 0; i < initializationTargets.Length; i++)
            {
                int waitCounter = 0;
                InitializationTarget target = initializationTargets[i];
                target.BeginLoad();
               
                while (!target.IsInitialized)
                {
                    yield return unitSecondWaitTime;
                    CheckForStuckWarning(ref waitCounter,target);                
                }
            }
        }
        #endregion


        #region INNERCLASS_DEFINITIONS
        #endregion


        #region EDITOR_ACCESSORS_OR_HELPERS
        #if UNITY_EDITOR

        #endif
        #endregion


    }
}
