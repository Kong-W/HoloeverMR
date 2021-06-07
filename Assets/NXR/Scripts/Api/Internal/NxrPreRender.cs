using UnityEngine;

namespace Nxr.Internal
{
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("NXR/Internal/NxrPreRender")]
    public class NxrPreRender : MonoBehaviour
    {

        public Camera cam { get; private set; }

        void Awake()
        {
            cam = GetComponent<Camera>();
        }

        void Reset()
        {
#if UNITY_EDITOR
            // Member variable 'cam' not always initialized when this method called in Editor.
            // So, we'll just make a local of the same name.
            var cam = GetComponent<Camera>();
#endif
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = Color.black;
            cam.cullingMask = 0;
            cam.useOcclusionCulling = false;
            cam.depth = -100;
            cam.farClipPlane = 100;
            if (NxrGlobal.isVR9Platform)
            {
                cam.clearFlags = CameraClearFlags.Nothing;
            }
        }
    }
}