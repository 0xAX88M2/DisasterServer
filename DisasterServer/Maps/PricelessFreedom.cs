using DisasterServer.Data;
using DisasterServer.Entities;
using DisasterServer.Session;
using ExeNet;

namespace DisasterServer.Maps;

public class PricelessFreedom : Map
{
	public override void Init(Server server)
	{
		Random random = new Random();
		if (Options.Get<bool>("random_mode"))
		{
			int addTimeRandom = random.Next(1, 128);
			SetTime(server, 155+addTimeRandom);
		}
		else
		{
			SetTime(server, 155);
		}
		Spawn(server, new PFLift(0, 1669f, 1016f));
		Spawn(server, new PFLift(1, 1069f, 704f));
		Spawn(server, new PFLift(2, 829f, 400f));
		Spawn(server, new PFLift(3, 1070f, 544f));
		if (Options.Get<bool>("random_mode"))
		{
			int countRandomSpawnBlackRing = random.Next(1, 29);
			for (int i = 0; i < countRandomSpawnBlackRing; i++)
			{
				Spawn<BlackRing>(server);
			}
			Terminal.Log($"[PricelessFreedom] Count Black Rings: {countRandomSpawnBlackRing}");
		}
		else
		{
			for (int i = 0; i < 29; i++)
			{
				Spawn<BlackRing>(server);
			}
		}
		base.Init(server);
	}

	public override void PeerTCPMessage(Server server, TcpSession session, BinaryReader reader)
	{
		reader.ReadBoolean();
		if (reader.ReadByte() == 87)
		{
			byte id = reader.ReadByte();
			FindOfType<PFLift>()?.Where((PFLift e) => e.ID == id).FirstOrDefault()?.Activate(server, session.ID);
		}
		base.PeerTCPMessage(server, session, reader);
	}

	protected override int GetPlayerOffset(Server server)
	{
		lock (server.Peers)
		{
			return (server.Peers.Count<KeyValuePair<ushort, Peer>>((KeyValuePair<ushort, Peer> e) => !e.Value.Waiting) - 1) * 10;
		}
	}

	protected override int GetRingSpawnCount()
	{
		return 38;
	}
}
