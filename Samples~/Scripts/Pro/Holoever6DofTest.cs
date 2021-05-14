using UnityEngine;
using System;
using UnityEngine.UI;
using Nxr.Internal;

namespace NXR.Samples
{
    public class Holoever6DofTest : MonoBehaviour
    {

        Text positionText;

        // Use this for initialization
        void Start()
        {
            NxrViewer.onSixDofPosition += OnHeadPosition;
            positionText = gameObject.GetComponent<Text>();
        }

        private void Update()
        {
            if (positionText != null)
            {
                Vector3 pos = NxrViewer.Instance.HeadPose.Position;
                string positionInfo = "Position " + Math.Round(pos.x, 2) + "," + Math.Round(pos.y, 2) + "," + Math.Round(pos.z, 2);
                positionText.text = positionInfo;
            }
        }

        void OnHeadPosition(float x, float y, float z)
        {
            if (positionText != null)
            {
                string positionInfo = "Position " + Math.Round(x, 2) + "," + Math.Round(y, 2) + "," + Math.Round(z, 2);
                positionText.text = positionInfo;
            }
        }

        private void OnDestroy()
        {
            NxrViewer.onSixDofPosition -= OnHeadPosition;
            Debug.Log("Holoever6DofTest.OnDestroy");
        }
    }
}