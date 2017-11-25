using System;


// let's just do dynamic byte arrays
public class NetBuffer
{
	public static int DEFAULT_BUFFER_SIZE = 256;	// in bytes;

	private byte[] m_internalBuffer;

	// current read/write index, the byte index that we want to read or write to
	// currently I don't support dynamic resizing yet
	private int m_curPosition;	
	private int m_numBytes;	// logical buffer size
	private int m_numBytesAllocated;


	// we are using this for reading data
	// we will be reading dataTypes form the internal buffer to this byteArray, 
	// then we will convert the byteArray data to the desired dataTypes, int32, int64, bool ..
	// hence this needs to the large enough for the biggest atom data type that we can read in
	private byte[] m_tempReadByteArray;	


	public NetBuffer()
	{
		Reset();
	}

	// this byteArrayIn could be in NetworkEndian
	public void InitForRead(byte[] byteArrayIn, int numBytesAllocIn, int numBytesIn)
	{
		m_internalBuffer = byteArrayIn;
		m_tempReadByteArray = new byte[8];

		m_numBytesAllocated = numBytesAllocIn;
		m_numBytes = numBytesIn;

	}

	public void InitForWrite()
	{
        m_numBytesAllocated = DEFAULT_BUFFER_SIZE;

        m_internalBuffer = new byte[m_numBytesAllocated];
		m_tempReadByteArray = new byte[8];
	}

	public byte[] GetByteArray()
	{
		return m_internalBuffer;	
	}

	public int GetNumBytes()
	{
		return m_numBytes;
	}

	// when we write stuff, we want to write it in NetworkEndian order
	public bool WriteInt32AtIndex(int indexIn, int valueIn)
	{
    	int oldPosition = m_curPosition;		
        SeekBegin(indexIn);
		bool b = WriteInt32(valueIn);
		SeekBegin(oldPosition);
		return b;
	}


	private void SeekBegin(int index)
	{
		int newIndex = index;

		// sanity check
		if (newIndex < 0)
		{
			newIndex = 0;
		}

		m_curPosition = newIndex;
	}

	public int GetPosition()
	{
		return m_curPosition;
	}

	// when we write stuff, we want to write it in NetworkEndian order
	public bool WriteInt32(int valueIn)
	{
   //     Util.LogError("WriteInt32 " + valueIn);
        byte[] byteArray = BitConverter.GetBytes(valueIn);

		NetUtil.NativeToNetworkEndian(byteArray, 0, byteArray.Length);

		// convert from Native Endian to Network Endian
		return WriteData(byteArray, 0, byteArray.Length);
	}

	public bool WriteInt64(Int64 valueIn)
	{
		int length = sizeof(Int64);
		byte[] byteArray = BitConverter.GetBytes(valueIn);

		// convert from Native Endian to Network Endian
		NetUtil.NativeToNetworkEndian(byteArray, 0, length);

		return WriteData(byteArray, 0, byteArray.Length);
	}


    public bool WriteFloat(float valueIn)
    {
        int length = sizeof(float);
        byte[] byteArray = BitConverter.GetBytes(valueIn);

        NetUtil.NativeToNetworkEndian(byteArray, 0, length);

        return WriteData(byteArray, 0, byteArray.Length);
    }

    /*
    // Primitive means the most basic data types
    public bool WritePrimitive<T> (T valueIn)
    {
        int length = sizeof(T);
        byte[] byteArray = BitConverter.GetBytes(valueIn);

        NetUtil.NativeToNetworkEndian(byteArray, 0, length);

        return WriteData(byteArray, 0, byteArray.Length);
    }
    */

    public bool WriteBool(bool valueIn)
    {
        int length = sizeof(bool);
        byte[] byteArray = BitConverter.GetBytes(valueIn);

        // convert from NativeEndian to Network Endian
        NetUtil.NativeToNetworkEndian(byteArray, 0, length);

        return WriteData(byteArray, 0, byteArray.Length);
    }

