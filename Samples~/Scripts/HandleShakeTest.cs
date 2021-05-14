using UnityEngine;
using Nxr.Internal;

namespace NXR.Samples
{
    public class HandleShakeTest : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            NxrHandleShake.init();
        }

        // Update is called once per frame
        void Update()
        {
            //参数解释：
            //left int 型，取值范围 0~255，代表左马达震动强度
            //right int 型，取值范围 0~255，代表右马达震动强度
            NxrHandleShake.startVibrate(255, 255);
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                NxrHandleShake.cancelVibrate();
            }
        }

        void OnDestroy()
        {
            NxrHandleShake.destroy();
            Debug.Log("NxrHandleShake.OnDestroy");
        }

    }
}