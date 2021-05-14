using UnityEngine;
using Nxr.Internal;

namespace NXR.Samples
{
    public class HoloeverVoiceGesture : MonoBehaviour, IHoloeverVoiceListener, IHoloeverGestureListener
    {
        public static string RESULR_MOVE_RIGHT = "向右移动";
        public static string RESULR_MOVE_LEFT = "向左移动";
        public static string RESULR_MOVE_RIGHT_EN = "move right";
        public static string RESULR_MOVE_LEFT_EN = "move left";
        public void OnGesture(GESTURE_ID gestureId)
        {
            Debug.Log("onGesture->" + gestureId);
            if (gestureField != null)
            {
                gestureField.text = "[" + gestureId + "]";
            }
        }

        public void OnVoiceBegin()
        {
            Debug.Log("onVoiceBegin");
            // 
            playVolumeAnim.PlayMode = PlayVolumeAnim.PLAY_MODE.BEGIN;
            if (voiceStatusField != null)
            {
                voiceStatusField.text = LocalizationManager.GetInstance.GetValue("voice_status_start");
            }
        }

        public void OnVoiceEnd()
        {
            Debug.Log("onVoiceEnd");
            playVolumeAnim.PlayMode = PlayVolumeAnim.PLAY_MODE.RECOGNIZE;
            if (voiceStatusField != null)
            {
                voiceStatusField.text = LocalizationManager.GetInstance.GetValue("voice_status_finish");
            }
        }

        public void OnVoiceFinishResult(string param)
        {
            Debug.Log("onVoiceFinishResult->" + param);
            playVolumeAnim.PlayMode = PlayVolumeAnim.PLAY_MODE.END;
            if (voiceStatusField != null)
            {
                voiceStatusField.text = LocalizationManager.GetInstance.GetValue("voice_status_result") + "[" + param + "]";
            }

            if (RESULR_MOVE_RIGHT.Equals(param) || param.Contains(RESULR_MOVE_RIGHT_EN))
            {
                // move to right
                VoiceAnim.transform.Translate(new Vector3(1, 0, 0));
            }
            else if (RESULR_MOVE_LEFT.Equals(param) || param.Contains(RESULR_MOVE_LEFT_EN))
            {
                // move to left
                VoiceAnim.transform.Translate(new Vector3(-1, 0, 0));
            }

            if (param.Contains("音量"))
            {
                UpdateVolumeProgress();
            }
        }

        private void UpdateVolumeProgress()
        {
            int cur = NxrViewer.Instance.GetHoloeverService().GetVolumeValue();
            int max = NxrViewer.Instance.GetHoloeverService().GetMaxVolume();

            Transform parentTransform = GameObject.Find("VolumeRoot").transform;
            Transform progressTransform = parentTransform.Find("VolumeProgressValue");
            float progress = cur * 1.0f / max;
            // Debug.Log("progress="+progress);
            // scaleX=1.98,position.x=0
            float newPX = -1.98f * (1.0f - progress) / 2;
            progressTransform.localPosition = new Vector3(newPX, progressTransform.localPosition.y, progressTransform.localPosition.z);
            progressTransform.localScale = new Vector3(1.98f * progress, progressTransform.localScale.y, progressTransform.localScale.z);
        }

        public void OnVoiceFinishError(string errorMsg)
        {
            Debug.Log("OnVoiceFinishError." + errorMsg);
            playVolumeAnim.PlayMode = PlayVolumeAnim.PLAY_MODE.NONE;
            if (voiceStatusField != null)
            {
                voiceStatusField.text = LocalizationManager.GetInstance.GetValue("voice_status_error");
            }
        }

        private int curVolume;
        private float lastUpdateTime = 0;
        /// <summary>
        /// 
        ///  0~100
        /// </summary>
        /// <param name="volume"></param>
        public void OnVoiceVolume(string volume)
        {
            if (lastUpdateTime == 0)
            {
                lastUpdateTime = Time.realtimeSinceStartup;
            }

            // Triggered once every 5 seconds to avoid meaningless consumption
            float systime = Time.realtimeSinceStartup;
            if (systime - lastUpdateTime < 5) return;

            // Debug.Log("onVoiceVolume->" + volume);
            int volumeValue = int.Parse(volume);
            if (curVolume != volumeValue)
            {
                curVolume = volumeValue;
                devibelValueField.text = LocalizationManager.GetInstance.GetValue("decibel_value") + volume;
            }
        }

        private TextMesh gestureField, voiceStatusField, devibelValueField;
        public GameObject BtnEnglish;
        public GameObject BtnChinese;
        public GameObject TextVoiceStatus;
        public GameObject TextControlTip;
        public GameObject TextDevibelValue;
        public GameObject VoiceAnim;
        PlayVolumeAnim playVolumeAnim;

        HoloeverService holoeverService;
        // Use this for initialization
        void Start()
        {
            holoeverService = NxrViewer.Instance.GetHoloeverService();
            // 注册回调接口
            NxrViewer.Instance.RegisterGestureListener(gameObject);
            NxrViewer.Instance.RegisterVoiceListener(gameObject);

            GameObject gestureObj = GameObject.Find("GestureValue");
            gestureField = gestureObj == null ? null : gestureObj.GetComponent<TextMesh>();

            voiceStatusField = TextVoiceStatus == null ? null : TextVoiceStatus.GetComponent<TextMesh>();
            devibelValueField = TextDevibelValue == null ? null : TextDevibelValue.GetComponent<TextMesh>();

            playVolumeAnim = VoiceAnim == null ? null : VoiceAnim.GetComponent<PlayVolumeAnim>();

            SystemLanguage systemLan = Application.systemLanguage;
            Debug.Log("SystemLanguage=" + systemLan.ToString() + "," + (BtnChinese == null));


            if (BtnChinese != null && (Application.systemLanguage == SystemLanguage.ChineseSimplified || Application.systemLanguage == SystemLanguage.Chinese))
            {
                // System language Chinese
                BtnChinese.GetComponent<Renderer>().material.color = Color.green;
                BtnEnglish.GetComponent<Renderer>().material.color = Color.white;
            }
            else if (BtnChinese != null)
            {
                BtnEnglish.GetComponent<Renderer>().material.color = Color.green;
                BtnChinese.GetComponent<Renderer>().material.color = Color.white;
                //  
                GameObject.Find("CubeLanguageEN").SetActive(false);
            }

            if (holoeverService != null && gameObject.name.Equals("CubeGesture"))
            {
                // Turn on gestures
                holoeverService.EnableGestureService(true);
            }
        }

        private void OnDestroy()
        {
            if (holoeverService != null && gameObject.name.Equals("CubeGesture"))
            {
                // Close gestures
                // nibiruService.EnableGestureService(false);
            }
        }

        public void OnVoiceCancel()
        {
            Debug.Log("OnVoiceCancel");
            if (voiceStatusField != null)
            {
                voiceStatusField.text = LocalizationManager.GetInstance.GetValue("voice_status_cancel");
            }
        }
    }
}