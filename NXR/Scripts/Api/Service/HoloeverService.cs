using System.Linq;
using UnityEngine;

namespace Nxr.Internal
{

    public class HoloeverService
    {
        private const string HoloeverSDKClassName = "com.nibiru.lib.vr.NibiruVR";
        private const string ServiceClassName = "com.nibiru.service.HoloeverService";
        protected AndroidJavaObject androidActivity;
        protected AndroidJavaClass holoeverSDKClass;
        protected AndroidJavaObject holoeverOsServiceObject;
        protected AndroidJavaObject holoeverSensorServiceObject;
        protected AndroidJavaObject holoeverVoiceServiceObject;
        protected AndroidJavaObject holoeverGestureServiceObject;
        protected AndroidJavaObject holoeverVRServiceObject;
        protected AndroidJavaObject holoeverCameraServiceObject;
        protected AndroidJavaObject holoeverMarkerServiceObject;

        protected CameraPreviewHelper cameraPreviewHelper;
        protected AndroidJavaObject cameraDeviceObject;
        protected AndroidJavaObject audioManager;

        public int HMDCameraId;
        public int ControllerCameraId;

        private bool isCameraPreviewing = false;

        private string systemDevice = "";

        public void Init()
        {
#if UNITY_ANDROID
            try
            {
                using (AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    androidActivity = player.GetStatic<AndroidJavaObject>("currentActivity");
                    audioManager = androidActivity.Call<AndroidJavaObject>("getSystemService",
                        new AndroidJavaObject("java.lang.String", "audio"));
                }
            }
            catch (AndroidJavaException e)
            {
                androidActivity = null;
                Debug.LogError("Exception while connecting to the Activity: " + e);
                return;
            }

            holoeverSDKClass = BaseAndroidDevice.GetClass(HoloeverSDKClassName);

            // 
            // systemDevice = holoeverSDKClass.CallStatic<string>("getSystemProperty", "ro.product.device", "");

            holoeverOsServiceObject = holoeverSDKClass.CallStatic<AndroidJavaObject>("getNibiruOSService", androidActivity);
            holoeverSensorServiceObject =
                holoeverSDKClass.CallStatic<AndroidJavaObject>("getNibiruSensorService", androidActivity);
            holoeverVoiceServiceObject =
                holoeverSDKClass.CallStatic<AndroidJavaObject>("getNibiruVoiceService", androidActivity);
            holoeverGestureServiceObject =
                holoeverSDKClass.CallStatic<AndroidJavaObject>("getNibiruGestureService", androidActivity);
            holoeverVRServiceObject = holoeverSDKClass.CallStatic<AndroidJavaObject>("getUsingNibiruVRServiceGL");

            holoeverCameraServiceObject =
                holoeverSDKClass.CallStatic<AndroidJavaObject>("getNibiruCameraService", androidActivity);
            holoeverMarkerServiceObject =
                holoeverSDKClass.CallStatic<AndroidJavaObject>("getNibiruMarkerService", androidActivity);

            HMDCameraId = holoeverCameraServiceObject.Call<int>("getHMDCameraId");
            ControllerCameraId = holoeverCameraServiceObject.Call<int>("getControllerCameraId");

            UpdateVoiceLanguage();
            // Debug.Log("holoeverOsServiceObject is "+ holoeverOsServiceObject.Call<AndroidJavaObject>("getClass").Call<string>("getName"));
            // Debug.Log("holoeverSensorServiceObject is " + holoeverSensorServiceObject.Call<AndroidJavaObject>("getClass").Call<string>("getName"));

            HoloeverTask.HoloeverTaskApi.Init();

            IsCaptureEnabled = -1;

            // 默认触发请求权限：
            RequsetPermission(new string[]
            {
                NxrGlobal.Permission.CAMERA,
                NxrGlobal.Permission.WRITE_EXTERNAL_STORAGE,
                NxrGlobal.Permission.READ_EXTERNAL_STORAGE,
                NxrGlobal.Permission.ACCESS_NETWORK_STATE,
                NxrGlobal.Permission.ACCESS_COARSE_LOCATION,
                NxrGlobal.Permission.BLUETOOTH,
                NxrGlobal.Permission.BLUETOOTH_ADMIN,
                NxrGlobal.Permission.INTERNET,
                NxrGlobal.Permission.GET_TASKS,
            });
#endif
        }

        public static int NKEY_SYS_HANDLE = 0;
        public static int NKEY_APP_HANDLE = 1;

