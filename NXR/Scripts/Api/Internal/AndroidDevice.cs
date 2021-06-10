#if UNITY_ANDROID

using UnityEngine;

namespace Nxr.Internal
{
    public class AndroidDevice : NxrDevice
    {
        // 
        private const string ActivityListenerClass =
            "com.nibiru.lib.xr.unity.NibiruVRUnityService";

        // sdk-class
        private const string HoloeverVRClass = "com.nibiru.lib.vr.NibiruVR";

        private static AndroidJavaObject activityListener, holoeverVR;

        AndroidJavaObject holoeverVRService = null;

        public override void Init()
        {
            SetApplicationState();

            ConnectToActivity();
            base.Init();
        }

        protected override void ConnectToActivity()
        {
            base.ConnectToActivity();
            if (androidActivity != null && activityListener == null)
            {
                activityListener = Create(ActivityListenerClass);
            }
            if (androidActivity != null && holoeverVR == null)
            {
                holoeverVR = Create(HoloeverVRClass);
            }
        }

        public override void TurnOff()
        {
            CallStaticMethod(activityListener, "shutdownBroadcast");
        }

        public override void Reboot()
        {
            CallStaticMethod(activityListener, "rebootBroadcast");
        }

        public override long CreateHoloeverVRService()
        {
            string hmdType = "NONE";
            CallStaticMethod(ref hmdType, holoeverVR, "getMetaData", androidActivity, "HMD_TYPE");
            if (hmdType != null)
            {
                NxrViewer.Instance.HmdType = hmdType.Equals("AR") ? HMD_TYPE.AR : (hmdType.Equals("VR") ? HMD_TYPE.VR : HMD_TYPE.NONE);
            }

            string initParams = "";
            long pointer = 0;
            CallStaticMethod(ref initParams, holoeverVR, "initNibiruVRServiceForUnity", androidActivity);
            // -1207076736_0_1_1_1_20.0_20.0
            Debug.Log("initParams is " + initParams + ",hmdType is " + hmdType);
            string[] data = initParams.Split('_');
            pointer = long.Parse(data[0]);
            NxrGlobal.supportDtr = (int.Parse(data[1]) == 1 ? true : false);
            NxrGlobal.distortionEnabled = (int.Parse(data[2]) == 1 ? true : false);
            NxrGlobal.useNvrSo = (int.Parse(data[3]) == 1 ? true : false);
            if (data.Length >= 5)
            {
                NxrGlobal.offaxisDistortionEnabled = (int.Parse(data[4]) == 1 ? true : false);
            }
            // 6dof
            if (NxrViewer.Instance.TrackerPosition)
            {
                CallStaticMethod(holoeverVR, "setTrackingModeForUnity", (int)TRACKING_MODE.POSITION);
            }


            int meshSizeX = -1;
            if (data.Length >= 6)
            {
                meshSizeX = (int)float.Parse(data[5]);
            }

            int meshSizeY = -1;
            if (data.Length >= 7)
            {
                meshSizeY = (int)float.Parse(data[6]);
            }

            if (data.Length >= 8)
            {
                float fps = float.Parse(data[7]);
                // 防止从系统获取的刷新率出现异常，此处保证最低为60
                NxrGlobal.refreshRate = Mathf.Max(60, fps > 0 ? fps : 0);
            }

            if (meshSizeX > 0 && meshSizeY > 0)
            {
                NxrGlobal.meshSize = new int[] { meshSizeX, meshSizeY };
            }

            string channelCode = "";
            CallStaticMethod<string>(ref channelCode, holoeverVR, "getChannelCode");
            NxrGlobal.channelCode = channelCode;

            // 系统支持
            int[] allVersion = new int[] { -1, -1, -1, -1 };
            CallStaticMethod(ref allVersion, holoeverVR, "getVersionForUnity");
            NxrGlobal.soVersion = allVersion[0];
            NxrGlobal.jarVersion = allVersion[1];
            NxrGlobal.platPerformanceLevel = allVersion[2];
            NxrGlobal.platformID = allVersion[3];
            NxrSDKApi.Instance.IsSptMultiThreadedRendering = NxrGlobal.soVersion >= 414;
            NxrGlobal.isVR9Platform = NxrGlobal.platformID == (int)PLATFORM.PLATFORM_VR9;
            if (NxrGlobal.isVR9Platform)
            {
                NxrGlobal.distortionEnabled = false;
                NxrGlobal.supportDtr = true;
                NxrViewer.Instance.SwitchControllerMode(false);
            }

            if(!NxrSDKApi.Instance.IsSptMultiThreadedRendering && SystemInfo.graphicsMultiThreaded)
            {
                AndroidLog("*****Warning******\n\n System Does Not Support Unity MultiThreadedRendering !!! \n\n*****Warning******");
                AndroidLog("Support Unity MultiThreadedRendering Need V2 Version >=414, Currently Is " + NxrGlobal.soVersion + " !!!");
            }

            Debug.Log("AndDev->Service : [pointer]=" + pointer + ", [dtrSpt] =" + NxrGlobal.supportDtr + ", [DistEnabled]=" +
            NxrGlobal.distortionEnabled + ", [useNvrSo]=" + NxrGlobal.useNvrSo + ", [code]=" + channelCode + ", [jar]=" + NxrGlobal.jarVersion + ", [so]=" + NxrGlobal.soVersion
            + ", [platform id]=" + NxrGlobal.platformID + ", [pl]=" + NxrGlobal.platPerformanceLevel + ",[offaxisDist]=" + NxrGlobal.offaxisDistortionEnabled + ",[mesh]=" + meshSizeX +
            "*" + meshSizeY + ",[fps]=" + NxrGlobal.refreshRate + "," + channelCode);

            // 读取cardboard参数
            string cardboardParams = "";
            CallStaticMethod<string>(ref cardboardParams, holoeverVR, "getNibiruVRConfigFull");
            if (cardboardParams.Length > 0)
            {
                Debug.Log("cardboardParams is " + cardboardParams);
                string[] profileData = cardboardParams.Split('_');
                for (int i = 0; i < NxrGlobal.dftProfileParams.Length; i++)
                {
                    if (i >= profileData.Length) break;

                    if (profileData[i] == null || profileData[i].Length == 0) continue;

                    NxrGlobal.dftProfileParams[i] = float.Parse(profileData[i]);
                }
            }
            else
            {
                Debug.Log("Nxr->AndroidDevice->getNibiruVRConfigFull Failed ! ");
            }

            // offaxis distortion
            if (NxrGlobal.offaxisDistortionEnabled)
            {
                string offaxisParams = "";
                CallStaticMethod<string>(ref offaxisParams, holoeverVR, "getOffAxisDistortionConfig");
                if (offaxisParams != null && offaxisParams.Length > 0)
                {
                    NxrGlobal.offaxisDistortionConfigData = offaxisParams;
                    // Debug.LogError(offaxisParams);
                }

                string sdkParams = "";
                CallStaticMethod<string>(ref sdkParams, holoeverVR, "getSDKConfig");
                if (sdkParams != null && sdkParams.Length > 0)
                {
                    NxrGlobal.sdkConfigData = sdkParams;
                    string[] linesCN = sdkParams.Split('\n');
                    //key=value
                    foreach (string line in linesCN)
                    {
                        if (line == null || line.Length <= 1)
                        {
                            continue;
                        }
                        string[] keyAndValue = line.Split('=');
                        //Debug.Log("line=" + line);
                        if (keyAndValue[0].Contains("oad_offset_x1"))
                        {
                            NxrGlobal.offaxisOffset[0] = int.Parse(keyAndValue[1]);
                        }
                        else if (keyAndValue[0].Contains("oad_offset_x2"))
                        {
                            NxrGlobal.offaxisOffset[1] = int.Parse(keyAndValue[1]);
                        }
                        else if (keyAndValue[0].Contains("oad_offset_y1"))
                        {
                            NxrGlobal.offaxisOffset[2] = int.Parse(keyAndValue[1]);
                        }
                        else if (keyAndValue[0].Contains("oad_offset_y2"))
                        {
                            NxrGlobal.offaxisOffset[3] = int.Parse(keyAndValue[1]);
                        }
                    }
                }

                Debug.Log("Offaxis Offset : " + NxrGlobal.offaxisOffset[0] + "," + NxrGlobal.offaxisOffset[1] + "," + NxrGlobal.offaxisOffset[2] + "," + NxrGlobal.offaxisOffset[3]);
            }

            // Debug.LogError("AndroidDevice-Ptr=" +this.GetHashCode());
            return pointer;
        }

