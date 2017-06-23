using System;


public class GameClient
{

    public GameClient()
    {

    }




    public NetGameConnection connection;
//    public Socket 
    public void StartNetworkSession(string hostIPAddress)
    {

        connection = new NetGameConnection();

        Util.Log("Esablishing Connection to " + hostIPAddress);

        connection.ConnectToHost(hostIPAddress, NetworkManager.SERVER_PORT);
        Util.Log("StartNetworkSession");
    }



    // all the messages
    public void SearchMatch()
    {
        Message message = Message.GetOne(Message.Type.SearchMatch);
        connection.SendMessage(message);
    }

}

