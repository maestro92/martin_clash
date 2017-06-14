using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

/*

the basic flow is below

	// init ip address and port
	server.init()
	{
		finds public IP address
		finds private IP address
		starts hosting IP address and port (which ip u ask? I don't know)
	}


	while()
	{
		accepting commands
	}
*/





public struct ServerConfig
{
	// backLog is essentially the number of pending connections
	// the queue will hold. Apparently in Network Programming, we call it 
	public int backLog;
	public ServerConfig(int backLog)
	{
		this.backLog = backLog;
	}
}

public class Server
{
	public const int SERVER_BACKLOG = 100;

	// server's ip address and port
	public NetAddress m_hostingNetAddress;


	public Socket m_listenerSocket;


	ServerConfig m_serverConfig;


	// Thread signal.  
	public static ManualResetEvent m_allDone = new ManualResetEvent(false);

	public Server()
	{
		m_serverConfig = new ServerConfig(SERVER_BACKLOG);
		Console.WriteLine("init server");
	}

	// set up the IP address and port, then start accepting commands
	public void startHosting(string hostIPAddress, int port)
	{
		m_hostingNetAddress = new NetAddress(hostIPAddress, port);

		m_listenerSocket = new Socket(AddressFamily.InterNetwork, 
		                              SocketType.Stream, 
		                              ProtocolType.Tcp);

		Util.LogError(m_hostingNetAddress.GetIPAddress().ToString());

		IPEndPoint hostingSocketEndPoint = new IPEndPoint(m_hostingNetAddress.GetIPAddress(), m_hostingNetAddress.GetPort());

		m_listenerSocket.Bind(hostingSocketEndPoint);
		m_listenerSocket.Listen(m_serverConfig.backLog);

	    Util.LogError("Hosting Dedicated Server at " + m_hostingNetAddress.ToString());
	}


	public void TryAcceptConnections()
	{
		Util.Log("Server TryAcceptConnections");
		// Bind the socket to the local endpoint and listen for incoming connections.  
		try
		{
			// Set the event to nonsignaled state.  
			m_allDone.Reset();

			// Start an asynchronous socket to listen for connections.  
			Console.WriteLine("Waiting for a connection...");
			m_listenerSocket.BeginAccept(
				new AsyncCallback(AsyncServerAcceptConnectionCallback),
				m_listenerSocket);

			// Wait until a connection is made before continuing.  
			m_allDone.WaitOne();
		}
		catch (Exception e)
		{
			Console.WriteLine(e.ToString());
		}

	}


	public void AsyncServerAcceptConnectionCallback(IAsyncResult ar)
	{
		// Signal the main thread to continue.
		m_allDone.Set();
		Socket rawTcpSocketNew = m_listenerSocket.EndAccept(ar);
		Util.Log("Accepted a connection");
	}

	public void processIncomingMessages()
	{
		// go through all connections
		// if they are connected, process incoming messages for that connection

	}

	public void update()
	{


		// 


	}

	public void stopHosting()
	{


	}
}
