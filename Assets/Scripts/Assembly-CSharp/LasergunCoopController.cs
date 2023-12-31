using CoMZ2;
using UnityEngine;

public class LasergunCoopController : WeaponCoopController
{
	private Ray ray = default(Ray);

	protected Transform fire_ori;

	protected GameObject laser_eff_root;

	protected GameObject laser_eff;

	protected Collider laser_eff_collider;

	protected Vector3 laser_target_pos = Vector3.zero;

	protected float laser_eff_offset = 0.5f;

	protected bool is_inited;

	private void Awake()
	{
		weapon_type = WeaponType.Laser;
	}

	private void Start()
	{
		fire_ori = base.transform.Find("fire_ori");
		if (fire_ori == null)
		{
			Debug.LogError(string.Concat("weapon:", weapon_type, " can't find fire_ori!"));
		}
		laser_eff_root = Object.Instantiate(Accessory[0]) as GameObject;
		laser_eff_root.transform.localPosition = Vector3.zero;
		laser_eff_root.transform.localRotation = Quaternion.identity;
		laser_eff = laser_eff_root.transform.Find("effect_lasergun_mod").gameObject;
		laser_eff_collider = laser_eff.gameObject.GetComponent<Collider>();
		is_inited = true;
	}

	public override void Update()
	{
		base.Update();
	}

	public override void SetWeaponAni(GameObject player, Transform spine)
	{
		if (weapon_data.weapon_name == "Laser")
		{
			ANI_IDLE_RUN = "LaserGun_UpperBody_Run01";
			ANI_IDLE_SHOOT = "LaserGun_UpperBody_Idle01";
			ANI_SHOOT = "LaserGun_UpperBody_Shooting02";
			ANI_RELOAD = "LaserGun_UpperBody_Reload";
			ANI_SHIFT_WEAPON = "LaserGun_UpperBody_Shiftweapon01";
			ANI_IDLE_DOWN = "AK_LowerBody_Idle01";
			ANI_SHOOT_DOWN = "LaserGun_LowerBody_Shooting01";
			ANI_RELOAD_DOWN = "AK_LowerBody_Reload01";
			ANI_FIRE_READY = "LaserGun_UpperBody_Shooting01";
		}
		player.GetComponent<Animation>()[ANI_IDLE_RUN].AddMixingTransform(spine);
		player.GetComponent<Animation>()[ANI_IDLE_SHOOT].AddMixingTransform(spine);
		player.GetComponent<Animation>()[ANI_SHOOT].AddMixingTransform(spine);
		player.GetComponent<Animation>()[ANI_RELOAD].AddMixingTransform(spine);
		player.GetComponent<Animation>()[ANI_SHIFT_WEAPON].AddMixingTransform(spine);
		ResetReloadAniSpeed(player);
		ResetFireAniSpeed(player);
		render_obj = base.gameObject;
		base.SetWeaponAni(player, spine);
	}

	public override void SetShopWeaponAni(GameObject player, Transform spine)
	{
		render_obj = base.gameObject;
	}

	public override void FireUpdate(PlayerController player, float deltaTime)
	{
		if (is_inited)
		{
			ResetLaserTransform(player);
			base.FireUpdate(player, deltaTime);
		}
	}

