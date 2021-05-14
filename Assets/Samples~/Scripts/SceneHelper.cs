
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Nxr.Internal;
using HoloeverTask;

namespace NXR.Samples
{
    public class SceneHelper : MonoBehaviour
    {

        Text distanceText;
        TextMesh textMesh;
        Text pathText;
        Text InfoText;
        // Use this for initialization
        void Start()
        {
            GameObject btnObj = GameObject.Find("Button_Jump");//"Button"为你的Button的名称  
            if (btnObj != null)
            {
                Button btn = btnObj.GetComponent<Button>();
                btn.onClick.AddListener(delegate ()
                {
                    Debug.Log("场景切换->DemoScene2");
                });
            }

            GameObject disObj = GameObject.Find("ObjectDistance");
            if (disObj != null)
            {
                distanceText = disObj.GetComponent<Text>();
            }

            GameObject frameIdObj = GameObject.Find("FrameIdText");
            if (frameIdObj != null)
            {
                textMesh = frameIdObj.GetComponent<TextMesh>();
            }

            GameObject pathObj = GameObject.Find("FilePath");
            if (pathObj != null)
            {
                pathText = pathObj.GetComponent<Text>();
            }

            GameObject infoObj = GameObject.Find("SystemInfo");
            if (infoObj != null)
            {
                InfoText = infoObj.GetComponent<Text>();
            }

            HoloeverTaskApi.setSelectionCallback(onSelectionResult);



        }

        string path = "";
        string info = "";    // Use this for initialization
        public void onSelectionResult(AndroidJavaObject task)
        {
            Debug.Log("onSelectionResult:");
            path = HoloeverTaskApi.GetResultPathFromSelectionTask(task);

        }

        // Update is called once per frame
        void Update()
        {
            if (distanceText != null)
            {
                distanceText.text = "Distance : " + Nxr.Internal.NxrGlobal.focusObjectDistance;
            }

            if (textMesh != null)
            {
                textMesh.text = "Frame: " + NxrViewer.Instance.GetFrameId();
            }

            if (pathText != null)
            {
                pathText.text = "GethFilePath: " + path;
            }

            if (InfoText != null && doUpdateSystemInfo)
            {
                InfoText.text = "System Info:" + info;
                doUpdateSystemInfo = false;
            }

        }

        private bool doUpdateSystemInfo = false;
        public void GetSystemInfo()
        {
            info = "GetChannelCode:" + HoloeverTaskApi.GetChannelCode() + "\n"
                //+ "GetModel:" + NibiruTaskApi.GetModel() + "\n"
                + "GetOSVersion:" + HoloeverTaskApi.GetOSVersion() + "\n"
                + "getOSVersionCode:" + HoloeverTaskApi.GetOSVersionCode() + "\n"
                //+ "GetVendorSWVersion:" + NibiruTaskApi.GetVendorSWVersion() + "\n"
                //+ "GetBrightnessValue:" + NibiruTaskApi.GetBrightnessValue() + "\n"
                + "GetCurrentLanguage:" + HoloeverTaskApi.GetCurrentLanguage() + "\n"
                + "GetCurrentTimezone:" + HoloeverTaskApi.GetCurrentTimezone() + "\n"
                + "GetDeviceName:" + HoloeverTaskApi.GetDeviceName() + "\n";
            doUpdateSystemInfo = true;
        }

        public void GoMarkerScene()
        {
            HoloeverKeyBoard.Instance.Dismiss();
            SceneManager.LoadScene("MarkerScene");
        }

        public void GoCameraScene()
        {
            HoloeverKeyBoard.Instance.Dismiss();
            SceneManager.LoadScene("CameraScene");
        }

        public void GoRecordScene()
        {
            HoloeverKeyBoard.Instance.Dismiss();
            SceneManager.LoadScene("RecordScene");
        }

        public void GoServiceScene()
        {
            HoloeverKeyBoard.Instance.Dismiss();
            SceneManager.LoadScene("ServiceScene");
        }

        public void GoNextARScene()
        {
            HoloeverKeyBoard.Instance.Dismiss();
            SceneManager.LoadScene("AR_DemoScene2");
        }

        public void GoARUIScene()
        {
            HoloeverKeyBoard.Instance.Dismiss();
            SceneManager.LoadScene("AR_DemoUIScene");
        }

        public void GoNextVRScene()
        {
            HoloeverKeyBoard.Instance.Dismiss();
            SceneManager.LoadScene("VR_DemoScene2");
        }

        public void GoVRUIScene()
        {
            HoloeverKeyBoard.Instance.Dismiss();
            SceneManager.LoadScene("VR_DemoUIScene");
        }

        public void GoVoiceScene()
        {
            HoloeverKeyBoard.Instance.Dismiss();
            SceneManager.LoadScene("VoiceScene");
        }

        public void GoGestureScene()
        {
            HoloeverKeyBoard.Instance.Dismiss();
            SceneManager.LoadScene("GestureScene");
        }

        public void GoSixDofScene()
        {
            HoloeverKeyBoard.Instance.Dismiss();
            SceneManager.LoadScene("SixdofScene");
        }

        public void OpenExplorer()
        {
            HoloeverTaskApi.OpenBrowerExplorer("http://www.holoever.com");
        }

        public void OpenSettings()
        {
            HoloeverTaskApi.OpenSettingsMain();
        }

        public void OpenImage()
        {
            HoloeverTaskApi.OpenImageGallery("sdcard/nibiru.png");
        }

        public void GetFilePath()
        {
            HoloeverTaskApi.GetFilePath("sdcard");
        }

        public void OpenVideoPlayer()
        {
            NxrViewer.Instance.OpenVideoPlayer(NxrViewer.Instance.GetStoragePath() + "/nibiru.mp4", 0, 2, 1);
        }

        public void GoSystemApiScene()
        {
            HoloeverKeyBoard.Instance.Dismiss();
            SceneManager.LoadScene("SystemApi");
        }

        public void GoRecognitionScene()
        {
            HoloeverKeyBoard.Instance.Dismiss();
            SceneManager.LoadScene("RecognizeScene");
        }
        /// <summary>
        /// 暂不支持-------
        /// </summary>
        //public void GoSyncFrameScene()
        //{
        //    NibiruKeyBoard.Instance.Dismiss();
        //    SceneManager.LoadScene("SyncFrameScene");
        //}

        public void PointerEnter()
        {
            Debug.Log("pointer enter");
        }

        public void PointerExit()
        {
            Debug.Log("pointer exit");
        }

        // VR=0, -1.8f, 2.8f
        // AR=0, -2f, 6f
        public void ShowKeyBoard_VR()
        {
            // 
            Text text = GetComponentInParent<Text>();
            HoloeverKeyBoard.Instance.SetText(text);
            // change keyboard postion or rotation
            HoloeverKeyBoard.Instance.GetKeyBoardTransform();
            // get the input string
            HoloeverKeyBoard.Instance.GetKeyBoardString();
            // show keyboard
            HoloeverKeyBoard.Instance.Show(0, new Vector3(0, -1.8f, 2.8f), new Vector3(30, 0, 0));
        }

        public void ShowKeyBoard_AR()
        {
            // 
            Text text = GetComponentInParent<Text>();
            HoloeverKeyBoard.Instance.SetText(text);
            // change keyboard postion or rotation
            HoloeverKeyBoard.Instance.GetKeyBoardTransform();
            // get the input string
            HoloeverKeyBoard.Instance.GetKeyBoardString();
            // show keyboard
            HoloeverKeyBoard.Instance.Show(0, new Vector3(0, -2f, 5f), new Vector3(30, 0, 0));
        }
    }
}