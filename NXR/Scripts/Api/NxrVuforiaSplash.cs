﻿using UnityEngine;
namespace Nxr.Internal
{
    public class NxrVuforiaSplash : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaObject holoeverVR = new AndroidJavaObject("com.nibiru.lib.vr.NibiruVR");
        holoeverVR.CallStatic("setSystemProperty", "nar.vuforia.splash.finished", "1");
#endif
        }

    }
}