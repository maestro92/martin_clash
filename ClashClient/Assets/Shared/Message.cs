﻿using System;




public class Message : INetSerializer
{

	public static string MSG_TYPE_DIVIDER = "<MSG>";
	public static string MSG_DIVIDER = "<EOF>";

    public enum Type
    {
        None,
        Filler1,
        Filler2,
        Filler3,
        ClientConnectRequest,   // client to server
        ServerConnectResponse,  // server to client
        Login,
        LoginResponse,
        SearchMatch,
		BattleStartingInfo,
    };

    public Type type;

	public BattleStartingInfo bs;
//	public int mapId;	// just testing writeIn using this

    private Message()
    {
        type = Type.None;
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
	//	message.data = strData;
		return message;
	}

	public static Message SearchMatch()
	{
		Message message = Message.GetOne(Type.SearchMatch);
		return message;
	}


	public static Message BattleStartingInfoMessage(BattleStartingInfo bs)
	{
		Message message = Message.GetOne(Type.BattleStartingInfo);
		message.bs = bs;
		return message;
	}


    public static Message Login()
    {
        Message message = Message.GetOne(Type.Login);
      //  message.data = "Login";
        return message;
    }

    public static Message LoginResponse()
    {
        Message message = Message.GetOne(Type.LoginResponse);
     //   message.data = "LoginResponse";
        return message;
    }

    public static Message ClientConnectRequest()
    {
        Message message = Message.GetOne(Type.ClientConnectRequest);
    //    message.data = "Client Connect Request";
        return message;
    }

    public static Message ServerConnectResponse()
    {
        Message message = Message.GetOne(Type.ServerConnectResponse);
    //    message.data = "Server Connect Response";
        return message;
    }

	public void Serialize(NetSerializer writer)
	{
        writer.WriteEnumAsInt<Message.Type>(type, "message.type");

		switch (type)
		{
            case Message.Type.ClientConnectRequest:
                Util.Log("serialize ClientConnectRequest");
                break;

            case Message.Type.Login:
                Util.Log("Login");
                break;

            case Message.Type.LoginResponse:
                Util.Log("serialize LoginResponse ");
                break;


			case Message.Type.SearchMatch:
                Util.Log("serialize SearchMatch");
			    break;

			case Message.Type.ServerConnectResponse:
				Util.Log("serialize ServerConnectResponse");
				break;

			case Message.Type.BattleStartingInfo:
                Util.Log("message serialize bs");
				writer.WriteOne<BattleStartingInfo>(bs, "battleStartingInfo");
				break;

            default:                            
                throw new Exception("Unsupported message type \"" + type.ToString() + "\"!!!");
                break;
		}
	}

	public void Deserialize(NetSerializer reader)
	{

        type = reader.ReadEnumAsInt<Message.Type>("message.type");
        Util.LogError("\t\tDeserializing Msg " + type.ToString());
        switch (type)
		{
			case Message.Type.ClientConnectRequest:
				Util.Log("msg deserialize ClientConnectRequestion");
				break;

            case Message.Type.Login:
                Util.Log("deserialize Login ");
                break;

            case Message.Type.LoginResponse:
                Util.Log("deserialize LoginResponse ");
                break;


			case Message.Type.SearchMatch:

				break;
				
			case Message.Type.ServerConnectResponse:
				Util.Log("deserialize ServerConnectResponse");
				break;
				
            case Message.Type.BattleStartingInfo:
                Util.Log("message deserialize, deserailize");
                Util.LogError("message BattleStartingInfo, deserailize");

                bs = reader.ReadOne<BattleStartingInfo>(() => BattleStartingInfo.GetOne(), "battleStartingInfo");


                Util.LogError("#### BattleStartingInfo MaiMeng");

                bs.Print();
                break;

            default:
                throw new Exception("Message.Deserialize() : produced unsupported message type: " + type.ToString() + "!!!");
                break;
		}


		Print();
	}

	public void Print()
	{
		Util.Log("\t\t>>type " + type.ToString());
    //    Util.Log("mapId " + mapId.ToString());

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

