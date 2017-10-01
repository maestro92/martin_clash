using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public enum NetGameConnectionState
{
    None,

    Disconnected,
    Connected,
    ClientContactingServer,
    ClientWaitingForServerValidation,
    ClientConnectError,
};
// we need a multiple bool "socketConnected" since there are multiple states assumes "socketConnected"
// For Example: 
//      ServerWaitingValidation
//      ClientWaitingForServerValidation
//      
// so we'll use an extra bool to do it

public enum NetGameConnectionSide
{
    None, 
    ServerSide,
    ClientSide,
}


public class NetGameConnection
{
	// need a lock around sendList 
	// just in case multiple threads are trying to call Message();
	// no need one for receive, cuz we only receive in one place
	public Object m_sendListLock;
	public List<Message> m_sendMessageList;

	public Object m_receiveMessageListLock;
	public List<Message> m_receiveMessageList;

	public bool sendInFlightFlag = false;
	public bool receiveInFlightFlag = false;
    public bool clientConnectToServerInFlightFlag = false;
    public double clientConnectToServerStartTime;
    private bool m_socketConnected = false;


//	private string m_curStringPayload;
	public byte[] sendDataBuffer;
//	public const int sendDataBufferSize = 2048;

	public byte[] receiveDataBuffer;
	public const int receiveDataBufferSize = 256;

	// all these variables are in bytes
	private int m_curMsgSizeInfoIndex;
	private byte[] m_curMsgSizeInfoReadBuffer;
	private const int NUM_BYTES_FOR_HEADER_MESSAGE_SIZE = 4;

	private int m_curMsgDataIndex;
	private int m_curMsgDataSize;
	private byte[] m_curMsgDataReadBuffer;  // or also the payload

    public string serverIpAddress;
    public int serverPort;

	private string m_connectionName;

    public double ClientConnectTimeOut_ms;

	public Action<NetGameConnection, Message> OnHandleMessage;
	
    private Socket                  m_rawTcpSocket = null;
    private NetGameConnectionState m_connectionState = NetGameConnectionState.None;
    private NetGameConnectionSide m_connectionSide = NetGameConnectionSide.None;

    public NetGameConnection()
    {
		//        SetConnectionState(NetGameConnectionState.DISCONNECTED); 
        sendInFlightFlag = false;
        receiveInFlightFlag = false;
        clientConnectToServerInFlightFlag = false;

		m_sendListLock = new Object();
		m_receiveMessageListLock = new Object();

        m_sendMessageList = new List<Message>();
		m_receiveMessageList = new List<Message>();

		m_connectionName = "";

		sendDataBuffer = new byte[1];
		receiveDataBuffer = new byte[receiveDataBufferSize];

		m_curMsgSizeInfoIndex = 0;
		m_curMsgSizeInfoReadBuffer = new byte[NUM_BYTES_FOR_HEADER_MESSAGE_SIZE];

		m_curMsgDataIndex = 0;
		m_curMsgDataSize = 0;
		m_curMsgDataReadBuffer = null;

        ClientConnectTimeOut_ms = 5000;
        m_connectionState = NetGameConnectionState.None;
        m_connectionSide = NetGameConnectionSide.None;
	}


    public bool IsClientSide()
    {
        return m_connectionSide == NetGameConnectionSide.ClientSide;
    }

    public bool IsServerSide()
    {
        return m_connectionSide == NetGameConnectionSide.ServerSide;
    }

    public void SetConnectionSide(NetGameConnectionSide side)
    {
        m_connectionSide = side;
    }

    public void SetConnectionState(NetGameConnectionState state)
    {
        m_connectionState = state;
    }

    public NetGameConnectionState GetConnectionState()
    {
        return m_connectionState;
    }

	public void SetConnectionName(string name)
	{
		m_connectionName = name;
	}

	public string GetConnectionName()
	{
		return m_connectionName;
	}
    // receive queue



    /*
    difference between BeginConnect and Connect

        Connect: 
            synchronous, it will bock your current thread until the connection is made

        BeginConnect:
            is asynchornous

        https://stackoverflow.com/questions/5416190/socket-beginconnect-vs-socket-connect

        lets try Connect first
    */ 




    ////////////////////////////////////////////////////
    // Server Side Specific Functions
    ////////////////////////////////////////////////////
    public void InitServerSideSocket(Socket socket)
    {
        m_rawTcpSocket = socket;
    }



    ////////////////////////////////////////////////////
    // Client Side Specific Functions
    ////////////////////////////////////////////////////
    public void InitClientSideSocket()
    {
        m_rawTcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }

