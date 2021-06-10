using UnityEngine;
namespace Nxr.Internal
{
    public class FpsStatistics : MonoBehaviour
    {
        HoloeverService mHoloeverService;
        TextMesh textMesh;
        // Use this for initialization
        void Start()
        {
            textMesh = GetComponent<TextMesh>();
            if (NxrViewer.Instance.ShowFPS)
            {
                mHoloeverService = NxrViewer.Instance.GetHoloeverService();
                if(mHoloeverService != null)
                {
                    mHoloeverService.SetEnableFPS(true);
                }
            } else
            {
                Debug.Log("Display FPS is disabled.");
            }

            Debug.Log("TrackerPosition=" + NxrViewer.Instance.TrackerPosition);
        }

        // Update is called once per frame
        void Update()
        {
            if(mHoloeverService != null)
            {
                float[] fps = mHoloeverService.GetFPS();
                textMesh.text = "FBO " + fps[0] + ", DTR " + fps[1];
            }
        }
    }
}