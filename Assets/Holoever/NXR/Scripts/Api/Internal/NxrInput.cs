﻿using HoloeverTask;
using UnityEngine;
using System;

namespace Nxr.Internal
{
    /// <summary>
    /// 
    /// </summary>
    public class NxrInput
    {
        const int MAX_INDEX = 256;
        private static int[] KeyStateHMD = new int[MAX_INDEX];
        private static int[] KeyStateControllerLeft = new int[MAX_INDEX];
        private static int[] KeyStateControllerRight = new int[MAX_INDEX];
        private static int[] KeyStateController3DOF = new int[MAX_INDEX];
        private static int[] KeyStateControllerNOLO_Left = new int[MAX_INDEX];
        private static int[] KeyStateControllerNOLO_Right = new int[MAX_INDEX];

        private static int[] KeyStateHMD_Pre = new int[MAX_INDEX];
        private static int[] KeyStateControllerLeft_Pre = new int[MAX_INDEX];
        private static int[] KeyStateControllerRight_Pre = new int[MAX_INDEX];
        private static int[] KeyStateController3DOF_Pre = new int[MAX_INDEX];
        private static int[] KeyStateControllerNOLO_Left_Pre = new int[MAX_INDEX];
        private static int[] KeyStateControllerNOLO_Right_Pre = new int[MAX_INDEX];

        public NxrInput()
        {
            Reset();
        }

        public void Reset()
        {
            for (int i = 0; i < MAX_INDEX; i++)
            {
                // 默认up
                KeyStateHMD[i] = CKeyEvent.ACTION_UP;
                KeyStateControllerRight[i] = CKeyEvent.ACTION_UP;
                KeyStateControllerLeft[i] = CKeyEvent.ACTION_UP;
                KeyStateController3DOF[i] = CKeyEvent.ACTION_UP;
                KeyStateControllerNOLO_Left[i] = CKeyEvent.ACTION_UP;
                KeyStateControllerNOLO_Right[i] = CKeyEvent.ACTION_UP;

                KeyStateHMD_Pre[i] = CKeyEvent.ACTION_UP;
                KeyStateControllerRight_Pre[i] = CKeyEvent.ACTION_UP;
                KeyStateControllerLeft_Pre[i] = CKeyEvent.ACTION_UP;
                KeyStateController3DOF_Pre[i] = CKeyEvent.ACTION_UP;
                KeyStateControllerNOLO_Left_Pre[i] = CKeyEvent.ACTION_UP;
                KeyStateControllerNOLO_Right_Pre[i] = CKeyEvent.ACTION_UP;
            }
        }

        public void OnChangeKeyEvent(int keyCode, int keyAction)
        {
            KeyStateHMD[CKeyEvent.KEYCODE_VOLUME_DOWN] = CKeyEvent.ACTION_DOWN;
        }

