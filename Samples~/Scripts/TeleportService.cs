

using UnityEngine;
using Nxr.Internal;

namespace NXR.Samples
{
    //  
    [RequireComponent(typeof(Collider))]
    public class TeleportService : MonoBehaviour, INxrGazeResponder
    {
        private Vector3 startingPosition;
        private bool gazeAt = false;

        void Start()
        {
            startingPosition = transform.localPosition;
            SetGazedAt(false);

            HoloeverService.OnCameraIdle += CameraIdle;
            HoloeverService.OnCameraBusy += CameraBusy;
        }


        public void SetGazedAt(bool gazedAt)
        {
            gazeAt = gazedAt;
            GetComponent<Renderer>().material.color = gazedAt ? Color.green : Color.white;
        }

        public bool isGazedAt()
        {
            return gazeAt;
        }

        #region INxrGazeResponder implementation

        /// Called when the user is looking on a GameObject with this script,
        /// as long as it is set to an appropriate layer (see NxrGaze).
        public void OnGazeEnter()
        {
            SetGazedAt(true);
        }

        /// Called when the user stops looking on the GameObject, after OnGazeEnter
        /// was already called.
        public void OnGazeExit()
        {
            SetGazedAt(false);
            if (gameObject.name.Equals("ButtonStartRec"))
            {
                Transform btnTransform = gameObject.transform.Find("ApiButtonText");
                LocalizationText localizationText = btnTransform.gameObject.GetComponent<LocalizationText>();
                localizationText.UpdateKey("voice_say_content_2");
            }
        }

        private bool cursorEnabled = false;
        private DISPLAY_MODE curMode = DISPLAY_MODE.MODE_3D;

        public void OnPointerDown()
        {
            HoloeverService holoeverService = NxrViewer.Instance.GetHoloeverService();
            Debug.Log("OnPointerDown : "+gameObject.name);
            // start
            GameObject apiObj = GameObject.Find("apiRoot");
            if (holoeverService != null && apiObj != null && apiObj.transform.Find("ButtonStartRec") != null)
            {
                GameObject startRecObj = apiObj.transform.Find("ButtonStartRec").gameObject;
                if (startRecObj == null) return;
                bool isActive = startRecObj.activeSelf;
                if (isActive && startRecObj.GetComponent<TeleportService>().isGazedAt())
                {
                    holoeverService.StartVoiceRecording();
                }
            }
        }

        public void OnPointerUp()
        {
            HoloeverService holoeverService = NxrViewer.Instance.GetHoloeverService();
            Debug.Log("OnPointerUp : " + gameObject.name);
            // stop
            GameObject apiObj = GameObject.Find("apiRoot");
            if (holoeverService != null && apiObj != null && apiObj.transform.Find("ButtonStartRec") != null)
            {
                GameObject startRecObj = apiObj.transform.Find("ButtonStartRec").gameObject;
                if (startRecObj == null) return;
                bool isActive = startRecObj.activeSelf;
                if (isActive && startRecObj.GetComponent<TeleportService>().isGazedAt())
                {
                    holoeverService.StopVoiceRecording();
                }
            }
        }

