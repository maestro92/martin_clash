using System;
using System.Net.Sockets;


using UnityEngine;


using System.IO;
using System.Collections;
using System.Collections.Generic;

/*
apparently in C#, No you cannot create "free functions" in C#

C# does not allow for free functons. Each function must reside in a type. 
This is just the way it works, it's not a matter of technical possibility, it was a design decision.

https://stackoverflow.com/questions/11893404/functions-in-a-namespace

*/ 

public class Util
{

    public static float DEGREE_TO_RADIAN = Mathf.PI / 180.0f;

    public static Action<string> OnLog;
    public static Action<string> OnLogWarning;
    public static Action<string> OnLogError;

    public static void Log(string s)
    {
        if (OnLog != null)
        {
            OnLog(s);
        }
    }

    public static void LogWarning(string s)
    {
        if (OnLogWarning != null)
        {
            OnLogWarning(s);
        }
    }

    public static void LogError(string s)
    {
        if (OnLogError != null)
        {
            OnLogError(s);
        }
    }


    private static Int64 g_ticksFirstCall = 0;
    public static Int64 GetRealTimeTicks()
    {
        Int64 ticks = DateTime.UtcNow.Ticks;
        if (g_ticksFirstCall == 0)
        {
            g_ticksFirstCall = ticks;
        }
        return ticks - g_ticksFirstCall + 1;    // always > 0
    }
    private static Int64 g_msFirstCall = 0;
    public static Int64 GetRealTimeMS()
    {
        Int64 ms = (Int64)(Util.GetRealTimeTicks() / TimeSpan.TicksPerMillisecond);
        if (g_msFirstCall == 0)
        {
            g_msFirstCall = ms;
        }
        return ms - g_msFirstCall + 1;  // always > 0
    }


	// any class that wants to use a RateRegulator, they can use this instead of creating local timestamps
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
		// 		now is the current time, and we want m_lastPumpTimeStamp to CATCH up
		//		hence the logic "now >= m_lastPumpTimeStamp". We always want m_lastPumpTimeStamp to catch up
		//		this is essentially the time accumulator method for FixedUpdate()
		public bool ShouldPump()
		{
			if (m_running == false)
			{
				return false;
			}

			float now = Util.GetRealTimeMS();

			// Util.LogError("Now " + now.ToString() + ", counter " + counter.ToString());
			// Util.LogError("\t\tm_lastPumpTimeStamp " + m_lastPumpTimeStamp.ToString());
			counter++;
			return now >= m_lastPumpTimeStamp;
//			return now - m_nextPumpTimestamp > m_msPerFrame;
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

		//	Util.LogError("\t\tm_lastPumpTimeStamp " + m_lastPumpTimeStamp.ToString());
		}

		public void Print()
		{
			Util.LogError("m_lastPumpTimeStamp " + m_lastPumpTimeStamp.ToString());
		}
	}


}
