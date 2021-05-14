

using UnityEngine;
using Nxr.Internal;

namespace NXR.Samples
{
    //  
    [RequireComponent(typeof(Collider))]
    public class Teleport : MonoBehaviour, INxrGazeResponder
    {
        private Vector3 startingPosition;
        private bool gazeAt = false;

        void Start()
        {
            startingPosition = transform.localPosition;
            SetGazedAt(false);
        }

        public void SetGazedAt(bool gazedAt)
        {
            gazeAt = gazedAt;
            Material mat = GetComponent<Renderer>().material;
            Color color = gazedAt ? Color.green : Color.red;
            mat.color = color;
            mat.SetColor("_BaseColor", color);
        }

        public void Reset()
        {
            transform.localPosition = startingPosition;
        }

        public void ToggleSplitMode()
        {
            NxrViewer.Instance.SplitScreenModeEnabled = !NxrViewer.Instance.SplitScreenModeEnabled;
        }

        public void ToggleDirectRender()
        {

        }

        public void TeleportRandomly()
        {
            Vector3 direction = UnityEngine.Random.onUnitSphere;
            direction.y = Mathf.Clamp(direction.y, 0.5f, 1f);
            float distance = 2 * UnityEngine.Random.value + 1.5f;
            transform.localPosition = direction * distance;
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
        }

        /// Called when the viewer's trigger is used, between OnGazeEnter and OnGazeExit.
        public void OnGazeTrigger()
        {
            TeleportRandomly();
        }

        public void OnUpdateIntersectionPosition(Vector3 position)
        {
            // Debug.Log("Teleport.OnUpdateIntersectionPosition=" + position.ToString());
        }

        #endregion
    }
}