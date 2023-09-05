using TNetSdk.BinaryProtocol;

namespace TNetSdk
{
	public class RoomKickUserNotifyCmd : UnPacker
	{
		public ushort m_user_id;

		public override bool ParserPacket(Packet packet)
		{
			if (!base.ParserPacket(packet))
			{
				return false;
			}
			if (!PopUInt16(ref m_user_id))
			{
				return false;
			}
			return true;
		}

		public override void ToTNetEventData(Packet packet, ref TNetEventData event_data, TNetObject target)
		{
			ParserPacket(packet);
			if (target == null || target.CurRoom == null)
			{
				return;
			}
			if (m_user_id == target.Myself.Id)
			{
				target.CurRoom = null;
				event_data.data.Add("user", target.Myself);
				return;
			}
			TNetUser userById = target.CurRoom.GetUserById(m_user_id);
			if (userById != null)
			{
				target.CurRoom.RemoveUser(userById);
				event_data.data.Add("user", userById);
			}
		}
	}
}
