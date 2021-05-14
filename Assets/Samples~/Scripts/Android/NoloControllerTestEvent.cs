using Nxr.Internal;
using UnityEngine;
namespace NXR.Samples
{
    public class NoloControllerTestEvent : MonoBehaviour
    { 
        public TextMesh textMesh_Controller; 
        
        public void onMenuDown(NxrInstantNativeApi.HoloeverDeviceType deviceType)
        {
            textMesh_Controller.text = "onMenuDown." + deviceType;
        }
        public void onMenuUp(NxrInstantNativeApi.HoloeverDeviceType deviceType)
        {
            textMesh_Controller.text = "onMenuUp." + deviceType.ToString();
        }

        public void onSystemDown(NxrInstantNativeApi.HoloeverDeviceType deviceType)
        {
            textMesh_Controller.text = "onSystemDown." + deviceType;
        }
        public void onSystemUp(NxrInstantNativeApi.HoloeverDeviceType deviceType)
        {
            textMesh_Controller.text = "onSystemUp." + deviceType.ToString();
        }

        public void onTriggerDown(NxrInstantNativeApi.HoloeverDeviceType deviceType)
        {
            textMesh_Controller.text = "onTriggerDown." + deviceType.ToString();
        }
        public void onTriggerUp(NxrInstantNativeApi.HoloeverDeviceType deviceType)
        {
            textMesh_Controller.text = "onTriggerUp." + deviceType.ToString();
        }

        public void onGripDown(NxrInstantNativeApi.HoloeverDeviceType deviceType)
        {
            textMesh_Controller.text = "onGripDown." + deviceType.ToString();
        }

        public void onGripUp(NxrInstantNativeApi.HoloeverDeviceType deviceType)
        {
            textMesh_Controller.text = "onGripUp." + deviceType.ToString();
        }

        public void onTouchpadDown(NxrInstantNativeApi.HoloeverDeviceType deviceType)
        {
            textMesh_Controller.text = "onTouchpadDown." + deviceType.ToString();
        }

        public void onTouchpadUp(NxrInstantNativeApi.HoloeverDeviceType deviceType)
        {
            textMesh_Controller.text = "onTouchpadUp." + deviceType.ToString();
        }

        public void onTouchpadTouch(NxrInstantNativeApi.HoloeverDeviceType deviceType)
        {
            textMesh_Controller.text = "onTouchpadTouch." + deviceType.ToString();
        }

        public void onTouchpadRelease(NxrInstantNativeApi.HoloeverDeviceType deviceType)
        {
            textMesh_Controller.text = "onTouchpadRelease." + deviceType.ToString();
        }

        public void onTouchPadPosition(NxrInstantNativeApi.HoloeverDeviceType deviceType, float x, float y)
        {
            textMesh_Controller.text = "onTouchpadPosition." + x + "," + y + "." + deviceType.ToString();
        }

      
    }
}