	// writing from byteArrayIn to internalBuffer
	public bool WriteData(byte[] srcByteArrayIn, int positionToReadFrom, int numBytesToWrite)
	{
		if (m_curPosition + numBytesToWrite > m_numBytesAllocated )
		{
     		return false;
		}

		if (numBytesToWrite > 0)
		{
			System.Buffer.BlockCopy(srcByteArrayIn, positionToReadFrom, m_internalBuffer, m_curPosition, numBytesToWrite);

            m_curPosition += numBytesToWrite;
		
            m_numBytes = Math.Max(m_curPosition, m_numBytes);        
        }

		return true;
	}



	public Int32 ReadInt32()
	{
		int numBytesToRead = sizeof(Int32);
		ReadData(m_tempReadByteArray, 0, numBytesToRead);

		NetUtil.NativeToNetworkEndian(m_tempReadByteArray, 0, numBytesToRead);

		Int32 val = 0;
		try
		{
			// the BitConverter.ToInt32 tries to convert 4 bytes from the specified position to an Int32
			// so no need to pass in numBytesToRead
			val = BitConverter.ToInt32(m_tempReadByteArray, 0);
		}
		catch (System.Exception exceptionIn)
		{
			Util.LogError("error reading in ReadInt32");
		}
		return val;
	}

	public Int64 ReadInt64()
	{
		int numBytesToRead = sizeof(Int64);
		ReadData(m_tempReadByteArray, 0, numBytesToRead);

		NetUtil.NativeToNetworkEndian(m_tempReadByteArray, 0, numBytesToRead);

		Int64 val = 0;
		try
		{
			val = BitConverter.ToInt64(m_tempReadByteArray, 0);
		}
		catch (System.Exception exceptionIn)
		{
			Util.LogError("error reading in ReadInt64");
		}
		return val;
	}


    public bool ReadBool()
    {
        int numberBytesToRead = sizeof(bool);
        ReadData(m_tempReadByteArray, 0, numberBytesToRead);

        NetUtil.NativeToNetworkEndian(m_tempReadByteArray, 0, numberBytesToRead);

        bool val = false;
        try
        {
            val = BitConverter.ToBoolean(m_tempReadByteArray, 0);
        }
        catch(System.Exception exceptionIn)
        {
            Util.LogError("error reading in ReadBool");
        }
        return val;
    }

    public float ReadFloat()
    {
        int numberBytesToRead = sizeof(float);
        ReadData(m_tempReadByteArray, 0, numberBytesToRead);

        NetUtil.NativeToNetworkEndian(m_tempReadByteArray, 0, numberBytesToRead);

        float val = 0;
        try 
        {
            val = BitConverter.ToSingle(m_tempReadByteArray, 0);
        }
        catch(System.Exception exceptionIn)
        {
            Util.LogError("error reading in ReadPrimitive");
        }

        return val;
    }

    /*
     doesn't work cuz BitConverter<T> doesn't work
    public T ReadPrimitive<T>()
    {
        int numberBytesToRead = sizeof(T);
        ReadData(m_tempReadByteArray, 0, numberBytesToRead);

        NetUtil.NativeToNetworkEndian(m_tempReadByteArray, 0, numberBytesToRead);

        T val = default(T);
        try 
        {
            val = BitConverter.To
        }
        catch(System.Exception exceptionIn)
        {
            Util.LogError("error reading in ReadPrimitive");
        }

        return T
    }
    */

	// reading from internalBuffer to desByteArray
	// positionToWriteFrom refers to the position to start writing for destByteArrayIn
	public bool ReadData(byte[] destByteArrayIn, int positionToWriteFrom, int numBytesToRead)
	{
		if (m_curPosition + numBytesToRead > m_numBytesAllocated )
		{
			return false;
		}

		if (numBytesToRead > 0)
		{
			System.Buffer.BlockCopy(m_internalBuffer, m_curPosition, destByteArrayIn, positionToWriteFrom, numBytesToRead);
			m_curPosition += numBytesToRead;
		}

		return true;
	}


	public void Reset()
	{
		m_curPosition = 0;
		m_numBytes = 0;
		m_numBytesAllocated = 0;

		m_internalBuffer = null;
		m_tempReadByteArray = null;
	}

}
