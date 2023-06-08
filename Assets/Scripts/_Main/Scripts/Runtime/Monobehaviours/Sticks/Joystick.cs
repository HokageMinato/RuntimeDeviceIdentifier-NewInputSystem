using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace InputManagement
{

    public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        #region PUBLIC_VARS
        public static float tapDelay = 1.8f;
        #endregion

        #region PUBLIC_PROPERTIES
        public float Horizontal { get { return (snapX) ? SnapFloat(input.x, AxisOptions.Horizontal) : input.x; } }
        public float Vertical { get { return (snapY) ? SnapFloat(input.y, AxisOptions.Vertical) : input.y; } }
        public Vector2 Direction { get { return new Vector2(Horizontal, Vertical); } }

        public float HandleRange
        {
            get { return handleRange; }
            set { handleRange = Mathf.Abs(value); }
        }

        public float DeadZone
        {
            get { return deadZone; }
            set { deadZone = Mathf.Abs(value); }
        }

        public AxisOptions AxisOptions { get { return AxisOptions; } set { axisOptions = value; } }
        public bool SnapX { get { return snapX; } set { snapX = value; } }
        public bool SnapY { get { return snapY; } set { snapY = value; } }
        public int StickIndex { get { return stickIdx; } }
        public float HoldTime { get { return holdTime; } }
        #endregion


        #region PRIVATE_SERIALIZED_VARS
        [SerializeField] private float handleRange = 1;
        [SerializeField] private float deadZone = 0;
        [SerializeField] private AxisOptions axisOptions = AxisOptions.Both;
        [SerializeField] private bool snapX = false;
        [SerializeField] private bool snapY = false;
        [SerializeField][Range(1, 3)] private int stickIdx;

        [SerializeField] protected RectTransform background = null;
        [SerializeField] private RectTransform handle = null;
        #endregion


        #region PRIVATE_VARS
        private RectTransform baseRect = null;
        private Canvas canvas;
        private Camera cam;
        private Vector2 input = Vector2.zero;


        private int pressedFrame;
        private int releasedFrame;
        private float holdTime;
        #endregion


        #region UNITY_CALLBACKS
        protected virtual void Start()
        {
            HandleRange = handleRange;
            DeadZone = deadZone;
            baseRect = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            if (canvas == null)
                Debug.LogError("The Joystick is not placed inside a canvas");

            Vector2 center = new Vector2(0.5f, 0.5f);
            background.pivot = center;
            handle.anchorMin = center;
            handle.anchorMax = center;
            handle.pivot = center;
            handle.anchoredPosition = Vector2.zero;

            JoystickAccessor.AddJoystick(this);
        }

        void OnDestroy()
        {
            JoystickAccessor.RemoveJoystick(this);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
            pressedFrame = Time.frameCount;
            holdTime = Time.unscaledTime;
            //Debug.Log($"Down {pressedFrame} {gameObject.name}");
        }

        public void OnDrag(PointerEventData eventData)
        {
            cam = null;
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
                cam = canvas.worldCamera;

            if (HoldTime < tapDelay)
                return;

            Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, background.position);
            Vector2 radius = background.sizeDelta / 2;
            input = (eventData.position - position) / (radius * canvas.scaleFactor);
            FormatInput();
            HandleInput(input.magnitude, input.normalized, radius, cam);
            handle.anchoredPosition = input * radius * handleRange;
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            input = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
            releasedFrame = Time.frameCount;
            holdTime = Time.unscaledTime - holdTime;
           // Debug.Log($"Up {releasedFrame} holdTime{holdTime} tapDelay{tapDelay} validClick { holdTime < tapDelay}  gameObject{gameObject.name}");
        }
        #endregion

        #region PUBLIC_METHODS
        public bool WasPressedThisFrame(int frameCount)
        {
            return frameCount == this.pressedFrame;
        }

        public bool WasReleasedThisFrame(int frameCount)
        {
            return frameCount == this.releasedFrame;
        }

        public bool WasReleasedWithinFrameDifference(int frameCount,int frameBuffer) 
        { 
            return (frameCount - releasedFrame) <= frameBuffer;
        }
        #endregion


        #region PROTECTED_METHODS
        protected virtual void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
        {
            if (magnitude > deadZone)
            {
                if (magnitude > 1)
                    input = normalised;
            }
            else
                input = Vector2.zero;
        }

        protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
        {
            Vector2 localPoint = Vector2.zero;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, cam, out localPoint))
            {
                Vector2 pivotOffset = baseRect.pivot * baseRect.sizeDelta;
                return localPoint - (background.anchorMax * baseRect.sizeDelta) + pivotOffset;
            }
            return Vector2.zero;
        }
        #endregion


        #region PRIVATE_METHODS
        private void FormatInput()
        {
            if (axisOptions == AxisOptions.Horizontal)
                input = new Vector2(input.x, 0f);
            else if (axisOptions == AxisOptions.Vertical)
                input = new Vector2(0f, input.y);
        }

        private float SnapFloat(float value, AxisOptions snapAxis)
        {
            if (value == 0)
                return value;

            if (axisOptions == AxisOptions.Both)
            {
                float angle = Vector2.Angle(input, Vector2.up);
                if (snapAxis == AxisOptions.Horizontal)
                {
                    if (angle < 22.5f || angle > 157.5f)
                        return 0;
                    else
                        return (value > 0) ? 1 : -1;
                }
                else if (snapAxis == AxisOptions.Vertical)
                {
                    if (angle > 67.5f && angle < 112.5f)
                        return 0;
                    else
                        return (value > 0) ? 1 : -1;
                }
                return value;
            }
            else
            {
                if (value > 0)
                    return 1;
                if (value < 0)
                    return -1;
            }
            return 0;
        }




        #endregion

    }

    public enum AxisOptions { Both, Horizontal, Vertical }
}