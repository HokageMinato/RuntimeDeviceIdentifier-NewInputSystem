using UnityEngine;
using UnityEditor;

namespace InputManagement.EditorUtils { 

    public class DeviceConnectorEditorExtension : MonoBehaviour
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


        [MenuItem("TouchScreenPad/Create " + nameof(TouchScreenPad) + " Device")]
        public static void AddDevice() => DeviceConnector.AddSingleDevice();



        [MenuItem("TouchScreenPad/Remove " + nameof(TouchScreenPad) + " Device")]
        public static void RemoveDevice() => DeviceConnector.RemoveDevice();


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void AddDeviceLayout() => DeviceConnector.AddDeviceLayout();
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