        public void OnGazeTrigger()
        {
            HoloeverService holoeverService = NxrViewer.Instance.GetHoloeverService();
            if (holoeverService != null && gameObject.name.Equals("CubeTouchCursor"))
            {
                cursorEnabled = !cursorEnabled;
                holoeverService.SetEnableTouchCursor(cursorEnabled);
            }
            else if (holoeverService != null && gameObject.name.Equals("CubeDisplayMode"))
            {
                curMode = curMode == DISPLAY_MODE.MODE_3D ? DISPLAY_MODE.MODE_2D : DISPLAY_MODE.MODE_3D;
                holoeverService.SetDisplayMode(curMode);
            }
            else if (holoeverService != null && gameObject.name.Equals("CubeBrightnessUp"))
            {
                holoeverService.SetBrightnessValue(holoeverService.GetBrightnessValue() + 1);
            }
            else if (holoeverService != null && gameObject.name.Equals("CubeBrightnessDown"))
            {
                holoeverService.SetBrightnessValue(holoeverService.GetBrightnessValue() - 1);
            }
            else if (gameObject.name.Equals("CubeLanguageCN"))
            {
                NxrViewer.Instance.VoiceLanguage = VOICE_LANGUAGE.CHINESE;

                gameObject.GetComponent<Renderer>().material.color = Color.green;

                GameObject cnObj = GameObject.Find("CubeLanguageEN");
                if (cnObj != null)
                {
                    cnObj.GetComponent<Renderer>().material.color = Color.white;
                }
                // resume
                Transform rootTransform = GameObject.Find("UIRoot").transform;
                rootTransform.Find("CubeNextDemo").transform.gameObject.SetActive(true);

                // refresh
                UpdateTextContent(LocalizationManager.chinese);

                LocalizationManager.GetInstance.ChangeLanguage(LocalizationManager.chinese);
            }
            else if (gameObject.name.Equals("CubeLanguageEN"))
            {
                NxrViewer.Instance.VoiceLanguage = VOICE_LANGUAGE.ENGLISH;
                gameObject.GetComponent<Renderer>().material.color = Color.green;
                GameObject enObj = GameObject.Find("CubeLanguageCN");
                if (enObj != null)
                {
                    enObj.GetComponent<Renderer>().material.color = Color.white;
                }

                if (GetPageNumber() == 2)
                {
                    Transform rootTransform = GameObject.Find("UIRoot").transform;
                    // dismiss
                    rootTransform.Find("CubeNextDemo").transform.gameObject.SetActive(false);
                }

                // refresh
                UpdateTextContent(LocalizationManager.english);

                LocalizationManager.GetInstance.ChangeLanguage(LocalizationManager.english);
            }
            else if (holoeverService != null && gameObject.name.Equals("CameraStart"))
            {
                // camera preview
                hasTrigger = false;
                holoeverService.GetCameraStatus();
            }
            else if (holoeverService != null && gameObject.name.Equals("CameraPause"))
            {
                holoeverService.StopCamereaPreView();
            }
            else if (gameObject.name.Equals("CubeNextDemo"))
            {
                int page = GetPageNumber();
                if (page == 0 || page == 2)
                {
                    UpdateVoiceApiDemoPage(true);
                    // dismiss text
                    Transform rootTransform = GameObject.Find("UIRoot").transform;
                    rootTransform.Find("TextControlTip").gameObject.SetActive(false);
                    rootTransform.Find("TextTip").gameObject.SetActive(false);
                    // dismiss progressbar
                    Transform parentTransform = GameObject.Find("VolumeRoot").transform;
                    parentTransform.Find("VolumeIcon").gameObject.SetActive(false);
                    parentTransform.Find("VolumeProgressBG").gameObject.SetActive(false);
                    parentTransform.Find("VolumeProgressValue").gameObject.SetActive(false);
                    // change en/cn
                    rootTransform.Find("CubeLanguageEN").transform.gameObject.SetActive(true);
                    rootTransform.Find("CubeLanguageCN").transform.gameObject.SetActive(true);
                    return;
                }
                else if (page == 1)
                {
                    UpdateVoiceApiDemoPage(false);
                    // resume text
                    Transform rootTransform = GameObject.Find("UIRoot").transform;
                    rootTransform.Find("TextControlTip").gameObject.SetActive(true);
                    rootTransform.Find("TextTip").gameObject.SetActive(true);
                }

                // next page
                LocalizationText localizationText = GameObject.Find("TextControlTip").GetComponent<LocalizationText>();
                string newKey = localizationText.key == "voice_say_content" ? "voice_say_content_volume" : "voice_say_content";
                localizationText.UpdateKey(newKey);


                //Debug.Log("-------------------:www---------------" + newKey);

                // back 
                LocalizationText localizationTextNxt = GameObject.Find("TextNextDemo").GetComponent<LocalizationText>();
                newKey = localizationTextNxt.key == "next_demo_text" ? "last_demo_text" : "next_demo_text";
                localizationTextNxt.UpdateKey(newKey);

                LocalizationText localizationTextTip = GameObject.Find("TextTip").GetComponent<LocalizationText>();
                newKey = localizationTextTip.key == "voice_tip_content" ? "voice_tip_content_v" : "voice_tip_content";
                localizationTextTip.UpdateKey(newKey);

                if (newKey == "voice_tip_content_v")
                {
                    Transform rootTransform = GameObject.Find("UIRoot").transform;

                    // dismiss en button
                    rootTransform.Find("CubeLanguageEN").transform.gameObject.SetActive(false);
                    rootTransform.Find("CubeLanguageCN").transform.gameObject.SetActive(false);
                    // 
                    // show progressbar
                    Transform parentTransform = GameObject.Find("VolumeRoot").transform;
                    parentTransform.Find("VolumeIcon").gameObject.SetActive(true);
                    parentTransform.Find("VolumeProgressBG").gameObject.SetActive(true);
                    parentTransform.Find("VolumeProgressValue").gameObject.SetActive(true);

                    UpdateVolumeProgress();
                }
                else if (newKey == "voice_tip_content")
                {
                    Transform rootTransform = GameObject.Find("UIRoot").transform;
                    rootTransform.Find("CubeLanguageEN").transform.gameObject.SetActive(true);
                    rootTransform.Find("CubeLanguageCN").transform.gameObject.SetActive(true);
                    //  
                    // dismiss progressbar
                    Transform parentTransform = GameObject.Find("VolumeRoot").transform;
                    parentTransform.Find("VolumeIcon").gameObject.SetActive(false);
                    parentTransform.Find("VolumeProgressBG").gameObject.SetActive(false);
                    parentTransform.Find("VolumeProgressValue").gameObject.SetActive(false);
                }

                page++;
            }
            else if (holoeverService != null && gameObject.name.Equals("CubeStartSensor"))
            {
                holoeverService.RegisterSensorListener(SENSOR_TYPE.ACCELEROMETER, SENSOR_LOCATION.HMD);
                holoeverService.RegisterSensorListener(SENSOR_TYPE.GYROSCOPE, SENSOR_LOCATION.HMD);
                holoeverService.RegisterSensorListener(SENSOR_TYPE.MAGNETIC_FIELD, SENSOR_LOCATION.HMD);
            }
            else if (holoeverService != null && gameObject.name.Equals("CubeStopSensor"))
            {
                holoeverService.UnRegisterSensorListener();
            }
            else if (holoeverService != null && gameObject.name.Equals("ButtonStopRec"))
            {
                //holoeverService.StopVoiceRecording();
            }
            else if (gameObject.name.Equals("ButtonStartRec"))
            {
                //holoeverService.StartVoiceRecording();
                Transform btnTransform = gameObject.transform.Find("ApiButtonText");
                LocalizationText localizationText = btnTransform.gameObject.GetComponent<LocalizationText>();
                localizationText.UpdateKey("voice_say_content_3");
            }
            else if (gameObject.name.Equals("SetIpdBtn"))
            {
                NxrViewer.Instance.SetIpd(0.040f);
            }
            else if (gameObject.name.Equals("ResetIpdBtn"))
            {
                NxrViewer.Instance.ResetIpd();
            }

        }

