using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class NetGameConnectionConfig
{
    public static bool DefaultServerAutoPingEnabled = false;
    public static int DefaultServerAutoPingInMs = 500;

    public static bool DefaultClientAutoPingEnabled = false;
    public static int DefaultClientAutoPingInMs = 500;

    public bool serverAutoPingEnabled = DefaultServerAutoPingEnabled;
    public int serverAutoPingInMs = DefaultServerAutoPingInMs;

    public bool clientAutoPingEnabled = DefaultClientAutoPingEnabled;
    public int clientAutoPingInMs = DefaultClientAutoPingInMs;

    public NetGameConnectionConfig()
    {

    }

    public void Init()
    {
 

    }

    public void Reset()
    {


    }
}

