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

    private Message()
    {
        
    }

    public static Message GetOne(Message.Type type)
    {
        Message message = new Message();
        message.type = type;
        return message;
    }
}