        /// <summary>
        /// Handle N key event
        /// 0=system handle 
        /// 1=app handle
        /// </summary>
        /// <param name="mode"></param>
        public void RegHandleNKey(int mode)
        {
            if (holoeverVRServiceObject != null)
            {
                RunOnUIThread(androidActivity,
                    new AndroidJavaRunnable(() => { holoeverVRServiceObject.Call("regHandleNKey", mode); }));
            }
            else
            {
                Debug.LogError("regHandleNKey failed, holoeverVRServiceObject is null !!!");
            }
        }

        /// <summary>
        /// Enable fps statistics
        /// </summary>
        /// <param name="isEnabled"></param>
        public void SetEnableFPS(bool isEnabled)
        {
            if (holoeverVRServiceObject != null)
            {
                holoeverVRServiceObject.Call("setEnableFPS", isEnabled);
            }
            else
            {
                Debug.LogError("SetEnableFPS failed, holoeverVRServiceObject is null !!!");
            }
        }

        /// <summary>
        /// Get fps : 0=app,1=dtr
        /// </summary>
        /// <returns></returns>
        public float[] GetFPS()
        {
            if (holoeverVRServiceObject != null)
            {
                return holoeverVRServiceObject.Call<float[]>("getFPS");
            }
            else
            {
                Debug.LogError("SetEnableFPS failed, holoeverVRServiceObject is null !!!");
            }

            return new float[] {-1, -1};
        }

        /// <summary>
        /// Register virtual mouse service
        /// </summary>
        /// <param name="serviceStatus"></param>
        public void RegisterVirtualMouseService(OnVirtualMouseServiceStatus serviceStatus)
        {
            if (holoeverOsServiceObject != null)
            {
                holoeverOsServiceObject.Call("registerVirtualMouseManagerService",
                    new HoloeverVirtualMouseServiceListener(serviceStatus));
            }
            else
            {
                Debug.LogError("RegisterVirtualMouseService failed, holoeverOsServiceObject is null !!!");
            }
        }

        /// <summary>
        /// UnRegister virtual mouse service
        /// </summary>
        public void UnRegisterVirtualMouseService()
        {
            if (holoeverOsServiceObject != null)
            {
                holoeverOsServiceObject.Call("unRegisterVirtaulMouseManagerService");
            }
            else
            {
                Debug.LogError("UnRegisterVirtualMouseService failed, holoeverOsServiceObject is null !!!");
            }
        }

        /// <summary>
        /// Set enable virtual mouse
        /// </summary>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public bool SetEnableVirtualMouse(bool enabled)
        {
            if (holoeverOsServiceObject != null)
            {
                return holoeverOsServiceObject.Call<bool>("setEnableVirtualMouse", enabled);
            }
            else
            {
                Debug.LogError("SetEnableVirtualMouse failed, holoeverOsServiceObject is null !!!");
                return false;
            }
        }

        public CameraPreviewHelper InitCameraPreviewHelper()
        {
            cameraPreviewHelper = new CameraPreviewHelper();
            return cameraPreviewHelper;
        }

        public CameraPreviewHelper CameraPreviewHelper
        {
            get { return cameraPreviewHelper; }
        }

        /// <summary>
        /// Get camera status async
        /// </summary>
        public void GetCameraStatus(int cameraId)
        {
            if (holoeverCameraServiceObject != null)
            {
                holoeverCameraServiceObject.Call("getCameraStatus", cameraId, new CameraStatusCallback());
            }
            else
            {
                Debug.LogError("GetCameraStatus failed, because holoeverCameraServiceObject is null !!!");
            }
        }

        /// <summary>
        /// Open camera
        /// </summary>
        /// <returns>HoloeverCameraDevice</returns>
        private AndroidJavaObject OpenCamera(int cameraId)
        {
            if (holoeverCameraServiceObject != null && cameraDeviceObject == null)
            {
                cameraDeviceObject = holoeverCameraServiceObject.Call<AndroidJavaObject>("openCamera", cameraId);
                return cameraDeviceObject;
            }
            else if (cameraDeviceObject != null)
            {
                return cameraDeviceObject;
            }
            else
            {
                Debug.LogError("OpenCamera failed, because holoeverCameraServiceObject is null !!!");
                return null;
            }
        }

        /// <summary>
        /// Get current cameraId
        /// </summary>
        /// <returns></returns>
        public CAMERA_ID GetCurrentCameraId()
        {
            return (CAMERA_ID) HMDCameraId;
        }

        /// <summary>
        /// Start camera preView
        /// </summary>
        public void StartCameraPreView(int cameraId)
        {
            StartCameraPreView(false,cameraId);
        }

