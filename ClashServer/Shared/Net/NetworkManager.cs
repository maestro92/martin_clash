using System;

public class NetworkManager
{
	public const string LOCAL_IP_ADDRESS = "127.0.0.1";
	public const string REMOTE_SERVER_DEFAULT_IP_ADDRESS = "127.0.0.1";
	public const string CUSTOM_IP_ADDRESS = "127.0.0.1";


    public const int SERVER_PORT = 11500;

	public NetworkManager()
	{
		
	}



    public string GetServerIPAddress()
    {
        return LOCAL_IP_ADDRESS;
    }

}
