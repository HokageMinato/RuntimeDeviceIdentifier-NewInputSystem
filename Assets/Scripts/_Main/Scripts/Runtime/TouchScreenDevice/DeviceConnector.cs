using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using System.Linq;


namespace InputManagement
{

#if UNITY_EDITOR
    [InitializeOnLoad()]
#endif

    public class DeviceConnector
    {
        static DeviceConnector()
        {
            CreateTouchScreenGamepad();        
        }

        public static void CreateTouchScreenGamepad() 
        {
            AddDeviceLayout();
            if (FindDevice() == null)
                AddSingleDevice();
        }


        public static void AddSingleDevice()
        {
            InputSystem.AddDevice(new InputDeviceDescription
            {
                interfaceName = nameof(TouchScreenPad),
                product = nameof(TouchScreenPad)
            });
        }


        public static void AddDeviceLayout()
        {
            InputSystem.RegisterLayout<TouchScreenPad>(
                    name: nameof(TouchScreenPad),
                    matches: new InputDeviceMatcher().WithInterface(nameof(TouchScreenPad)));
        }


        public static void RemoveDevice()
        {
            var customDevice = FindDevice();
            if (customDevice != null)
                InputSystem.RemoveDevice(customDevice);
        }


        static InputDevice FindDevice()
        {
            return InputSystem.devices.FirstOrDefault(x => x is TouchScreenPad);
        }


        public void TestPrint() => Debug.Log("osos");

    }
}