        /// <summary>
        /// Start camera preview
        /// </summary>
        /// <param name="triggerFocus">trigger focus</param>
        public void StartCameraPreView(bool triggerFocus,int cameraId)
        {
            OpenCamera(cameraId);
            AndroidJavaObject surfaceTextureObject = cameraPreviewHelper.GetSurfaceTexture();
            cameraDeviceObject.Call<bool>("startPreviewWithBestSize", surfaceTextureObject);
            if (triggerFocus)
            {
                DoCameraAutoFocus();
            }

            isCameraPreviewing = true;
        }

        /// <summary>
        /// Stop camera preview
        /// </summary>
        public void StopCamereaPreView()
        {
            if (holoeverCameraServiceObject != null)
            {
                isCameraPreviewing = false;
                holoeverCameraServiceObject.Call("stopPreview");
                cameraDeviceObject = null;
            }
            else
            {
                Debug.LogError("StopCamereaPreView failed, because holoeverCameraServiceObject is null !!!");
            }
        }

        /// <summary>
        /// Determine whether the camera is in preview. 
        /// </summary>
        /// <returns></returns>
        public bool IsCameraPreviewing()
        {
            return isCameraPreviewing;
        }

        public void SetCameraPreviewing(bool enabled)
        {
            isCameraPreviewing = true;
        }

        public void DoCameraAutoFocus()
        {
            if (cameraDeviceObject != null)
            {
                cameraDeviceObject.Call("doAutoFocus");
            }
            else
            {
                Debug.LogError("DoCameraAutoFocus failed, because cameraDeviceObject is null !!!");
            }
        }

        public void EnableVoiceService(bool enabled)
        {
            if (holoeverVoiceServiceObject != null)
            {
                holoeverVoiceServiceObject.Call("setEnableVoice", enabled);
            }
            else
            {
                Debug.LogError("EnableVoiceService failed, because holoeverVoiceServiceObject is null !!!");
            }
        }

        /// <summary>
        /// Start voice recording
        /// </summary>
        public void StartVoiceRecording()
        {
            if (holoeverVoiceServiceObject != null)
            {
                holoeverVoiceServiceObject.Call("startRecording");
            }
            else
            {
                Debug.LogError("StartVoiceRecording failed, because holoeverVoiceServiceObject is null !!!");
            }
        }

        /// <summary>
        /// Stop voice recording
        /// </summary>
        public void StopVoiceRecording()
        {
            if (holoeverVoiceServiceObject != null)
            {
                holoeverVoiceServiceObject.Call("stopRecording");
            }
            else
            {
                Debug.LogError("StopVoiceRecording failed, because holoeverVoiceServiceObject is null !!!");
            }
        }

        /// <summary>
        /// Cancel voice recognizer
        /// </summary>
        public void CancelVoiceRecognizer()
        {
            if (holoeverVoiceServiceObject != null)
            {
                holoeverVoiceServiceObject.Call("cancelRecognizer");
            }
            else
            {
                Debug.LogError("CancelVoiceRecognizer failed, because holoeverVoiceServiceObject is null !!!");
            }
        }

        public bool IsSupportVoice()
        {
            if (holoeverVoiceServiceObject != null)
            {
                return holoeverVoiceServiceObject.Call<bool>("isMicrophoneVoice");
            }
            else
            {
                Debug.LogError("IsSupportVoice failed, because holoeverVoiceServiceObject is null !!!");
            }

            return false;
        }

        public bool IsSupport6DOF()
        {
            if (holoeverVRServiceObject != null)
            {
                return holoeverVRServiceObject.Call<bool>("isSupport6Dof");
            }
            else
            {
                Debug.LogError("IsSupport6DOF failed, because holoeverVRServiceObject is null !!!");
            }

            return false;
        }

        public bool IsSupportGesture()
        {
            if (holoeverGestureServiceObject != null)
            {
                return holoeverGestureServiceObject.Call<bool>("isCameraGesture");
            }
            else
            {
                Debug.LogError("isSupportGesture failed, because holoeverGestureServiceObject is null !!!");
            }

            return false;
        }

        public void UpdateVoiceLanguage()
        {
            if (holoeverVoiceServiceObject != null)
            {
                holoeverVoiceServiceObject.Call("setVoicePID", (int) NxrGlobal.voiceLanguage);
            }
            else
            {
                Debug.LogError("UpdateVoiceLanguage failed, because holoeverVoiceServiceObject is null !!!");
            }
        }

