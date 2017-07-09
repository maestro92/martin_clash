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




	private string m_curStringPayload;
	public byte[] sendDataBuffer;
//	public const int sendDataBufferSize = 2048;

	public byte[] receiveDataBuffer;
	public const int receiveDataBufferSize = 2048;


	public Action<Message> OnHandleMessage;
	public NetGameConnection()
    {
		//        SetConnectionState(NetGameConnectionState.DISCONNECTED); 

		m_sendListLock = new Object();
		m_receiveMessageListLock = new Object();

        m_sendMessageList = new List<Message>();
		m_receiveMessageList = new List<Message>();

		m_curStringPayload = "";

		sendDataBuffer = new byte[1];
		receiveDataBuffer = new byte[receiveDataBufferSize];


	}

    private Socket                  m_rawTcpSocket = null;
    private NetGameConnectionState m_connectionState = NetGameConnectionState.DISCONNECTED;


    public void SetConnectionState(NetGameConnectionState state)
    {
        m_connectionState = state;
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
        m_rawTcpSocket.Connect(ipAddress, port);
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

			string data = "";
			foreach (var msg in tempSendList)
			{
				data += msg.Serialize();
			}

			sendDataBuffer = Encoding.ASCII.GetBytes(data);
			Util.LogError("sending data " + data);
			PrintBuffer(sendDataBuffer, sendDataBuffer.Length);

			m_rawTcpSocket.BeginSend(sendDataBuffer, 0, sendDataBuffer.Length, 0, new AsyncCallback(SendCallback), null);


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

        if (numBytesReceived > 0)
        {
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




	public void ProcessIncomingMessages()
	{
		lock (m_receiveMessageListLock)
		{
			while (m_receiveMessageList.Count != 0)
			{
				Message message = m_receiveMessageList[0];

				if (OnHandleMessage != null)
				{
					OnHandleMessage(message);
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

