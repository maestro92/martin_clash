using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class HeartbeatHelper
{
    private Int64 m_timeStampConnected = 0;

    public Int64 m_timeStampLastSent;
    public bool m_enabled;
    public Int64 m_interval;
    public int m_cargoSize;

    public void Init(bool enabled, Int64 interval, int cargoSizeIn)
    {
        m_enabled = enabled;
        m_interval = interval;
        m_cargoSize = cargoSizeIn;
        m_timeStampLastSent = 0;
    }

    public bool IsEnabled()
    {
        return m_enabled;
    }

    public int GetCargoSize()
    {
        return m_cargoSize;
    }

    public void UpdateTimeStampConnected(Int64 timeStamp_ms)
    {
        m_timeStampConnected = timeStamp_ms;
    }

    public void UpdateTimeStampLastSend(Int64 now_ms)
    {
        m_timeStampLastSent = now_ms;
    }

    public bool CanSendHeartbeatNow(Int64 now_ms)
    {
        if (m_timeStampLastSent == 0)
        {
            if ((now_ms - m_timeStampConnected) > m_interval)
            {
                return true;
            }
        }
        else
        {
            if ((now_ms - m_timeStampLastSent) > m_interval)
            {
                return true;
            }
        }
        return false;
    }

    public void Reset()
    {
        m_timeStampConnected = 0;
    }
}
