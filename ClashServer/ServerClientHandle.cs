using System;


// server's copy of the client
public class ServerClientHandle
{
	public int id;
	private NetGameConnection m_connection;
	public ServerClientHandle(int idIn)
	{
		this.id = idIn;
		m_connection = null;
	}

	public void SetGameConnection(NetGameConnection connectionIn)
	{
		m_connection = connectionIn;
	}

	public NetGameConnection GetGameConnection()
	{
		return m_connection;
	}
}

