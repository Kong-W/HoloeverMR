using Nxr.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NXR.Samples
{
    public class SixdofTest : MonoBehaviour
    {
        public TextMesh PositionText;
        public TextMesh RotationText;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            NxrHead head = NxrViewer.Instance.GetHead();
            if(head != null)
            {
                PositionText.text = "Pos:" + Math.Round(head.transform.position.x,2) + "," + Math.Round(head.transform.position.y,2) + "," + Math.Round(head.transform.position.z,2);
                RotationText.text = "Rot:" + Math.Round(head.transform.eulerAngles.x,2) + "," + Math.Round(head.transform.eulerAngles.y,2) + "," + Math.Round(head.transform.eulerAngles.z,2);
            }
        }
    }
}