    public void SetConnectToHostInfo(string ipAddress, int port)
    {

        this.serverIpAddress = ipAddress;
        this.serverPort = port;
        this.SetConnectionState(NetGameConnectionState.ClientContactingServer);
    }

    /*
    public void ConnectToHost(string ipAddress, int port)
    {   
        // this is a blocking call

        bool connected = false; 
        try
        {
            SetConnectionState(NetGameConnectionState.ClientContactingServer);
            m_rawTcpSocket.Connect(ipAddress, port);
            connected = true;

            OnClientSocketConnected();
        }
        catch( System.Net.Sockets.SocketException socketExceptionIn )
        {
            Util.LogError("Socket Exception, can't connect");
        }

        if (connected)
        {
            // if socket connection succeeds, start sending a connection request

            Message message = Message.ClientConnectRequest();
            SendMessage(message);
        }
    }
*/






	public void Shutdown()
	{
        SetConnectionState(NetGameConnectionState.Disconnected);
		if (m_rawTcpSocket != null)
		{
			m_rawTcpSocket.Shutdown(SocketShutdown.Both);
			m_rawTcpSocket.Close();
		}
	}



    public void SendMessage(Message message)
    {
    //    Util.Log("\t>>> calling SendMessage " + message.type.ToString());
		lock (m_sendListLock)
		{
			m_sendMessageList.Add(message);
		}
    }






	public void SetSendInFlightFlag(bool flag, string whereIn)
	{
		if (sendInFlightFlag == flag)
		{
			
		}
		else
		{
			sendInFlightFlag = flag;
		//	Util.Log("Calling SetSendInFlightFlag from " + whereIn);
		}
	}


	public void SetReceiveInFlightFlag(bool flag, string whereIn)
	{
		if (receiveInFlightFlag == flag)
		{

		}
		else
		{
			receiveInFlightFlag = flag;
		//	Util.Log("Calling SetReceiveInFlightFlag from " + whereIn);
		}
	}

    public void SetClientConnectToserverInFlightFlag(bool flag)
    {
        if (clientConnectToServerInFlightFlag == flag)
        {

        }
        else
        {
            clientConnectToServerInFlightFlag = flag;
        }
    }




    // this is called only in one thread
    public void PumpSocketSend()
    {
        //     Util.LogError("PumpSocketSend " + m_connectionState.ToString());

        if (sendInFlightFlag == true)
        {
            //  Util.Log("balling cuz sendInFlightFLag is true ");
        }
        else if (m_connectionState != NetGameConnectionState.Connected)
        {
            //   Util.LogError("PumpSocketSend Error, not Connected ");
        }
        else if (m_rawTcpSocket == null)
        {
            Util.LogError("m_rawTcpSocket == null ");
        }
        else
        {
            // Begin sending the data to the remote device.  
            List<Message> tempSendList = null;
            lock (m_sendListLock)
            {
                tempSendList = new List<Message>();
                foreach (var msg in m_sendMessageList)
                {
                    tempSendList.Add(msg);
                }

                m_sendMessageList.Clear();
            }

            if (tempSendList.Count > 0)
            {
            //    Util.Log("\t>>> Sending shit in SocketSend");

                MemsetZeroBuffer(sendDataBuffer, sendDataBuffer.Length);

                NetSerializer writer = NetSerializer.GetOne();
                writer.SetupWriteMode("SocketSend", Globals.SerializeWithDebugMarkers);


                /*
                msg0 5

                msg1 7

                - ----- - -------

                0       6


                */

                //  Util.LogError("tempSendList count " + tempSendList.Count.ToString());

                int oldCount = writer.GetWriteBufferNumBytes();
                int newCount = writer.GetWriteBufferNumBytes();
                // string data = "";
                foreach (var msg in tempSendList)
                {
                    // size of msg, putting 0 for now as a place holder
                    Int32 msgSizeHeader = 0;

                    writer.WriteInt32(msgSizeHeader, "msgSizeHeader");


                    msg.Serialize(writer);

                    newCount = writer.GetWriteBufferNumBytes();

                    int msgSize = newCount - oldCount - 4;
                    writer.WriteInt32AtIndex(oldCount, msgSize, "msgSizeHeader");
              //      Util.LogError("Write msgSize " + msgSize.ToString());
                    oldCount = newCount;
                }

                sendDataBuffer = writer.GetWriteBufferByteArray();
                int numBytes = writer.GetWriteBufferNumBytes();

                /*
                int i = 0;
                while (i < numBytes)
                {
                    Util.LogError("byte is " + sendDataBuffer[i]);
                    i++;
                }
                Util.LogError("Sending " + numBytes.ToString() + " of data");
                */
                SetSendInFlightFlag(true, "BeingSend");
                m_rawTcpSocket.BeginSend(sendDataBuffer, 0, numBytes, 0, new AsyncCallback(SendCallback), null);
            //    Util.LogError("BeginSend");
            }
        }
    }