        /// <summary>
        /// Control camera status of gesture service：false-Turn off camera occupation，true-Turn on camera occupation
        /// </summary>
        /// <param name="enabled"></param>
        public void EnableGestureService(bool enabled)
        {
            if (holoeverGestureServiceObject != null)
            {
                holoeverGestureServiceObject.Call("setEnableGesture", enabled);
            }
            else
            {
                Debug.LogError("EnableGestureService failed, because holoeverGestureServiceObject is null !!!");
            }
        }

        public bool IsCameraGesture()
        {
            if (holoeverGestureServiceObject != null)
            {
                return holoeverGestureServiceObject.Call<bool>("isCameraGesture");
            }

            return false;
        }

        public delegate void OnSensorDataChanged(HoloeverSensorEvent sensorEvent);

        /// <summary>
        /// The callback when sensor data changes. 
        /// </summary>
        public static OnSensorDataChanged OnSensorDataChangedHandler;

        class HoloeverSensorDataListenerCallback : AndroidJavaProxy
        {
            public HoloeverSensorDataListenerCallback() : base(
                "com.nibiru.service.NibiruSensorService$INibiruSensorDataListener")
            {
            }

            public void onSensorDataChanged(AndroidJavaObject sensorEventObject)
            {
                float x = sensorEventObject.Get<float>("x");
                float y = sensorEventObject.Get<float>("y");
                float z = sensorEventObject.Get<float>("z");
                long timestamp = sensorEventObject.Get<long>("timestamp");
                AndroidJavaObject locationObject = sensorEventObject.Get<AndroidJavaObject>("sensorLocation");
                AndroidJavaObject typeObject = sensorEventObject.Get<AndroidJavaObject>("sensorType");
                SENSOR_LOCATION sensorLocation = (SENSOR_LOCATION) locationObject.Call<int>("ordinal");
                SENSOR_TYPE sensorType = (SENSOR_TYPE) typeObject.Call<int>("ordinal");

                HoloeverSensorEvent sensorEvent = new HoloeverSensorEvent(x, y, z, timestamp, sensorType, sensorLocation);
                // sensorEvent.printLog();

                // 用Loom的方法在Unity主线程中调用Text组件
                Loom.QueueOnMainThread((param) =>
                {
                    if (OnSensorDataChangedHandler != null)
                    {
                        OnSensorDataChangedHandler((HoloeverSensorEvent) param);
                    }
                }, sensorEvent);
            }
        }


        private HoloeverSensorDataListenerCallback holoeverSensorDataListenerCallback;

        public void RegisterSensorListener(SENSOR_TYPE type, SENSOR_LOCATION location)
        {
            if (holoeverSensorServiceObject != null)
            {
                if (holoeverSensorDataListenerCallback == null)
                {
                    holoeverSensorDataListenerCallback = new HoloeverSensorDataListenerCallback();
                }

                // UI线程执行
                RunOnUIThread(androidActivity, new AndroidJavaRunnable(() =>
                    {
                        AndroidJavaClass locationClass =
                            BaseAndroidDevice.GetClass("com.nibiru.service.NibiruSensorService$SENSOR_LOCATION");
                        AndroidJavaObject locationObj =
                            locationClass.CallStatic<AndroidJavaObject>("valueOf", location.ToString());

                        AndroidJavaClass typeClass =
                            BaseAndroidDevice.GetClass("com.nibiru.service.NibiruSensorService$SENSOR_TYPE");
                        AndroidJavaObject typeObj = typeClass.CallStatic<AndroidJavaObject>("valueOf", type.ToString());

                        holoeverSensorServiceObject.Call<bool>("registerSensorListener", typeObj, locationObj,
                            holoeverSensorDataListenerCallback);
                        Debug.Log("registerSensorListener=" + type.ToString() + "," + location.ToString());
                    }
                ));
            }
            else
            {
                Debug.LogError("RegisterControllerSensor failed, holoeverSensorServiceObject is null !");
            }
        }

        public void UnRegisterSensorListener()
        {
            if (holoeverSensorServiceObject != null)
            {
                // UI线程执行
                RunOnUIThread(androidActivity, new AndroidJavaRunnable(() =>
                    {
                        holoeverSensorServiceObject.Call("unregisterSensorListenerAll");
                    }
                ));
            }
            else
            {
                Debug.LogError("UnRegisterSensorListener failed, holoeverSensorServiceObject is null !");
            }
        }

        //4.1 获取屏幕亮度值：
        /// <summary>
        /// Get system's brightness value
        /// </summary>
        /// <returns></returns>
        public int GetBrightnessValue()
        {
            int BrightnessValue = 0;
#if UNITY_ANDROID
            BaseAndroidDevice.CallObjectMethod<int>(ref BrightnessValue, holoeverOsServiceObject, "getBrightnessValue");
#endif
            return BrightnessValue;
        }

