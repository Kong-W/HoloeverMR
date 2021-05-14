

using UnityEngine;

/// Clears the entire screen.  This script and NxrPostRender work together
/// to draw the whole screen in XR Mode.  There should be exactly one of each
/// component in any NXR-enabled scene.  It is part of the _NxrCamera_
/// prefab, which is included in _NxrMain_.  The NxrViewer script will
/// create one at runtime if the scene doesn't already have it, so generally
/// it is not necessary to manually add it unless you wish to edit the _Camera_
/// component that it controls.
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