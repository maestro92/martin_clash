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

    DISCONNECTED,
    CONNECTED,
};



public class NetGameConnection
{
	// need a lock around sendList 
	// just in case multiple threads are trying to call Message();
	// no need one for receive, cuz we only receive in one place
	public Object m_sendListLock;
	public List<Message> m_sendMessageList;

	public Object m_receiveMessageListLock;
	public List<Message> m_receiveMessageList;

//	private string m_curStringPayload;
	public byte[] sendDataBuffer;
//	public const int sendDataBufferSize = 2048;

	public byte[] receiveDataBuffer;
	public const int receiveDataBufferSize = 2048;

	// all these variables are in bytes
	private int m_curMsgSizeInfoIndex;
	private byte[] m_curMsgSizeInfoReadBuffer;
	private const int NUM_BYTES_FOR_HEADER_MESSAGE_SIZE = 4;

	private int m_curMsgDataIndex;
	private int m_curMsgDataSize;
	private byte[] m_curMsgDataReadBuffer;  // or also the payload

	private string m_connectionName;

	public Action<NetGameConnection, Message> OnHandleMessage;
	public NetGameConnection()
    {
		//        SetConnectionState(NetGameConnectionState.DISCONNECTED); 

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
	}

    private Socket                  m_rawTcpSocket = null;
    private NetGameConnectionState m_connectionState = NetGameConnectionState.DISCONNECTED;


