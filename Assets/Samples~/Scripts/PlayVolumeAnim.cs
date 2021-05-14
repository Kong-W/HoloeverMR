using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NXR.Samples
{
    public class PlayVolumeAnim : MonoBehaviour
    {
        public enum PLAY_MODE
        {
            NONE = -1,
            BEGIN = 0,
            END,
            LOOP,
            RECOGNIZE
        }

        public Texture[] frames_begin;
        public Texture[] frames_end;
        public Texture[] frames_loop;
        public Texture[] frames_recognize;


        // 100ms=1frame
        public int framesPerSecond = 10;
        // 
        private float timePerFrame = 0;

        protected Renderer _renderer;

        Texture defaultTexture;
        // Use this for initialization
        void Start()
        {
            _renderer = GetComponent<Renderer>();
            defaultTexture = _renderer.material.mainTexture;
            timePerFrame = 1.0f / framesPerSecond;
        }

        public PLAY_MODE curPlayMode = PLAY_MODE.NONE;
        public PLAY_MODE PlayMode
        {
            set
            {
                curPlayMode = value;
                totalTime = 0;
                frameIndex = 0;
            }
            get
            {
                return curPlayMode;
            }
        }

        private float totalTime = 0;
        private int frameIndex = 0;
        // Update is called once per frame
        void Update()
        {
            if (curPlayMode == PLAY_MODE.NONE)
            {
                _renderer.material.mainTexture = defaultTexture;
                return;
            }

            totalTime += Time.deltaTime;
            if (totalTime > timePerFrame)
            {
                frameIndex++;
                totalTime = 0;
                // Debug.Log("frameIndex="+ frameIndex + "," + Time.time);
            }

            Texture[] frames = null;
            if (curPlayMode == PLAY_MODE.BEGIN)
            {
                frames = frames_begin;
                if (frameIndex >= 8)
                {
                    frameIndex = 0;
                    curPlayMode = PLAY_MODE.LOOP;
                    return;
                }
            }
            else if (curPlayMode == PLAY_MODE.END)
            {
                frames = frames_end;
                if (frameIndex >= 8)
                {
                    frameIndex = 0;
                    curPlayMode = PLAY_MODE.NONE;
                    return;
                }
            }
            else if (curPlayMode == PLAY_MODE.LOOP)
            {
                frames = frames_loop;
            }
            else if (curPlayMode == PLAY_MODE.RECOGNIZE)
            {
                frames = frames_recognize;
            }

            // int index = (int) (Time.time * framesPerSecond) % frames.Length;
            _renderer.material.mainTexture = frames[frameIndex % frames.Length];
        }
    }
}