        //4.2 调节屏幕亮度：
        /// <summary>
        /// Set system's brightness value
        /// </summary>
        /// <returns></returns>
        public void SetBrightnessValue(int value)
        {
            if (holoeverOsServiceObject == null) return;
#if UNITY_ANDROID
            RunOnUIThread(androidActivity,
                new AndroidJavaRunnable(() =>
                {
                    BaseAndroidDevice.CallObjectMethod(holoeverOsServiceObject, "setBrightnessValue", value, 200.01f);
                }));
#endif
        }

        //4.3 获取当前2D/3D显示模式：
        /// <summary>
        /// Get display mode 2d/3d
        /// </summary>
        /// <returns></returns>
        public DISPLAY_MODE GetDisplayMode()
        {
            if (holoeverOsServiceObject == null) return DISPLAY_MODE.MODE_2D;
            AndroidJavaObject androidObject = holoeverOsServiceObject.Call<AndroidJavaObject>("getDisplayMode");
            int mode = androidObject.Call<int>("ordinal");
            return (DISPLAY_MODE) mode;
        }

        //4.4 切换2D/3D显示模式:
        /// <summary>
        /// Set display mode 2d/3d
        /// </summary>
        /// <param name="displayMode"></param>
        public void SetDisplayMode(DISPLAY_MODE displayMode)
        {
            if (holoeverOsServiceObject != null)
            {
                RunOnUIThread(androidActivity,
                    new AndroidJavaRunnable(() =>
                    {
                        holoeverOsServiceObject.Call("setDisplayMode", (int) displayMode);
                    }));
            }
        }

        // 渠道ID
        /// <summary>
        /// Get system's channel code
        /// </summary>
        /// <returns></returns>
        public string GetChannelCode()
        {
            if (holoeverOsServiceObject == null) return "NULL";
            return holoeverOsServiceObject.Call<string>("getChannelCode");
        }

        // 型号
        /// <summary>
        /// Get device's model
        /// </summary>
        /// <returns></returns>
        public string GetModel()
        {
            if (holoeverOsServiceObject == null) return "NULL";
            return holoeverOsServiceObject.Call<string>("getModel");
        }

        // 系统OS版本
        /// <summary>
        /// Get system's os version
        /// </summary>
        /// <returns></returns>
        public string GetOSVersion()
        {
            if (holoeverOsServiceObject == null) return "NULL";
            return holoeverOsServiceObject.Call<string>("getOSVersion");
        }

        // 系统OS版本号
        /// <summary>
        /// Get system's service version
        /// </summary>
        /// <returns></returns>
        public int GetOSVersionCode()
        {
            if (holoeverOsServiceObject == null) return -1;
            return holoeverOsServiceObject.Call<int>("getOSVersionCode");
        }

        // 系统服务版本
        /// <summary>
        /// Get system's service version code
        /// </summary>
        /// <returns></returns>
        public string GetServiceVersionCode()
        {
            if (holoeverOsServiceObject == null) return "NULL";
            return holoeverOsServiceObject.Call<string>("getServiceVersionCode");
        }

        // 获取厂家软件版本：（对应驱动板软件版本号）
        /// <summary>
        /// Get system's vendor SW version
        /// </summary>
        /// <returns></returns>
        public string GetVendorSWVersion()
        {
            if (holoeverOsServiceObject == null) return "NULL";
            return holoeverOsServiceObject.Call<string>("getVendorSWVersion");
        }

        // 控制touchpad是否显示 value为true表示显示，false表示不显示 
        /// <summary>
        /// Control whether touchpad is displayed. true-display false-not display
        /// </summary>
        /// <param name="isEnable"></param>
        public void SetEnableTouchCursor(bool isEnable)
        {
            RunOnUIThread(androidActivity, new AndroidJavaRunnable(() =>
            {
                if (holoeverOsServiceObject != null)
                {
                    holoeverOsServiceObject.Call("setEnableTouchCursor", isEnable);
                }
            }));
        }

        /// <summary>
        /// Get the value of the distance sensor.
        /// </summary>
        /// <returns></returns>
        public int GetProximityValue()
        {
            if (holoeverSensorServiceObject == null) return -1;
            return holoeverSensorServiceObject.Call<int>("getProximityValue");
        }

        /// <summary>
        /// Get the value of light perception.
        /// </summary>
        /// <returns></returns>
        public int GetLightValue()
        {
            if (holoeverSensorServiceObject == null) return -1;
            return holoeverSensorServiceObject.Call<int>("getLightValue");
        }

