// Copyright 2016 Nibiru. All rights reserved.
//
using UnityEngine;

namespace Nxr.Internal
{
    public interface INxrGazePointer
    {
        /// This is called when the 'BaseInputModule' system should be enabled.
        void OnGazeEnabled();
        /// This is called when the 'BaseInputModule' system should be disabled.
        void OnGazeDisabled();

        /// Called when the user is looking on a valid GameObject. This can be a 3D
        /// or UI element.
        ///
        /// The camera is the event camera, the target is the object
        /// the user is looking at, and the intersectionPosition is the intersection
        /// point of the ray sent from the camera on the object.
        void OnGazeStart(Camera camera, GameObject targetObject, Vector3 intersectionPosition,
                         bool isInteractive);

        /// Called every frame the user is still looking at a valid GameObject. This
        /// can be a 3D or UI element.
        ///
        /// The camera is the event camera, the target is the object the user is
        /// looking at, and the intersectionPosition is the intersection point of the
        /// ray sent from the camera on the object.
        void OnGazeStay(Camera camera, GameObject targetObject, Vector3 intersectionPosition,
                        bool isInteractive);

        /// Called when the user's look no longer intersects an object previously
        /// intersected with a ray projected from the camera.
        /// This is also called just before **OnGazeDisabled** and may have have any of
        /// the values set as **null**.
        ///
        /// The camera is the event camera and the target is the object the user
        /// previously looked at.
        void OnGazeExit(Camera camera, GameObject targetObject);

        /// Called when a trigger event is initiated. This is practically when
        /// the user begins pressing the trigger.
        void OnGazeTriggerStart(Camera camera);

        /// Called when trigger event is finished. This is practically when
        /// the user releases the trigger.
        void OnGazeTriggerEnd(Camera camera);

        /// Return the radius of the gaze pointer. This is used when searching for
        /// valid gaze targets. If a radius is 0, the NxrGaze will use a ray
        /// to find a valid gaze target. Otherwise it will use a SphereCast.
        /// The *innerRadius* is used for finding new targets while the *outerRadius*
        /// is used to see if you are still nearby the object currently looked at
        /// to avoid a flickering effect when just at the border of the intersection.
        void GetPointerRadius(out float innerRadius, out float outerRadius);

        void UpdateStatus();
    }
}