using UnityEngine;
using UnityEngine.SceneManagement;
using Nxr.Internal;

namespace NXR.Samples
{
    public class FirstScene : MonoBehaviour
    {
        public void JumpToDragScene()
        {
            SceneManager.LoadScene("ControllerDragScene");
        }

        public void JumpToTeleportScene()
        {
            SceneManager.LoadScene("CameraTeleportScene");
        }

        public void JumpToVRScene()
        {
            SceneManager.LoadScene("VR_DemoScene");
        }

        public void JumpToARScene()
        {
            SceneManager.LoadScene("AR_DemoScene");
        }

        public void JumpToKeyEventScene()
        {
            SceneManager.LoadScene("InputKeyScene");
        }

        public void JumpToCustomCtrlScene()
        {
            SceneManager.LoadScene("CustomCtrlScene");
        }

        private GameObject gazeObject;

        void OnGazeEvent(GameObject targetObject)
        {
            gazeObject = targetObject;
            // Debug.Log("OnGazeEvent->" + targetObject.name);
        }

        // Use this for initialization
        void Start()
        {
            NxrOverrideSettings.OnGazeEvent += OnGazeEvent;
        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            if (gazeObject != null)
            {
                Vector3 start = NxrViewer.Instance.GetHead().transform.position;
                float zLength = gazeObject.transform.position.z - start.z;
                Vector3 vector = NxrViewer.Instance.GetHead().transform.TransformDirection(Vector3.forward);
                UnityEngine.Debug.DrawRay(start, vector * zLength, Color.red);
            }
#endif
        }

        private void OnDestroy()
        {
            NxrOverrideSettings.OnGazeEvent = OnGazeEvent;
            Debug.Log("FirstScene.OnDestroy");
        }
    }
}