        // UI线程中运行
        public void RunOnUIThread(AndroidJavaObject activityObj, AndroidJavaRunnable r)
        {
            activityObj.Call("runOnUiThread", r);
        }

        public delegate void CameraIdle();

        public delegate void CameraBusy();

        /// <summary>
        /// The callback when Camera is idle.
        /// </summary>
        public static CameraIdle OnCameraIdle;

        /// <summary>
        /// The callback when Camera is busy.
        /// </summary>
        public static CameraBusy OnCameraBusy;

        public delegate void OnRecorderSuccess();

        public delegate void OnRecorderFailed();

        public static OnRecorderSuccess OnRecorderSuccessHandler;
        public static OnRecorderFailed OnRecorderFailedHandler;

        class CameraStatusCallback : AndroidJavaProxy
        {
            public CameraStatusCallback() : base("com.nibiru.lib.vr.listener.NVRCameraStatusListener")
            {
            }

            public void cameraBusy()
            {
                // 从Android UI线程回调过来的，加入到Unity主线程处理
                // NxrViewer.Instance.TriggerCameraStatus(1);
                Loom.QueueOnMainThread((param) =>
                {
                    if (OnCameraBusy != null)
                    {
                        OnCameraBusy();
                    }
                }, 1);
                Debug.Log("cameraBusy");
            }

            public void cameraIdle()
            {
                // 从Android UI线程回调过来的，加入到Unity主线程处理
                // NxrViewer.Instance.TriggerCameraStatus(0);
                Loom.QueueOnMainThread((param) =>
                {
                    if (OnCameraIdle != null)
                    {
                        OnCameraIdle();
                    }
                }, 0);
                Debug.Log("cameraIdle");
            }
        }

        class CaptureCallback : AndroidJavaProxy
        {
            public CaptureCallback() : base("com.nibiru.lib.vr.listener.NVRVideoCaptureListener")
            {
            }

            public void onSuccess()
            {
                // 从Android UI线程回调过来的，加入到Unity主线程处理
                // NxrViewer.Instance.TriggerCaptureStatus(1);
                Loom.QueueOnMainThread((param) =>
                {
                    if (OnRecorderSuccessHandler != null)
                    {
                        OnRecorderSuccessHandler();
                    }
                }, 1);
            }

            public void onFailed()
            {
                // 从Android UI线程回调过来的，加入到Unity主线程处理
                // NxrViewer.Instance.TriggerCaptureStatus(0);
                Loom.QueueOnMainThread((param) =>
                {
                    if (OnRecorderFailedHandler != null)
                    {
                        OnRecorderFailedHandler();
                    }
                }, 0);
            }
        }

        public int IsCaptureEnabled { set; get; }

        public static int BIT_RATE = 4000000;

        /// <summary>
        /// Start capture
        /// </summary>
        /// <param name="path"></param>
        public void StartCapture(string path)
        {
            StartCapture(path, -1);
        }

        /// <summary>
        ///  Start capture
        /// </summary>
        /// <param name="path"></param>
        /// <param name="seconds"></param>
        public void StartCapture(string path, int seconds)
        {
            StartCapture(path, BIT_RATE, seconds);
        }

        private static int videoSize = (int) VIDEO_SIZE.V720P;

        // private static int captureCameraId = (int)CAMERA_ID.FRONT;
        /// <summary>
        ///  Start capture
        /// </summary>
        /// <param name="path"></param>
        /// <param name="bitRate"></param>
        /// <param name="seconds"></param>
        public void StartCapture(string path, int bitRate, int seconds)
        {
            IsCaptureEnabled = 1;
            holoeverSDKClass.CallStatic("startCaptureForUnity", new CaptureCallback(), path, bitRate, seconds, videoSize,
                HMDCameraId);
        }

        public static void SetCaptureVideoSize(VIDEO_SIZE video_Size)
        {
            videoSize = (int) video_Size;
        }

        /// <summary>
        /// Stop capture
        /// </summary>
        public void StopCapture()
        {
            holoeverSDKClass.CallStatic("stopCaptureForUnity");
            IsCaptureEnabled = 0;
        }

        public bool CaptureDrawFrame(int textureId, int frameId)
        {
            if (IsCaptureEnabled <= -3)
            {
                return false;
            }
            else if (IsCaptureEnabled <= 0 && IsCaptureEnabled >= -2)
            {
                // 在stop后，多执行3次，用于内部处理stop的逻辑。
                IsCaptureEnabled--;
            }

            return holoeverSDKClass.CallStatic<bool>("onDrawFrameForUnity", textureId, frameId);
        }

