using HoloeverTask;
using Nxr.Internal;
using UnityEngine;
using UnityEngine.UI;

namespace Holoever
{
    public class Holoever_Api
    {
        /// <summary>
        ///  Change the left and right camera's fov
        ///  fov range [40~90]
        /// </summary>
        /// <param name="fov"></param>
        public static void Holoever_UpdateCameraFov(float fov)
        {
            NxrViewer.Instance.UpdateCameraFov(fov);
        }

        /// <summary>
        ///  Reset the left and right camera's fov to default.
        /// </summary>
        public static void Holoever_ResetCameraFov()
        {
            NxrViewer.Instance.ResetCameraFov();
        }

        /// <summary>
        ///  Update the left and right camera's farClipPlane
        /// </summary>
        /// <param name="far"></param>
        public static void Holoever_UpdateCameraFar(float far)
        {
            NxrViewer.Instance.UpateCameraFar(far);
        }

        public static PERFORMANCE Holoever_GetPlatPerformanceLevel()
        {
            return (PERFORMANCE)NxrGlobal.platPerformanceLevel;
        }

        /// <summary>
        ///  Set UI.Text which the input content will display on.
        /// </summary>
        /// <param name="_text"></param>
        public static void Holoever_SetText(Text _text)
        {
            HoloeverKeyBoard.Instance.SetText(_text);
        }

        /// <summary>
        /// Get keyboard's transform
        /// </summary>
        /// <returns></returns>
        public static Transform Holoever_GetKeyBoardTransform()
        {
            return HoloeverKeyBoard.Instance.GetKeyBoardTransform();
        }

        /// <summary>
        /// Get current input content
        /// </summary>
        /// <returns></returns>
        public static string Holoever_GetKeyBoardString()
        {
            return HoloeverKeyBoard.Instance.GetKeyBoardString();
        }

        /// <summary>
        /// Show Keyboard 
        /// </summary>
        /// <param name="_pageIndex">PageIndex ： 0=alphabet，1=number</param>
        /// <param name="position">keyBoardTransform's position</param>
        /// <param name="rotation">keyBoardTransform's rotation</param>
        public static void Holoever_Show(int _pageIndex, Vector3 position, Vector3 rotation)
        {
            HoloeverKeyBoard.Instance.Show(_pageIndex, position, rotation);
        }

        /// <summary>
        /// Show Keyboard 
        /// </summary>
        /// <param name="_pageIndex">PageIndex ： 0=alphabet，1=number</param>
        public static void Holoever_Show()
        {
            HoloeverKeyBoard.Instance.Show();
        }

        public static NxrHead Holoever_GetHead()
        {
            return NxrViewer.Instance.GetHead();
        }

        public static Transform Holoever_GetHeadTransform()
        {
            return NxrViewer.Instance.GetHead().transform;
        }

        /// Determine whether the scene renders in stereo or mono.
        /// _True_ means to render in stereo, and _false_ means to render in mono.
        public static void Holoever_SetSplitScreenModeEnabled(bool isEnabled)
        {
            NxrViewer.Instance.SplitScreenModeEnabled = isEnabled;
        }

        public static bool Holoever_GetSplitScreenModeEnabled()
        {
            return NxrViewer.Instance.SplitScreenModeEnabled;
        }

        /// <summary>
        /// The callback when Camera is idle.
        /// </summary>
        public static void Holoever_OnCameraIdle(HoloeverService.CameraIdle CameraIdle)
        {
            HoloeverService.OnCameraIdle += CameraIdle;
        }

        public static void Holoever_RemoveCameraIdle(HoloeverService.CameraIdle CameraIdle)
        {
            HoloeverService.OnCameraIdle -= CameraIdle;
        }

        /// <summary>
        /// The callback when Camera is busy.
        /// </summary>
        public static void Holoever_OnCameraBusy(HoloeverService.CameraBusy CameraBusy)
        {
            HoloeverService.OnCameraBusy += CameraBusy;
        }
    
