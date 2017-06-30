using System;


public class GameClient
{

    public GameClient()
    {

    }

    public NetGameConnection connection;

    public void StartNetworkSession(string hostIPAddress)
    {
        connection = new NetGameConnection();
        connection.InitClientSideSocket();

        Util.Log("Esablishing Connection to " + hostIPAddress);

        connection.ConnectToHost(hostIPAddress, NetworkManager.SERVER_PORT);
        Util.Log("StartNetworkSession");
    }


    public void Pump()
    {
        connection.Pump();
    }


    public void Login()
    {
        Message message = Message.Login();
        connection.SendMessage(message);
    }

    public void SearchMatch()
    {
        Message message = Message.SearchMessage();
        connection.SendMessage(message);
    }

}