        public override void SetDisplayQuality(int level)
        {
            CallStaticMethod(holoeverVR, "setDisplayQualityForUnity", level);
        }

        public override bool GazeApi(GazeTag tag, string param)
        {
            bool show = false;
            CallStaticMethod<bool>(ref show, holoeverVR, "gazeApiForUnity", (int)tag, param);
            return show;
        }

        public override void SetSplitScreenModeEnabled(bool enabled)
        {

        }
        public override void AndroidLog(string msg)
        {
            CallStaticMethod(activityListener, "log", msg);
        }
        public override void SetSystemParameters(string key, string value)
        {
            if (holoeverVR != null)
            {
                CallStaticMethod(holoeverVR, "setSystemParameters", key, value);
            }
        }

        public override void OnApplicationPause(bool pause)
        {
            base.OnApplicationPause(pause);
            //CallObjectMethod(activityListener, "onPause", pause);

            if (!pause && androidActivity != null)
            {
                RunOnUIThread(androidActivity, new AndroidJavaRunnable(runOnUiThread));
            }

        }

        public override void AppQuit()
        {
            if (androidActivity != null)
            {
                RunOnUIThread(androidActivity, new AndroidJavaRunnable(() =>
                {
                    androidActivity.Call("finish");
                }));
            }
        }

