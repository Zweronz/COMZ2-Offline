using UnityEngine;

namespace CoMZ2
{
	public class PlayerRunCoopState : PlayerState
	{
		public override void DoStateLogic(float deltaTime)
		{
			m_player.UpdateRunAni(deltaTime);
			if (!AnimationUtil.IsPlayingAnimation(m_player.gameObject, m_player.ANI_MOVE_RUN))
			{
				AnimationUtil.CrossAnimate(m_player.gameObject, m_player.ANI_MOVE_RUN, WrapMode.Loop);
			}
		}

		public override void OnEnterState()
		{
		}

		public override void OnExitState()
		{
		}
	}
}
