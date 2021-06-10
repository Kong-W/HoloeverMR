using UnityEngine;

namespace Nxr.Internal
{
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("NXR/Internal/StereoRenderEffect")]
    public class StereoRenderEffect : MonoBehaviour
    {
        private Material material;

        private Camera cam;

#if UNITY_5_6_OR_NEWER
        private Rect fullRect;
        public NxrViewer.Eye eye;
#else
  private static readonly Rect fullRect = new Rect(0, 0, 1, 1);
#endif  // UNITY_5_6_OR_NEWER

        void Awake()
        {
            cam = GetComponent<Camera>();
        }

        void Start()
        {
            material = new Material(Shader.Find("NAR/UnlitTexture"));
#if UNITY_5_6_OR_NEWER
            fullRect = (eye == NxrViewer.Eye.Left ? new Rect(0, 0, 0.5f, 1) : new Rect(0.5f, 0, 0.5f, 1));
#endif
        }

        public void UpdateEye(NxrViewer.Eye eyeTmp)
        {
#if UNITY_5_6_OR_NEWER
            this.eye = eyeTmp;
            fullRect = (eye == NxrViewer.Eye.Left ? new Rect(0, 0, 0.5f, 1) : new Rect(0.5f, 0, 0.5f, 1));
#endif
        }

        void OnRenderImage(RenderTexture source, RenderTexture dest)
        {

            GL.PushMatrix();
            int width = dest ? dest.width : Screen.width;
            int height = dest ? dest.height : Screen.height;
            GL.LoadPixelMatrix(0, width, height, 0);
            // Camera rects are in screen coordinates (bottom left is origin), but DrawTexture takes a
            // rect in GUI coordinates (top left is origin).
            Rect blitRect = cam.pixelRect;
            blitRect.y = height - blitRect.height - blitRect.y;
            RenderTexture oldActive = RenderTexture.active;
            RenderTexture.active = dest;
            Graphics.DrawTexture(blitRect, source, fullRect, 0, 0, 0, 0, Color.white, material);
            RenderTexture.active = oldActive;
            GL.PopMatrix();
        }
    }
}