        private const int STREAM_VOICE_CALL = 0;
        private const int STREAM_SYSTEM = 1;
        private const int STREAM_RING = 2;
        private const int STREAM_MUSIC = 3;
        private const int STREAM_ALARM = 4;
        private const int STREAM_NOTIFICATION = 5;
        private const string currentVolume = "getStreamVolume"; //当前音量
        private const string maxVolume = "getStreamMaxVolume"; //最大音量

        public int GetVolumeValue()
        {
            if (audioManager == null) return 0;
            return audioManager.Call<int>(currentVolume, STREAM_MUSIC);
        }

        public int GetMaxVolume()
        {
            if (audioManager == null) return 1;
            return audioManager.Call<int>(maxVolume, STREAM_MUSIC);
        }

        public void EnabledMarkerAutoFocus(bool enabled)
        {
            if (holoeverMarkerServiceObject == null)
            {
                Debug.LogError("holoeverMarkerServiceObject is null");
            }
            else if (isMarkerRecognizeRunning)
            {
                holoeverMarkerServiceObject.Call(enabled ? "doAutoFocus" : "stopAutoFocus");
            }
        }

        /// <summary>
        /// Set marker recognize cameraId
        /// </summary>
        /// <param name="cameraID"></param>
        private void SetMarkerRecognizeCameraId(int cameraID)
        {
            if (holoeverMarkerServiceObject == null)
            {
                Debug.LogError("holoeverMarkerServiceObject is null");
            }
            else
            {
                holoeverMarkerServiceObject.Call("setCameraId", cameraID);
            }
        }

        private bool isMarkerRecognizeRunning;

        public bool IsMarkerRecognizeRunning
        {
            get { return isMarkerRecognizeRunning; }
            set { isMarkerRecognizeRunning = value; }
        }

        /// <summary>
        /// Start marker recognize
        /// </summary>
        public void StartMarkerRecognize()
        {
            if (holoeverMarkerServiceObject == null)
            {
                Debug.LogError("holoeverMarkerServiceObject is null");
            }
            else if (!isMarkerRecognizeRunning)
            {
                // 默认使用前置相机
                SetMarkerRecognizeCameraId(HMDCameraId);
                // 焦距，具体不同机器可以需要微调 16 , 640 * 480
                holoeverMarkerServiceObject.Call("setCameraZoom", NxrGlobal.GetMarkerCameraZoom());
                holoeverMarkerServiceObject.Call("setPreviewSize", 640, 480);
                holoeverMarkerServiceObject.Call("startMarkerRecognize");
                isMarkerRecognizeRunning = true;
            }
        }

        /// <summary>
        /// Stop marker recognize
        /// </summary>
        public void StopMarkerRecognize()
        {
            if (holoeverMarkerServiceObject == null)
            {
                Debug.LogError("holoeverMarkerServiceObject is null");
            }
            else if (isMarkerRecognizeRunning)
            {
                holoeverMarkerServiceObject.Call("stopMarkerRecognize");
                isMarkerRecognizeRunning = false;
            }
        }

        /// <summary>
        /// Get the ViewMatrix of Marker.
        /// </summary>
        /// <returns></returns>
        public float[] GetMarkerViewMatrix()
        {
            if (holoeverMarkerServiceObject == null)
            {
                Debug.LogError("holoeverMarkerServiceObject is null");
                return null;
            }
            else
            {
                float[] result = holoeverMarkerServiceObject.Call<float[]>("getMarkerViewMatrix");
                if (result == null || result.Length == 0) return null;
                // 全是0
                if (IsAllZero(result)) return null;
                return result;
            }
        }

        public static bool IsAllZero(float[] array)
        {
            for (int i = 0, l = array.Length; i < l; i++)
            {
                if (array[i] != 0) return false;
            }

            return true;
        }

        public float[] GetMarkerViewMatrix(int eyeType)
        {
            if (holoeverMarkerServiceObject == null)
            {
                Debug.LogError("holoeverMarkerServiceObject is null");
                return null;
            }
            else
            {
                float[] result = holoeverMarkerServiceObject.Call<float[]>("getMarkerViewMatrix", eyeType);
                if (result == null || result.Length == 0) return null;
                // 全是0
                if (IsAllZero(result)) return null;
                return result;
            }
        }

        public float[] GetMarkerProjectionMatrix()
        {
            if (holoeverMarkerServiceObject == null)
            {
                Debug.LogError("holoeverMarkerServiceObject is null");
                return null;
            }
            else
            {
                float[] projArr = holoeverMarkerServiceObject.Call<float[]>("getProjection");
                if (projArr == null || projArr.Length == 0)
                    return null;
                return projArr;
            }
        }

