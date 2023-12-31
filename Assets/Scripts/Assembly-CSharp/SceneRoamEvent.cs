using UnityEngine;

public class SceneRoamEvent : MonoBehaviour, IRoamEvent
{
	public void OnRoamTrigger()
	{
		Debug.Log("on roam triger!");
	}

	public void OnRoamStop()
	{
		Debug.Log("on roam stop!");
		if (GameSceneController.Instance.IsSkipCg)
		{
			return;
		}
		if (GetComponent<CameraFadeEvent>() != null)
		{
			CameraFadeEvent component = GetComponent<CameraFadeEvent>();
			if (component.isFadeOut)
			{
				component.on_fadeout_end = OnGameCgEnd;
			}
		}
		else
		{
			OnGameCgEnd();
		}
	}

	private void OnGameCgEnd()
	{
		GameSceneController.Instance.OnGameCgEnd();
	}
}
