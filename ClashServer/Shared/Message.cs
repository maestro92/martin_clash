using System;




public class Message
{

	public static string MSG_TYPE_DIVIDER = "<MSG>";
	public static string MSG_DIVIDER = "<EOF>";

    public enum Type
    {
        None,
        ClientConnectRequest,   // client to server
        ServerConnectResponse,  // server to client
        Login,
        LoginResponse,
        SearchMatch,
		BattleStartingInfo,
    };

    public Type type;
    public string data; // using string 

	public BattleStartingInfo bs;
	public int mapId;	// just testing writeIn using this

    private Message()
    {
        type = Type.None;
        data = "";
    }

	public static Message GetOne()
	{
		return GetOne(Type.None);
	}


	public static Message GetOne(Message.Type type)
    {
        Message message = new Message();
        message.type = type;
        return message;
    }


	public static Message GetOne(String str)
	{
		Message message = new Message();

		int index = str.IndexOf(MSG_TYPE_DIVIDER, StringComparison.CurrentCulture);

		string strType = str.Substring(0, index);
		int numType = Int32.Parse(strType);

		string strData = str.Substring(index+MSG_TYPE_DIVIDER.Length, str.Length - strType.Length + MSG_TYPE_DIVIDER.Length);

		message.type = (Message.Type)(numType);
		message.data = strData;
		return message;
	}

	public static Message SearchMessage()
	{
		Message message = Message.GetOne(Type.SearchMatch);
		return message;
	}


	public static Message BattleStartingInfo(BattleStartingInfo bs)
	{
		Message message = Message.GetOne(Type.BattleStartingInfo);
		message.bs = bs;
		return message;
	}


    public static Message Login()
    {
        Message message = Message.GetOne(Type.Login);
        message.data = "Login";
        return message;
    }

    public static Message LoginResponse()
    {
        Message message = Message.GetOne(Type.LoginResponse);
        message.data = "LoginResponse";
        return message;
    }

    public static Message ClientConnectRequest()
    {
        Message message = Message.GetOne(Type.ClientConnectRequest);
        message.data = "Client Connect Request";
        return message;
    }

    public static Message ServerConnectResponse()
    {
        Message message = Message.GetOne(Type.ServerConnectResponse);
        message.data = "Server Connect Response";
        return message;
    }

	public void Serialize(NetSerializer writer)
	{
		switch (type)
		{
			case Message.Type.SearchMatch:

			break;

			case Message.Type.BattleStartingInfo:
				//	writer.WriteOne<BattleStartingInfo>("battleStartingInfo", bs);

				writer.WriteInt32("mapId", mapId);
				break;
				



		}
	}

	public void Deserializer(NetSerializer reader)
	{
		switch (type)
		{
			case Message.Type.SearchMatch:

				break;
				
			case Message.Type.BattleStartingInfo:
				mapId = reader.ReadInt32("mapId");
				break;
		}
	}

	/*
	public string Serialize()
	{
		return ((int)(type)).ToString() + MSG_TYPE_DIVIDER + data + MSG_DIVIDER;
	}

	public void Deserialize(string stream)
	{
		int index = stream.IndexOf(MSG_TYPE_DIVIDER, StringComparison.CurrentCulture);

		string strType = stream.Substring(0, index);
		int numType = Int32.Parse(strType);

		string strData = stream.Substring(index + MSG_TYPE_DIVIDER.Length, stream.Length - (strType.Length + MSG_TYPE_DIVIDER.Length));

		type = (Message.Type)(numType);
		data = strData;
	}
	*/
	/*
	public void Serialize(string stream)
	{
		stream += ((int)(type)).ToString() + MSG_TYPE_DIVIDER + data + MSG_DIVIDER;
	}

	public void Deserialize(string stream)
	{
		int index = stream.IndexOf(MSG_TYPE_DIVIDER, StringComparison.CurrentCulture);

		string strType = stream.Substring(0, index);
		int numType = Int32.Parse(strType);

		string strData = stream.Substring(index + MSG_TYPE_DIVIDER.Length, stream.Length - strType.Length + MSG_TYPE_DIVIDER.Length);

		type = (Message.Type)(numType);
		data = strData;
	}
	*/

}

