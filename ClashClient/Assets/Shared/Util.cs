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

class Util
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
        
}