        public static void Holoever_RemoveCameraBusy(HoloeverService.CameraBusy CameraBusy)
        {
            HoloeverService.OnCameraBusy -= CameraBusy;
        }

        public static void Holoever_OnRecorderSuccessHandler(HoloeverService.OnRecorderSuccess onRecorderSuccess)
        {
            HoloeverService.OnRecorderSuccessHandler += onRecorderSuccess;
        }

        public static void Holoever_RemoveRecorderSuccessHandler(HoloeverService.OnRecorderSuccess onRecorderSuccess)
        {
            HoloeverService.OnRecorderSuccessHandler -= onRecorderSuccess;
        }
  
        public static void Holoever_OnRecorderFailedHandler(HoloeverService.OnRecorderFailed onRecorderFailed)
        {
            HoloeverService.OnRecorderFailedHandler += onRecorderFailed;
        }
    
        public static void Holoever_RemoveRecorderFailedHandler(HoloeverService.OnRecorderFailed onRecorderFailed)
        {
            HoloeverService.OnRecorderFailedHandler -= onRecorderFailed;
        }

        /// <summary>
        /// Get camera status async
        /// </summary>
        public static void Holoever_GetCameraStatus(int cameraId)
        {
            NxrViewer.Instance.GetHoloeverService().GetCameraStatus(cameraId);
        }

        /// <summary>
        /// Start camera preview
        /// </summary>
        public static void Holoever_StartCameraPreView(int cameraId)
        {
            NxrViewer.Instance.GetHoloeverService().StartCameraPreView(cameraId);
        }

        public static void Holoever_StartCameraPreView(bool triggerFocus, int cameraId)
        {
            NxrViewer.Instance.GetHoloeverService().StartCameraPreView(triggerFocus,cameraId);
        }

        /// <summary>
        /// Stop camera preview
        /// </summary>
        public static void Holoever_StopCamereaPreView()
        {
            NxrViewer.Instance.GetHoloeverService().StopCamereaPreView();
        }

        /// <summary>
        /// Set Head Control 
        /// </summary>
        public static void Holoever_SetHeadControl(HeadControl headControl)
        {
            NxrViewer.Instance.HeadControl = headControl;
        }
   
        public static HeadControl Holoever_GetHeadControl()
        {
            return NxrViewer.Instance.HeadControl;
        }
  
        public static void Holoever_SetGazeSize(string param)
        {
            NxrViewer.Instance.GazeApi(GazeTag.Set_Size, param);
        }

        public static void Holoever_SetGazeColor(string param)
        {
            NxrViewer.Instance.GazeApi(GazeTag.Set_Color, param);
        }
 
        public static void Holoever_SetGazeHide()
        {
            NxrViewer.Instance.GazeApi(GazeTag.Hide);
        }

        public static void Holoever_LockHeadTracker()
        {
            NxrViewer.Instance.LockHeadTracker = true;
        }

        public static void Holoever_UnLockHeadTracker()
        {
            NxrViewer.Instance.LockHeadTracker = false;
        }

        /// <summary>
        /// Resets the tracker so that the user's current direction becomes forward.
        /// </summary>
        public static void Holoever_Recenter()
        {
            NxrViewer.Instance.Recenter();
        }

        /// <summary>
        ///  Set system split mode : 1=split by system，0=split by app
        /// </summary>
        public static void Holoever_SetSystemSplitMode(int flag)
        {
            NxrViewer.Instance.SetSystemSplitMode(flag);
        }

        /// <summary>
        /// Get android SD Card's path（/storage/emulated/0）
        /// </summary>
        /// <returns>exp: /storage/emulated/0</returns>
        public static string Holoever_GetStoragePath()
        {
            return NxrViewer.Instance.GetStoragePath();
        }

