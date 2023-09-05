using UnityEngine;

public class MissionBossPanelController : UIPanelController
{
	public TUILabel mission_content;

	public TUIMeshSprite npc_icon;

	public GameObject mission_eff;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetContent(string content)
	{
		if (mission_content != null)
		{
			mission_content.Text = content;
		}
	}

	public void SetIcon(string frame)
	{
		if (npc_icon != null)
		{
			npc_icon.texture = frame;
		}
	}

	public override void Show()
	{
		mission_eff.SetActive(true);
		base.Show();
		Invoke("ShowEff", 0.5f);
	}

	public void ShowEff()
	{
		AnimationUtil.PlayAnimate(mission_eff, "mubiao", WrapMode.Once);
		Invoke("HideEff", mission_eff.GetComponent<Animation>()["mubiao"].length);
	}

	public void HideEff()
	{
		if (mission_eff != null)
		{
			mission_eff.SetActive(false);
		}
	}
}