    private void SendCallback(IAsyncResult ar)
    {
		bool endSendSuccess = false;

		try
		{
			// Complete sending the data to the remote device
            int numByteSent = m_rawTcpSocket.EndSend(ar);
		//	Util.Log("Sent " + byteSent.ToString() + " to client");
			endSendSuccess = true;

            if( ( NetGlobal.netMeter != null ) && ( NetGlobal.netMeter.IsEnabled() == true ) )
            {
                lock(NetGlobal.netMeter)
                {
                    if(IsClientSide())
                    {
                        NetGlobal.netMeter.Record(NetMeter.EntryFlag.Client | 
                                                        NetMeter.EntryFlag.Send | 
                                                        NetMeter.EntryFlag.Tcp, numByteSent);
                    }
                    else if (IsServerSide())
                    {
                        NetGlobal.netMeter.Record(NetMeter.EntryFlag.Server | 
                                                        NetMeter.EntryFlag.Send | 
                                                        NetMeter.EntryFlag.Tcp, numByteSent);
                    }
                }
            }
		}
		catch (System.Net.Sockets.SocketException socketExceptionIn)
		{
			endSendSuccess = false;
		}
		catch (System.Exception exceptionIn)
		{
			endSendSuccess = false;
		}

		if (endSendSuccess == true)
		{
			SetSendInFlightFlag(false, "SendCallback");
		}
	}


