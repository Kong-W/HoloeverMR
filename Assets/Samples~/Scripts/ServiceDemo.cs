using Nxr.Internal;
using UnityEngine;
namespace NXR.Samples
{
    public class ServiceDemo : MonoBehaviour
    {
        HoloeverService holoeverService;
        // Use this for initialization
        TextMesh textMesh_DriverVersion;
        TextMesh textMesh_VendorVersion;
        TextMesh textMesh_Light;
        TextMesh textMesh_Proximity;
        TextMesh textMesh_Brightness;
        TextMesh textMesh_SensorData;

        void Start()
        {
            holoeverService = NxrViewer.Instance.GetHoloeverService();

            if (holoeverService == null)
            {
                Debug.Log("nibiruService is null >>>>>>>>>>>>>>>>>>");
            }
            HoloeverService.OnSensorDataChangedHandler += onSensorDataChanged;

            Debug.Log("----------------nibiruService is Start----------------");

        }

        void onSensorDataChanged(HoloeverSensorEvent sensorEvent)
        {
            // sensorEvent.printLog();
            if (textMesh_SensorData != null)
            {
                textMesh_SensorData.text = sensorEvent.sensorType.ToString() + "\nx="
                    + sensorEvent.x + ",\ny=" + sensorEvent.y + ",\nz=" + sensorEvent.z + "\n timestamp=" + sensorEvent.timestamp;
            }
        }


        bool updateOnce = false;
        int updateCount = 0;
        // Update is called once per frame
        void Update()
        {
            updateCount++;
            if (textMesh_SensorData == null)
            {
                GameObject Obj = GameObject.Find("TextSensorData");
                if (Obj != null)
                {
                    textMesh_SensorData = Obj.GetComponent<TextMesh>();
                    Debug.Log("find TextSensorData");
                    return;
                }
            }

            if (textMesh_DriverVersion == null)
            {
                GameObject Obj = GameObject.Find("DriverVersion");
                if (Obj != null)
                {
                    textMesh_DriverVersion = Obj.GetComponent<TextMesh>();
                    Debug.Log("find DriverVersion");
                    return;
                }
            }

            if (textMesh_VendorVersion == null)
            {
                GameObject Obj = GameObject.Find("VendorSwVersion");
                if (Obj != null)
                {
                    textMesh_VendorVersion = Obj.GetComponent<TextMesh>();
                    Debug.Log("find VendorSwVersion");
                    return;
                }
            }

            if (textMesh_Light == null)
            {
                GameObject Obj = GameObject.Find("LightValue");
                if (Obj != null)
                {
                    textMesh_Light = Obj.GetComponent<TextMesh>();
                    Debug.Log("find LightValue");
                    return;
                }
            }

            if (textMesh_Proximity == null)
            {
                GameObject Obj = GameObject.Find("ProximityValue");
                if (Obj != null)
                {
                    textMesh_Proximity = Obj.GetComponent<TextMesh>();
                    Debug.Log("find ProximityValue");
                    return;
                }
            }

            if (textMesh_Brightness == null)
            {
                GameObject Obj = GameObject.Find("BrightnessValue");
                if (Obj != null)
                {
                    textMesh_Brightness = Obj.GetComponent<TextMesh>();
                    Debug.Log("find BrightnessValue");
                    return;
                }
            }


            if (holoeverService != null && !updateOnce)
            {
                // Avoid frequent calls
                updateOnce = true;

                Debug.Log("----------------------------------------Service-------------");
                textMesh_DriverVersion.text = "Driver board software version:" + holoeverService.GetVendorSWVersion();
                textMesh_VendorVersion.text = "System version:" + holoeverService.GetModel() + "," + holoeverService.GetOSVersion();
                textMesh_Brightness.text = "Screen brightness:" + holoeverService.GetBrightnessValue();
                textMesh_Light.text = "Light sensor:" + holoeverService.GetLightValue();
                textMesh_Proximity.text = "Distance sensor:" + holoeverService.GetProximityValue();


            }

            // 1s1次
            if (updateCount > 60 && textMesh_Brightness != null && holoeverService != null)
            {
                updateCount = 0;
                textMesh_Brightness.text = "Screen brightness:" + holoeverService.GetBrightnessValue();
            }
        }
    }
}