        /// <summary>
        /// Register virtual mouse service
        /// </summary>
        /// <param name="serviceStatus"></param>
        public static void Holoever_RegisterVirtualMouseService(HoloeverService.OnVirtualMouseServiceStatus onVirtualMouseServiceStatus)
        {
           NxrViewer.Instance.GetHoloeverService().RegisterVirtualMouseService(onVirtualMouseServiceStatus);
        }

        /// <summary>
        /// UnRegister virtual mouse service
        /// </summary>
        public static void Holoever_UnRegisterVirtualMouseService()
        {
            NxrViewer.Instance.GetHoloeverService().UnRegisterVirtualMouseService();
        }

        /// <summary>
        /// Set enable virtual mouse
        /// </summary>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public static void Holoever_SetEnableVirtualMouse(bool enabled)
        {
            NxrViewer.Instance.GetHoloeverService().SetEnableVirtualMouse(enabled);
        }
  
        public static void Holoever_OpenVideoPlayer(string path, int loop = Video.VIDEO_KEY_LOOP_ON, int decode = Video.VIDEO_PARAMETERS_DECODE_HARDWARE, int mode = Video.VIDEO_PARAMETERS_MODE_NORMAL, int type = Video.VIDEO_PARAMETERS_TYPE_2D)
        {
            HoloeverTaskApi.OpenVideoPlayer(path,loop,decode,mode,type);
        }
  
        public static void Holoever_OpenSettingsMain()
        {
            HoloeverTaskApi.OpenSettingsMain();
        }
 
        public static void Holoever_OpenBrowerExplorer(string url, string actionBarState = Brower.EXPLORER_KEY_ACTIONBAR_SHOW)
        {
            HoloeverTaskApi.OpenBrowerExplorer(url, actionBarState);
        }
   
        public static void Holoever_OpenImageGallery(string path, int type = Gallery.SHOW_IMAGE_KEY_2D)
        {
            HoloeverTaskApi.OpenImageGallery(path, type);
        }

        /// <summary>
        /// Get device's network status
        /// </summary>
        /// <returns>0=disconneted,1= conneted=</returns>
        public static int Holoever_GetNetworkStatus()
        {
            return HoloeverTaskApi.GetNetworkStatus();
        }
 
        public static int Holoever_GetBluetoothStatus()
        {
            return HoloeverTaskApi.GetBluetoothStatus();
        }
  
        public static void Holoever_GetFilePath(string basePath)
        {
            HoloeverTaskApi.GetFilePath(basePath);
        }

        /// <summary>
        /// Change eye's ipd
        /// </summary>
        /// <param name="ipd">0.064</param>
        public static void Holoever_SetIpd(float ipd)
        {
            NxrViewer.Instance.SetIpd(ipd);
        }

        /// <summary>
        /// Get eye's ipd
        /// </summary>
        /// <returns></returns>
        public static float Holoever_GetIpd()
        {
            return NxrViewer.Instance.GetIpd();
        }

        public static void Holoever_ResetIpd()
        {
            NxrViewer.Instance.ResetIpd();
        }

        public static string Holoever_GetMacaddress()
        {
            return HoloeverTaskApi.GetMacAddress();
        }

        public static string Holoever_GetDeviceId()
        {
            return HoloeverTaskApi.GetDeviceId();
        }

        public static string Holoever_GetChannelCode()
        {
            return HoloeverTaskApi.GetChannelCode();
        }

        public static string Holoever_GetVRVersion()
        {
            return HoloeverTaskApi.GetVRVersion();
        }

        public static string Holoever_GetOSVersion()
        {
            return HoloeverTaskApi.GetOSVersion();
        }

        public static int Holoever_GetOSVersionCode()
        {
            return HoloeverTaskApi.GetOSVersionCode();
        }

        public static int Holoever_GetSysSleepTime()
        {
            return HoloeverTaskApi.GetSysSleepTime();
        }
 
