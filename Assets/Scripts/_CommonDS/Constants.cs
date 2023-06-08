using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonDS
{
    public class Constants : MonoBehaviour
    {
        #region INNERCLASS_DEFINITIONS
        public static class GameplayConstants 
        {
            #region PUBLIC_VARS
            public const float AngleDifferenceOnYAxis=55;
            #endregion
        }

        public static class InputConstants
        {
            #region PUBLIC_VARS
            public const string UI = "UI";
            public const string Player = "Player";

            public static class ControlNames
            {
                public const string AnalogueLock = "AnalogueLock";
                public const string AutoLock = "AutoLock";
            }
            #endregion
        }

        public static class CompareConstants 
        {
            public const int GREATER = 1;
            public const int EQUAL = 0;
            public const int LESSER = -1;
        }
        #endregion
    }
}
