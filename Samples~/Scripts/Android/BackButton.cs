using Nxr.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;
using HoloeverTask;

namespace NXR.Samples
{
    //  
    [RequireComponent(typeof(Collider))]
    public class BackButton : MonoBehaviour, INxrGazeResponder
    {
        private bool gazeAt = false;
        public void OnGazeEnter()
        {
            SetGazedAt(true);
        }

        public void OnGazeExit()
        {
            SetGazedAt(false);
        }

        public void OnGazeTrigger()
        {
            if (gameObject.name.Equals("ReqPermissionButton"))
            {
                if (NxrViewer.Instance.GetHoloeverService() == null) return;
                NxrViewer.Instance.GetHoloeverService().RequsetPermission(new string[] {
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
                return;
            }

            // Return the current Active Scene in order to get the current Scene name.
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name.Equals("CameraTeleportScene") || scene.name.Equals("ControllerDragScene") || scene.name.Equals("InputKeyScene") || scene.name.Equals("CustomCtrlScene"))
            {
                SceneManager.LoadScene("FirstScene");
            }
            else if (scene.name.Equals("FirstScene"))
            {
                if (Application.isMobilePlatform)
                {
                    Application.Quit();
                }
            }
            else if (scene.name.Equals("VR_DemoScene2") || scene.name.Equals("VR_DemoUIScene"))
            {
                SceneManager.LoadScene("VR_DemoScene");
            }
            else if (scene.name.Equals("VR_DemoScene") || scene.name.Equals("AR_DemoScene"))
            {
                SceneManager.LoadScene("FirstScene");
            }
            else if (scene.name.Equals("AR_DemoScene2") || scene.name.Equals("AR_DemoUIScene") || scene.name.Equals("CameraScene")
                || scene.name.Equals("GestureScene") || scene.name.Equals("MarkerScene") || scene.name.Equals("RecognizeScene")
                 || scene.name.Equals("RecordScene") || scene.name.Equals("ServiceScene") || scene.name.Equals("SystemApi") || scene.name.Equals("VoiceScene") ||
                 scene.name.Equals("SixdofScene"))
            {
                SceneManager.LoadScene("AR_DemoScene");
            }
        }

        public void OnUpdateIntersectionPosition(Vector3 position)
        {
             
        }

        // Start is called before the first frame update
        void Start()
        {
            SetGazedAt(false);
        }

        public void SetGazedAt(bool gazedAt)
        {
            gazeAt = gazedAt;
            Material mat = GetComponent<Renderer>().material;
            Color color = gazedAt ? Color.green : Color.white;
            mat.color = color;
            mat.SetColor("_BaseColor", color);
        }

        private void Update()
        {
            if (NxrInput.GetKeyUp(CKeyEvent.KEYCODE_BACK) || NxrInput.GetControllerKeyUp(CKeyEvent.KEYCODE_CONTROLLER_MENU)
               || NxrInput.GetControllerKeyUp(CKeyEvent.KEYCODE_CONTROLLER_MENU, InteractionManager.NACTION_HAND_TYPE.HAND_LEFT)
               || NxrInput.GetControllerKeyUp(CKeyEvent.KEYCODE_CONTROLLER_MENU, InteractionManager.NACTION_HAND_TYPE.HAND_RIGHT))
            {
                // 返回键
                Debug.Log("Update->OnGazeTrigger");
                OnGazeTrigger();
            }
 
        }
    }
}