        public static string Holoever_GetCurrentLanguage()
        {
            return HoloeverTaskApi.GetCurrentLanguage();
        }

        public static string Holoever_GetCurrentTimezone()
        {
            return HoloeverTaskApi.GetCurrentTimezone();
        }
  
        public static string Holoever_GetDeviceName()
        {
            return HoloeverTaskApi.GetDeviceName();
        }

        /// <summary>
        /// Launch app by package name
        /// </summary>
        /// <param name="pkgName"></param>
        /// <returns></returns>
        public static void Holoever_LaunchAppByPkgName(string pkgName)
        {
            HoloeverTaskApi.LaunchAppByPkgName(pkgName);
        }
  
        public static string Holoever_GetResultPathFromSelectionTask(AndroidJavaObject selectionTask)
        {
            return HoloeverTaskApi.GetResultPathFromSelectionTask(selectionTask);
        }
 
        public static void Holoever_AddOnPowerChangeListener(HoloeverTaskApi.onPowerChange listener)
        {
            HoloeverTaskApi.addOnPowerChangeListener(listener);
        }
   
        public static void Holoever_RemoveOnPowerChangeListener(HoloeverTaskApi.onPowerChange listener)
        {
            HoloeverTaskApi.removeOnPowerChangeListener(listener);
        }
 
        public static void Holoever_SetSelectionCallback(HoloeverTaskApi.onSelectionResult onSelectionResult)
        {
            HoloeverTaskApi.setSelectionCallback(onSelectionResult);
        }

        public static HoloeverService Holoever_GetHoloeverService()
        {
            return NxrViewer.Instance.GetHoloeverService();
        }

        /// <summary>
        /// Get system's vendor SW version
        /// </summary>
        /// <returns></returns>
        public static string Holoever_GetVendorSWVersion()
        {
            return NxrViewer.Instance.GetHoloeverService().GetVendorSWVersion();
        }
   
        public static string Holoever_GetModel()
        {
            return NxrViewer.Instance.GetHoloeverService().GetModel();
        }

        /// <summary>
        /// Get the value of light perception.
        /// </summary>
        /// <returns></returns>
        public static int Holoever_GetLightValue()
        {
            return NxrViewer.Instance.GetHoloeverService().GetLightValue();
        }

        /// <summary>
        /// Get the value of the distance sensor.
        /// </summary>
        /// <returns></returns>
        public static int Holoever_GetProximityValue()
        {
            return NxrViewer.Instance.GetHoloeverService().GetProximityValue();
        }

        /// <summary>
        /// Get system's brightness value
        /// </summary>
        /// <returns></returns>
        public static int Holoever_GetBrightnessValue()
        {
            return NxrViewer.Instance.GetHoloeverService().GetBrightnessValue();
        }

        /// <summary>
        /// Set system's brightness value
        /// </summary>
        /// <returns></returns>
        public static void Holoever_SetBrightnessValue(int value)
        {
            NxrViewer.Instance.GetHoloeverService().SetBrightnessValue(value);
        }

        public static int Holoever_GetVolumeValue()
        {
            return NxrViewer.Instance.GetHoloeverService().GetVolumeValue();
        }

        public static int Holoever_GetMaxVolume()
        {
            return NxrViewer.Instance.GetHoloeverService().GetMaxVolume();
        }

        public static int Holoever_GetHMDCameraId()
        {
            return NxrViewer.Instance.GetHoloeverService().HMDCameraId;
        }

        /// <summary>
        /// Set display mode 2d/3d
        /// </summary>
        /// <param name="displayMode"></param>
        public static void Holoever_SetDisplayMode(DISPLAY_MODE displayMode)
        {
            NxrViewer.Instance.GetHoloeverService().SetDisplayMode(displayMode);
        }

        /// <summary>
        /// Get display mode 2d/3d
        /// </summary>
        /// <returns></returns>
        public static DISPLAY_MODE Holoever_GetDisplayMode()
        {
            return NxrViewer.Instance.GetHoloeverService().GetDisplayMode();
        }

