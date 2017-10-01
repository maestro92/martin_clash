using System;

public class Game
{
	public static void Main(string[] args)
	{		
        Config.Init();

		Util.OnLog = (s) =>
			{
				Console.WriteLine(s);
			};

		Util.OnLogWarning = (s) =>
			{
				Console.WriteLine("WARNING: " + s);
			};

		Util.OnLogError = (s) =>
			{
				Console.WriteLine("ERROR: " + s);
			};

		NetGlobal.netMeter.Init();

		// server start hosting
		Server server = new Server();

		server.startHosting(NetworkManager.LOCAL_IP_ADDRESS, NetworkManager.SERVER_PORT);

		while (true)
		{
			// listen for messages

//			server.SyncAcceptConnection();

//			server.TryAsyncAcceptConnections();

			// 
	//		server.processIncomingMessages();

			// all game simulations
			server.update();


		}


		server.stopHosting();

		return;
	}
}

