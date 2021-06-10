using UnityEngine;
namespace Nxr.Internal
{
    public class HoloeverVirtualMouse : MonoBehaviour
    {
        HoloeverService holoeverService;
        // Use this for initialization
        void Start()
        {
            holoeverService = NxrViewer.Instance.GetHoloeverService();
            if (holoeverService != null)
            {
                holoeverService.RegisterVirtualMouseService(OnServiceConnected);
            }
        }

        void OnServiceConnected(bool succ)
        {
            // when service is connected, succ = true, call the api SetEnableVirtualMouse to show/dismiss virtual mouse;
            // holoeverService.SetEnableVirtualMouse(true);
            Debug.Log("------------VirtualMouse Service Connected : " + succ);
        }

        private void OnDestroy()
        {
            if (holoeverService != null)
            {
                holoeverService.UnRegisterVirtualMouseService();
                Debug.Log("HoloeverVirtualMouse.OnDestroy");
            }
        }
    }
}