        /// <summary>
        /// Control whether touchpad is displayed. true-display false-not display
        /// </summary>
        /// <param name="isEnable"></param>
        public static void Holoever_SetEnableTouchCursor(bool isEnable)
        {
            NxrViewer.Instance.GetHoloeverService().SetEnableTouchCursor(isEnable);
        }

        public static bool Holoever_IsSupport6DOF()
        {
            return NxrViewer.Instance.GetHoloeverService().IsSupport6DOF();
        }

        /// <summary>
        /// The callback when sensor data changes. 
        /// </summary>
        public static void Holoever_OnSensorDataChangedHandler(HoloeverService.OnSensorDataChanged onSensorDataChanged)
        {
            HoloeverService.OnSensorDataChangedHandler += onSensorDataChanged;
        }

        public static void Holoever_RemoveSensorDataChangedHandler(HoloeverService.OnSensorDataChanged onSensorDataChanged)
        {
            HoloeverService.OnSensorDataChangedHandler -= onSensorDataChanged;
        }

        public static void Holoever_RegisterSensorListener(SENSOR_TYPE type, SENSOR_LOCATION location)
        {
            NxrViewer.Instance.GetHoloeverService().RegisterSensorListener(type,location);
        }

        public static void Holoever_UnRegisterSensorListener()
        {
            NxrViewer.Instance.GetHoloeverService().UnRegisterSensorListener();
        }

        /// <summary>
        /// Get the service connection status (after the service is connected, you can check the plug-in support status)
        /// </summary>
        public static void Holoever_ServiceReadyUpdatedDelegate(NxrViewer.ServiceReadyUpdatedDelegate OnServiceReadyUpdate)
        {
            NxrViewer.serviceReadyUpdatedDelegate += OnServiceReadyUpdate;
        }

        public static void Holoever_RemoveServiceReadyUpdatedDelegate(NxrViewer.ServiceReadyUpdatedDelegate OnServiceReadyUpdate)
        {
            NxrViewer.serviceReadyUpdatedDelegate -= OnServiceReadyUpdate;
        }

        /// <summary>
        /// Check whether the plugin such as Record and Marker is declared.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Holoever_IsPluginDeclared(PLUGIN_ID id)
        {
            return HoloeverTaskApi.IsPluginDeclared(id);
        }

        /// <summary>
        /// Check whether the plugin such as Record and Marker is supported.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Holoever_IsPluginSupported(PLUGIN_ID id)
        {
            return HoloeverTaskApi.IsPluginSupported(id);
        }

        public static void Holoever_SetTrackPosition(bool trackPosition)
        {
            NxrViewer.Instance.TrackerPosition = trackPosition; 
        }

        /// <summary>
        /// The callback of the change of hmd 6dof position data
        /// </summary>
        public static void Holoever_OnSixDofPosition(NxrViewer.OnSixDofPosition onSixDofPosition)
        {
            NxrViewer.onSixDofPosition += onSixDofPosition;
        }

        public static void Holoever_RemoveSixDofPosition(NxrViewer.OnSixDofPosition onSixDofPosition)
        {
            NxrViewer.onSixDofPosition -= onSixDofPosition;
        }
 
        public static void Holoever_SetCaptureVideoSize(VIDEO_SIZE video_Size)
        {
            HoloeverService.SetCaptureVideoSize(video_Size);
        }
 
        public static void Holoever_StartCapture(string filePath)
        {
            NxrViewer.Instance.GetHoloeverService().StartCapture(filePath);
        }
   
        public static void Holoever_StopCapture()
        {
            NxrViewer.Instance.GetHoloeverService().StopCapture();
        }

        public static void Holoever_StartVoiceRecording()
        {
            NxrViewer.Instance.GetHoloeverService().StartVoiceRecording();
        }

