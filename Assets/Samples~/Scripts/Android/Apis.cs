using HoloeverTask;
using UnityEngine;

namespace NXR.Samples
{
    public class Apis : MonoBehaviour {
    public TextMesh macAddress;
    public TextMesh deviceId;
    public TextMesh sixDofStatus;

    public void LaunchSDKDemo()
    {
        Debug.Log("LaunchSDKDemo");
        HoloeverTaskApi.LaunchAppByPkgName("com.nibiru.vr.lib2.test");
    }

	// Use this for initialization
	void Start () {
        if(macAddress != null) macAddress.text = "MacAddress: " + HoloeverTaskApi.GetMacAddress();
        if (deviceId != null) deviceId.text = "DeviceId: " + HoloeverTaskApi.GetDeviceId();
        if (sixDofStatus != null) sixDofStatus.text = "SixDof Plugin Status: [Declared " + HoloeverTaskApi.IsPluginDeclared(Nxr.Internal.PLUGIN_ID.SIX_DOF)
            + "], [Suppored " + HoloeverTaskApi.IsPluginSupported(Nxr.Internal.PLUGIN_ID.SIX_DOF) + "]";
    }
	
 
}


}