        // One Frame Update
        public void Process()
        {
            //when ime show unity is still running, disable input process
            if (TouchScreenKeyboard.visible)
            {
                NxrViewer.Instance.Triggered = false;
                return;
            }
            Array.Copy(KeyStateHMD, KeyStateHMD_Pre, MAX_INDEX);
            Array.Copy(KeyStateControllerLeft, KeyStateControllerLeft_Pre, MAX_INDEX);
            Array.Copy(KeyStateControllerRight, KeyStateControllerRight_Pre, MAX_INDEX);

            bool dpadCenterDown = Input.GetKey(KeyCode.JoystickButton0) || Input.GetKey((KeyCode) 10) ||
                                  Input.GetMouseButtonDown(0);
            bool backDown = Input.GetKey(KeyCode.Escape);
            bool dpadLeftDown = Input.GetKey(KeyCode.LeftArrow);
            bool dpadRightDown = Input.GetKey(KeyCode.RightArrow);
            bool dpadUpDown = Input.GetKey(KeyCode.UpArrow);
            bool dpadDownDown = Input.GetKey(KeyCode.DownArrow);
            // 功能按键nf1/nf2
            bool nf1Down = Input.GetKey(KeyCode.Joystick6Button1);
            bool nf2Down = Input.GetKey(KeyCode.Joystick6Button2);
            bool dpadCenterUp = false;
            if (!InteractionManager.IsControllerConnected() && !ControllerAndroid.isQuatConn() &&
                !ControllerAndroid.IsNoloConn())
            {
                dpadCenterUp = Input.GetKeyUp(KeyCode.JoystickButton0) || Input.GetKeyUp((KeyCode) 10) ||
                               Input.GetMouseButtonUp(0);
            }

            // 模拟一体机按键： WASD 上左下右，空格返回，回车确定
            if (Application.isEditor)
            {
                dpadCenterDown = Input.GetKey(KeyCode.Return) || Input.GetMouseButton(0);
                dpadLeftDown = Input.GetKey(KeyCode.A);
                dpadRightDown = Input.GetKey(KeyCode.D);
                dpadUpDown = Input.GetKey(KeyCode.W);
                dpadDownDown = Input.GetKey(KeyCode.S);
                backDown = Input.GetKey(KeyCode.Space);
            }
            if (nf1Down)
            {
                KeyStateHMD[CKeyEvent.KEYCODE_NF_1] = CKeyEvent.ACTION_DOWN;
            }
            else
            {
                KeyStateHMD[CKeyEvent.KEYCODE_NF_1] = CKeyEvent.ACTION_UP;
            }

            if (nf2Down)
            {
                KeyStateHMD[CKeyEvent.KEYCODE_NF_2] = CKeyEvent.ACTION_DOWN;
            }
            else
            {
                KeyStateHMD[CKeyEvent.KEYCODE_NF_2] = CKeyEvent.ACTION_UP;
            }

            if (backDown)
            {
                KeyStateHMD[CKeyEvent.KEYCODE_BACK] = CKeyEvent.ACTION_DOWN;
            }
            else
            {
                KeyStateHMD[CKeyEvent.KEYCODE_BACK] = CKeyEvent.ACTION_UP;
            }

            if (dpadCenterUp)
            {
                NxrViewer.Instance.Triggered = true;
            }

            if (dpadCenterDown)
            {
                KeyStateHMD[CKeyEvent.KEYCODE_DPAD_CENTER] = CKeyEvent.ACTION_DOWN;
            }
            else
            {
                KeyStateHMD[CKeyEvent.KEYCODE_DPAD_CENTER] = CKeyEvent.ACTION_UP;
            }

            if (dpadLeftDown)
            {
                KeyStateHMD[CKeyEvent.KEYCODE_DPAD_LEFT] = CKeyEvent.ACTION_DOWN;
            }
            else
            {
                KeyStateHMD[CKeyEvent.KEYCODE_DPAD_LEFT] = CKeyEvent.ACTION_UP;
            }

            if (dpadRightDown)
            {
                KeyStateHMD[CKeyEvent.KEYCODE_DPAD_RIGHT] = CKeyEvent.ACTION_DOWN;
            }
            else
            {
                KeyStateHMD[CKeyEvent.KEYCODE_DPAD_RIGHT] = CKeyEvent.ACTION_UP;
            }

            if (dpadUpDown)
            {
                KeyStateHMD[CKeyEvent.KEYCODE_DPAD_UP] = CKeyEvent.ACTION_DOWN;
            }
            else
            {
                KeyStateHMD[CKeyEvent.KEYCODE_DPAD_UP] = CKeyEvent.ACTION_UP;
            }

            if (dpadDownDown)
            {
                KeyStateHMD[CKeyEvent.KEYCODE_DPAD_DOWN] = CKeyEvent.ACTION_DOWN;
            }
            else
            {
                KeyStateHMD[CKeyEvent.KEYCODE_DPAD_DOWN] = CKeyEvent.ACTION_UP;
            }

            // 3Dof/6Dof Controller
            if (InteractionManager.IsControllerConnected())
            {
                //3DOF
                Array.Copy(KeyStateController3DOF, KeyStateController3DOF_Pre, MAX_INDEX);
                int[] keyAction = InteractionManager.GetKeyAction();
                KeyStateController3DOF = keyAction;

                // NOLO
                if (InteractionManager.IsSixDofController)
                {
                    Array.Copy(KeyStateControllerNOLO_Left, KeyStateControllerNOLO_Left_Pre, MAX_INDEX);
                    Array.Copy(KeyStateControllerNOLO_Right, KeyStateControllerNOLO_Right_Pre, MAX_INDEX);
                    int[] keyActionLeft =
                        InteractionManager.GetKeyAction((int) InteractionManager.NACTION_HAND_TYPE.HAND_LEFT);
                    int[] keyActionRight =
                        InteractionManager.GetKeyAction((int) InteractionManager.NACTION_HAND_TYPE.HAND_RIGHT);
                    KeyStateControllerNOLO_Left = keyActionLeft;
                    KeyStateControllerNOLO_Right = keyActionRight;
                }
            }
            else if (ControllerAndroid.isQuatConn() || ControllerAndroid.IsNoloConn())
            {
                // 交互库Close
                Array.Copy(KeyStateController3DOF, KeyStateController3DOF_Pre, MAX_INDEX);
                int[] keyAction = HoloeverTaskApi.GetKeyAction();
                KeyStateController3DOF = keyAction;
                // type, action, x, y
                float[] touchInfo = ControllerAndroid.getTouch();
                if (touchInfo[1] == CKeyEvent.ACTION_MOVE)
                {
                    InteractionManager.TouchPadPosition = new Vector2(touchInfo[2], touchInfo[3]);
                    KeyStateController3DOF[CKeyEvent.KEYCODE_CONTROLLER_TOUCHPAD_TOUCH] = CKeyEvent.ACTION_DOWN;
                }
                else if (touchInfo[1] == CKeyEvent.ACTION_UP)
                {
                    KeyStateController3DOF[CKeyEvent.KEYCODE_CONTROLLER_TOUCHPAD_TOUCH] = CKeyEvent.ACTION_UP;
                }

                bool isNoloLeftConnected = ControllerAndroid.isDeviceConn((int) CDevice.NOLO_TYPE.LEFT);
                bool isNoloRightConnected = ControllerAndroid.isDeviceConn((int) CDevice.NOLO_TYPE.RIGHT);
                if (isNoloLeftConnected)
                {
                    Array.Copy(KeyStateControllerNOLO_Left, KeyStateControllerNOLO_Left_Pre, MAX_INDEX);
                    int[] keyActionLeft =
                        ControllerAndroid.getKeyState((int) InteractionManager.NACTION_HAND_TYPE.HAND_LEFT, 0);
                    KeyStateControllerNOLO_Left = keyActionLeft;

                    float[] touchInfoLeft = ControllerAndroid.getTouchEvent((int) CDevice.NOLO_TYPE.LEFT);
                    if (touchInfoLeft[1] == CKeyEvent.ACTION_MOVE)
                    {
                        InteractionManager.TouchPadPosition = new Vector2(touchInfoLeft[2], touchInfoLeft[3]);
                        KeyStateControllerNOLO_Left[CKeyEvent.KEYCODE_CONTROLLER_TOUCHPAD_TOUCH] =
                            CKeyEvent.ACTION_DOWN;
                    }
                    else if (touchInfoLeft[1] == CKeyEvent.ACTION_UP)
                    {
                        KeyStateControllerNOLO_Left[CKeyEvent.KEYCODE_CONTROLLER_TOUCHPAD_TOUCH] = CKeyEvent.ACTION_UP;
                    }
                }

                if (isNoloRightConnected)
                {
                    Array.Copy(KeyStateControllerNOLO_Right, KeyStateControllerNOLO_Right_Pre, MAX_INDEX);
                    int[] keyActionRight =
                        ControllerAndroid.getKeyState((int) InteractionManager.NACTION_HAND_TYPE.HAND_RIGHT, 0);
                    KeyStateControllerNOLO_Right = keyActionRight;

                    float[] touchInfoRight = ControllerAndroid.getTouchEvent((int) CDevice.NOLO_TYPE.RIGHT);
                    if (touchInfoRight[1] == CKeyEvent.ACTION_MOVE)
                    {
                        InteractionManager.TouchPadPosition = new Vector2(touchInfoRight[2], touchInfoRight[3]);
                        KeyStateControllerNOLO_Right[CKeyEvent.KEYCODE_CONTROLLER_TOUCHPAD_TOUCH] =
                            CKeyEvent.ACTION_DOWN;
                    }
                    else if (touchInfoRight[1] == CKeyEvent.ACTION_UP)
                    {
                        KeyStateControllerNOLO_Right[CKeyEvent.KEYCODE_CONTROLLER_TOUCHPAD_TOUCH] = CKeyEvent.ACTION_UP;
                    }
                }
            }

            // 3Dof/6Dof Controller

            if (Application.isEditor && NxrViewer.Instance.RemoteDebug && NxrViewer.Instance.RemoteController)
            {
                Array.Copy(KeyStateController3DOF, KeyStateController3DOF_Pre, MAX_INDEX);
                HoloeverEmulatorManager holoeverEmulatorManager = HoloeverEmulatorManager.Instance;
                Array.Copy(holoeverEmulatorManager.KeyStateController3DOF, KeyStateController3DOF, MAX_INDEX);
            }

            // 内部事件处理返回键逻辑
            if (GetKeyUp(CKeyEvent.KEYCODE_BACK) ||
                GetControllerKeyUp(CKeyEvent.KEYCODE_CONTROLLER_MENU) ||
                GetControllerKeyUp(CKeyEvent.KEYCODE_CONTROLLER_MENU, InteractionManager.NACTION_HAND_TYPE.HAND_LEFT) ||
                GetControllerKeyUp(CKeyEvent.KEYCODE_CONTROLLER_MENU, InteractionManager.NACTION_HAND_TYPE.HAND_RIGHT))
            {
                bool EatBackKeyEvent = false;
                if (HoloeverRemindBox.Instance && HoloeverRemindBox.Instance.remindbox != null)
                {
                    HoloeverRemindBox.Instance.ReleaseDestory();
                    EatBackKeyEvent = true;
                }

                if (HoloeverShutDownBox.Instance && HoloeverShutDownBox.Instance.isShow)
                {
                    HoloeverShutDownBox.Instance.ReleaseDestory();
                    EatBackKeyEvent = true;
                }

                if (HoloeverKeyBoard.Instance.isShown())
                {
                    HoloeverKeyBoard.Instance.Dismiss();
                    EatBackKeyEvent = true;
                    Debug.Log("HoloeverKeyBoard->Dismiss");
                }

                if (EatBackKeyEvent)
                {
                    Debug.Log("EatBackKeyEvent");
                    KeyStateHMD[CKeyEvent.KEYCODE_BACK] = CKeyEvent.ACTION_UP;
                    KeyStateController3DOF[CKeyEvent.KEYCODE_CONTROLLER_MENU] = CKeyEvent.ACTION_UP;
                    KeyStateControllerNOLO_Left[CKeyEvent.KEYCODE_CONTROLLER_MENU] = CKeyEvent.ACTION_UP;
                    KeyStateControllerNOLO_Right[CKeyEvent.KEYCODE_CONTROLLER_MENU] = CKeyEvent.ACTION_UP;

                    KeyStateHMD_Pre[CKeyEvent.KEYCODE_BACK] = CKeyEvent.ACTION_UP;
                    KeyStateController3DOF_Pre[CKeyEvent.KEYCODE_CONTROLLER_MENU] = CKeyEvent.ACTION_UP;
                    KeyStateControllerNOLO_Left_Pre[CKeyEvent.KEYCODE_CONTROLLER_MENU] = CKeyEvent.ACTION_UP;
                    KeyStateControllerNOLO_Right_Pre[CKeyEvent.KEYCODE_CONTROLLER_MENU] = CKeyEvent.ACTION_UP;
                }
            }

            bool IsTouchpadUp = GetControllerKeyUp(CKeyEvent.KEYCODE_CONTROLLER_TOUCHPAD) ||
                                GetControllerKeyUp(CKeyEvent.KEYCODE_CONTROLLER_TOUCHPAD,
                                    InteractionManager.NACTION_HAND_TYPE.HAND_LEFT) ||
                                GetControllerKeyUp(CKeyEvent.KEYCODE_CONTROLLER_TOUCHPAD,
                                    InteractionManager.NACTION_HAND_TYPE.HAND_RIGHT);

            if (IsTouchpadUp)
            {
                NxrViewer.Instance.Triggered = true;
            }
        }

