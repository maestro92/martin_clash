using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class NetGameConnectionConfig
{
    // Ping
    public static bool DefaultServerAutoPingEnabled = false;
    public static int DefaultServerAutoPingInMs = 500;

    public static bool DefaultClientAutoPingEnabled = false;
    public static int DefaultClientAutoPingInMs = 500;

    // Heartbeat
    public static bool DefaultServerHeartbeatEnabled = true;
    public static int DefaultServerHeartbeatInMs = (5 * 1000);
    public static int DefaultServerHeartbeatCargoSize = 0;

    public static bool DefaultClientHeartbeatEnabled = true;
    public static int DefaultClientHeartbeatInMs = (5 * 1000);
    public static int DefaultClientHeartbeatCargoSize = 0;

    public static bool DefaultClientReconnectEnabled = true;
    public static int DefaultClientReconnectCoolOffTimeInMS = (8 * 1000);



    // local variables
    // Ping
    public bool serverAutoPingEnabled = DefaultServerAutoPingEnabled;
    public int serverAutoPingInMs = DefaultServerAutoPingInMs;

    public bool clientAutoPingEnabled = DefaultClientAutoPingEnabled;
    public int clientAutoPingInMs = DefaultClientAutoPingInMs;

    // Heartbeat
    public bool serverHeartbeatEnabled = DefaultServerHeartbeatEnabled;
    public int serverHeartbeatInMs = DefaultServerHeartbeatInMs;
    public int serverHeartbeatCargoSize = DefaultServerHeartbeatCargoSize;

    public bool clientHeartbeatEnabled = DefaultClientHeartbeatEnabled;
    public int clientHeartbeatInMs = DefaultClientHeartbeatInMs;
    public int clientHeartbeatCargoSize = DefaultClientHeartbeatCargoSize;

    public bool clientReconnectEnabled = DefaultClientReconnectEnabled;
    public int clientReconnectCoolOffTimeInMs = DefaultClientReconnectCoolOffTimeInMS;

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

