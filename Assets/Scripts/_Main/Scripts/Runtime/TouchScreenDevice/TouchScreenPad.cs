using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;


namespace InputManagement
{

    public struct TouchScreenPadState : IInputStateTypeInfo
    {
        public const byte DEFAULT_STATE = 127;

        public FourCC format => new FourCC('T', 'A', 'J', 'S');


        #region BUTTON_BINDING

        //Normal Buttons
        [InputControl(name = "Button 1", layout = "Button", bit = 0, displayName = "1st Button")]
        [InputControl(name = "Button 2", layout = "Button", bit = 1, displayName = "2nd Button")]
        [InputControl(name = "Button 3", layout = "Button", bit = 2, displayName = "3rd Button")]
        [InputControl(name = "Button 4", layout = "Button", bit = 3, displayName = "4th Button")]
        [InputControl(name = "Button 5", layout = "Button", bit = 4, displayName = "5th Button")]
        [InputControl(name = "Button 6", layout = "Button", bit = 5, displayName = "6th Button")]
        [InputControl(name = "Button 7", layout = "Button", bit = 6, displayName = "7th Button")]
        [InputControl(name = "Button 8", layout = "Button", bit = 7, displayName = "8th Button")]
        [InputControl(name = "Button 9", layout = "Button", bit = 8, displayName = "9th Button")]
        [InputControl(name = "Button 10", layout = "Button", bit = 9, displayName = "10th Button")]
        [InputControl(name = "Button 11", layout = "Button", bit = 10, displayName = "11th Button")]
        [InputControl(name = "Button 12", layout = "Button", bit = 11, displayName = "12th Button")]
        [InputControl(name = "Button 13", layout = "Button", bit = 12, displayName = "13th Button")]
        [InputControl(name = "Button 14", layout = "Button", bit = 13, displayName = "14th Button")]
        [InputControl(name = "Button 15", layout = "Button", bit = 14, displayName = "15th Button")]
        [InputControl(name = "Button 16", layout = "Button", bit = 15, displayName = "16th Button")]
        [InputControl(name = "Button 17", layout = "Button", bit = 16, displayName = "17th Button")]

        //Axised Buttons
        [InputControl(name = "Button 18", layout = "Button", bit = 17, displayName = "Axis Button 1")]
        [InputControl(name = "Button 19", layout = "Button", bit = 18, displayName = "Axis Button 2")]
        [InputControl(name = "Button 20", layout = "Button", bit = 19, displayName = "Axis Button 3")]
        [InputControl(name = "Button 21", layout = "Button", bit = 20, displayName = "Axis Button 4")]
        [InputControl(name = "Button 22", layout = "Button", bit = 21, displayName = "Axis Button 5")]
        [InputControl(name = "Button 23", layout = "Button", bit = 22, displayName = "Axis Button 6")]
        public uint buttons;
        #endregion



        #region AXIS_1
        [InputControl(name = "Axis 1", format = "VC2B", layout = "Stick", displayName = "First Stick")]
        [InputControl(name = "Axis 1/x", defaultState = DEFAULT_STATE, format = "BYTE", offset = 0,
                     parameters = "normalize,normalizeMin = 0,normalizeMax=1,normalizeZero=0.5")]
        public byte x;

        [InputControl(name = "Axis 1/y", defaultState = DEFAULT_STATE, format = "BYTE", offset = 1,
                    parameters = "normalize,normalizeMin = 0,normalizeMax=1,normalizeZero=0.5")]

