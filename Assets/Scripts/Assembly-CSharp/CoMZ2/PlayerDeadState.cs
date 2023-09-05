using UnityEngine;

namespace CoMZ2
{
	public class PlayerDeadState : PlayerState
	{
		public override void DoStateLogic(float deltaTime)
		{
			if (GameSceneController.Instance.main_camera != null)
			{
				GameSceneController.Instance.main_camera.ZoomOut(deltaTime);
			}
			if (!AnimationUtil.IsPlayingAnimation(m_player.gameObject, m_player.GetFireStateAnimation(m_player.MoveState, this)))
			{
				AnimationUtil.Stop(m_player.gameObject);
				AnimationUtil.CrossAnimate(m_player.gameObject, m_player.GetFireStateAnimation(m_player.MoveState, this), WrapMode.ClampForever);
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
