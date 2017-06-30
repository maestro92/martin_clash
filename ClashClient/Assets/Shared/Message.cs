using System;




public class Message
{

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

    private static Message GetOne(Message.Type type)
    {
        Message message = new Message();
        message.type = type;
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
}