        public static void Holoever_StopVoiceRecording()
        {
            NxrViewer.Instance.GetHoloeverService().StopVoiceRecording();
        }

        public static void Holoever_StartMarkerRecognize()
        {
            NxrViewer.Instance.GetHoloeverService().StartMarkerRecognize();
        }

        public static void Holoever_StopMakerRecognize()
        {
            NxrViewer.Instance.GetHoloeverService().StopMarkerRecognize();
        }

        /// <summary>
        /// The callback when Marker is found.
        /// </summary>
        public static void Holoever_OnMarkerFoundHandler(HoloeverMarker.OnMarkerFound onMarkerFound)
        {
            HoloeverMarker.OnMarkerFoundHandler+= onMarkerFound;
        }

        public static void Holoever_RemoveMarkerFoundHandler(HoloeverMarker.OnMarkerFound onMarkerFound)
        {
            HoloeverMarker.OnMarkerFoundHandler -= onMarkerFound;
        }

        /// <summary>
        /// The callback when Marker is lost.
        /// </summary>
        public static void Holoever_OnMarkerLostHandler(HoloeverMarker.OnMarkerLost onMarkerLost)
        {
            HoloeverMarker.OnMarkerLostHandler += onMarkerLost;
        }

        public static void Holoever_RemoveMarkerLostHandler(HoloeverMarker.OnMarkerLost onMarkerLost)
        {
            HoloeverMarker.OnMarkerLostHandler -= onMarkerLost;
        }

        /// <summary>
        /// Request permission
        /// </summary>
        /// <param name="names">NxrGlobal.Permission</param>
        public static void Holoever_RequsetPermission(string[] permissions)
        {
            NxrViewer.Instance.GetHoloeverService().RequsetPermission(permissions);
        }

        #region KeyEvent
        public static class Holoever_KeyEvent
        {
            public static int ACTION_DOWN = 0;
            public static int ACTION_UP = 1;
            public static int ACTION_MOVE = 2;

            public static int KEYCODE_DPAD_UP = 19;
            public static int KEYCODE_DPAD_DOWN = 20;
            public static int KEYCODE_DPAD_LEFT = 21;
            public static int KEYCODE_DPAD_RIGHT = 22;
            public static int KEYCODE_DPAD_CENTER = 23;
            public static int KEYCODE_VOLUME_UP = 24;
            public static int KEYCODE_VOLUME_DOWN = 25;
            public static int KEYCODE_BUTTON_Y = 100;
            public static int KEYCODE_BUTTON_B = 97;
            public static int KEYCODE_BUTTON_A = 96;
            public static int KEYCODE_BUTTON_X = 99;
            public static int KEYCODE_BUTTON_L1 = 102;
            public static int KEYCODE_BUTTON_R1 = 103;
            public static int KEYCODE_BUTTON_L2 = 104;
            public static int KEYCODE_BUTTON_R2 = 105;
            public static int KEYCODE_BUTTON_THUMBL = 106;
            public static int KEYCODE_BUTTON_THUMBR = 107;
            public static int KEYCODE_BUTTON_START = 108;
            public static int KEYCODE_BUTTON_SELECT = 109;
            public static int KEYCODE_BUTTON_HOLOEVER = 110;
            public static int KEYCODE_BUTTON_HOME = 3;
            public static int KEYCODE_BUTTON_APP = 255;
            public static int KEYCODE_BACK = 255;

            public static int KEYCODE_NF_1 = 144;
            public static int KEYCODE_NF_2 = 145;

            public static int KEYCODE_CONTROLLER_TOUCHPAD_TOUCH = 254;
            public static int KEYCODE_CONTROLLER_TRIGGER = 103;
            public static int KEYCODE_CONTROLLER_MENU = 255;
            public static int KEYCODE_CONTROLLER_TOUCHPAD = 23;
            public static int KEYCODE_CONTROLLER_VOLUMN_DOWN = 25;
            public static int KEYCODE_CONTROLLER_VOLUMN_UP = 24;
            public static int KEYCODE_3DOF_CONTROLLER_TRIGGER = 103;
            public static int KEYCODE_CONTROLLER_GRIP = 109;

