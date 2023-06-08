using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonDS
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {

        #region PRIVATE_SERIALIZED_VARS
        #endregion


        #region PRIVATE_VARS
        #endregion


        #region PRIVATE_PROPERTIES
        #endregion


        #region PUBLIC_VARS
        private static T instance;
        #endregion


        #region PUBLIC_PROPERTIES
        public static T Instance { get{ return instance; }} 
        #endregion


        #region UNITY_CALLBAKCS

        public virtual void Awake()
        {
            if (instance == null)
                instance = this as T;
        }

        public virtual void OnDestroy()
        {
            instance = null;
            //GC Cleanup
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
