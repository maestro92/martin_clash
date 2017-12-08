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

    // Reconnect
    public static bool DefaultClientReconnectEnabled = true;
    public static int DefaultClientReconnectCoolOffTimeInMS = (8 * 1000);

    // Timeout
    public static bool DefaultServerTimeoutEnabled = true;
    public static int DefaultServerTimeoutInMs = 20 * 1000;

    public static bool DefaultClientTimeoutEnabled = true;
    public static int DefaultClientTimeoutInMs = 20 * 1000;

    public static bool DefaultServerKickingEnabled = true;
    public static int DefaultServerKickingInMs = 10 * 1000;

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

    // Timeout
    public bool serverTimeoutEnabled = DefaultServerTimeoutEnabled;
    public int serverTimeoutInMs = DefaultServerTimeoutInMs;

    public bool clientTimeoutEnabled = DefaultClientTimeoutEnabled;
    public int clientTimeoutInMs = DefaultClientTimeoutInMs;

    public bool serverKickingEnabled = DefaultServerKickingEnabled;
    public int serverKickingInMs = DefaultServerKickingInMs;


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

