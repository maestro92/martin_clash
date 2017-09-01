﻿using System;
using System.Collections;
using System.Collections.Generic;


public enum NetSerializationFormat
{
	None,
	Binary,
	// maybe json in the future
}

public enum NetSerializationMode
{
	None,
	Writer,
	Reader,
	// maybe json in the future
}

public interface INetSerializer
{
	void Serialize(NetSerializer writerIn);
	void Deserialize(NetSerializer readerIn);
}

// It feels like the serializer 
public class NetSerializer
{
    // a delegate is like a function pointer
    // we the function pointer ObjectFactory which takes a template, and returns that same template type
    public delegate T ObjectFactory<T>();

	private NetBuffer m_writeBuffer;
	private NetBuffer m_readBuffer;
	public NetSerializationMode m_mode;
	private bool m_serializeWithDebugMarkers;

	private List<WriteContext> m_writeContextList;
	private List<ReadContext> m_readContextList;

	public class WriteContext
	{
		public string objectName = "";
		public int writeLevel = 0;
		public int writerHeaderPos = 0;
		public int writerDataPos = 0;

		public WriteContext(string objectNameIn, int writeLevelIn, int writerHeadPosIn, int writerDataPosIn)
		{
			this.objectName = objectNameIn;
			this.writeLevel = writeLevelIn;
			this.writerHeaderPos = writerHeadPosIn;
			this.writerDataPos = writerDataPosIn;
		}
	}

	public class ReadContext
	{
		public string objectName = "";
		public int readLevel = 0;
		public int readerBeginPos = 0;	// notice this is different from the two variables stored in WriteContext
		public int readerEndPos = 0;
//		public int readerOldSize = 0;

		public ReadContext(string objectNameIn, int readLevelIn, int readerBeginPosIn, int readEndPosIn)
		{
			this.objectName = objectNameIn;
			this.readLevel = readLevelIn;
			this.readerBeginPos = readerBeginPosIn;
			this.readerEndPos = readEndPosIn;
		}


		/*
		public ReadContext(string objectNameIn, int readLevelIn, int readerBeginPosIn, int readEndPosIn, int readerOldSizeIn)
		{
			this.objectName = objectNameIn;
			this.readLevel = readLevelIn;
			this.readerBeginPos = readerBeginPosIn;
			this.readerEndPos = readEndPosIn;
			this.readerOldSize = readerOldSizeIn;
		}
		*/
	}


	private NetSerializer()
	{
		m_writeBuffer = new NetBuffer();
		m_readBuffer = new NetBuffer();

		m_writeContextList = new List<WriteContext>();
		m_readContextList = new List<ReadContext>();
	}

	public static NetSerializer GetOne()
	{
		NetSerializer serializer = new NetSerializer();
		return serializer;
	}

	public void SetupWriteMode(string objectNameIn, bool serializeWithDebugMarkersIn)
	{
		m_mode = NetSerializationMode.Writer;

		m_serializeWithDebugMarkers = serializeWithDebugMarkersIn;
		m_writeBuffer.InitForWrite();
		m_readBuffer.Reset();

		m_writeContextList.Clear();
		m_readContextList.Clear();

		WriteContext writeContext = new WriteContext(objectNameIn, m_writeContextList.Count + 1, 0, 0);
		m_writeContextList.Add(writeContext);
	}

	public void SetupReadMode(string objectNameIn, bool serializeWithDebugMarkersIn, byte[] byteArrayIn, int numBytesIn)
	{
		m_mode = NetSerializationMode.Reader;

		m_serializeWithDebugMarkers = serializeWithDebugMarkersIn;
		m_writeBuffer.Reset();
		m_readBuffer.InitForRead(byteArrayIn, numBytesIn, numBytesIn);

		m_writeContextList.Clear();
		m_readContextList.Clear();

		ReadContext readContext = new ReadContext(objectNameIn, m_readContextList.Count + 1, 0, 0);
		m_readContextList.Add(readContext);

	}

	public byte[] GetWriteBufferByteArray()
	{
		return m_writeBuffer.GetByteArray();
	}

	public int GetWriteBufferNumBytes()
	{
		return m_writeBuffer.GetNumBytes();
	}

	public void WriteOne<T>(T itemIn, string logVarNameIn) where T : class, INetSerializer
	{
		BeginWriteObject(logVarNameIn);

		itemIn.Serialize(this);

		EndWriteObject();
	}

	public void BeginWriteObject(string objectNameIn)
	{
		if (m_mode != NetSerializationMode.Writer)
		{
			ThrowException("NetSerializer.BeginWriteObject(): Not in writer mode!!!!");
		}

		int headerPos = m_writeBuffer.GetPosition();
		m_writeBuffer.WriteInt32(0);	// placeholder for size of 
		int dataPos = m_writeBuffer.GetPosition();

		WriteContext writeContext = new WriteContext(objectNameIn, m_writeContextList.Count + 1, headerPos, dataPos);
		m_writeContextList.Add(writeContext);
	}

	public void EndWriteObject()
	{
		WriteContext writeContext = m_writeContextList[m_writeContextList.Count - 1];

		m_writeContextList.RemoveAt(m_writeContextList.Count - 1);

		int blockSize = m_writeBuffer.GetPosition() - writeContext.writerDataPos;

		m_writeBuffer.WriteInt32AtIndex(writeContext.writerHeaderPos, blockSize);
	
	}



	public void ReadOne<T>(T itemIn, string logVarNameIn) where T : class, INetSerializer
	{
		BeginWriteObject(logVarNameIn);

		itemIn.Serialize(this);

		EndWriteObject();
	}


	public void BeginReadObject(string objectNameIn)
	{
		if (m_mode != NetSerializationMode.Reader)
		{
			ThrowException("NetSerializer.BeginReadObject(): Not in reader mode!!!!");
		}
		int blockSize = m_readBuffer.ReadInt32();

		int headerPos = m_readBuffer.GetPosition();
		int endPos = headerPos + blockSize;

		ReadContext readContext = new ReadContext(objectNameIn, m_readContextList.Count + 1, headerPos, endPos);

	}

	public void EndReadObject()
	{
		var readContext = m_readContextList[m_readContextList.Count - 1];
		m_readContextList.RemoveAt(m_readContextList.Count - 1);
	}

	public void WriteInt32AtIndex(string varLogName, int value, int index)
	{
		if (m_mode != NetSerializationMode.Writer)
		{
			ThrowException("NetSerializer.WriteInt32(): Not in writer mode!!!!");
		}

		m_writeBuffer.WriteInt32AtIndex(value, index);
	}

	public void WriteInt32(string varLogName, int value)
	{
		if (m_mode != NetSerializationMode.Writer)
		{
			ThrowException("NetSerializer.WriteInt32(): Not in writer mode!!!!");
		}

		m_writeBuffer.WriteInt32(value);
	}

	public Int32 ReadInt32(string varLogName)
	{
		if (m_mode != NetSerializationMode.Reader)
		{
			ThrowException("NetSerializer.ReadInt32(): Not in reader mode!!!!");
		}

		Int32 value = 0;

		try
		{
			value = m_readBuffer.ReadInt32();
		}
		catch (System.Exception exceptionIn)
		{
			ThrowException(exceptionIn.ToString());
		}
		return value;
	}

	public void ThrowException(string messageIn)
	{
		Util.LogError(messageIn);
		throw new Exception(messageIn);
	}



}
