using System;
using System.Collections.Generic;
using System.Net.Sockets;

/*
apparently in C#, No you cannot create "free functions" in C#

C# does not allow for free functons. Each function must reside in a type. 
This is just the way it works, it's not a matter of technical possibility, it was a design decision.

https://stackoverflow.com/questions/11893404/functions-in-a-namespace

*/ 

class Util
{

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

        
}
