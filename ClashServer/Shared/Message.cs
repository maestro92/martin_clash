using System;




public class Message
{

	public static string MSG_TYPE_DIVIDER = "<MSG>";
	public static string MSG_DIVIDER = "<EOF>";

    public enum Type
    {
        None,
        Login,
        SearchMatch,
    };

    public Type type;
    public string data; // using string 
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



    public static Message Login()
    {
        Message message = Message.GetOne(Type.Login);
        message.data = "Login";
        return message;
    }


    public static Message SearchMessage()
    {
        Message message = Message.GetOne(Type.SearchMatch);
        message.data = "SearchMatch";
        return message;
    }

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

