using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;


/*


From what I have seen, we pretty much dedicate an entire thread, 




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



public class StateObject
{
	// client socket
	public Socket workSocket = null;

	// size of receive buffer
	public const int BufferSize = 1024;

	// Receive Buffer
	public byte[] buffer = new byte[BufferSize];

	// Received data string
	public StringBuilder sb = new StringBuilder();
}

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

	private Thread m_listenerThread;
	public Socket m_listenerSocket;

	public Dictionary<ServerClientHandle, NetGameConnection> connections;
	ServerConfig m_serverConfig;

	private bool m_isRunning;
	private int m_clientCounter;

	byte[] bytes = new Byte[1024];

	// Thread signal.  
	public static ManualResetEvent m_allDone = new ManualResetEvent(false);

	public Server()
	{
		m_serverConfig = new ServerConfig(SERVER_BACKLOG);
		m_isRunning = false;
		m_clientCounter = 0;
		Console.WriteLine("init server");
	}

	// set up the IP address and port, then start accepting commands


	public void startHosting(string hostIPAddress, int port)
	{
		m_isRunning = true;
		m_hostingNetAddress = new NetAddress(hostIPAddress, port);

		m_listenerSocket = new Socket(AddressFamily.InterNetwork,
									  SocketType.Stream,
									  ProtocolType.Tcp);

		Util.LogError(m_hostingNetAddress.GetIPAddress().ToString());

		connections = new Dictionary<ServerClientHandle, NetGameConnection>();

		IPEndPoint hostingSocketEndPoint = new IPEndPoint(m_hostingNetAddress.GetIPAddress(), m_hostingNetAddress.GetPort());



		Util.LogError("Hosting Dedicated Server at " + m_hostingNetAddress.ToString());

		/*

		// async server listening socket thread
		Thread listeningThread = new Thread(new ThreadStart(() =>
		{
			m_listenerSocket.Bind(hostingSocketEndPoint);
			m_listenerSocket.Listen(m_serverConfig.backLog);

			while (true)
			{


			}

		}));
		*/

		// sync server listening socket thread
		m_listenerThread = new Thread(new ThreadStart( () =>
		{
			m_listenerSocket.Bind(hostingSocketEndPoint);
			m_listenerSocket.Listen(m_serverConfig.backLog);

			while (m_isRunning)
			{
				Util.Log("Waiting for a connection...");
				Socket handlerSocket = m_listenerSocket.Accept();

				Util.Log(" >> got a connection");
				Thread clientThread = new Thread(() => spawnHandleClientThread(handlerSocket));
				clientThread.Start();
			}

			// C# specs says
			// "it is recommended that you call Shutdown before the Close method.
			// This ensures that all data is sent and received on the conneceted socket 
			// before it is closed
		//	m_listenerSocket.Shutdown(SocketShutdown.Both);
			m_listenerSocket.Close();
			Util.Log(" >> " + "shutting down server listening socket");						              
		}
       ));


		m_listenerThread.Start();
	}



	private void spawnHandleClientThread(Socket handlerSocket)
	{
		// 
		bool clientAlive = true;
		while (clientAlive)
		{
			Util.LogError("Handling client");
			Thread.Sleep(5000);
		}
		handlerSocket.Shutdown(SocketShutdown.Both);
		handlerSocket.Close();
	}

	// https://docs.microsoft.com/en-us/dotnet/framework/network-programming/synchronous-server-socket-example
	public void SyncAcceptConnection()
	{
		string data = null;
		Util.Log("Waiting for a connection...");

		Socket handler = m_listenerSocket.Accept();
		data = null;

		while (true)
		{
			bytes = new byte[1024];
			int bytesReceived = handler.Receive(bytes);
			data += Encoding.ASCII.GetString(bytes, 0, bytesReceived);
			if (data.IndexOf("<EOF>") > -1)
			{
				break;
			}
		}

		// Show the data on the console.
		Util.Log("Text received : " + data);

		// Echo the data back to the client
		byte[] msg = Encoding.ASCII.GetBytes(data);

		// shutting down the new socket
		handler.Send(msg);
		handler.Shutdown(SocketShutdown.Both);
		handler.Close();
	}


	// accepting connections asynchronously gives you the ability
	// to send and receive data with a separate execution thread

	// If you want the original thread to block after you call the 
	// BeginAccept method, use WaitHandle.WaitOne. 
	// Call the Set method on a ManualResetEvent in the callback method
	// when you want the original thread to continue executing


	// seems like the allDone.WaitOne is blocking. So how is this Async? 
	public void TryAsyncAcceptConnections()
	{
		Util.Log("Check to See if there is a connection...");
  
		try
		{
			// Set the event to nonsignaled state.  
			m_allDone.Reset();


			// from https://msdn.microsoft.com/en-us/library/5bb431f9(v=vs.110).aspx
			// I must pass the listening Socket object to BeginAccept
			// through the state parameter

			// BeginAccept uses a separate thread to execute the specified callback method
			// and blocks on EndAccept until a pending connection is 
			// retrieved
			m_listenerSocket.BeginAccept(
				new AsyncCallback(AsyncServerAcceptConnectionCallback),
				m_listenerSocket);

			// Wait until a connection is made before continuing.  
			m_allDone.WaitOne();
		}
		catch (Exception e)
		{
			Console.WriteLine("No connections...");
			Console.WriteLine(e.ToString());
		}

	}


	// my call back should invoke the EndAccept method. 
	public void AsyncServerAcceptConnectionCallback(IAsyncResult ar)
	{
		// Signal the main thread to continue.
		m_allDone.Set();

		Socket listerner = (Socket)ar.AsyncState;

		// EndAccept will return a new Socket object that you can use to send and 
		// receive data with the remote host. 
		Socket handler = listerner.EndAccept(ar);

		// Socket rawTcpSocketNew = m_listenerSocket.EndAccept(ar);
		Util.Log("Accepted a connection");

		/*
		// Create the state object
		StateObject state = new StateObject();

		state.workSocket = handler;

		handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
		*/

		string data = null;
		bytes = new byte[1024];
		int bytesReceived = handler.Receive(bytes);
		data += Encoding.ASCII.GetString(bytes, 0, bytesReceived);
		if (data.IndexOf("<EOF>") > -1)
		{
			return;
		}
	}


	public static void ReadCallback(IAsyncResult ar)
	{
		/*
		String content = String.Empty;

		// Retrieve the state object and the handler socket from asynchronous state object.
		StateObject state = (StateObject)ar.AsyncState;
		Socket handler = state.workSocket;

		// Read data from the client socket
		int bytesRead = handler.EndReceive(ar);

		if (bytesRead > 0)
		{
			// There might be more data, so store the data received so far.
			state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));


			// Check for end=of-file tag. If it is not there, read more data
			content = state.sb.ToString();
			if (content.IndexOf("<EOF>") > -1)
			{
				// All the data has been read from the client, Display it on the console
				Util.LogError("Read " + content.Length + " bytes from socket. \n Data : " + content.ToString());
				Send


			}


		}
		*/
	}






	public void processIncomingMessages()
	{
		// go through all connections
		// if they are connected, process incoming messages for that connection

		foreach (var connection in connections)
		{
			/*
			if ()
			{
				
			}
			*/
		}

	}



	public void update()
	{


		// 
		// tick simulation

	}

	public void stopHosting()
	{


	}
}
