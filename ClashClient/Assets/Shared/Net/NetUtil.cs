using System;

public class NetUtil
{
	public NetUtil()
	{

	}

	public static void NativeToNetworkEndian(byte[] buffer, int startIndex, int numBytes)
	{
		if (BitConverter.IsLittleEndian && numBytes > 1 && buffer != null)
		{
			Array.Reverse(buffer, startIndex, numBytes);
		}
	}

	public static void NetworkToNativeEndian(byte[] buffer, int startIndex, int numBytes)
	{
		if (BitConverter.IsLittleEndian && numBytes > 1 && buffer != null)
		{
			Array.Reverse(buffer, startIndex, numBytes);
		}
	}

}


