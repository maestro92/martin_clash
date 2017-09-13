using System;

public class ServerFrameInfo : INetSerializer
{
    public int frameCount;

    // debug information can be done here as well
    private ServerFrameInfo()
    {

    }

    static public ServerFrameInfo GetOne()
    {
        ServerFrameInfo info = new ServerFrameInfo();
        return info;
    }

    public void Serialize(NetSerializer writer)
    {
        writer.WriteInt32(frameCount, "frameCount");
    }

    public void Deserialize(NetSerializer reader)
    {
        frameCount = reader.ReadInt32("frameCount");
    }

    public void Print()
    {
    //    Util.LogError("frameCount " + frameCount.ToString());
    }
}

