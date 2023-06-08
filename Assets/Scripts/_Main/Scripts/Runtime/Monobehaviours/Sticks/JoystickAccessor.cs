using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;

namespace InputManagement
{
    public static class JoystickAccessor
    {

        #region PRIVATE_VARS
        private static List<Joystick> onScreenSticks = new List<Joystick>();
        #endregion

        #region PUBLIC_PROPERTIES
        public static int TotalJoysticks
        {
            
            get
            { 
                if (onScreenSticks != null)
                    return onScreenSticks.Count;
                return 0;
            }
        }
        #endregion

        #region PRIVATE_METHODS
        private static void CheckForIdConflicts()
        {
            Dictionary<int, string> ids = new Dictionary<int, string>();

            foreach (var item in onScreenSticks)
            {
                if (ids.ContainsKey(item.StickIndex))
                {
                    throw new System.Exception($"Joystick id {item.gameObject}:{item.StickIndex} conflicts with id {ids[item.StickIndex]}:{item.StickIndex}");
                }

                ids.Add(item.StickIndex, item.gameObject.name);
            }
        }

        private static void SortDevices()
        {
            onScreenSticks.Sort(new JoystickComparer());

            string joystickDevices = string.Empty;

            foreach (var item in onScreenSticks)
                joystickDevices += (item.gameObject) + "\n";
        }
      
        #endregion

        #region PUBLIC_METHODS

        public static Joystick GetStick(int stickIndex)
        {
            if (stickIndex >= TotalJoysticks)
                throw new System.Exception($"Stick {stickIndex} is either not created or wrong index accessed");

            return onScreenSticks[stickIndex];
        }

        public static void AddJoystick(Joystick newStick)
        {
            if (onScreenSticks.Contains(newStick))
                throw new System.Exception($"Multiple add requests for {newStick.gameObject.name} ");

            CheckForIdConflicts();
            onScreenSticks.Add(newStick);
            SortDevices();
        }

        public static void RemoveJoystick(Joystick newStick)
        {
            if (!onScreenSticks.Contains(newStick))
                throw new System.Exception($"Multiple add requests for {newStick.gameObject.name}");

            CheckForIdConflicts();
            onScreenSticks.Remove(newStick);
            SortDevices();
        }

        #endregion

        #region INNERCLASS_DEFINITIONS
        public class JoystickComparer : Comparer<Joystick>
        {
            public override int Compare(Joystick x, Joystick y)
            {
                return x.StickIndex.CompareTo(y.StickIndex);
            }
        }
        #endregion

    }


}

