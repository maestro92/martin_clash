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


    // problems like these
    // u want to split arguments inputs that give out the same answer

    // 1 ~ 1024 = 1024
    // 1025 ~ 2048 = 2048
    // ..
    // ..
    // 0 ~ -1023 = 0
    // -1024 ~ -2047 = -1024
    public static Int64 QuantizeUpToNearestMS(Int64 value, Int64 quantizeAmount)
    {
        // QuantizeUpToNearestMS( 0, 1024 ) returns 0
        // QuantizeUpToNearestMS( 1, 1024 ) returns 1024
        // QuantizeUpToNearestMS( 1023, 1024 ) returns 1024
        // QuantizeUpToNearestMS( 1024, 1024 ) returns 1024
        // QuantizeUpToNearestMS( 1025, 1024 ) returns 2048
        // QuantizeUpToNearestMS( 2047, 1024 ) returns 2048
        // QuantizeUpToNearestMS( 2048, 1024 ) returns 2048
        // QuantizeUpToNearestMS( 2049, 1024 ) returns 3072
        // QuantizeUpToNearestMS( 4000, 1024 ) returns 4096
        // QuantizeUpToNearestMS( -1, 1024 ) returns 0
        // QuantizeUpToNearestMS( -1023, 1024 ) returns 0
        // QuantizeUpToNearestMS( -1024, 1024 ) returns -1024
        // QuantizeUpToNearestMS( -1025, 1024 ) returns -1024
        // QuantizeUpToNearestMS( -4000, 1024 ) returns -3072

        // should be atleast one
        Int64 properQuantizeAmount = Math.Max(quantizeAmount, 1);

        if (value > 0)
        {
            return ((value - 1) / properQuantizeAmount + 1) * properQuantizeAmount;
        }
        else
        {
            return (value / properQuantizeAmount) * properQuantizeAmount;
        }

    }
}


