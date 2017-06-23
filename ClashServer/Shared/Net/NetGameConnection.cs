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


    // send queue
//    public List<Message> m_sendQueue;
//    public List<Message> m_receiveQueue;
	public List<Message> m_sendList;
	public List<Message> m_receiveList;

    public NetGameConnection()
    {
        m_rawTcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        SetConnectionState(NetGameConnectionState.DISCONNECTED); 

        m_sendList = new List<Message>();
		m_receiveList = new List<Message>();
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

    public void ConnectToHost(string ipAddress, int port)
    {        
        m_rawTcpSocket.Connect(ipAddress, port);
        // connect to ipAddress
        SetConnectionState(NetGameConnectionState.CONNECTED);


        byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");

        int bytesSend = m_rawTcpSocket.Send(msg);

    }

    public void SendMessage(Message message)
    {
		m_sendList.Add(message);
    }

    // pump means tick
    public void Pump()
    {
        // listen for stuff

        // Receive the response from the remote device.  


        // If the remote host shuts down the Socket connection with the Shutdown method, 
        // and all available data has been received, the Receive method will complete immediately and return zero bytes.

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



        while (m_sendList.Count > 0)
        {
            
        }
    }
}