        public override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
        }

        void runOnUiThread()
        {
            //mActivity.getWindow().addFlags(128);
            //mActivity.getWindow().getDecorView().setSystemUiVisibility(5894);
            AndroidJavaObject androidWindow = androidActivity.Call<AndroidJavaObject>("getWindow");
            androidWindow.Call("addFlags", 128);
            AndroidJavaObject androidDecorView = androidWindow.Call<AndroidJavaObject>("getDecorView");
            androidDecorView.Call("setSystemUiVisibility", 5894);
        }

        public override void SetIsKeepScreenOn(bool keep)
        {
            if (androidActivity != null)
            {
                RunOnUIThread(androidActivity, new AndroidJavaRunnable(() =>
                {
                    SetScreenOn(keep);
                }));
            }
        }
        //if(enable) {
        //	getWindow().addFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);
        //} else {
        //	getWindow().clearFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);
        //}
        void SetScreenOn(bool enable)
        {
            if (enable)
            {
                AndroidJavaObject androidWindow = androidActivity.Call<AndroidJavaObject>("getWindow");
                androidWindow.Call("addFlags", 128);
            }
            else
            {
                AndroidJavaObject androidWindow = androidActivity.Call<AndroidJavaObject>("getWindow");
                androidWindow.Call("clearFlags", 128);
            }
        }

        private void SetApplicationState()
        {
        }

        /// <summary>
        ///    * @param path
        ///    * @param type23d  0=2d,1=3d
        ///    * @param mode  0=normal,1=360,2=180,3=fullmode
        ///    * @param decode 0=hardware,1=software
        /// </summary>
        public override void ShowVideoPlayer(string path, int type2D3D, int mode, int decode)
        {
            CallStaticMethod(holoeverVR, "showVideoPlayer", path, type2D3D, mode, decode);
        }

        //public override void DismissVideoPlayer()
        //{
        //    CallStaticMethod(holoeverVR, "dismissVideoPlayer");
        //}

        void InitHoloevevrVRService()
        {
            if (holoeverVRService == null)
            {
                // getHoloeverVRService
                CallStaticMethod<AndroidJavaObject>(ref holoeverVRService, holoeverVR, "getNibiruVRService", null);
            }
        }

        public override void SetIpd(float ipd)
        {
            InitHoloevevrVRService();
            if (holoeverVRService != null)
            {
                CallObjectMethod(holoeverVRService, "setIpd", ipd);
            }
            else
            {
                Debug.LogError("SetIpd failed, because holoeverVRService is null !!!!");
            }
        }

