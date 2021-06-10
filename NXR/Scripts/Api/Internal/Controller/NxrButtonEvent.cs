using HoloeverTask;
using UnityEngine;
using UnityEngine.Events;

namespace Nxr.Internal
{
    public class NxrButtonEvent : MonoBehaviour
    {
        [System.Serializable]
        public class HoloeverControllerEvent : UnityEvent<NxrInstantNativeApi.HoloeverDeviceType>
        {
           
        }

        [System.Serializable]
        public class HoloeverControllerTouchEvent : UnityEvent<NxrInstantNativeApi.HoloeverDeviceType, float, float>
        {

        }

        public HoloeverControllerEvent onMenuDown;
        public HoloeverControllerEvent onMenuUp;
        public HoloeverControllerEvent onSystemDown;
        public HoloeverControllerEvent onSystemUp;
        public HoloeverControllerEvent onTriggerDown;
        public HoloeverControllerEvent onTriggerUp;
        public HoloeverControllerEvent onGripDown;
        public HoloeverControllerEvent onGripUp;
        public HoloeverControllerEvent onTouchpadDown;
        public HoloeverControllerEvent onTouchpadUp;
        public HoloeverControllerEvent onTouchpadTouch;
        public HoloeverControllerEvent onTouchpadRelease;

        public HoloeverControllerTouchEvent onTouchPosition;

        public UnityEvent onDpadLeftDown;
        public UnityEvent onDpadLeftUp;

        public UnityEvent onDpadRightDown;
        public UnityEvent onDpadRightUp;

        public UnityEvent onDpadUpLifted;
        public UnityEvent onDpadUpDown;

        public UnityEvent onDpadDownUp;
        public UnityEvent onDpadDownPressed;

        public UnityEvent onDpadCenterDown;
        public UnityEvent onDpadCenterUp;

        NxrInstantNativeApi.Holoever_ControllerStates _prevStates_hmd;
        NxrInstantNativeApi.Holoever_ControllerStates _currentStates_hmd;

        NxrTrackedDevice[] nvrTrackedDevices;
        private void Start()
        {
            nvrTrackedDevices = FindObjectsOfType<NxrTrackedDevice>();
        }

        public void RefreshTrackedDevices()
        {
            nvrTrackedDevices = FindObjectsOfType<NxrTrackedDevice>();
        }

        private int getNoloType(NxrInstantNativeApi.HoloeverDeviceType deviceType)
        {
            int noloType = (int)CDevice.NOLO_TYPE.NONE;
            if (deviceType == NxrInstantNativeApi.HoloeverDeviceType.LeftController)
            {
                noloType = (int)CDevice.NOLO_TYPE.LEFT;
            }
            else if (deviceType == NxrInstantNativeApi.HoloeverDeviceType.RightController)
            {
                noloType = (int)CDevice.NOLO_TYPE.RIGHT;
            }
            else if (deviceType == NxrInstantNativeApi.HoloeverDeviceType.Hmd)
            {
                noloType = (int)CDevice.NOLO_TYPE.HEAD;
            }
            return noloType;
        }

