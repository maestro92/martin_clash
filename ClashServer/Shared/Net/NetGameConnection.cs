using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;  

public enum NetGameConnectionState
{
    None,

    DISCONNECTED,
    CONNECTED,
};



public class NetGameConnection
{

    public NetGameConnection()
    {
        m_rawTcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        SetConnectionState(NetGameConnectionState.DISCONNECTED); 
    }

    private Socket                  m_rawTcpSocket = null;
    private NetGameConnectionState m_connectionState = NetGameConnectionState.DISCONNECTED;


    public void SetConnectionState(NetGameConnectionState state)
    {
        m_connectionState = state;
    }

    // send queue
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

//        int bytesReceive = m_rawTcpSocket.Receive(bytesSend)

    }
}

