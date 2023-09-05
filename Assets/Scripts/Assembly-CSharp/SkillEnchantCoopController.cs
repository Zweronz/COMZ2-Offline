using UnityEngine;

public class SkillEnchantCoopController : SkillCoopController
{
	protected DoctorCoopController doctor;

	public override void Init(SkillData data, ObjectController owner)
	{
		base.Init(data, owner);
		doctor = owner as DoctorCoopController;
		if (owner == null)
		{
			Debug.LogError("SkillEnchantCoopController owner error.");
		}
	}

	public override void Dologic(float deltaTime)
	{
		base.Dologic(deltaTime);
	}

	public override void OnSkillConjure()
	{
		base.OnSkillConjure();
		doctor.SetFireState(doctor.ENCHANT_SKILL_STATE);
		Debug.Log("SkillEnchantController OnSkillConjure");
	}

	public bool EnableEnchantMonst(EnemyType enemy_type)
	{
		for (int i = 0; i < skill_data.level + 1; i++)
		{
			if (GameConfig.Instance.Skill_Enchant_Monster_List[i] == enemy_type)
			{
				return true;
			}
		}
		return false;
	}
}