        [InputControl(name = "Axis 1/up", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=0,clampMax=1")]
        [InputControl(name = "Axis 1/down", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=-1,clampMax=0,invert")]
        [InputControl(name = "Axis 1/left", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=-1,clampMax=0,invert")]
        [InputControl(name = "Axis 1/right", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=0,clampMax=1")]
        public byte y;

        #endregion



        #region AXIS_2

        [InputControl(name = "Axis 2", format = "VC2B", layout = "Stick", displayName = "Second Stick")]

        [InputControl(name = "Axis 2/x", defaultState = DEFAULT_STATE, format = "BYTE", offset = 0,
                     parameters = "normalize,normalizeMin = 0,normalizeMax=1,normalizeZero=0.5")]
        public byte x2;


        [InputControl(name = "Axis 2/y", defaultState = DEFAULT_STATE, format = "BYTE", offset = 1,
                    parameters = "normalize,normalizeMin = 0,normalizeMax=1,normalizeZero=0.5")]

        [InputControl(name = "Axis 2/up", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=0,clampMax=1")]
        [InputControl(name = "Axis 2/down", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=-1,clampMax=0,invert")]
        [InputControl(name = "Axis 2/left", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=-1,clampMax=0,invert")]
        [InputControl(name = "Axis 2/right", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=0,clampMax=1")]
        public byte y2;

        #endregion



        #region AXIS_3

        [InputControl(name = "Axis 3", format = "VC2B", layout = "Stick", displayName = "Third Stick")]

        [InputControl(name = "Axis 3/x", defaultState = DEFAULT_STATE, format = "BYTE", offset = 0,
                     parameters = "normalize,normalizeMin = 0,normalizeMax=1,normalizeZero=0.5")]
        public byte x3;



        [InputControl(name = "Axis 3/y", defaultState = DEFAULT_STATE, format = "BYTE", offset = 1,
                    parameters = "normalize,normalizeMin = 0,normalizeMax=1,normalizeZero=0.5")]

        [InputControl(name = "Axis 3/up", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=0,clampMax=1")]
        [InputControl(name = "Axis 3/down", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=-1,clampMax=0,invert")]
        [InputControl(name = "Axis 3/left", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=-1,clampMax=0,invert")]
        [InputControl(name = "Axis 3/right", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=0,clampMax=1")]
        public byte y3;
        #endregion



        #region AXIS_4

        [InputControl(name = "Axis 4", format = "VC2B", layout = "Stick", displayName = "Fourth Stick")]

        [InputControl(name = "Axis 4/x", defaultState = DEFAULT_STATE, format = "BYTE", offset = 0,
                     parameters = "normalize,normalizeMin = 0,normalizeMax=1,normalizeZero=0.5")]
        public byte x4;



        [InputControl(name = "Axis 4/y", defaultState = DEFAULT_STATE, format = "BYTE", offset = 1,
                    parameters = "normalize,normalizeMin = 0,normalizeMax=1,normalizeZero=0.5")]

        [InputControl(name = "Axis 4/up", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=0,clampMax=1")]
        [InputControl(name = "Axis 4/down", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=-1,clampMax=0,invert")]
        [InputControl(name = "Axis 4/left", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=-1,clampMax=0,invert")]
        [InputControl(name = "Axis 4/right", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=0,clampMax=1")]
        public byte y4;
        #endregion


        #region AXIS_5

        [InputControl(name = "Axis 5", format = "VC2B", layout = "Stick", displayName = "Fifth Stick")]

        [InputControl(name = "Axis 5/x", defaultState = DEFAULT_STATE, format = "BYTE", offset = 0,
                     parameters = "normalize,normalizeMin = 0,normalizeMax=1,normalizeZero=0.5")]
        public byte x5;



        [InputControl(name = "Axis 5/y", defaultState = DEFAULT_STATE, format = "BYTE", offset = 1,
                    parameters = "normalize,normalizeMin = 0,normalizeMax=1,normalizeZero=0.5")]

        [InputControl(name = "Axis 5/up", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=0,clampMax=1")]
        [InputControl(name = "Axis 5/down", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=-1,clampMax=0,invert")]
        [InputControl(name = "Axis 5/left", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=-1,clampMax=0,invert")]
        [InputControl(name = "Axis 5/right", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=0,clampMax=1")]
        public byte y5;
        #endregion


        #region AXIS_6

        [InputControl(name = "Axis 6", format = "VC2B", layout = "Stick", displayName = "Fifth Stick")]

        [InputControl(name = "Axis 6/x", defaultState = DEFAULT_STATE, format = "BYTE", offset = 0,
                     parameters = "normalize,normalizeMin = 0,normalizeMax=1,normalizeZero=0.5")]
        public byte x6;



        [InputControl(name = "Axis 6/y", defaultState = DEFAULT_STATE, format = "BYTE", offset = 1,
                    parameters = "normalize,normalizeMin = 0,normalizeMax=1,normalizeZero=0.5")]

        [InputControl(name = "Axis 6/up", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=0,clampMax=1")]
        [InputControl(name = "Axis 6/down", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=-1,clampMax=0,invert")]
        [InputControl(name = "Axis 6/left", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=-1,clampMax=0,invert")]
        [InputControl(name = "Axis 6/right", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=0,clampMax=1")]
        public byte y6;
        #endregion


    }



    [InputControlLayout(stateType = typeof(TouchScreenPadState))]
    public class TouchScreenPad : InputDevice, IInputUpdateCallbackReceiver
    {

        #region PRIVATE_VARS
        private const int FRAME_DIFFERENCE = 1;
        private static int REQUIRED_ONSCREEN_JOYSTICKS = 2;
        #endregion

        #region PUBLIC_PROPERTIES
        public static TouchScreenPad current { get; private set; }


        public StickControl firstStick { get; private set; }
        public ButtonControl firstStickButton { get; private set; }


        public StickControl secondStick { get; private set; }
        public ButtonControl secondStickButton { get; private set; }


        public StickControl thirdStick { get; private set; }
        public ButtonControl thirdStickButton { get; private set; }


        public StickControl fourthStick { get; private set; }
        public ButtonControl fourthStickButton { get; private set; }


        public StickControl fifthStick { get; private set; }
        public ButtonControl fifthStickButton { get; private set; }


        public StickControl sixthStick { get; private set; }
        public ButtonControl sixthStickButton { get; private set; }
        #endregion



        #region INPUT_SYSTEM_CALLBACK
        protected override void FinishSetup()
        {
            base.FinishSetup();
            Joystick.tapDelay = InputSystem.settings.defaultTapTime;

            firstStick = GetChildControl<StickControl>("Axis 1");
            firstStickButton = GetChildControl<ButtonControl>("Button 18");

            secondStick = GetChildControl<StickControl>("Axis 2");
            secondStickButton = GetChildControl<ButtonControl>("Button 19");

            thirdStick = GetChildControl<StickControl>("Axis 3");
            thirdStickButton = GetChildControl<ButtonControl>("Button 20");

            fourthStick = GetChildControl<StickControl>("Axis 4");
            fourthStickButton = GetChildControl<ButtonControl>("Button 21");

            fifthStick = GetChildControl<StickControl>("Axis 5");
            fifthStickButton = GetChildControl<ButtonControl>("Button 22");

            sixthStick = GetChildControl<StickControl>("Axis 6");
            sixthStickButton = GetChildControl<ButtonControl>("Button 23");

        }

        public override void MakeCurrent()
        {
            base.MakeCurrent();
            current = this;
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();
            if (current == this)
                current = null;
        }

        private bool RequiredOnScreenJoysticksPresent() 
        {
            return JoystickAccessor.TotalJoysticks == REQUIRED_ONSCREEN_JOYSTICKS;
        }

        public void OnUpdate()
        {
            if (!RequiredOnScreenJoysticksPresent())
            {
                return;
            }

            var state = new TouchScreenPadState();
            state.x = state.x2 = state.x3 = state.x4 = state.x5 = state.x6 = TouchScreenPadState.DEFAULT_STATE;
            state.y = state.y2 = state.y3 = state.y4 = state.y5 = state.y6 = TouchScreenPadState.DEFAULT_STATE;

            int frameIdx = Time.frameCount;
            int offset = TouchScreenPadState.DEFAULT_STATE;

            #region STICK_1_MAP
            Joystick stick1 = GetOnScreenStick(0);

            state.x = (byte)(offset + (offset * stick1.Horizontal));
            state.y = (byte)(offset + (offset * stick1.Vertical));


            if (stick1.WasReleasedWithinFrameDifference(frameIdx, FRAME_DIFFERENCE) && stick1.HoldTime < Joystick.tapDelay)
            {
                state.buttons |= 1 << 17;
            }
            #endregion

            #region STICK_2_MAP
            Joystick stick2 = GetOnScreenStick(1);
            state.x2 = (byte)(offset + (offset * stick2.Horizontal));
            state.y2 = (byte)(offset + (offset * stick2.Vertical));

            //Debug.Log($"FrameIdx{frameIdx} - Was Released:{stick2.WasReleasedWithinFrameDifference(frameIdx,FRAME_DIFFERENCE)}   Was Released FB:{stick2.WasReleasedWithinFrameDifference(frameIdx, FRAME_DIFFERENCE)}  HoldValid:{stick2.HoldTime < Joystick.tapDelay}   HoldTime{stick2.HoldTime}:{Joystick.tapDelay}");

            if (stick2.WasReleasedWithinFrameDifference(frameIdx, FRAME_DIFFERENCE) && stick2.HoldTime < Joystick.tapDelay)
            {
                state.buttons |= 1 << 18;
            }
            #endregion

            //#region STICK_3_MAP
            //Joystick stick3 = GetOnScreenStick(2);

            //state.x3 = (byte)(offset + (offset * stick3.Horizontal));
            //state.y3 = (byte)(offset + (offset * stick3.Vertical));

            //if (stick3.WasReleasedWithinFrameDifference(frameIdx,FRAME_DIFFERENCE) && stick3.HoldTime < Joystick.tapDelay)
            //{
            //    state.buttons |= 1 << 19;
            //}

            //#endregion

            //#region STICK_4_MAP
            //Joystick stick4 = GetOnScreenStick(3);

            //state.x4 = (byte)(offset + (offset * stick4.Horizontal));
            //state.y4 = (byte)(offset + (offset * stick4.Vertical));

            //if (stick4.WasReleasedWithinFrameDifference(frameIdx,FRAME_DIFFERENCE) && stick4.HoldTime < Joystick.tapDelay)
            //{
            //    state.buttons |= 1 << 20;
            //}

            //#endregion

            //#region STICK_5_MAP
            //Joystick stick5 = GetOnScreenStick(4);

            //state.x5 = (byte)(offset + (offset * stick5.Horizontal));
            //state.y5 = (byte)(offset + (offset * stick5.Vertical));

            //if (stick5.WasReleasedWithinFrameDifference(frameIdx,FRAME_DIFFERENCE) && stick5.HoldTime < Joystick.tapDelay)
            //{
            //    state.buttons |= 1 << 21;
            //}
            //#endregion

            //#region STICK_6_MAP
            //Joystick stick6 = GetOnScreenStick(5);

            //state.x6 = (byte)(offset + (offset * stick6.Horizontal));
            //state.y6 = (byte)(offset + (offset * stick6.Vertical));

            //if (stick6.WasReleasedWithinFrameDifference(frameIdx,FRAME_DIFFERENCE) && stick6.HoldTime < Joystick.tapDelay)
            //{
            //    state.buttons |= 1 << 22;
            //}

            //#endregion

            // Finally, queue the event.
            // NOTE: We are replacing the current device state wholesale here. An alternative
            //       would be to use QueueDeltaStateEvent to replace only select memory contents.


            InputSystem.QueueStateEvent(this, state);

        }

        private static Joystick GetOnScreenStick(int idx)
        {
            return JoystickAccessor.GetStick(idx);
        }


        #endregion


    }
}