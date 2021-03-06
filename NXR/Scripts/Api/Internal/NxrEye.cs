using UnityEngine;
using UnityEngine.Rendering;

namespace Nxr.Internal
{
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("NXR/Internal/NxrEye")]
    public class NxrEye : MonoBehaviour
    {
        public delegate void OnPostRenderCallback(int cacheTextureId, NxrViewer.Eye eyeType);
        public OnPostRenderCallback OnPostRenderListener;

        public delegate void OnPreRenderCallback(int cacheTextureId, NxrViewer.Eye eyeType);
        public OnPreRenderCallback OnPreRenderListener;


        /// Whether this is the left eye or the right eye.
        /// Determines which stereo eye to render, that is, which `EyeOffset` and
        /// `Projection` matrix to use and which half of the screen to render to.
        public NxrViewer.Eye eye;

        /// The StereoController in charge of this eye (and whose mono camera
        /// we will copy settings from).
        public NxrStereoController Controller
        {
            // This property is set up to work both in editor and in player.
            get
            {
                if (transform.parent == null)
                { // Should not happen.
                    return null;
                }
                if ((Application.isEditor && !Application.isPlaying) || controller == null)
                {
                    // Go find our controller.
                    controller = transform.parent.GetComponentInParent<NxrStereoController>();
                    if (controller == null)
                    {
                        controller = FindObjectOfType<NxrStereoController>();
                    }
                }
                return controller;
            }
            set
            {
                controller = value;
            }
        }

        private NxrStereoController controller;
        private StereoRenderEffect stereoEffect;
        private Camera monoCamera;

        // Convenient accessor to the camera component used throughout this script.
        public Camera cam { get; private set; }

        public Transform cacheTransform;
        void Awake()
        {
            cam = GetComponent<Camera>();
        }

        void Start()
        {
            var ctlr = Controller;
            if (ctlr == null)
            {
                Debug.LogError("NxrEye must be child of a StereoController.");
                enabled = false;
                return;
            }
            // Save reference to the found controller and it's camera.
            controller = ctlr;
            monoCamera = controller.GetComponent<Camera>();
            cacheTransform = transform;
        }

        public void UpdateCameraProjection()
        {
            if (NxrGlobal.hasInfinityARSDK) return;
            // Debug.Log("NxrEye->UpdateStereoValues,"+eye.ToString());
            Matrix4x4 proj = NxrViewer.Instance.Projection(eye);
            Debug.Log("NxrEye->UpdateCameraProjection," + eye.ToString() + "/" + proj.ToString());
            bool useDFT = NxrViewer.USE_DTR && !NxrGlobal.supportDtr;
            //DTR???????????????
            if (!NxrViewer.Instance.IsWinPlatform && (Application.isEditor || useDFT))
            {
                if (monoCamera == null) monoCamera = controller.GetComponent<Camera>();
                // Fix aspect ratio and near/far clipping planes.
                float nearClipPlane = monoCamera.nearClipPlane;
                float farClipPlane = monoCamera.farClipPlane;

                float near = (NxrGlobal.fovNear >= 0 && NxrGlobal.fovNear < nearClipPlane) ? NxrGlobal.fovNear : nearClipPlane;
                float far = (NxrGlobal.fovFar >= 0 && NxrGlobal.fovFar > farClipPlane) ? NxrGlobal.fovFar : farClipPlane;
                // DFT & ?????????????????????????????????      
                Debug.Log(eye.ToString() + ", " + cam.rect.ToString());
                NxrCameraUtils.FixProjection(cam.rect, near, far, ref proj);
            }

            // Set the eye camera's projection for rendering.
            cam.projectionMatrix = proj;
            NxrViewer.Instance.UpdateEyeCameraProjection(eye);

            float ipd = NxrViewer.Instance.Profile.viewer.lenses.separation;
            Vector3 localPosition = (eye == NxrViewer.Eye.Left ? -ipd / 2 : ipd / 2) * Vector3.right;
            if (localPosition.x != transform.localPosition.x)
            {
                transform.localPosition = localPosition;
            }

            BaseARDevice nxrDevice = NxrViewer.Instance.GetDevice();
            if (nxrDevice != null && nxrDevice.IsSptEyeLocalRotPos())
            {
                transform.localRotation = nxrDevice.GetEyeLocalRotation(eye);
                transform.localPosition = nxrDevice.GetEyeLocalPosition(eye);
                Debug.Log(eye + ". Local Rotation : " + transform.localRotation.eulerAngles.ToString());
                Debug.Log(eye + ". Local Position : " + transform.localPosition.x + "," + transform.localPosition.y + "," + transform.localPosition.z);
            }
        }

        private int cacheTextureId = -1;

        public int GetTargetTextureId()
        {
            return cacheTextureId;
        }

        public void UpdateTargetTexture()
        {
            // ???so????????????idx
            int eyeType = eye == NxrViewer.Eye.Left ? 0 : 1;
            cacheTextureId = NxrViewer.Instance.GetEyeTextureId(eyeType);
            cam.targetTexture = NxrViewer.Instance.GetStereoScreen(eyeType);
            cam.targetTexture.DiscardContents();
        }

        void OnPreRender()
        {
            if (cacheTextureId == -1 && cam.targetTexture != null)
            {
                cacheTextureId = (int)cam.targetTexture.GetNativeTexturePtr();
            }
            if(OnPreRenderListener != null) OnPreRenderListener(cacheTextureId, eye);
        }

        int frameId = 0;
        void OnPostRender()
        {
            if (cacheTextureId == -1 && cam.targetTexture != null)
            {
                cacheTextureId = (int)cam.targetTexture.GetNativeTexturePtr();
            }
            if(OnPostRenderListener != null) OnPostRenderListener(cacheTextureId, eye);

            if (eye == NxrViewer.Eye.Left)
            {
                // ??????
                RenderTexture stereoScreen = cam.targetTexture;
                if (stereoScreen != null && NxrViewer.Instance.GetHoloeverService() != null)
                {
                    int textureId = (int)stereoScreen.GetNativeTexturePtr();
                    bool isCapturing = NxrViewer.Instance.GetHoloeverService().CaptureDrawFrame(textureId, frameId);
                    if (isCapturing)
                    {
                        GL.InvalidateState();
                    }
                    frameId++;
                }
            }
        }

        private void SetupStereo()
        {
            int eyeType = eye == NxrViewer.Eye.Left ? 0 : 1;
            if (cam.targetTexture == null
                 && NxrViewer.Instance.GetStereoScreen(eyeType) != null)
            {
                cam.targetTexture = monoCamera.targetTexture ?? NxrViewer.Instance.GetStereoScreen(eyeType);
            }
        }

        void OnPreCull()
        {
            if (NxrGlobal.DEBUG_LOG_ENABLED) Debug.Log(eye+".NxrEye.OnPreCull." + cam.rect.ToString());
            if (NxrGlobal.isVR9Platform)
            {
                cam.targetTexture = null;
                return;
            } else
            {
                // Debug.Log("OnPreCull.eye[" + eye + "]");
                if (!NxrViewer.Instance.SplitScreenModeEnabled)
                {
                    // Keep stereo enabled flag in sync with parent mono camera.
                    cam.enabled = false;
                    return;
                }

                SetupStereo();

                int eyeType = eye == NxrViewer.Eye.Left ? 0 : 1;
                if (NxrGlobal.DEBUG_LOG_ENABLED) Debug.Log("OnPreCull.eye[" + eyeType + "]");

                if (NxrViewer.Instance.OpenEffectRender && NxrViewer.Instance.GetStereoScreen(eyeType) != null)
                {
                    // Some image effects clobber the whole screen.  Add a final image effect to the chain
                    // which restores side-by-side stereo.
                    stereoEffect = GetComponent<StereoRenderEffect>();
                    if (stereoEffect == null)
                    {
                        stereoEffect = gameObject.AddComponent<StereoRenderEffect>();
#if UNITY_5_6_OR_NEWER
                        stereoEffect.UpdateEye(eye);
#endif  // UNITY_5_6_OR_NEWER
                    }
                    stereoEffect.enabled = true;
                }
                else if (stereoEffect != null)
                {
                    // Don't need the side-by-side image effect.
                    stereoEffect.enabled = false;
                }
            }
        }

        /// Helper to copy camera settings from the controller's mono camera.  Used in SetupStereo() and
        /// in the custom editor for StereoController.  The parameters parx and pary, if not left at
        /// default, should come from a projection matrix returned by the SDK.  They affect the apparent
        /// depth of the camera's window.  See SetupStereo().
        public void CopyCameraAndMakeSideBySide(NxrStereoController controller,
                                              float parx = 0, float pary = 0)
        {
#if UNITY_EDITOR
            // Member variable 'cam' not always initialized when this method called in Editor.
            // So, we'll just make a local of the same name.
            var cam = GetComponent<Camera>();
#endif

            float ipd = NxrViewer.Instance.Profile.viewer.lenses.separation;
            Vector3 localPosition = (eye == NxrViewer.Eye.Left ? -ipd / 2 : ipd / 2) * Vector3.right;

            if (monoCamera == null)
            {
                monoCamera = controller.GetComponent<Camera>();
            }

            // Sync the camera properties.
            cam.CopyFrom(monoCamera);
#if UNITY_5_6_OR_NEWER
            cam.allowHDR = false;
            cam.allowMSAA = false;
            // cam.allowDynamicResolution = false;
#endif
            monoCamera.useOcclusionCulling = false;

            // Not sure why we have to do this, but if we don't then switching between drawing to
            // the main screen or to the stereo rendertexture acts very strangely.
            cam.depth = eye == NxrViewer.Eye.Left ? monoCamera.depth + 1 : monoCamera.depth + 2;

            // Reset transform, which was clobbered by the CopyFrom() call.
            // Since we are a child of the mono camera, we inherit its transform already.
            transform.localPosition = localPosition;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;

            Rect left = new Rect(0.0f, 0.0f, 0.5f, 1.0f);
            Rect right = new Rect(0.5f, 0.0f, 0.5f, 1.0f);
            if (eye == NxrViewer.Eye.Left)
                cam.rect = left;
            else
                cam.rect = right;

            // VR9 ?????????????????????????????????
            if (!NxrGlobal.isVR9Platform && NxrViewer.USE_DTR && NxrGlobal.supportDtr && Application.platform == RuntimePlatform.Android)
            {
                // DTR&DFT???Android?????????????????????????????????0~1
                cam.rect = new Rect(0, 0, 1, 1);
            }

            if (cam.farClipPlane < NxrGlobal.fovFar)
            {
                cam.farClipPlane = NxrGlobal.fovFar;
            }

            if(NxrGlobal.isVR9Platform)
            {
                // ??????????????????????????????????????????????????????
                cam.clearFlags = CameraClearFlags.Nothing;
                monoCamera.clearFlags = CameraClearFlags.Nothing;
            }

#if NIBIRU_VR
            cam.aspect = 1.0f;
#endif

            Debug.Log(eye.ToString() + "," + cam.transform.localPosition.x);

        }
#if UNITY_STANDALONE_WIN || ANDROID_REMOTE_NRR
        public Material _flipMat;
        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if(_flipMat == null)
            {
                Debug.Log((eye == NxrViewer.Eye.Left ? "Materials/LeftEyeFlip" : "Materials/RightEyeFlip"));
                _flipMat = Resources.Load<Material>(eye == NxrViewer.Eye.Left ? "Materials/LeftEyeFlip" : "Materials/RightEyeFlip");
            }
            Graphics.Blit(source, destination, _flipMat);
        }
#endif

        private void OnEnable()
        {
#if UNITY_2019_1_OR_NEWER
            if (UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset != null)
            {
                RenderPipelineManager.beginCameraRendering += CameraPreRender;
                RenderPipelineManager.endCameraRendering += CameraPostRender;
            }
#endif
        }

        private void OnDisable()
        {
#if UNITY_2019_1_OR_NEWER
            if (UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset != null)
            {
                RenderPipelineManager.beginCameraRendering -= CameraPreRender;
                RenderPipelineManager.endCameraRendering -= CameraPostRender;
            }
#endif
        }

#if UNITY_2019_1_OR_NEWER
        public void CameraPreRender(ScriptableRenderContext context, Camera mcam)
        {
            //Debug.Log(Time.frameCount + "_CameraPreRender_" + mcam.name);
            if (mcam.gameObject != this.gameObject)
                return;
            OnPreCull();
            OnPreRender();
        }

        public void CameraPostRender(ScriptableRenderContext context, Camera mcam)
        {
            if (mcam.gameObject != this.gameObject)
                return;
            OnPostRender();
        }
#endif

    }
}
