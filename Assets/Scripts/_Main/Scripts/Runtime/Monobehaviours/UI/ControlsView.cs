using System.Collections.Generic;
using UnityEngine;


namespace InputManagement
{
    public class ControlsView : MonoBehaviour
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

        void Start()
        {
            InputManager.Instance.RegisterOnSwitchedToGamepad(OnSwitchedToNonTouchDevice);
            InputManager.Instance.RegisterOnSwitchedToKeyboard(OnSwitchedToNonTouchDevice);
            InputManager.Instance.RegisterOnSwitchedToTouch(OnSwitchedToTouchDevice);
        }

        #endregion


        #region CONSTRUCTOR
        #endregion


        #region PRIVATE_METHODS
        private void OnSwitchedToNonTouchDevice()
        {
            ToggleView(false);
        }

        private void OnSwitchedToTouchDevice()
        {
            ToggleView(true);
        }

        private void ToggleView(bool value) 
        {
            gameObject.SetActive(value);
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
        #endregion


        #region EDITOR_ACCESSORS_OR_HELPERS
#if UNITY_EDITOR

#endif
        #endregion

    }
}