        /// <summary>
        /// Get the connect status of nolo 6Dof controller
        /// </summary>
        /// <returns>
        /// true connected
        /// false disconnected
        /// </returns>
        public static bool IsNoloControllerConnected()
        {
            return InteractionManager.IsSixDofController;
        }

        /// <summary>
        /// Get the connect status of 3Dof controller
        /// </summary>
        /// <returns>
        /// true connected
        /// false disconnected
        /// </returns>
        public static bool Is3DofControllerConnected()
        {
            return InteractionManager.Is3DofControllerConnected();
        }

        /// <summary>
        /// Check hmd key status, whether the button is pressed
        /// </summary>
        /// <param name="key">refer to CKeyEvent</param>
        /// <returns></returns>
        public static bool GetKeyPressed(int key)
        {
            if (KeyStateHMD[key] == CKeyEvent.ACTION_DOWN)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check hmd key status, whether the button is down
        /// </summary>
        /// <param name="key">refer to CKeyEvent</param>
        /// <returns></returns>
        public static bool GetKeyDown(int key)
        {
            if (KeyStateHMD[key] == CKeyEvent.ACTION_DOWN && KeyStateHMD_Pre[key] == CKeyEvent.ACTION_UP)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check hmd key status, whether the button is up
        /// </summary>
        /// <param name="key">refer to CKeyEvent</param>
        /// <returns></returns>
        public static bool GetKeyUp(int key)
        {
            if (KeyStateHMD_Pre[key] == CKeyEvent.ACTION_DOWN && KeyStateHMD[key] == CKeyEvent.ACTION_UP)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check 3dof controller key status, whether the button is down
        /// </summary>
        /// <param name="key">refer to CKeyEvent</param>
        /// <returns></returns>
        public static bool GetControllerKeyDown(int key)
        {
            if (KeyStateController3DOF[key] == CKeyEvent.ACTION_DOWN &&
                KeyStateController3DOF_Pre[key] == CKeyEvent.ACTION_UP)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check 3dof controller key status, whether the button is pressed
        /// </summary>
        /// <param name="key">refer to CKeyEvent</param>
        /// <returns></returns>
        public static bool GetControllerKeyPressed(int key)
        {
            if (KeyStateController3DOF[key] == CKeyEvent.ACTION_DOWN)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check 3dof controller status, whether the button is up
        /// </summary>
        /// <param name="key">refer to CKeyEvent</param>
        /// <returns></returns>
        public static bool GetControllerKeyUp(int key)
        {
            if (KeyStateController3DOF_Pre[key] == CKeyEvent.ACTION_DOWN &&
                KeyStateController3DOF[key] == CKeyEvent.ACTION_UP)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///  Check 6dof controller key status, whether the button is down
        /// </summary>
        /// <param name="key">refer to CKeyEvent</param>
        /// <param name="handType">left or right</param>
        /// <returns></returns>
        public static bool GetControllerKeyDown(int key, InteractionManager.NACTION_HAND_TYPE handType)
        {
            bool isDown = false;
            if (handType == InteractionManager.NACTION_HAND_TYPE.HAND_LEFT)
            {
                isDown = KeyStateControllerNOLO_Left[key] == CKeyEvent.ACTION_DOWN &&
                         KeyStateControllerNOLO_Left_Pre[key] == CKeyEvent.ACTION_UP;
            }
            else if (handType == InteractionManager.NACTION_HAND_TYPE.HAND_RIGHT)
            {
                isDown = KeyStateControllerNOLO_Right[key] == CKeyEvent.ACTION_DOWN &&
                         KeyStateControllerNOLO_Right_Pre[key] == CKeyEvent.ACTION_UP;
            }

            if (isDown)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///  Check 6dof controller key status, whether the button is pressed
        /// </summary>
        /// <param name="key">refer to CKeyEvent</param>
        /// <param name="handType">left or right</param>
        /// <returns></returns>
        public static bool GetControllerKeyPressed(int key, InteractionManager.NACTION_HAND_TYPE handType)
        {
            bool isPressed = false;
            if (handType == InteractionManager.NACTION_HAND_TYPE.HAND_LEFT)
            {
                isPressed = KeyStateControllerNOLO_Left[key] == CKeyEvent.ACTION_DOWN;
            }
            else if (handType == InteractionManager.NACTION_HAND_TYPE.HAND_RIGHT)
            {
                isPressed = KeyStateControllerNOLO_Right[key] == CKeyEvent.ACTION_DOWN;
            }

            if (isPressed)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///  Check 6dof controller key status, whether the button is up
        /// </summary>
        /// <param name="key">refer to CKeyEvent</param>
        /// <param name="handType">left or right</param>
        /// <returns></returns>
        public static bool GetControllerKeyUp(int key, InteractionManager.NACTION_HAND_TYPE handType)
        {
            bool isUp = false;
            if (handType == InteractionManager.NACTION_HAND_TYPE.HAND_LEFT)
            {
                isUp = KeyStateControllerNOLO_Left_Pre[key] == CKeyEvent.ACTION_DOWN &&
                       KeyStateControllerNOLO_Left[key] == CKeyEvent.ACTION_UP;
            }
            else if (handType == InteractionManager.NACTION_HAND_TYPE.HAND_RIGHT)
            {
                isUp = KeyStateControllerNOLO_Right_Pre[key] == CKeyEvent.ACTION_DOWN &&
                       KeyStateControllerNOLO_Right[key] == CKeyEvent.ACTION_UP;
            }

            if (isUp)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///  Whether the back button is down. (hmd or controller)
        /// </summary>
        /// <returns></returns>
        public static bool OnBackDown()
        {
            return GetKeyDown(CKeyEvent.KEYCODE_BACK)
                   || GetControllerKeyDown(CKeyEvent.KEYCODE_CONTROLLER_MENU)
                   || GetControllerKeyDown(CKeyEvent.KEYCODE_CONTROLLER_MENU,
                       InteractionManager.NACTION_HAND_TYPE.HAND_LEFT)
                   || GetControllerKeyDown(CKeyEvent.KEYCODE_CONTROLLER_MENU,
                       InteractionManager.NACTION_HAND_TYPE.HAND_RIGHT);
        }

        /// <summary>
        ///  Whether the back button is up. (hmd or controller)
        /// </summary>
        /// <returns></returns>
        public static bool OnBackUp()
        {
            return GetKeyUp(CKeyEvent.KEYCODE_BACK)
                   || GetControllerKeyUp(CKeyEvent.KEYCODE_CONTROLLER_MENU)
                   || GetControllerKeyUp(CKeyEvent.KEYCODE_CONTROLLER_MENU,
                       InteractionManager.NACTION_HAND_TYPE.HAND_LEFT)
                   || GetControllerKeyUp(CKeyEvent.KEYCODE_CONTROLLER_MENU,
                       InteractionManager.NACTION_HAND_TYPE.HAND_RIGHT);
        }

        /// <summary>
        ///  Get Controller Touchpad Touch Position
        ///  Horizontal direction： (Left->Right) -1~1
        ///  Vertical direction： (Up->Down) -1~1
        /// </summary>
        public static Vector2 GetTouchPadPosition()
        {
            return InteractionManager.TouchPadPosition;
        }

        /// <summary>
        ///  Get Controller Touchpad Touch Position
        ///  Horizontal direction： (Left->Right) -1~1
        ///  Vertical direction： (Up->Down) -1~1
        /// </summary>
        /// <param name="handType">left or right</param>
        /// <returns></returns>
        public static Vector2 GetTouchPadPosition(InteractionManager.NACTION_HAND_TYPE handType)
        {
            return handType == InteractionManager.NACTION_HAND_TYPE.HAND_LEFT
                ? InteractionManager.TouchPadPositionLeft
                : InteractionManager.TouchPadPositionRight;
        }
    }
}