            public static int KEYCODE_CONTROLLER_TRIGGER_INTERNAL = 1;

            public static bool IsTriggerKeyCode(int keycode)
            {
                return keycode == KEYCODE_3DOF_CONTROLLER_TRIGGER || keycode == KEYCODE_CONTROLLER_TRIGGER;
            }

            public static int[] KeyCodeIds = new int[]
            {
                KEYCODE_DPAD_UP,
                KEYCODE_DPAD_DOWN,
                KEYCODE_DPAD_LEFT,
                KEYCODE_DPAD_RIGHT,
                KEYCODE_DPAD_CENTER,
                KEYCODE_VOLUME_UP,
                KEYCODE_VOLUME_DOWN,
                KEYCODE_BUTTON_Y,
                KEYCODE_BUTTON_B,
                KEYCODE_BUTTON_A,
                KEYCODE_BUTTON_X,
                KEYCODE_BUTTON_L1,
                KEYCODE_BUTTON_R1,
                KEYCODE_BUTTON_L2,
                KEYCODE_BUTTON_R2,
                KEYCODE_BUTTON_THUMBL,
                KEYCODE_BUTTON_THUMBR,
                KEYCODE_BUTTON_START,
                KEYCODE_BUTTON_SELECT,
                KEYCODE_BUTTON_HOLOEVER,
                KEYCODE_BUTTON_HOME,
                KEYCODE_BUTTON_APP,
                KEYCODE_CONTROLLER_TOUCHPAD_TOUCH,
                KEYCODE_NF_1,
                KEYCODE_NF_2
            };
        }

        public static bool Holoever_Is3DofControllerConnected()
        {
            return InteractionManager.Is3DofControllerConnected();
        }

        public static bool Holoever_GetKeyDown(int key)
        {
            return NxrInput.GetKeyDown(key);
        }
  
        public static bool Holoever_GetKeyPressed(int key)
        {
            return NxrInput.GetKeyPressed(key);
        }

        public static bool Holoever_GetKeyUp(int key)
        {
            return NxrInput.GetKeyUp(key);
        }

        public static bool Holoever_GetControllerKeyDown(int key)
        {
            return NxrInput.GetControllerKeyDown(key);
        }

        public static bool Holoever_GetControllerKeyPressed(int key)
        {
            return NxrInput.GetControllerKeyPressed(key); 
        }

        public static bool Holoever_GetControllerKeyUp(int key)
        {
            return NxrInput.GetControllerKeyUp(key);
        }

        public static bool Holoever_GetControllerKeyDown(int key, HAND_TYPE handType)
        {
            return NxrInput.GetControllerKeyDown(key, (InteractionManager.NACTION_HAND_TYPE)handType);
        }

        public static bool Holoever_GetControllerKeyPressed(int key, HAND_TYPE handType)
        {
            return NxrInput.GetControllerKeyPressed(key, (InteractionManager.NACTION_HAND_TYPE)handType);
        }

        public static bool Holoever_GetControllerKeyUp(int key, HAND_TYPE handType)
        {
            return NxrInput.GetControllerKeyUp(key, (InteractionManager.NACTION_HAND_TYPE)handType);
        }

        public enum HAND_TYPE
        {
            HAND_LEFT,//左手
            HAND_RIGHT,//右手
            HEAD,
            NONE
        }

        public static bool Holoever_OnBackDown()
        {
            return NxrInput.OnBackDown();
        }

        public static bool Holoever_OnBackUp()
        {
            return NxrInput.OnBackUp();
        }
   
        public static Vector2 Holoever_GetControllerTouchPadPosition()
        {
            return InteractionManager.TouchPadPosition;
        }
        #endregion

    }
}