        public string GetMarkerDetectStatus()
        {
            if (holoeverMarkerServiceObject == null)
            {
                Debug.LogError("GetMarkerDetectStatus failed, holoeverMarkerServiceObject is null");
                return "-1";
            }

            string res = holoeverMarkerServiceObject.Call<string>("getParameters", "p_detect_status");
            return res == null ? "-1" : res;
        }

        public delegate void OnVirtualMouseServiceStatus(bool succ);

        public class HoloeverVirtualMouseServiceListener : AndroidJavaProxy
        {
            OnVirtualMouseServiceStatus _OnVirtualMouseServiceStatus;

            public HoloeverVirtualMouseServiceListener(OnVirtualMouseServiceStatus onVirtualMouseServiceStatus) : base(
                "com.nibiru.service.NibiruVirtualMouseManager$VirtualMouseServiceListener")
            {
                _OnVirtualMouseServiceStatus = onVirtualMouseServiceStatus;
            }

            public void onServiceRegisterResult(bool succ)
            {
                if (_OnVirtualMouseServiceStatus != null)
                {
                    _OnVirtualMouseServiceStatus(succ);
                }
            }
        }

        public void PauseGestureService()
        {
            if (holoeverGestureServiceObject != null)
            {
                holoeverGestureServiceObject.Call("onPause");
            }
            else
            {
                Debug.LogError("onPause failed, because holoeverGestureServiceObject is null !!!");
            }
        }

        public void ResumeGestureService()
        {
            if (holoeverGestureServiceObject != null)
            {
                holoeverGestureServiceObject.Call("onResume");
            }
            else
            {
                Debug.LogError("onResume failed, because holoeverGestureServiceObject is null !!!");
            }
        }

        private AndroidJavaObject javaArrayFromCS(string[] values)
        {
            AndroidJavaClass arrayClass = new AndroidJavaClass("java.lang.reflect.Array");
            AndroidJavaObject arrayObject = arrayClass.CallStatic<AndroidJavaObject>("newInstance",
                new AndroidJavaClass("java.lang.String"), values.Count());
            for (int i = 0; i < values.Count(); ++i)
            {
                arrayClass.CallStatic("set", arrayObject, i, new AndroidJavaObject("java.lang.String", values[i]));
            }

            return arrayObject;
        }

        /// <summary>
        /// Request permission
        /// </summary>
        /// <param name="names">NxrGlobal.Permission</param>
        public void RequsetPermission(string[] names)
        {
            if (holoeverOsServiceObject != null)
            {
                holoeverOsServiceObject.Call("requestPermission", javaArrayFromCS(names));
            }
        }

        /// <summary>
        /// Get QCOM of product device.
        /// </summary>
        /// <returns></returns>
        public QCOMProductDevice GetQCOMProductDevice()
        {
            if ("msm8996".Equals(systemDevice))
            {
                return QCOMProductDevice.QCOM_820;
            }
            else if ("msm8998".Equals(systemDevice))
            {
                return QCOMProductDevice.QCOM_835;
            }
            else if ("sdm710".Equals(systemDevice))
            {
                return QCOMProductDevice.QCOM_XR1;
            }
            else if ("sdm845".Equals(systemDevice))
            {
                return QCOMProductDevice.QCOM_845;
            }

            return QCOMProductDevice.QCOM_UNKNOW;
        }


        public void LockToCur()
        {
            if (holoeverVRServiceObject != null)
            {
                RunOnUIThread(androidActivity,
                    new AndroidJavaRunnable(() => { holoeverVRServiceObject.Call("lockTracker"); }));
            }
            else
            {
                Debug.LogError("LockToCur failed, holoeverVRServiceObject is null !!!");
            }
        }

        public void LockToFront()
        {
            if (holoeverVRServiceObject != null)
            {
                RunOnUIThread(androidActivity,
                    new AndroidJavaRunnable(() => { holoeverVRServiceObject.Call("lockTrackerToFront"); }));
            }
            else
            {
                Debug.LogError("LockToFront failed, holoeverVRServiceObject is null !!!");
            }
        }

        public void UnLock()
        {
            if (holoeverVRServiceObject != null)
            {
                RunOnUIThread(androidActivity,
                    new AndroidJavaRunnable(() => { holoeverVRServiceObject.Call("unlockTracker"); }));
            }
            else
            {
                Debug.LogError("UnLock failed, holoeverVRServiceObject is null !!!");
            }
        }


    }
}