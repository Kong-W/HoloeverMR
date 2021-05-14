

/// This script provides an interface for gaze based responders used with
/// the NxrGaze script.
using UnityEngine;
namespace Nxr.Internal
{
    public interface INxrGazeResponder
    {
        /// Called when the user is looking on a GameObject with this script,
        /// as long as it is set to an appropriate layer (see NxrGaze).
        void OnGazeEnter();

        /// Called when the user stops looking on the GameObject, after OnGazeEnter
        /// was already called.
        void OnGazeExit();

        /// Called when the trigger is used, between OnGazeEnter and OnGazeExit.
        void OnGazeTrigger();

        /// <summary>
        /// 
        ///  
        /// </summary>
        /// <param name="position"></param>
        void OnUpdateIntersectionPosition(Vector3 position);
    }
}