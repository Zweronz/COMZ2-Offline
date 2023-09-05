namespace CoMZ2
{
	public class GuardianFireCoopState : GuardianState
	{
		public override void DoStateLogic(float deltaTime)
		{
			m_guardian.FireUpdate(deltaTime);
		}

		public override void OnEnterState()
		{
		}

		public override void OnExitState()
		{
		}
	}
}
