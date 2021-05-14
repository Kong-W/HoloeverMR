
using UnityEngine;
using UnityEngine.UI;
using HoloeverTask;
namespace NXR.Samples
{
    public class SystemApiScript : MonoBehaviour
    {

        Text pathText;
        Text InfoText;

        string path = "";
        string info = "";
        // Use this for initialization
        void Start()
        {

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

        public void onSelectionResult(AndroidJavaObject task)
        {
            path = HoloeverTaskApi.GetResultPathFromSelectionTask(task);

        }

        // Update is called once per frame
        void Update()
        {
            if (pathText != null)
            {
                pathText.text = "GethFilePath: " + path;
            }
            if (InfoText != null)
            {
                InfoText.text = "SystenInfo: " + info;
            }
        }

        public void PointerEnter()
        {
            Debug.Log("pointer enter");
        }

        public void PointerExit()
        {
            Debug.Log("pointer exit");
        }

        public void OpenVideoPlayer()
        {
            //NvrViewer.Instance.OpenVideoPlayer(NvrViewer.Instance.GetStoragePath() + "/nibiru.mp4", 0, 2, 1);
            HoloeverTaskApi.OpenVideoPlayer("sdcard/nibiru.mp4");
        }

        public void GetSystemInfo()
        {
            info = "GetVRVersion:" + HoloeverTaskApi.GetVRVersion() + "\n"
                + "GetOSVersion:" + HoloeverTaskApi.GetOSVersion() + "\n"
                + "GetSysSleepTime:" + HoloeverTaskApi.GetSysSleepTime() + "\n"
                + "GetCurrentLanguage:" + HoloeverTaskApi.GetCurrentLanguage() + "\n"
                + "GetCurrentTimezone:" + HoloeverTaskApi.GetCurrentTimezone() + "\n"
                + "GetDeviceName:" + HoloeverTaskApi.GetDeviceName() + "\n";

        }

        public void OpenExplorer()
        {
            HoloeverTaskApi.OpenBrowerExplorer("http://www.inibiru.com");
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

    }
}