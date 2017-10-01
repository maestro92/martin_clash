using System;
using System.Collections;
using System.Collections.Generic;

public class NetMeter
{
    // flags will be used as a filter
    [Flags]
    public enum EntryFlag : uint
    {
        None = 0,           // 0x00000000
        Server = 1 << 0,    // 0x00000001
        Client = 1 << 1,    // 0x00000002
        Send = 1 << 2,      // 0x00000004
        Receive = 1 << 3,   // 0x00000008
        Tcp = 1 << 4        // 0x00000010
    };


    // If your CaptureFlag is None, then we will capture everything
    [Flags]
    public enum CaptureFlag : uint
    {
        None = 0,           // 0x00000000
        Server = 1 << 0,    // 0x00000001
        Client = 1 << 1,    // 0x00000002
        Tcp = 1 << 2
    };


    private int m_numMaxEntries;
    private Int64 m_captureWindowSizeMS;
    private float m_numVerticalBytesInMeter;
    private NetMeter.CaptureFlag m_netMeterCaptureFlags;

    private bool m_enabled;
    // the nearest MS value that we will quantize to
    private Int64 m_quantizeToNearestMS;
    public List<Entry> m_entryList;

    public List<Entry> EntryList
    { 
        get 
        { 
            return m_entryList; 
        } 
    }

    public class Entry
    {
        private Int64 m_timeStamp;
        private NetMeter.EntryFlag m_entryFlag;
        private Int64 m_numBytes;

        public Entry(Int64 timeStampIn, NetMeter.EntryFlag entryFlag, Int64 numBytesIn)
        {
            m_timeStamp = timeStampIn;
            m_entryFlag = entryFlag;
            m_numBytes = numBytesIn;
        }

        public Int64 GetTimeStamp()
        {
            return m_timeStamp;
        }

        public void SetNumBytes(Int64 numBytesIn)
        {
            m_numBytes = numBytesIn;
        }

        public Int64 GetNumBytes()
        {
            return m_numBytes;
        }

        public void SetEntryFlag(NetMeter.EntryFlag entryFlag)
        {
            m_entryFlag = entryFlag;
        }

        public NetMeter.EntryFlag GetEntryFlag()
        {
            return m_entryFlag;
        }

        public bool IsEntryFlagSet(NetMeter.EntryFlag flag)
        {
            return (m_entryFlag & flag) == flag;
        }

        public bool IsEntryFlagClear(NetMeter.EntryFlag flag)
        {
            return (m_entryFlag & flag) != flag;
        }
    }

    public NetMeter()
    {}

    public void Init()
    {
        // we will capture 3 seconds worth of data
        m_numMaxEntries = 3000;
        SetCaptureWindowSizeMs(3000);
        SetNumVerticalBytesInMeter(50);
        m_quantizeToNearestMS = 25;  // 5 millisecond
        m_enabled = false;
        m_netMeterCaptureFlags = CaptureFlag.None;
        m_entryList = new List<Entry>();
    }


    public bool IsCaptureFlagSet(CaptureFlag flag)
    {
        if (m_netMeterCaptureFlags == CaptureFlag.None)
        {
            return true;
        }
        else
        {
            return ((m_netMeterCaptureFlags & flag) == flag);
        }
    }

    public bool IsCaptureFlagClear(CaptureFlag flag)
    {
        if (m_netMeterCaptureFlags == CaptureFlag.None)
        {
            return false;
        }
        else
        {
            return ((m_netMeterCaptureFlags & flag) != flag);
        }
    }

    public Int64 GetQuantizeToNearestMS()
    {
        return m_quantizeToNearestMS;
    }
    

    public void Reset()
    {
        m_entryList.Clear();
    }

    public void SetCaptureWindowSizeMs(Int64 captureWindowSizeMS)
    {
        m_captureWindowSizeMS = captureWindowSizeMS;
        if (m_captureWindowSizeMS > (60 * 1000))
        {
            m_captureWindowSizeMS = (60 * 1000);
        }
    }

