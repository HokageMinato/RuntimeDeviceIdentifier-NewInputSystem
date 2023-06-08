using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using InputManagement;

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

    void Awake()
    {
        DeviceIdentityHandler.RegisterOnSwitchedToGamepad(OnSwitchedToNonTouchDevice);
        DeviceIdentityHandler.RegisterOnSwitchedToKeyboard(OnSwitchedToNonTouchDevice);
        DeviceIdentityHandler.RegisterOnSwitchedToTouch(OnSwitchedToTouchDevice);
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
    private void OnSwitchedToNonTouchDevice() 
    { 
        
    }
    
    private void OnSwitchedToTouchDevice() 
    { 
        
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
