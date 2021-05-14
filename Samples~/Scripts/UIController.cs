using UnityEngine;
using UnityEngine.SceneManagement;
using Nxr.Internal;

namespace NXR.Samples
{
    [RequireComponent(typeof(Collider))]
    public class UIController : MonoBehaviour, INxrGazeResponder
    {


        public void SetGazedAt(bool gazedAt)
        {
            GetComponent<Renderer>().material.color = gazedAt ? Color.green : Color.red;
        }

        public void OnGazeEnter()
        {
            Debug.Log("OnGazeEnter::" + gameObject.name);
            SetGazedAt(true);
        }

        public void OnGazeExit()
        {
            Debug.Log("OnGazeExit::" + gameObject.name);
            SetGazedAt(false);
        }

        public void OnGazeTrigger() { }

        public void OnGazeTriggerII(int hmdType)
        {
            Debug.Log("OnGazeTrigger::" + gameObject.name);
            gameObject.SetActive(false);
            SceneManager.LoadScene(hmdType == (int) HMD_TYPE.AR ? "AR_DemoScene" : "VR_DemoScene");
        }

        public void OnUpdateIntersectionPosition(Vector3 position)
        {
            // Debug.Log("UIController.OnUpdateIntersectionPosition=" + position.ToString());
        }
    }
}