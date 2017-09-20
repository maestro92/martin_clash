using System;

// any class that wants to use a RateRegulator, they can use this instead of creating local timestamps
// use this class if you want something to pump at a specific rate
public class RateRegulator
{
    private float m_fps;
    private float m_msPerFrame;
    private double m_lastPumpTimeStamp;
    private bool m_running;
    private int counter;
    public RateRegulator(float fps)
    {
        m_fps = fps;
        m_msPerFrame = (float)(1000 / m_fps);
        m_lastPumpTimeStamp = 0;
        m_running = false;
        // Util.LogError("fps " + fps.ToString());
        // Util.LogError("m_msPerFrame " + m_msPerFrame.ToString());
        counter = 0;
    }

    public void Start()
    {
        if (m_msPerFrame > 0)
        {
            m_lastPumpTimeStamp = Util.GetRealTimeMS();
            m_running = true;
        }
    }

    public void Reset()
    {
        m_fps = 0;
        m_msPerFrame = 0;
        m_lastPumpTimeStamp = 0;
        m_running = false;
    }

    public void Stop()
    {
        m_running = false;
    }

    // the idea is that
    // as long as now is ahead of m_lastPumpTimestamp
    // we allow it to pump
    //      now is the current time, and we want m_lastPumpTimeStamp to CATCH up
    //      hence the logic "now >= m_lastPumpTimeStamp". We always want m_lastPumpTimeStamp to catch up
    //      this is essentially the time accumulator method for FixedUpdate()
    public bool ShouldPump()
    {
        if (m_running == false)
        {
            return false;
        }

        float now = Util.GetRealTimeMS();

        counter++;
        return now >= m_lastPumpTimeStamp;
        //          return now - m_nextPumpTimestamp > m_msPerFrame;
    }

    /*
        public bool ShouldPump2()
        {
            if (m_running == false)
            {
                return false;
            }

            float now = Util.GetRealTimeMS();
            return now >= m_lastPumpTimeStamp;
        }
        */

    public void Pump()
    {
        if (!ShouldPump())
        {
            return;
        }
        m_lastPumpTimeStamp += m_msPerFrame;

        //  Util.LogError("\t\tm_lastPumpTimeStamp " + m_lastPumpTimeStamp.ToString());
    }

    public void Print()
    {
        Util.LogError("m_lastPumpTimeStamp " + m_lastPumpTimeStamp.ToString());
    }
}