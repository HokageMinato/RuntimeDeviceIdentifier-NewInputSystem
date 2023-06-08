using UnityEngine;
using UnityEngine.InputSystem;
using CommonDS;
using UnityEngine.InputSystem.Users;

namespace InputManagement
{


    public class InputManager : Singleton<InputManager>
    {
        #region PRIVATE_SERIALIZED_VARS
        [SerializeField] private InputActionAsset inputActionAsset;
        [SerializeField] private DeviceIdentityHandler deviceIdentityHandler;
        #endregion

        #region PRIVATE_VARS
        private InputActionMap activeInputMap = null;
        #endregion


        #region PRIVATE_PROPERTIES
        #endregion


        #region PUBLIC_VARS
        #endregion


        #region PUBLIC_PROPERTIES
        public InputActionAsset InputActionAsset { get { return inputActionAsset; } }
        #endregion


        #region UNITY_CALLBAKCS

        public override void Awake()
        {
            base.Awake();
            deviceIdentityHandler.Init(inputActionAsset);
            SwitchMapTo(Constants.InputConstants.Player); //TODO Handle this every UI change.
        }
        #endregion

        #region PRIVATE_METHODS
        private InputActionMap FindActionMap(string inputMapName)
        {
            return inputActionAsset.FindActionMap(inputMapName, true);
        }
        #endregion


        #region PUBLIC_METHODS
        public void SwitchMapTo(string inputMapName)
        {
            activeInputMap?.Disable();
            activeInputMap = FindActionMap(inputMapName);
            activeInputMap.Enable();
        }
        #endregion


        #region EVENT_CALLBACKS
        #endregion

        #region COROUTINES
        #endregion

        #region INNERCLASS_DEFINITIONS
        #endregion

    }

}