namespace Nxr.Internal
{
    public interface IHoloeverGestureListener
    {
        /// <summary>
        ///  The result of Gesture.
        /// </summary>
        /// <param name="gesture"></param>
        void OnGesture(GESTURE_ID gesture);
    }
}
