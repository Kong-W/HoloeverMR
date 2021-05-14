using Nxr.Internal;
using UnityEngine;
namespace NXR.Samples
{
    public class MarkerTestScript : MonoBehaviour
    {

        HoloeverService holoeverService;
        // Use this for initialization
        void Start()
        {
            holoeverService = NxrViewer.Instance.GetHoloeverService();
            HoloeverMarker.OnMarkerFoundHandler = MarkerFound;
            HoloeverMarker.OnMarkerLostHandler = MarkerLost;
            if (holoeverService != null)
            {
                holoeverService.StartMarkerRecognize();
            }
        }

        void MarkerFound()
        {
            Debug.Log("MarkerTestScript->onMarkerFound");
        }

        void MarkerLost()
        {
            Debug.Log("MarkerTestScript->onMarkerLost");
        }

        private void OnApplicationPause(bool pause)
        {
            Debug.LogError("Marker-OnApplicationPause=" + pause);
            if (pause && holoeverService != null)
            {
                holoeverService.StopMarkerRecognize();
            }
            else if (holoeverService != null)
            {
                holoeverService.StartMarkerRecognize();
            }
        }

        private void OnDestroy()
        {
            if (holoeverService != null && holoeverService.IsMarkerRecognizeRunning)
            {
                holoeverService.StopMarkerRecognize();
                Debug.Log("MarkerTestScript.OnDestroy");
            }
        }

        private void OnApplicationQuit()
        {
            if (holoeverService != null && holoeverService.IsMarkerRecognizeRunning)
            {
                holoeverService.StopMarkerRecognize();
                Debug.LogError("Marker-StopMarkerRecognize");
            }
            Debug.LogError("Marker-OnApplicationQuit");
        }

    }
}