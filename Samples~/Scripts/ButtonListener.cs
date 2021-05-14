
using Nxr.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace NXR.Samples
{
    //  button click
public class ButtonListener : MonoBehaviour//, INxrButtonListener
     {
    //     NibiruService nibiruService;
    //     void Start()
    //     {
    //         // change camera FOV
    //         //  NxrViewer.Instance.UpdateCameraFov(60);
    //         //  chagne camera Far
    //         //  NxrViewer.Instance.UpateCameraFar(20000);
    //
    //         nibiruService = NxrViewer.Instance.GetNibiruService();
    //
    //         if (nibiruService != null)
    //         {
    //             //  Os Info : MR0001,Ned+Glass X2,1.00.015,15,3,1.5,7,MODE_3D,1,255
    //             //Debug.Log("Os Info : "+
    //             //    nibiruService.GetChannelCode() +","+nibiruService.GetModel()+","+nibiruService.GetOSVersion()+","+nibiruService.GetOSVersionCode()+
    //             //    ","+nibiruService.GetServiceVersionCode()+","+nibiruService.GetVendorSWVersion()+","+nibiruService.GetBrightnessValue() +","
    //             //    + nibiruService.GetDisplayMode()+","+nibiruService.GetProximityValue()
    //             //    +","+nibiruService.GetLightValue());
    //         }
    //         else
    //         {
    //             Debug.Log("nibiruService null >>>>");
    //         }
    //
    //         NibiruMarker.OnMarkerFoundHandler = MarkerFound;
    //         NibiruMarker.OnMarkerLostHandler = MarkerLost;
    //
    //     }
    //
    //     void MarkerFound()
    //     {
    //         Debug.Log("Callback->onMarkerFound");
    //     }
    //
    //     void MarkerLost()
    //     {
    //         Debug.Log("Callback->onMarkerLost");
    //     }
    //
    //     public void OnPressBack()
    //     {
    //         Debug.Log("OnPressBack");
    //     }
    //
    //     public void OnPressDown()
    //     {
    //         Debug.Log("OnPressDown");
    //         //NxrViewer.Instance.GazeApi(GazeTag.Hide, "");
    //         //NxrViewer.Instance.ForceUseReticle(true);
    //         if (nibiruService != null)
    //         {
    //             // nibiruService.SetDisplayMode(DISPLAY_MODE.MODE_2D);
    //         }
    //     }
    //
    //     public void OnPressEnter(bool isKeyUp)
    //     {
    //         //if (isKeyUp)
    //         //{
    //         //    //NxrViewer.Instance.GazeApi(GazeTag.Show, "");
    //         //    //NxrViewer.Instance.ForceUseReticle(false);
    //         //    Debug.Log("Confirm Key Up." + gameObject.name);
    //
    //         //    // stop
    //         //    GameObject apiObj = GameObject.Find("apiRoot");
    //         //    if (apiObj != null && apiObj.transform.Find("ButtonStartRec") != null)
    //         //    {
    //         //        GameObject startRecObj = apiObj.transform.Find("ButtonStartRec").gameObject;
    //         //        if (startRecObj == null) return;
    //         //        bool isActive = startRecObj.activeSelf;
    //         //        if (isActive && startRecObj.GetComponent<TeleportService>().isGazedAt())
    //         //        {
    //         //            nibiruService.StopVoiceRecording();
    //         //        }
    //         //    }
    //
    //         //}
    //         //else
    //         //{
    //         //    Debug.Log("Confirm Key Down." + gameObject.name);
    //
    //         //    // start
    //         //    GameObject apiObj = GameObject.Find("apiRoot");
    //         //    if (apiObj != null && apiObj.transform.Find("ButtonStartRec") != null)
    //         //    {
    //         //        GameObject startRecObj = apiObj.transform.Find("ButtonStartRec").gameObject;
    //         //        if (startRecObj == null) return;
    //         //        bool isActive = startRecObj.activeSelf;
    //         //        if (isActive && startRecObj.GetComponent<TeleportService>().isGazedAt())
    //         //        {
    //         //            nibiruService.StartVoiceRecording();
    //         //        }
    //         //    }
    //
    //         //    //NxrViewer.Instance.SetIpd(0.043f);
    //         //}
    //         Debug.Log("OnPressEnter." + gameObject.name + ", isKeyUp." + isKeyUp);
    //     }
    //
    //     public void OnPressLeft()
    //     {
    //         Debug.Log("OnPressLeft.");
    //         // NxrViewer.Instance.GazeApi(GazeTag.Show, "");
    //         // NxrViewer.Instance.GazeApi(GazeTag.Set_Size, ((int)GazeSize.Large).ToString());
    //         // NxrViewer.Instance.ResetCameraFov();
    //         // NxrViewer.Instance.SetHorizontalAngleRange(-30, 30);
    //         // NxrViewer.Instance.RequestLock();
    //
    //         if (nibiruService != null)
    //         {
    //             //nibiruService.SetBrightnessValue(nibiruService.GetBrightnessValue() - 1);
    //             // nibiruService.SetEnableTouchCursor(true);
    //
    //             if (gameObject.name.Equals("ButtonListenerObect"))
    //             {
    //                 // nibiruService.StartMarkerRecognize();
    //             }
    //         }
    //
    //         //NibiruService.OnRecorderSuccessHandler();
    //
    //         //NxrViewer.Instance.ForceUseReticle(false);
    //     }
    //
    //     public void OnPressRight()
    //     {
    //         Debug.Log("OnPressRight_" + gameObject.name);
    //         // NxrViewer.Instance.GazeApi(GazeTag.Set_Color, "0.043f_0.435f_0.043f");
    //         // NxrViewer.Instance.UpdateCameraFov(45);
    //         // NxrViewer.Instance.SetVerticalAngleRange(-30, 30);
    //         // NxrViewer.Instance.OpenVideoPlayer("/storage/emulated/0/nibiru.mp4", 0,2,1);// 跳转视频播放
    //         // NxrViewer.Instance.RequestUnLock();
    //         // NxrViewer.Instance.ForceUseReticle(true);
    //         if (nibiruService != null)
    //         {
    //             //  nibiruService.SetBrightnessValue(nibiruService.GetBrightnessValue() + 1);
    //             if (gameObject.name.Equals("ButtonListenerObect"))
    //             {
    //                 // nibiruService.StopMarkerRecognize();
    //             }
    //         }
    //     }
    //
    //     public void OnPressUp()
    //     {
    //         Debug.Log("OnPressUp");
    //         // NxrViewer.Instance.UpdateCameraFov(40);
    //         // NxrViewer.Instance.GazeApi(GazeTag.Set_Size, ((int)GazeSize.Small).ToString());
    //         // NxrViewer.Instance.RemoveAngleLimit();
    //         if (nibiruService != null)
    //         {
    //             //nibiruService.SetDisplayMode(DISPLAY_MODE.MODE_3D);
    //             //nibiruService.SetEnableTouchCursor(false);
    //         }
    //     }
    //
    //     public void OnPressVolumnUp()
    //     {
    //         Debug.Log("OnPressVolumnUp");
    //     }
    //
    //     public void OnPressVolumnDown()
    //     {
    //         Debug.Log("OnPressVolumnDown");
    //
    //     }
    //
    //     public void OnLiftLeft()
    //     {
    //         Debug.Log("OnLiftLeft");
    //     }
    //
    //     public void OnLiftRight()
    //     {
    //         Debug.Log("OnLiftRight");
    //     }
    //
    //     public void OnLiftUp()
    //     {
    //         Debug.Log("OnLiftUp");
    //     }
    //
    //     public void OnLiftDown()
    //     {
    //         Debug.Log("OnLiftDown");
    //     }
    //
    //     public void OnLiftBack()
    //     {
    //         Debug.Log("OnLiftBack." + (gameObject != null ? gameObject.name : ""));
    //
    //         //if (NibiruRemindBox.Instance.remindbox != null)
    //         //{
    //         //    return;
    //         //}
    //         //if (NibiruKeyBoard.Instance.isShown() && Application.isMobilePlatform)
    //         //{
    //         //    NibiruKeyBoard.Instance.Dismiss();
    //         //    Debug.Log("NibiruKeyBoard->Dismiss");
    //         //    return;
    //         //}
    //         //if (gameObject != null && (gameObject.name.Equals("Cube2_AR") || gameObject.name.Equals("NARUIObj") || gameObject.name.Equals("BG")
    //         //    || gameObject.name.Equals("CubeVoice") || gameObject.name.Equals("CubeGesture") || gameObject.name.Equals("CameraTitle") ||
    //         //    gameObject.name.Equals("RecordTitle") || gameObject.name.Equals("CubeGoto") || gameObject.name.Equals("ButtonListenerObject") ||
    //         //    gameObject.name.Equals("LosBack") || gameObject.name.Equals("RecogButtonListener") || gameObject.name.Equals("SyncFrame")))
    //         //{
    //         //    SceneManager.LoadScene("AR_DemoScene");
    //         //    return;
    //         //}
    //
    //         //if (gameObject != null && (gameObject.name.Equals("Cube2_VR") || gameObject.name.Equals("NVRUIObj_VR")))
    //         //{
    //         //    SceneManager.LoadScene("VR_DemoScene");
    //         //    return;
    //         //}
    //
    //         //if (gameObject != null && (gameObject.name.Equals("Cube_VR") || gameObject.name.Equals("Cube_AR")))
    //         //{
    //         //    SceneManager.LoadScene("FirstScene");
    //         //    return;
    //         //}
    //
    //         //if (Application.isMobilePlatform)
    //         //{
    //         //    Application.Quit();
    //         //}
    //     }
    //
    //     public void OnFuctionKeyDown(FunctionKeyCode keyCode)
    //     {
    //         Debug.Log("OnFuctionKeyDown :" + keyCode);
    //     }
    //
    //     public void OnLiftFuctionKey(FunctionKeyCode keyCode)
    //     {
    //         Debug.Log("OnLiftFuctionKey :" + keyCode);
    //     }
    //
    //
    }
}