using System;
using UnityEngine;

// this analyzes the number of Frames we need to bufferAhead for the RateSmoother
public class FrameBufferAnalyzer
{
    public class FrameBufferAnalyzerConfig
    {
        public float maxBufferTimeWindowMS;
        public float minBufferTimeWindowMS;
        public float initialBufferTimeWindowMS;

        public void Init()
        {
            minBufferTimeWindowMS = 100;
            maxBufferTimeWindowMS = 1200;
            initialBufferTimeWindowMS = 500;
        }

        public void Reset()
        {
            maxBufferTimeWindowMS = 0;
            minBufferTimeWindowMS = 0;
            initialBufferTimeWindowMS = 0;
        }
    }

    public FrameBufferAnalyzerConfig frameBufferAnalyzerConfig;

    // how often do we want to re-evaluate our buffer
    private float m_sampleRate;
    private float m_sampleTimeSample;
    private float m_msPerFrame;

    private float m_frameHeadTime;
    private int m_frameHead;
    private int m_frameTail;

    private bool hasComputedValidBuffer;

    private int m_numInitialFramesToIgnore;

    private float m_computedNumFrameToBufferTimeWindow; 
    private int m_computedNumFrameToBuffer; // m_computedNumFrameToBufferTimeWindow / m_msPerFrame;

    public FrameBufferAnalyzer()
    {

    }


    public void Init()
    {
        SetMsPerFrame(33.3333f);
        SetNumInitialFramesToIgnore(0);
        SetSampleRate(5000.0f);

        frameBufferAnalyzerConfig = new FrameBufferAnalyzerConfig();
        frameBufferAnalyzerConfig.Init();

        m_frameHeadTime = 0;
        m_frameHead = 0;
        m_frameTail = 0;
    }

    public void Reset()
    {
        SetMsPerFrame(0);
        SetNumInitialFramesToIgnore(0);
        SetSampleRate(0);

        frameBufferAnalyzerConfig.Reset();

        m_frameHeadTime = 0;
        m_frameHead = 0;
        m_frameTail = 0;
    }

    public void SetMsPerFrame(float msPerFrame)
    {
        m_msPerFrame = msPerFrame;
    }

    public void SetSampleRate(float sampleRate)
    {
        m_sampleRate = sampleRate;
    }

    public void SetNumInitialFramesToIgnore(int numFrames)
    {
        m_numInitialFramesToIgnore = numFrames;
    }

    public int GetComputedNumFrameToBuffer()
    {
        return m_computedNumFrameToBuffer;
    }

    public void SetFrameHead(int newFrameHead)
    {
        Int64 now = Util.GetRealTimeMS();

        // it's basically NOT calculating anything until it's got enough data
        if (m_frameHead > m_numInitialFramesToIgnore)
        {
            float msTimeSinceLastFrame = (float)(now - m_frameHeadTime);
            float msTimeSinceLastFramePlusOneFrame = msTimeSinceLastFrame + m_msPerFrame;

            // if the data is comming in faster
            if (m_computedNumFrameToBufferTimeWindow < msTimeSinceLastFramePlusOneFrame)
            {
                m_computedNumFrameToBufferTimeWindow = msTimeSinceLastFramePlusOneFrame;
            }
                
            if( frameBufferAnalyzerConfig.maxBufferTimeWindowMS >= 0)
            {
                m_computedNumFrameToBufferTimeWindow = Mathf.Min(m_computedNumFrameToBufferTimeWindow, 
                    frameBufferAnalyzerConfig.maxBufferTimeWindowMS);
            }
                
            if (frameBufferAnalyzerConfig.minBufferTimeWindowMS >= 0)
            {
                m_computedNumFrameToBufferTimeWindow = Mathf.Max(m_computedNumFrameToBufferTimeWindow, 
                    frameBufferAnalyzerConfig.minBufferTimeWindowMS);
            }

            m_computedNumFrameToBuffer = (int)(m_computedNumFrameToBufferTimeWindow / m_msPerFrame);
        }

        m_frameHead = newFrameHead;
        m_frameHeadTime = now;
    }

    public void SetFrameTail(int newFrameTail)
    {
        m_frameTail = newFrameTail;
    }


    // see if we can shrink
    public void Pump()
    {
        Int64 now = Util.GetRealTimeMS();

        if(now - m_sampleTimeSample > m_sampleRate)
        {   
            // we re-evaluate

            m_sampleTimeSample = now;


            // see if we can shrink our buffer
        }
    }
}