	public override void Fire(PlayerController player, float deltaTime)
	{
		if (!is_inited || !OnReadyFire(player, deltaTime))
		{
			return;
		}
		if (!laser_eff_root.activeSelf)
		{
			laser_eff_root.SetActive(true);
		}
		base.Fire(player, deltaTime);
		AnimationUtil.Stop(player.gameObject, ANI_SHOOT);
		AnimationUtil.PlayAnimate(player.gameObject, ANI_SHOOT, WrapMode.ClampForever);
		player.CouldEnterAfterFire = true;
		ray = new Ray(fire_ori.position, laser_target_pos - fire_ori.position);
		RaycastHit[] array = Physics.RaycastAll(ray, (laser_target_pos - fire_ori.position).magnitude + laser_eff_offset, (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.DYNAMIC_SCENE) | (1 << PhysicsLayer.ANIMATION_SCENE) | (1 << PhysicsLayer.WALL_METAL) | (1 << PhysicsLayer.WALL_WOOD) | (1 << PhysicsLayer.WOOD_BOX));
		RaycastHit[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			RaycastHit raycastHit = array2[i];
			Vector3 zero = Vector3.zero;
			if (raycastHit.collider.gameObject.layer == PhysicsLayer.ENEMY)
			{
				EnemyController component = raycastHit.collider.GetComponent<EnemyController>();
				if (component != null)
				{
					component.OnHit(GetDamageValWithAvatar(player, player.avatar_data), this, player, raycastHit.point, raycastHit.normal);
				}
			}
			else if (raycastHit.collider.gameObject.layer == PhysicsLayer.WOOD_BOX)
			{
				WoodBoxController component2 = raycastHit.collider.GetComponent<WoodBoxController>();
				component2.OnHit(GetDamageValWithAvatar(player, player.avatar_data), this, player, raycastHit.point, raycastHit.normal);
			}
			else if (raycastHit.collider.gameObject.layer == PhysicsLayer.DYNAMIC_SCENE)
			{
				GameObject gameObject = raycastHit.collider.gameObject;
				if (gameObject.GetComponent<Rigidbody>() == null)
				{
					gameObject.AddComponent<Rigidbody>();
				}
				Vector3 force = fire_ori.forward.normalized * 50f;
				Rigidbody component3 = gameObject.GetComponent<Rigidbody>();
				component3.mass = 5f;
				component3.drag = 1.5f;
				component3.angularDrag = 1.5f;
				component3.AddForceAtPosition(force, raycastHit.point, ForceMode.Impulse);
				gameObject.layer = PhysicsLayer.WALL;
				gameObject.AddComponent<RemoveTimerScript>().life = 3f;
			}
			else if (raycastHit.collider.gameObject.layer == PhysicsLayer.ANIMATION_SCENE)
			{
				GameObject gameObject2 = raycastHit.collider.transform.parent.gameObject;
				if (gameObject2.GetComponent<Animation>() != null)
				{
					gameObject2.GetComponent<Animation>()["Take 001"].clip.wrapMode = WrapMode.Once;
					gameObject2.GetComponent<Animation>().Play("Take 001");
				}
			}
			else
			{
				zero = player.transform.InverseTransformPoint(raycastHit.point);
			}
		}
	}

	public override void OnWeaponReload()
	{
	}

	public void ResetLaserTransform(PlayerController player)
	{
		if (!is_inited)
		{
			return;
		}
		laser_eff_root.transform.position = fire_ori.position;
		ray = new Ray(fire_ori.position, fire_ori.forward);
		RaycastHit hitInfo = default(RaycastHit);
		Physics.Raycast(ray, out hitInfo, 1000f, (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.NPC) | (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.DYNAMIC_SCENE) | (1 << PhysicsLayer.ANIMATION_SCENE) | (1 << PhysicsLayer.WALL_METAL) | (1 << PhysicsLayer.WALL_WOOD) | (1 << PhysicsLayer.WOOD_BOX));
		if (player.transform.InverseTransformPoint(hitInfo.point).z > 0f)
		{
			laser_target_pos = hitInfo.point;
			float magnitude = (laser_target_pos - fire_ori.position).magnitude;
			magnitude -= laser_eff_offset;
			if (magnitude <= 0f)
			{
				magnitude = 0f;
			}
			laser_eff.transform.localScale = new Vector3(1f, magnitude / 4f, 1f);
			laser_eff_root.transform.LookAt(laser_target_pos);
		}
	}

	public override void StopFire(PlayerController player)
	{
		if (is_inited)
		{
			if (laser_eff_root != null)
			{
				laser_eff_root.SetActive(false);
			}
			if (GameSceneController.Instance != null && GameSceneController.Instance.player_controller != null)
			{
				player.StopPlayerAudio("ShotLaserGun01");
			}
		}
	}

	public override void OnEnterAfterFire(PlayerController player)
	{
		player.StopPlayerAudio("ShotLaserGun01");
	}
}