        private void UpdateVoiceApiDemoPage(bool show)
        {
            Transform parentTransform = GameObject.Find("apiRoot").transform;
            parentTransform.Find("ButtonStartRec").gameObject.SetActive(show);
            parentTransform.Find("ApiTextDesc").gameObject.SetActive(show);
        }

        public int GetPageNumber()
        {
            GameObject textControllObj = GameObject.Find("TextControlTip");
            if (textControllObj != null)
            {
                LocalizationText localizationText = textControllObj.GetComponent<LocalizationText>();
                return localizationText.key == "voice_say_content" ? 0 : 2;
            }
            else
            {
                return 1;
            }
        }

        private void UpdateVolumeProgress()
        {
            if (NxrViewer.Instance.GetHoloeverService() == null) return;
            int cur = NxrViewer.Instance.GetHoloeverService().GetVolumeValue();
            int max = NxrViewer.Instance.GetHoloeverService().GetMaxVolume();

            Transform parentTransform = GameObject.Find("VolumeRoot").transform;
            Transform progressTransform = parentTransform.Find("VolumeProgressValue");
            float progress = cur * 1.0f / max;
            //  scaleX=1.98,position.x=0
            float newPX = -1.98f * (1.0f - progress) / 2;

            progressTransform.localScale = new Vector3(1.98f * progress, progressTransform.localScale.y, progressTransform.localScale.z);
            progressTransform.localPosition = new Vector3(newPX, progressTransform.localPosition.y, progressTransform.localPosition.z);
        }

        bool hasTrigger = false;
        void CameraIdle()
        {
            if (hasTrigger)
            {
                return;
            }
            Debug.Log(".CameraIdle");
            HoloeverService holoeverService = NxrViewer.Instance.GetHoloeverService();
            if (holoeverService != null)
            {
                holoeverService.StartCameraPreView(true);
            }
            hasTrigger = true;
        }

        void CameraBusy()
        {
            Debug.Log(".CameraBusy");
        }

        void UpdateTextContent(string language)
        {
            GameObject textControlTipObj = GameObject.Find("TextControlTip");
            if (textControlTipObj != null) textControlTipObj.GetComponent<LocalizationText>().refresh(language);

            GameObject textVoiceStausObj = GameObject.Find("TextVoiceStaus");
            if (textVoiceStausObj != null) textVoiceStausObj.GetComponent<LocalizationText>().refresh(language);

            GameObject textDecibelValueObj = GameObject.Find("TextDecibelValue");
            if (textDecibelValueObj != null) textDecibelValueObj.GetComponent<LocalizationText>().refresh(language);

            GameObject textTipObj = GameObject.Find("TextTip");
            if (textTipObj != null) textTipObj.GetComponent<LocalizationText>().refresh(language);

            GameObject apiBtnTextObj = GameObject.Find("ApiButtonText");
            if (apiBtnTextObj != null) apiBtnTextObj.GetComponent<LocalizationText>().refresh(language);

            GameObject apiTextDescObj = GameObject.Find("ApiTextDesc");
            if (apiTextDescObj != null) apiTextDescObj.GetComponent<LocalizationText>().refresh(language);

            GameObject nextObject = GameObject.Find("TextNextDemo");
            if (nextObject != null) nextObject.GetComponent<LocalizationText>().refresh(language);
        }

        private void OnDestroy()
        {
            HoloeverService.OnCameraIdle -= CameraIdle;
            HoloeverService.OnCameraBusy -= CameraBusy;
            Debug.Log("TeleportService.OnDestroy");
        }

        public void OnUpdateIntersectionPosition(Vector3 position)
        {
            // Debug.Log("TeleportService.OnUpdateIntersectionPosition=" + position.ToString());
        }
        #endregion
    }
}