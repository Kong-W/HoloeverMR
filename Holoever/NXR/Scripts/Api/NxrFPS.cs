
using UnityEngine;
//Frame rate printing.Just mount on any object.
namespace Nxr.Internal
{
    public class NxrFPS : MonoBehaviour
    {

        private string fpsFormat;
        private float updateInterval = 0.2f;//设定更新帧率的时间间隔为0.2秒  
        private float accum = .0f;
        private int frames = 0;
        private float timeLeft;
        public static float fpsDeltaTime;

        TextMesh textMesh;
        // Use this for initialization
        void Start()
        {
            textMesh = GetComponent<TextMesh>();
        }

        // Update is called once per frame
        void Update()
        {
            calculate_fps();
            fpsDeltaTime += Time.deltaTime;
            if (fpsDeltaTime > 1)
            {
                //Debug.Log(fpsFormat);
                fpsDeltaTime = 0;
                if (textMesh != null)
                {
                    textMesh.text = fpsFormat;
                }
            }

        }

        private void calculate_fps()
        {
            timeLeft -= Time.deltaTime;
            accum += Time.timeScale / Time.deltaTime;
            ++frames;

            if (timeLeft <= 0)
            {
                float fps = accum / frames;
                fpsFormat = System.String.Format("{0:F3}fps", fps);
                // Debug.Log("FPS:" + fpsFormat);
                timeLeft = updateInterval;
                accum = .0f;
                frames = 0;
            }
        }
    }
}