using System;
using UnityEngine;

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

        CastCard,
        EndFrame,

        SysPing,
        SysHeartbeat,
    };

    public bool wantReply;
    public int pingId;
    public Int64 timeStampInMs;

    // for SysHeartbeat
    public int cargoSize;

    public Type type;
	public int userId;
    public int playerId;
    public Enums.Team teamId;
    public Vector3 simPosition;
    public int frameCount;
	public BattleStartingInfo bs;
    public Card card;
	public Enums.CardType cardType;
    public ServerFrameInfo serverFrameInfo;
//	public int mapId;	// just testing writeIn using this

    public bool isSimFrameSensitive;

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
        message.isSimFrameSensitive = false;
		return message;
	}

    public static Message SysPingMessage(int pingIdIn, Int64 timeStampInMsIn, bool wantReplyIn)
    {
        Message message = Message.GetOne(Type.SysPing);
        message.pingId = pingIdIn;
        message.timeStampInMs = timeStampInMsIn;
        message.wantReply = wantReplyIn;
        return message;     
    }

    public static Message SysHeartbeatMessage(Int64 timeStampInMsIn, int cargoSizeIn, bool wantReplyIn)
    {
        Message message = Message.GetOne(Type.SysHeartbeat);
        message.timeStampInMs = timeStampInMsIn;
        message.cargoSize = cargoSizeIn;
        message.wantReply = wantReplyIn;
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

    public static Message LoginResponse(int userId)
    {
        Message message = Message.GetOne(Type.LoginResponse);
		message.userId = userId;
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

    public static Message CastCard(Enums.CardType cardType, int playerId, Vector3 simPosition, int frameCount, bool isSimFrameSensitive)
    {
        Message message = Message.GetOne(Type.CastCard);
        message.cardType = cardType;
        message.playerId = playerId;
        message.simPosition = simPosition;
        message.frameCount = frameCount;
        message.isSimFrameSensitive = isSimFrameSensitive;
        return message;
    }

        
    public static Message EndFrame(ServerFrameInfo serverFrameInfo)
    {
        Message message = Message.GetOne(Type.EndFrame);
		message.serverFrameInfo = serverFrameInfo;
        return message;
    }

	public void Serialize(NetSerializer writer)
	{
        writer.WriteEnumAsInt<Message.Type>(type, "message.type");


		switch (type)
		{
            case Message.Type.ClientConnectRequest:
       //         Util.Log("serialize ClientConnectRequest");
                break;

            case Message.Type.Login:
        //        Util.Log("Login");
                break;

            case Message.Type.LoginResponse:
          //      Util.Log("serialize LoginResponse ");
                writer.WriteInt32(userId, "userId");
                break;

			case Message.Type.SearchMatch:
         //       Util.Log("serialize SearchMatch");
			    break;

			case Message.Type.ServerConnectResponse:
		//		Util.Log("serialize ServerConnectResponse");
				break;

            case Message.Type.CastCard:
                writer.WriteEnumAsInt<Enums.CardType>(cardType, "cardType");
                writer.WriteInt32(playerId, "playerId");
                writer.WriteVector3(simPosition, "simPosition");
                writer.WriteInt32(frameCount, "frameCount");
                writer.WriteBool(isSimFrameSensitive, "isSimFrameSensitive");
                break;

			case Message.Type.BattleStartingInfo:
          //      Util.Log("message serialize bs");
				writer.WriteOne<BattleStartingInfo>(bs, "battleStartingInfo");
				break;
            
            case Message.Type.EndFrame:
           //     Util.Log("EndFrame");
                writer.WriteOne<ServerFrameInfo>(serverFrameInfo, "ServerFrameInfo");
				break;

            case Message.Type.SysHeartbeat:
                writer.WriteInt64(timeStampInMs, "timeStampInMs");
                writer.WriteInt32(cargoSize, "cargoSize");
                writer.WriteBool(wantReply, "wantReply");
                break;

            case Message.Type.SysPing:
                writer.WriteInt32(pingId, "pingId");
                writer.WriteInt64(timeStampInMs, "timeStampInMs");
                writer.WriteBool(wantReply, "wantReply");
                break;
            default:   
                Util.LogError("Message.Deserialize() : produced unsupported message type: " + type.ToString() + "!!!");
                throw new Exception("Unsupported message type \"" + type.ToString() + "\"!!!");
                break;
		}
	}

	public void Deserialize(NetSerializer reader)
	{

        type = reader.ReadEnumAsInt<Message.Type>("message.type");
    //    Util.LogError("\t\tDeserializing Msg " + type.ToString());



        switch (type)
		{
			case Message.Type.ClientConnectRequest:
			//	Util.Log("msg deserialize ClientConnectRequestion");
				break;

            case Message.Type.Login:
            //    Util.Log("deserialize Login ");

                break;

            case Message.Type.LoginResponse:
            //    Util.Log("deserialize LoginResponse ");
                userId = reader.ReadInt32("userId");
                break;


			case Message.Type.SearchMatch:

				break;
				
			case Message.Type.ServerConnectResponse:
			//	Util.Log("deserialize ServerConnectResponse");
				break;
				
            case Message.Type.BattleStartingInfo:
          //      Util.Log("message deserialize, deserailize");
          //      Util.LogError("message BattleStartingInfo, deserailize");

                bs = reader.ReadOne<BattleStartingInfo>(() => BattleStartingInfo.GetOne(), "battleStartingInfo");


          //      Util.LogError("#### BattleStartingInfo MaiMeng");

                bs.Print();
                break;

            case Message.Type.CastCard:
                cardType = reader.ReadEnumAsInt<Enums.CardType>("cardType");
                playerId = reader.ReadInt32("playerId");
                simPosition = reader.ReadVector3("simPosition");
                frameCount = reader.ReadInt32("frameCount");
                isSimFrameSensitive = reader.ReadBool("isSimFrameSensitive");
                break;


            case Message.Type.EndFrame:
           //     Util.LogError("message EndFrame, deserailize");
                serverFrameInfo = reader.ReadOne<ServerFrameInfo>(() => ServerFrameInfo.GetOne(), "serverFrameInfo"); 

                break;

            case Message.Type.SysHeartbeat:
                timeStampInMs = reader.ReadInt64("timeStampInMs");
                cargoSize = reader.ReadInt32("cargoSize");
                wantReply = reader.ReadBool("wantReply");
                break;

            case Message.Type.SysPing:
                pingId = reader.ReadInt32("pingId");
                timeStampInMs = reader.ReadInt64("timeStampInMs");
                wantReply = reader.ReadBool("wantReply");
                break;

            default:
                Util.LogError("Message.Deserialize() : produced unsupported message type: " + type.ToString() + "!!!");
                throw new Exception("Message.Deserialize() : produced unsupported message type: " + type.ToString() + "!!!");
                break;
		}


	//	Print();
	}

	public void Print()
	{
	//	Util.Log("\t\t>>type " + type.ToString());
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