    public void SetConnectionState(NetGameConnectionState state)
    {
        m_connectionState = state;
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


    public void ConnectToHost(string ipAddress, int port)
    {   
        // this is a blocking call

        bool connected = false; 
        try
        {
            
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


    private void OnClientSocketConnected()
    {
        // myself
        IPEndPoint localIpEndPoint = (IPEndPoint)(m_rawTcpSocket.LocalEndPoint);
        string localAddressAndPortString = localIpEndPoint.Address.ToString() + ":" + localIpEndPoint.Port.ToString();

        // the server, or whoever I am connected to
        IPEndPoint remoteIpEndPoint = (IPEndPoint)(m_rawTcpSocket.RemoteEndPoint);
        string remoteAddressAndPortString = remoteIpEndPoint.Address.ToString() + ":" + remoteIpEndPoint.Port.ToString();
        string connectionName = remoteAddressAndPortString;

        string tmpStr = "ClientConnectCallback(client@" + localAddressAndPortString + " " + this.m_connectionName + " to server@" + remoteAddressAndPortString + ")";
        Util.Log(tmpStr);
    }

	public void Shutdown()
	{
		if (m_rawTcpSocket != null)
		{
			m_rawTcpSocket.Shutdown(SocketShutdown.Both);
			m_rawTcpSocket.Close();
		}
	}



    public void SendMessage(Message message)
    {
        Util.Log("calling SendMessage " + message.type.ToString());
		lock (m_sendListLock)
		{
			m_sendMessageList.Add(message);
		}
    }



	// this is called only in one thread
    public void SocketSend()
    {
		// Begin sending the data to the remote device.  
		List<Message> tempSendList = null;
		lock(m_sendListLock)
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
            MemsetZeroBuffer(sendDataBuffer, sendDataBuffer.Length);

			NetSerializer writer = NetSerializer.GetOne();
			writer.SetupWriteMode("SocketSend", Globals.SerializeWithDebugMarkers);


			/*
			msg0 5

			msg1 7

			- ----- - -------

			0		6


			*/

			int oldCount = writer.GetWriteBufferNumBytes();
			int newCount = writer.GetWriteBufferNumBytes();
			// string data = "";
			foreach (var msg in tempSendList)
			{
				// size of msg, putting 0 for now as a place holder
				Int32 msgSizeHeader = 0;
				writer.WriteInt32("msgSizeHeader", msgSizeHeader);
				msg.Serialize(writer);

				newCount = writer.GetWriteBufferNumBytes();
				int msgSize = newCount - oldCount - 1;
				writer.WriteInt32AtIndex("msgSizeHeader", newCount, oldCount);

				oldCount = newCount;
			}

			sendDataBuffer = writer.GetWriteBufferByteArray();
			int numBytes = writer.GetWriteBufferNumBytes();

			m_rawTcpSocket.BeginSend(sendDataBuffer, 0, numBytes, 0, new AsyncCallback(SendCallback), null);
        }
    }





    private void SendCallback(IAsyncResult ar)
    {
        // Complete sending the data to the remote device
		int byteSent = m_rawTcpSocket.EndSend(ar);
        Util.Log("Sent " + byteSent.ToString() + " to client");
    }


    public void SocketReceive()
    {
//		MemsetZeroBuffer(receiveDataBuffer, receiveDataBufferSize);

        // apparenltly BeginReceive and BeginSend is thread-safe, so you can call them without locks
        m_rawTcpSocket.BeginReceive(receiveDataBuffer, 0, receiveDataBufferSize, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
    }



    private void ReceiveCallback(IAsyncResult ar)
	{
		int numBytesReceived = m_rawTcpSocket.EndReceive(ar);

		// byte counter
		int i = 0;

		if (numBytesReceived > 0)
		{

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
					m_curMsgDataReadBuffer[m_curMsgSizeInfoIndex] = receiveDataBuffer[i];
					m_curMsgDataIndex++;

					if (m_curMsgDataIndex == m_curMsgDataSize)
					{
						Message message = Message.GetOne();
						// deserialize current payload to a message

						NetSerializer reader = NetSerializer.GetOne();
						reader.SetupReadMode("ReceiveCallback", Globals.SerializeWithDebugMarkers, m_curMsgDataReadBuffer, m_curMsgDataSize);

						message.Deserializer(reader);
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


			/*
			NetSerializer reader = NetSerializer.GetOne();

			reader.SetupReadMode();

			// string data = "";
			foreach (var msg in tempSendList)
			{
				msg.Serialize(writer);
			}

			string stream = Encoding.ASCII.GetString(receiveDataBuffer, 0, numBytesReceived);

			PrintBuffer(receiveDataBuffer, numBytesReceived);

			Util.LogError("Here");
			Util.LogError("Received stuff: numBytesReceived " + numBytesReceived.ToString() + " " + stream.ToString());



			Util.LogError("temp is " + stream.ToString());
			stream = m_curStringPayload + stream;


			int index = stream.IndexOf(Message.MSG_DIVIDER, StringComparison.CurrentCulture);

			while (index != -1)
			{
				string sub = stream.Substring(0, index);
				Message message = Message.GetOne();
				message.Deserialize(sub);

				stream = stream.Substring(index + Message.MSG_DIVIDER.Length, stream.Length - (sub.Length + Message.MSG_DIVIDER.Length));
				index = stream.IndexOf(Message.MSG_DIVIDER, StringComparison.CurrentCulture);

				Util.LogError("type is " + message.type.ToString());
				Util.LogError("data is " + message.data);

				lock (m_receiveMessageListLock)
				{
					m_receiveMessageList.Add(message);
				}
			}

			m_curStringPayload = stream;
			*/
		}
	}



	/*
    private void ReceiveCallback(IAsyncResult ar)
    {
		int numBytesReceived = m_rawTcpSocket.EndReceive(ar);

        if (numBytesReceived > 0)
        {
			NetSerializer reader = NetSerializer.GetOne();

			reader.SetupReadMode();

			// string data = "";
			foreach (var msg in tempSendList)
			{
				msg.Serialize(writer);
			}

			string stream = Encoding.ASCII.GetString(receiveDataBuffer, 0, numBytesReceived);

			PrintBuffer(receiveDataBuffer, numBytesReceived);

			Util.LogError("Here");
			Util.LogError("Received stuff: numBytesReceived " + numBytesReceived.ToString() + " " + stream.ToString());



			Util.LogError("temp is " + stream.ToString());
			stream = m_curStringPayload + stream;


			int index = stream.IndexOf(Message.MSG_DIVIDER, StringComparison.CurrentCulture);

			while (index != -1)
			{
				string sub = stream.Substring(0, index);
				Message message = Message.GetOne();
				message.Deserialize(sub);

				stream = stream.Substring(index + Message.MSG_DIVIDER.Length, stream.Length - (sub.Length + Message.MSG_DIVIDER.Length));
				index = stream.IndexOf(Message.MSG_DIVIDER, StringComparison.CurrentCulture);

				Util.LogError("type is " + message.type.ToString());
				Util.LogError("data is " + message.data);

				lock(m_receiveMessageListLock)
				{
					m_receiveMessageList.Add(message);
				}	
			}

			m_curStringPayload = stream;
        }
    }
    */




	public void ProcessIncomingMessages()
	{
		lock (m_receiveMessageListLock)
		{
			// probably better to use a queue?
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


	// pump means tick
	public void Pump()
    {
		SocketSend();
		SocketReceive();
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

