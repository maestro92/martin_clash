using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PingHelper
{
    private Int64 m_timeStampConnected = 0;

    private int m_pingValue = -1;   // -1= don't have value, else 0....x = milliseconds
    private int m_pingIdLastSent = 0;   // 0=not sent, else 1...x is that id
    private int m_pingIdLastReceived = 0;   // 0=not received, else 1...x is that id
    private Int64 m_timeStampLastSendPing = 0;
    private Int64 m_timeStampLastReceivedPing = 0;

    private bool m_autoPingFlag;
    private Int64 m_autoPingInterval;

    public void Init(bool autoPingFlagIn, Int64 autoPingIntervalIn)
    {
        m_autoPingFlag = autoPingFlagIn;
        m_autoPingInterval = autoPingIntervalIn;
    }

    public bool IsAutoPingEnabled()
    {

        return (m_autoPingFlag == true && m_autoPingInterval >= 0);
    }

    public void UpdateTimeStampConnected(Int64 timeStamp_ms)
    {
        m_timeStampConnected = timeStamp_ms;
    }

    public void UpdateTimeStamp(Int64 now_ms)
    {
        m_timeStampLastSendPing = now_ms;
    }

    public void UpdatePing(Int64 now_ms, Int64 timeStampInitialSend, int pingId)
    {
        m_pingValue = (int)(now_ms - timeStampInitialSend);
        m_pingIdLastReceived = pingId;
        m_timeStampLastReceivedPing = now_ms;
    }

    public bool CanSendPingNow(Int64 now_ms)
    {
        if(m_timeStampLastSendPing == 0)
        {
            if((now_ms - m_timeStampConnected) > m_autoPingInterval)
            {
                return true;
            }
        }
        else
        {
            if((now_ms - m_timeStampLastSendPing) > m_autoPingInterval)
            {
                return true;
            }
        }
        return false;
    }

    public int GetNewPingId()
    {
        int pingId = m_pingIdLastSent++;
        return pingId;
    }

    public int GetPing()
    {
        return m_pingValue;
    }

    public void ClearPing()
    {
        m_pingValue = -1;
    }

}
