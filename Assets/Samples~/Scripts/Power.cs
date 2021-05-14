

using HoloeverTask;
using UnityEngine;
namespace NXR.Samples
{
    [RequireComponent(typeof(TextMesh))]
    public class Power : MonoBehaviour
    {
        private TextMesh textField;
        private double power = 0;
        private bool IsNeedRefreshStatus = true;
        private void Start()
        {
            HoloeverTaskApi.addOnPowerChangeListener(onPowerChanged);


            UpdateBluetoothAndNetwordStatus();
        }

        void UpdateBluetoothAndNetwordStatus()
        {
            if (!IsNeedRefreshStatus) return;
            IsNeedRefreshStatus = false;
            // 0=off, 1=on
            int networkStatus = HoloeverTaskApi.GetNetworkStatus();
            int bluetoothStatus = HoloeverTaskApi.GetBluetoothStatus();
            GameObject.Find("Bluetooth").GetComponent<TextMesh>().text = "Bluetooth: " + (bluetoothStatus == 1 ? "on" : "off");
            GameObject.Find("Network").GetComponent<TextMesh>().text = "Network: " + (networkStatus == 1 ? "on" : "off");
        }

        public void onPowerChanged(double value)
        {
            power = value;

        }

        void Awake()
        {
            textField = GetComponent<TextMesh>();

            //// change keyboard postion or rotation
            // NibiruKeyBoard.Instance.keyBoardTransform.Rotate(new Vector3(30, 0, 0));
            // // show keyboard
            // NibiruKeyBoard.Instance.Show();
        }


        void Update()
        {

            if (textField != null)
            {
                textField.text = "Power:" + ((int)(power * 100)) + "%";
            }

            UpdateBluetoothAndNetwordStatus();
        }

        private void OnDestroy()
        {
            HoloeverTaskApi.removeOnPowerChangeListener(onPowerChanged);
            Debug.Log("Power.OnDestroy");
        }

        private void OnApplicationPause(bool pause)
        {
            Debug.Log("Power-OnApplicationPause." + pause);
            IsNeedRefreshStatus = !pause;
        }

    }
}