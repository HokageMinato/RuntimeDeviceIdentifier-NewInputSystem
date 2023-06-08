using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonDS
{
    public class InitializationTarget : MonoBehaviour
    {
        #region PRIVATE_SERIALIZED_VARS
        [SerializeField] private bool overrideAutoInitialize;
        [SerializeField] private bool isInitialized;
        #endregion


        #region PRIVATE_VARS
        #endregion


        #region PRIVATE_PROPERTIES
        #endregion


        #region PUBLIC_VARS
        #endregion


        #region PUBLIC_PROPERTIES
        public bool IsInitialized { get { return isInitialized; } }
        #endregion


        #region UNITY_CALLBAKCS

        void Awake()
        {
            //Insert Your Code Here

            SetInitFinalizationStatus();
        }

        #endregion


        #region PRIVATE_METHODS
        private void SetInitFinalizationStatus()
        {
            if (!overrideAutoInitialize)
                isInitialized = true;
        }
        #endregion


        #region PUBLIC_METHODS
        public void BeginLoad() 
        {
            gameObject.SetActive(true);
        }

        public void MarkInitializationComplete() 
        {
            if (!overrideAutoInitialize)
                throw new System.Exception("Trying to complete an auto load Init target, Make sure Auto-load is set to false");

            isInitialized = true;
        }
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
