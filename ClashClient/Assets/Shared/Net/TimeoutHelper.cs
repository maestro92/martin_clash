using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TimeoutHelper 
{
    private Int64 m_timeStampConnected = 0;
    private Int64 m_timeStampLastReceivedData = 0;

    private bool m_timeoutEnabledFlag;
    private Int64 m_timeoutInMs;

    private Int64 m_kickingTimeStampStart = 0;
    private Int64 m_kickingInMs;

    public void Init(bool timeoutEnabledFlagIn, Int64 timeoutInMsIn, Int64 kickingInMs)
    {
        m_timeoutEnabledFlag = timeoutEnabledFlagIn;
        m_timeoutInMs = timeoutInMsIn;

        m_kickingInMs = kickingInMs;
        m_kickingTimeStampStart = 0;
    }

    public void UpdateTimeStampLastReceivedData(Int64 timeStampLastReceivedDataIn)
    {
        m_timeStampLastReceivedData = timeStampLastReceivedDataIn;
    }

    public void UpdateTimeStampConnected(Int64 timeStamp_ms)
    {
        m_timeStampConnected = timeStamp_ms;
    }

    public bool IsEnabled()
    {
        return m_timeoutEnabledFlag;
    }

    public bool ShouldTriggerTimeoutNow(Int64 now)
    {
        if (m_timeoutInMs > 0)
        {
            if (m_timeStampLastReceivedData == 0)
            {
                if (now - m_timeStampConnected > m_timeoutInMs)
                {
                    return true;
                }
            }
            else
            {
                if ((now - m_timeStampLastReceivedData) > m_timeoutInMs)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void StartKickRequestTimer(Int64 now)
    {
        m_kickingTimeStampStart = now;
    }


    public bool HaveAKickRequest()
    {
        return m_kickingTimeStampStart > 0;
    }


    public bool ShouldKickNow(Int64 now)
    {
        if (m_kickingInMs > 0)
        {
            if (now - m_kickingTimeStampStart > m_kickingInMs)
            {
                return true;
            }
        }
        return false;
    }
}
