

using UnityEngine;

//Utility class that includes functions for calculating camera properties.
namespace Nxr.Internal
{
public class NxrCameraUtils
{
  public static void FixProjection(Rect camRect, float nearClipPlane, float farClipPlane,
                                   ref Matrix4x4 proj) {
    // Adjust for non-fullscreen camera.  NxrViewer assumes fullscreen,
    // so the aspect ratio might not match.
    proj[0, 0] *= camRect.height / camRect.width / 2;

    // NxrViewer had to pass "nominal" values of near/far to the native layer, which
    // we fix here to match our mono camera's specific values.
    proj[2, 2] = (nearClipPlane + farClipPlane) / (nearClipPlane - farClipPlane);
    proj[2, 3] = 2 * nearClipPlane * farClipPlane / (nearClipPlane - farClipPlane);
  }

  public static Rect FixViewport(Rect rect, Rect viewport, bool isRightEye) {
    // We are rendering straight to the screen.  Use the reported rect that is visible
    // through the device's lenses.
    if (isRightEye) {
      rect.x -= 0.5f;
    }
    rect.width *= 2 * viewport.width;
    rect.x = viewport.x + 2 * rect.x * viewport.width;
    rect.height *= viewport.height;
    rect.y = viewport.y + rect.y * viewport.height;
    return rect;
  }

  public static Rect FixEditorViewport(Rect rect, float profileAspect, float windowAspect) {
    float aspectComparison = profileAspect / windowAspect;
    if (aspectComparison < 1) {
      rect.width *= aspectComparison;
      rect.x *= aspectComparison;
      rect.x += (1 - aspectComparison) / 2;
    } else {
      rect.height /= aspectComparison;
      rect.y /= aspectComparison;
    }
    return rect;
  }

  public static void ZoomStereoCameras(float matchByZoom, float matchMonoFOV, float monoProj11,
    ref Matrix4x4 proj) {
    float lerp = Mathf.Clamp01(matchByZoom) * Mathf.Clamp01(matchMonoFOV);
    // Lerping the reciprocal of proj(1,1), so zoom is linear in frustum height not the depth.
    float zoom = 1 / Mathf.Lerp(1 / proj[1, 1], 1 / monoProj11, lerp) / proj[1, 1];
    proj[0, 0] *= zoom;
    proj[1, 1] *= zoom;
  }
}




}