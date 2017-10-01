using System;


public class RateSmoother
{

    public enum RateSmootherMode
    {
        None,
        Fastest,
        Smooth,
    }

    private int m_frameHead;
    private int m_frameTail;

    private int m_numFramesAvailableToConsumeThisTick;
    private int m_numFramesConsumedThisTick;
    private int m_numFramesToBufferAhead;

    private int m_consumeCounter;
    private bool m_isRunning;

    private RateSmootherMode m_mode;


    public RateSmoother()
    {
        
    }

    // I prefer to have a init function, so it is more explicit
    public void Init()
    {
        m_frameHead = 0;
        m_frameTail = 0;

        m_numFramesAvailableToConsumeThisTick = 0;
        m_numFramesConsumedThisTick = 0;
        m_numFramesToBufferAhead = 0;

        m_consumeCounter = 0;
        m_isRunning = false;

    }

    public bool AddNewFrame(int newFrame)
    {
        if (newFrame == 0)
        {
            Util.LogError("Don't know why is this happened");
            m_frameHead++;
        }
        else if (newFrame > m_frameHead)
        {
            m_frameHead = newFrame;
        }
        else
        {
            return false;
        }

        return true;
    }

    public void StartLoop()
    {
        m_numFramesAvailableToConsumeThisTick = GetNumFramesAvailableToConsumeThisTick();     
        m_numFramesConsumedThisTick = 0;

        if (m_numFramesAvailableToConsumeThisTick <= 0)
        {
            m_isRunning = false;
        }
    }

    public void UpdateFrameToBufferAhead(int numFramesToBufferAhead)
    {
        m_numFramesToBufferAhead = numFramesToBufferAhead;
    }


    // this tells us how many frames we can consume This Tick
    private int GetNumFramesAvailableToConsumeThisTick()
    {
        int availableFrames = m_frameHead - m_frameTail;

        if (availableFrames <= 0)
        {
            return 0;
        }

        switch (m_mode)
        {
            case RateSmootherMode.None:
                return 0;

            case RateSmootherMode.Fastest:
                return availableFrames;

            case RateSmootherMode.Smooth:
                if (m_isRunning == true)
                {
                    // see my notes for eric_net_code_rate_smoother.cs
                    if (availableFrames > m_numFramesToBufferAhead)
                    {
                        return availableFrames - m_numFramesToBufferAhead;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else
                {
                    if (availableFrames > m_numFramesToBufferAhead)
                    {
                        return availableFrames - m_numFramesToBufferAhead;
                    }
                    else
                    {
                        return 0;
                    }
                }                    
        }

        return 0;
    }


    public bool ConsumeFrame()
    {
        Util.LogError("Consuming Frames");
        if (m_numFramesAvailableToConsumeThisTick <= 0)
        {
            return false;
        }

        m_numFramesAvailableToConsumeThisTick--;
        m_numFramesConsumedThisTick++;

        if (m_isRunning == false)
        {
            Util.LogError("Game Resume running");
            m_isRunning = true;
        }

        m_frameTail++;
        m_consumeCounter++;

        return true;
    }

}


