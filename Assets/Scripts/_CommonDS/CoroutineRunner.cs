using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonDS
{
    public class CoroutineRunner : Singleton<CoroutineRunner>
    {

        #region PRIVATE_SERIALIZED_VARS
        #endregion


        #region PRIVATE_VARS
        private List<string> activeRoutines = new();
        #endregion


        #region PRIVATE_PROPERTIES
        #endregion


        #region PUBLIC_VARS
        #endregion


        #region PUBLIC_PROPERTIES
        #endregion


        #region UNITY_CALLBAKCS
        public override void OnDestroy()
        {
            StopAllCoroutines();
            base.OnDestroy();
        }

        #endregion


        #region CONSTRUCTOR
        #endregion


        #region PRIVATE_METHODS

        #endregion


        #region PUBLIC_METHODS
        public void DelayedInvoke(Action action,float waitTimeInSeconds) 
        {
            IEnumerator routine = DelayedInvokeRoutine(action, waitTimeInSeconds);
            StartCoroutine(Schedule(routine));
            
        }

        public void StartRoutine(IEnumerator targetCoroutine,string explicitName = null) 
        {
            StartCoroutine(Schedule(targetCoroutine, explicitName));
        }
        #endregion


        #region EVENT_CALLBACKS
        #endregion


        #region COROUTINES
        private IEnumerator Schedule(IEnumerator targetRoutine,string explicitName = null)
        {
            var tuple = targetRoutine.GetType().Name;
            activeRoutines.Add(tuple);
            //Debug.Log("Added");
            yield return targetRoutine;
            //Debug.Log("Removed");
            activeRoutines.Remove(tuple);
        }

        private IEnumerator DelayedInvokeRoutine(Action action, float waitTimeInSeconds) 
        {
            WaitForSeconds seconds = new WaitForSeconds(waitTimeInSeconds);
            yield return seconds;
            action();
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
