namespace Nxr.Internal
{
    public interface IHoloeverVoiceListener
    {
        /// <summary>
        /// Speech recognition starts.
        /// </summary>
        void OnVoiceBegin();
        /// <summary>
        /// Speech recognition ends.
        /// </summary>
        void OnVoiceEnd();
        /// <summary>
        /// The result of speech recognition.
        /// </summary>
        /// <param name="param"></param>
        void OnVoiceFinishResult(string param);
        /// <summary>
        /// The change of volume.
        /// </summary>
        /// <param name="volume"></param>
        void OnVoiceVolume(string volume);
        /// <summary>
        /// The error of speech recognition.
        /// </summary>
        void OnVoiceFinishError(string errorMsg);

        /// <summary>
        /// Cancel speech recognition.
        /// </summary>
        void OnVoiceCancel();
    }
}