        public override void SetTimeWarpEnable(bool enabled)
        {
            InitHoloevevrVRService();
            if (holoeverVRService != null)
            {
                CallObjectMethod(holoeverVRService, "setTimeWarpEnable", enabled);
            }
            else
            {
                Debug.LogError("SetTimeWarpEnable failed, because holoeverVRService is null !!!!");
            }
        }
        /// <summary>
        /// Not currently supported.
        /// </summary>
        /// <returns></returns>
        //public override void SetEnableSyncFrame(bool enabled)
        //{
        //    InitHoloevevrVRService();
        //    if (holoeverVRService != null)
        //    {
        //        CallObjectMethod(holoeverVRService, "setEnableSyncFrame", enabled);
        //    }
        //    else
        //    {
        //        Debug.LogError("SetEnableSyncFrame failed, because holoeverVRService is null !!!!");
        //    }
        //}

        //public override string GetSyncFrameUrl()
        //{
        //    InitHoloevevrVRService();
        //    if (holoeverVRService != null)
        //    {
        //        return holoeverVRService.Call<string>("getSyncFrameUrl");
        //    }
        //    else
        //    {
        //        Debug.LogError("GetSyncFrameUrl failed, because holoeverVRService is null !!!!");
        //    }
        //    return null;
        //}

        //public override bool IsSyncFrameEnabled()
        //{
        //    InitHoloevevrVRService();
        //    if (holoeverVRService != null)
        //    {
        //        return holoeverVRService.Call<bool>("isSyncFrameEnabled");
        //    }
        //    return false;
        //}

        //public override bool IsSyncFrameSupported()
        //{
        //    InitHoloevevrVRService();
        //    if (holoeverVRService != null)
        //    {
        //        return holoeverVRService.Call<bool>("isSyncFrameSupported");
        //    }
        //    return false;
        //}

        public override string GetStoragePath() { return GetAndroidStoragePath(); }

        public override void SetCameraNearFar(float near, float far)
        {
            CallStaticMethod(holoeverVR, "setProjectionNearFarForUnity", near, far);
        }

        public override void StopCapture()
        {
            CallStaticMethod(holoeverVR, "stopCaptureForUnity");
        }

        public override void OnDrawFrameCapture(int frameId)
        {
            CallStaticMethod(holoeverVR, "onDrawFrameForUnity", frameId);
        }

        public override NxrInstantNativeApi.HoloeverDeviceType GetSixDofControllerPrimaryDeviceType()
        {
            string result = "3";
            CallStaticMethod<string>(ref result, holoeverVR, "getSystemProperty", "nxr.ctrl.primaryhand", "3");
            Debug.Log("primaryhand_" + result);
            int type = int.Parse(result);
            // 1 = left, 0 = right
            if (type == 0)
            {
                return NxrInstantNativeApi.HoloeverDeviceType.RightController;
            } else if(type == 1)
            {
                return NxrInstantNativeApi.HoloeverDeviceType.LeftController;
            }
            return NxrInstantNativeApi.HoloeverDeviceType.None;
        }

        public override void SetSixDofControllerPrimaryDeviceType(NxrInstantNativeApi.HoloeverDeviceType deviceType)
        {
            int type = -1;
            if(deviceType == NxrInstantNativeApi.HoloeverDeviceType.LeftController)
            {
                type = 1;
            } else if(deviceType == NxrInstantNativeApi.HoloeverDeviceType.RightController)
            {
                type = 0;
            }

            if (type >=0) CallStaticMethod(holoeverVR, "setSystemProperty", "nxr.ctrl.primaryhand", "" + type);
        }
        
        public override int GetControllerTipState()
        {
            string result = "0";
            CallStaticMethod<string>(ref result, holoeverVR, "getSystemProperty", "nxr.ctrl.calib.tip", "0");
            int state = int.Parse(result);
            // 1-手柄校准提示框已弹出 , 0-未弹出
            return state;
        }

        public override void SetControllerTipState(int state)
        {
            CallStaticMethod(holoeverVR, "setSystemProperty", "nxr.ctrl.calib.tip", "" + state);
        }
    }
}
/// @endcond

#endif