        //-------------------------------------------------
        void Update()
        {
            _prevStates_hmd = _currentStates_hmd;
#if UNITY_STANDALONE_WIN || ANDROID_REMOTE_NRR
            _currentStates_hmd = NxrInstantNativeApi.GetControllerStates(NxrInstantNativeApi.HoloeverDeviceType.Hmd);
#endif

            if (GetHMDButtonDown(NxrTrackedDevice.ButtonID.DPadCenter))
            {
                NxrViewer.Instance.Triggered = true;
                onDpadCenterDown.Invoke();
            }

            if (GetHMDButtonUp(NxrTrackedDevice.ButtonID.DPadCenter))
            {
                onDpadCenterUp.Invoke();
            }

            if (GetHMDButtonDown(NxrTrackedDevice.ButtonID.DPadUp))
            {
                onDpadUpDown.Invoke();
            }

            if (GetHMDButtonUp(NxrTrackedDevice.ButtonID.DPadUp))
            {
                onDpadUpLifted.Invoke();
            }

            if (GetHMDButtonDown(NxrTrackedDevice.ButtonID.DPadDown))
            {
                onDpadDownPressed.Invoke();
            }

            if (GetHMDButtonUp(NxrTrackedDevice.ButtonID.DPadDown))
            {
                onDpadDownUp.Invoke();
            }

            if (GetHMDButtonDown(NxrTrackedDevice.ButtonID.DPadLeft))
            {
                onDpadLeftDown.Invoke();
            }

            if (GetHMDButtonUp(NxrTrackedDevice.ButtonID.DPadLeft))
            {
                onDpadLeftUp.Invoke();
            }
             
            if (GetHMDButtonDown(NxrTrackedDevice.ButtonID.DPadRight))
            {
                onDpadRightDown.Invoke();
            }

            if (GetHMDButtonUp(NxrTrackedDevice.ButtonID.DPadRight))
            {
                onDpadRightUp.Invoke();
            }

            if(nvrTrackedDevices != null)
            {
                foreach(NxrTrackedDevice device in nvrTrackedDevices)
                {
                    if(device.GetButtonDown(NxrTrackedDevice.ButtonID.TouchPad))
                    {
                        if (!device.isGamePad)
                        {
                            NxrViewer.Instance.Triggered = true;
                        }
                        onTouchpadDown.Invoke(device.deviceType);
                    }

                    if (device.GetButtonUp(NxrTrackedDevice.ButtonID.TouchPad))
                    {
                        onTouchpadUp.Invoke(device.deviceType);
                    }

                    if (device.GetButtonDown(NxrTrackedDevice.ButtonID.System))
                    {
                        onSystemDown.Invoke(device.deviceType);
                    }

                    if (device.GetButtonUp(NxrTrackedDevice.ButtonID.System))
                    {
                        onSystemUp.Invoke(device.deviceType);
                    }

                    if (device.GetButtonDown(NxrTrackedDevice.ButtonID.Menu))
                    {
                        onMenuDown.Invoke(device.deviceType);
                    }

                    if (device.GetButtonUp(NxrTrackedDevice.ButtonID.Menu))
                    {
                        onMenuUp.Invoke(device.deviceType);
                    }

                    if (device.GetButtonDown(NxrTrackedDevice.ButtonID.Grip))
                    {
                        onGripDown.Invoke(device.deviceType);
                    }

                    if (device.GetButtonUp(NxrTrackedDevice.ButtonID.Grip))
                    {
                        onGripUp.Invoke(device.deviceType);
                    }

                    if (device.GetButtonDown(NxrTrackedDevice.ButtonID.Trigger))
                    {
                        onTriggerDown.Invoke(device.deviceType);
                    }

                    if (device.GetButtonUp(NxrTrackedDevice.ButtonID.Trigger))
                    {
                        onTriggerUp.Invoke(device.deviceType);
                    }

                    if (device.GetTouchDown(NxrTrackedDevice.ButtonID.TrackpadTouch))
                    {
                        onTouchpadTouch.Invoke(device.deviceType);
                    }

                    if (device.GetTouchUp(NxrTrackedDevice.ButtonID.TrackpadTouch))
                    {
                        onTouchpadRelease.Invoke(device.deviceType);
                    }

                    Vector2 position = device.GetTouchPosition();
                    if (position.x != 0 || position.y !=0)
                    {
                        onTouchPosition.Invoke(device.deviceType, position.x, position.y);
                    }
                }
            }
        }

        public bool GetHMDButtonDown(NxrTrackedDevice.ButtonID btn)
        {
            return (_currentStates_hmd.hmdButtons & (1 << (int)btn)) != 0 && (_prevStates_hmd.hmdButtons & (1 << (int)btn)) == 0;
        }

        public bool GetHMDButtonUp(NxrTrackedDevice.ButtonID btn)
        {
            return (_currentStates_hmd.hmdButtons & (1 << (int)btn)) == 0 && (_prevStates_hmd.hmdButtons & (1 << (int)btn)) != 0;
        }

        public bool GetHMDButtonPressed(NxrTrackedDevice.ButtonID btn)
        {
            return (_currentStates_hmd.hmdButtons & (1 << (int)btn)) != 0;
        }
    }
}