    public void SetNumVerticalBytesInMeter(float numVerticalBytesInMeter)
    {
        m_numVerticalBytesInMeter = numVerticalBytesInMeter;
        if (m_numVerticalBytesInMeter < 1)
        {
            m_numVerticalBytesInMeter = 1;
        }
    }

    public void SetQuantizeToNearestMS( Int64 quantizeToNearestMSIn )
    {
        m_quantizeToNearestMS = quantizeToNearestMSIn;
        if( m_quantizeToNearestMS < 0 )
        {
            m_quantizeToNearestMS = 0;
        }
    }

    public Int64 GetCaptureWindowSizeMS()
    {
        return m_captureWindowSizeMS;
    }

    public float GetNumVerticalBytesInMeter()
    {
        return m_numVerticalBytesInMeter;
    }

    public void Pump()
    {
        if (!m_enabled)
        {
            return;
        }

        if (m_numMaxEntries >= 0)
        {
            while(m_entryList.Count > m_numMaxEntries)
            {
                m_entryList.RemoveAt(0);
            }
        }

        Int64 windowStartMS = Util.GetRealTimeMS() - GetCaptureWindowSizeMS();
        while (m_entryList.Count > 0)
        {
            var entry = m_entryList[0];
            if (windowStartMS <= entry.GetTimeStamp())
            {
                break;
            }
            m_entryList.RemoveAt(0);
        }
    }

    public void Record(NetMeter.EntryFlag netMeterEntryFlagsIn, Int64 numBytesIn)
    {
        if (!m_enabled)
        {
            return;
        }

        if ( (netMeterEntryFlagsIn & NetMeter.EntryFlag.Server) == NetMeter.EntryFlag.Server)
        {
            // Util.LogError("Recording2");
            if(!IsCaptureFlagSet(CaptureFlag.Server))
            {
            //    Util.LogError("Recording3");

                return;
            }
        }
        else if ( (netMeterEntryFlagsIn & NetMeter.EntryFlag.Client) == NetMeter.EntryFlag.Client)
        {
            // Util.LogError("Recording4");
            if(!IsCaptureFlagSet(CaptureFlag.Client) )
            {
            //    Util.LogError("Recording5");
                return;
            }
        }

        Int64 now = Util.GetRealTimeMS();
        AddEntry(now, netMeterEntryFlagsIn, numBytesIn);
        if(m_numMaxEntries >= 0)
        {
            while(m_entryList.Count > m_numMaxEntries)
            {
                m_entryList.RemoveAt(0);
            }
        }
            
    }


    private void AddEntry(Int64 msIn, NetMeter.EntryFlag netMeterEntryFlagsIn, Int64 numBytesIn)
    {
        Int64 ms = msIn;

        if (m_quantizeToNearestMS > 1)
        {
            ms = ms - (ms % m_quantizeToNearestMS);
        }
            
        Entry entry = FindEntry(ms, netMeterEntryFlagsIn);
        if (entry != null)
        {
            entry.SetNumBytes(entry.GetNumBytes() + numBytesIn);
        }
        else
        {
            m_entryList.Add( new Entry(ms, netMeterEntryFlagsIn, numBytesIn));
        }
    }

    private Entry FindEntry(Int64 timeStampIn, NetMeter.EntryFlag netMeterEntryFlagIn)
    {
        foreach (var entry in m_entryList)
        {
            if (entry.GetTimeStamp() == timeStampIn && entry.GetEntryFlag() == netMeterEntryFlagIn)
            {
                return entry;
            }
            if (entry.GetTimeStamp() > timeStampIn)
            {
                return null;
            }
        }
        return null;
    }


    public void Enable( bool enabledIn )
    {
        if (m_enabled == enabledIn)
        {
            return;
        }

        m_enabled = enabledIn;
        if (m_enabled)
        {
            m_entryList.Clear();
        }
    }


    public bool IsEnabled()
    {
        return m_enabled;
    }

}

