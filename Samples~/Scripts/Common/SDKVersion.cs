using UnityEngine;

namespace NXR.Samples {
[RequireComponent(typeof(TextMesh))]
public class SDKVersion : MonoBehaviour {
    TextMesh textMesh;

	// Use this for initialization
	void Start () {
        textMesh = GetComponent<TextMesh>();
        textMesh.text = "V" + Nxr.Internal.NxrViewer.NXR_SDK_VERSION;
	}

    }
}