	public void PumpSocketReceive()
	{
    //    Util.LogError("PumpSocketReceive " + m_connectionState.ToString());
		//		MemsetZeroBuffer(receiveDataBuffer, receiveDataBufferSize);

		// apparenltly BeginReceive and BeginSend is thread-safe, so you can call them without locks
		// Util.Log("SocketReceive");
		if (receiveInFlightFlag == true)
		{
		//	Util.Log("balling cuz receiveInFlightFlag is true ");
		}
        else if (m_connectionState != NetGameConnectionState.Connected)
        {
        //    Util.LogError("PumpSocketReceive Error, not Connected ");
        }
        else if (m_rawTcpSocket == null)
        {

        }
		else
		{
			SetReceiveInFlightFlag(true, "BeginReceive in SocketReceive");

			m_rawTcpSocket.BeginReceive(receiveDataBuffer, 0, receiveDataBufferSize, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
		}
	}



    private void ReceiveCallback(IAsyncResult ar)
	{
		int numBytesReceived = 0;
		bool endReceiveSuccess = false;

		try
		{
			numBytesReceived = m_rawTcpSocket.EndReceive(ar);
			endReceiveSuccess = true;
		}

		// a socket exception is the most general excepetion that signals
		// a problem when trying to open or access a socket
		// https://www.quora.com/What-is-SocketException-and-why-does-it-occur
		catch (System.Net.Sockets.SocketException socketExceptionIn)
		{
			endReceiveSuccess = false;
		}
		catch (SystemException exceptionIn)
		{
			endReceiveSuccess = false;
		}


	//	Util.LogError("ReceivedCallback, numBytesReceived " + numBytesReceived.ToString());
		// byte counter


		if(endReceiveSuccess == true)
		{
            if (numBytesReceived > 0)
            {
                if( NetGlobal.netMeter != null && NetGlobal.netMeter.IsEnabled() == true )
                {
                    lock( NetGlobal.netMeter )
                    {
                        if( IsClientSide() )
                        {
                            NetGlobal.netMeter.Record( NetMeter.EntryFlag.Client | NetMeter.EntryFlag.Receive | NetMeter.EntryFlag.Tcp, numBytesReceived );
                        }
                        else if( IsServerSide() )
                        {
                            NetGlobal.netMeter.Record( NetMeter.EntryFlag.Server | NetMeter.EntryFlag.Receive | NetMeter.EntryFlag.Tcp, numBytesReceived );
                        }
                    }
                }

                int i = 0;
                while (i < numBytesReceived)
                {
                    if (m_curMsgSizeInfoIndex < NUM_BYTES_FOR_HEADER_MESSAGE_SIZE)
                    {
                        // i'm assuming receiveDataBuffer is in network order?
                        m_curMsgSizeInfoReadBuffer[m_curMsgSizeInfoIndex] = receiveDataBuffer[i];
                        m_curMsgSizeInfoIndex++;

                        if (m_curMsgSizeInfoIndex == NUM_BYTES_FOR_HEADER_MESSAGE_SIZE)
                        {
                            if (BitConverter.IsLittleEndian == true)
                            {
                                Array.Reverse(m_curMsgSizeInfoReadBuffer);
                            }

                            m_curMsgDataSize = BitConverter.ToInt32(m_curMsgSizeInfoReadBuffer, 0);// data size
                            m_curMsgDataReadBuffer = new byte[m_curMsgDataSize];
                        }
                    }
                    else if (m_curMsgDataIndex < m_curMsgDataSize)
                    {
                        m_curMsgDataReadBuffer[m_curMsgDataIndex] = receiveDataBuffer[i];
                        m_curMsgDataIndex++;

                        if (m_curMsgDataIndex == m_curMsgDataSize)
                        {
                            Message message = Message.GetOne();
                            // deserialize current payload to a message

                            NetSerializer reader = NetSerializer.GetOne();
                            reader.SetupReadMode("ReceiveCallback", Globals.SerializeWithDebugMarkers, m_curMsgDataReadBuffer, m_curMsgDataSize);

                            message.Deserialize(reader);
                            // then add it to the receiveMsgList

                            lock (m_receiveMessageListLock)
                            {
                                m_receiveMessageList.Add(message);
                            }

                            m_curMsgSizeInfoIndex = 0;
                            m_curMsgDataIndex = 0;
                        }

                    }

                    i++;
                }
            }
            else
            {
                Util.LogError("numBytesReceived " + numBytesReceived.ToString());
            }
			SetReceiveInFlightFlag(false, "ReceiveCallback");
		}

	//	Util.LogError("At the end of ReceiveCallback");
	}
        


	public void ProcessIncomingMessages()
	{
		lock (m_receiveMessageListLock)
		{
			// probably better to use a queue?
        //    Util.LogError("m_receiveMessageList.Count " + m_receiveMessageList.Count.ToString());

			while (m_receiveMessageList.Count != 0)
			{
				Message message = m_receiveMessageList[0];

				if (OnHandleMessage != null)
				{
					OnHandleMessage(this, message);
				}
				m_receiveMessageList.RemoveAt(0);
			}
		}
	}


	private void MemsetZeroBuffer(byte[] buffer, int size)
	{
		for (int i = 0; i < size; i++)
		{
			buffer[i] = 0;
		}
	}


	private void PrintBuffer(byte[] buffer, int size)
	{
		string temp = "";
		for (int i = 0; i < size; i++)
		{
			temp += buffer[i].ToString();
		}
		Util.LogError(temp);
	}



    public void PumpClientConnect()
    {
        if( m_connectionState == NetGameConnectionState.ClientContactingServer )
        {
            double now = Util.GetRealTimeMS();

            if (clientConnectToServerInFlightFlag == false && (now - clientConnectToServerStartTime >= ClientConnectTimeOut_ms) )
            {
                clientConnectToServerInFlightFlag = true;
                PrivateAsyncClientConnect();
                clientConnectToServerStartTime = now;
            }
        }
    }


    private void PrivateAsyncClientConnect()
    {
        // blocking async version

        try
        {
            m_rawTcpSocket.Disconnect( true );
        }
        catch( System.Exception exceptionIn )
        {
            Util.LogError("PrivateAsyncClientConnect, m_rawTcpSocket.Disconnect exception");
        //    Util.UnusedVar<System.Exception>( exceptionIn );
        }
        try
        {
            m_rawTcpSocket.Close();
        }
        catch( System.Exception exceptionIn )
        {
            Util.LogError("PrivateAsyncClientConnect, m_rawTcpSocket.Close exception");
        //    Util.UnusedVar<System.Exception>( exceptionIn );
        }
        m_rawTcpSocket = null;

        InitClientSideSocket();


        try
        {
            Util.LogError("BeginConnect");
            m_rawTcpSocket.BeginConnect( serverIpAddress, serverPort, new AsyncCallback( PrivateAsyncClientConnectCallback ), m_rawTcpSocket );
        }
        catch( System.Exception exceptionIn )
        {
            Util.LogError( "PumpClientConnect " + ": Exception=\"" + exceptionIn.ToString() + "\"" );
            SetConnectionState( NetGameConnectionState.ClientConnectError );
        }
    }




    private void PrivateAsyncClientConnectCallback(IAsyncResult ar )
    {
        try
        {
            // to know if it is a successful connect,
            // if it fails, it will just go into Connection
            // otherwise, this will go through and run the OnConnectedCallBack

            if( m_rawTcpSocket == null )
            {
                return;
            }

            m_rawTcpSocket.EndConnect(ar); 

            OnClientSocketConnected();
        }
        catch( System.Net.Sockets.SocketException socketExceptionIn )
        {
            Util.LogError("socketExceptionIn " );
            if( socketExceptionIn.ErrorCode == (int) System.Net.Sockets.SocketError.ConnectionRefused ) // 10061 )
            {
                // no need to log this.  This means there isn't a server running at that IP (or there is one but it's not letting us in)
                Util.LogError("connection refused? " );
            }

            // Netw
            else if( socketExceptionIn.ErrorCode == (int) System.Net.Sockets.SocketError.NetworkDown )  // 10050 )
            {
                // no need to log this.  This means there isn't a server running at that IP (or there is one but it's not letting us in)
                Util.LogError("connection NetworkDown? " );
            }
            else
            {
                string errString = "SOCKET EXCEPTION! ErrorCode = " + socketExceptionIn.ErrorCode.ToString() + ", Exception = \"" + socketExceptionIn.ToString() + "\"";
                Util.LogError( errString );
            }
        }
        catch( System.Exception exceptionIn )
        {
            string errString = "EXCEPTION! Exception = \"" + exceptionIn.ToString() + "\"";
            Util.LogError( errString );
        }
        clientConnectToServerInFlightFlag = false;
    }


    public bool IsConnected()
    {
        return m_socketConnected;
    }


    private void OnClientSocketConnected()
    {
        Util.LogError("SetConnectionState(NetGameConnectionState.Connected)");;

        m_socketConnected = true;

        SetConnectionState(NetGameConnectionState.Connected);
        SetConnectionSide(NetGameConnectionSide.ClientSide);
        // myself
        IPEndPoint localIpEndPoint = (IPEndPoint)(m_rawTcpSocket.LocalEndPoint);
        string localAddressAndPortString = localIpEndPoint.Address.ToString() + ":" + localIpEndPoint.Port.ToString();

        // the server, or whoever I am connected to
        IPEndPoint remoteIpEndPoint = (IPEndPoint)(m_rawTcpSocket.RemoteEndPoint);
        string remoteAddressAndPortString = remoteIpEndPoint.Address.ToString() + ":" + remoteIpEndPoint.Port.ToString();
        string connectionName = remoteAddressAndPortString;

        string tmpStr = "ClientConnectCallback(client@" + localAddressAndPortString + " " + this.m_connectionName + " to server@" + remoteAddressAndPortString + ")";
        Util.Log(tmpStr);

        // if socket connection succeeds, start sending a connection request

        Message message = Message.ClientConnectRequest();
        SendMessage(message);
    }




    public void OnServerSocketConnected()
	{
		SetConnectionState(NetGameConnectionState.Connected);
		SetConnectionSide(NetGameConnectionSide.ServerSide);
	}
	/*
		public void AsyncClientConnectToHost(string ipAddress, int port)
		{
			// this is a blocking call

			bool connected = false; 
			try
			{
				SetConnectionState(NetGameConnectionState.ClientContactingServer);
				m_rawTcpSocket.Connect(ipAddress, port);
				connected = true;

				OnClientSocketConnected();
			}
			catch( System.Net.Sockets.SocketException socketExceptionIn )
			{
				Util.LogError("Socket Exception, can't connect");
			}

			if (connected)
			{
				// if socket connection succeeds, start sending a connection request

				Message message = Message.ClientConnectRequest();
				SendMessage(message);
			}

		}
			*/

	// pump means tick
	public void Pump()
    {
        PumpClientConnect();
		PumpSocketSend();
		PumpSocketReceive();
		ProcessIncomingMessages();

        // listen for stuff

        // Receive the response from the remote device.  


        // If the remote host shuts down the Socket connection with the Shutdown method, 
        // and all available data has been received, the Receive method will complete immediately and return zero bytes.


        /*
        byte[] bytes = new byte[1024]; 
        try 
        {           
            int bytesRec = m_rawTcpSocket.Receive(bytes);  
            Util.Log("Echoed test = " + Encoding.ASCII.GetString(bytes,0,bytesRec));
        }
        catch (SocketException e)
        {
            Console.WriteLine("{0} Error code: {1}.", e.Message, e.ErrorCode);
        }
        */

        /*
        while (m_sendList.Count > 0)
        {
            
        }
        */
    }
}

