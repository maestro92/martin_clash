using System;

using System.Net;
// i am only going to support IPv4 addresses

public enum NetAddressType
{
	None,
	IPv4,
	IPv6,	// not supported, but leaving it here.
}


// assuming this is ipv4, which takes the form of 255.255.255.255, 
// ipByte3.ipByte2.ipByte1.ipByte0
// so 4 bytes (or an integer) will suffice
public class NetAddress
{
	/*
	public byte ipByte0;
	public byte ipByte1;
	public byte ipByte2;
	public byte ipByte3;
	*/
	public byte[] ipBytes;
	public int m_port;

	private IPAddress m_ipAddress;
	NetAddressType m_netAddressType;

	public NetAddress(string ipAddress, int port)
	{
		m_netAddressType = NetAddressType.None;
		InitIPv4(ipAddress, port);
	}

	public void InitIPv4(string ipAddress, int port)
	{
		// split string by .
		ipBytes = new byte[4];

		string[] separators = new string[] { "." };
		string[] ipStringList = ipAddress.Split(separators, StringSplitOptions.None);

		if (ipStringList.Length > 4)
		{
			Util.LogError("Error in initing NetAddress " + ipAddress );
		}

		int i = 0;
		foreach (var s in ipStringList)
		{
			int temp = Convert.ToInt32(s);
			ipBytes[i] = Convert.ToByte(temp);
			i++;
		}

		Util.LogError("GetIPAddressString() " + GetIPAddressString());


		m_ipAddress = IPAddress.Parse(GetIPAddressString());

		this.m_port = port;

		m_netAddressType = NetAddressType.IPv4;
	}

	public IPAddress GetIPAddress()
	{
		return m_ipAddress;
	}


	public int GetPort()
	{
		return m_port;
	}


	public string GetIPAddressString()
	{
		string s = "";
		int i = 0;
		foreach (var b in ipBytes)
		{
			if (i == 0)
			{
				s += b.ToString();
			}
			else
			{
				s += "." + b.ToString();
			}
			i++;
		}

		return s;
	}



	public void Print()
	{
	//	Util.Log(GetString());
	}
}
