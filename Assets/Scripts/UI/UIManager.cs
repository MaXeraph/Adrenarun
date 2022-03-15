using System.Collections;
using UnityEngine;
using DG.Tweening;
//For healing text
//using TMPro;

public class UIManager : MonoBehaviour
{

	public static bool dead = false;

	public static Weapon weapon
	{
		get { return _weapon; }
		set
		{
			_weapon = value;

			AmmoCapacity = _weapon._magazineSize;
			Ammo = _weapon._magazineSize;
			reloadSpeed = _weapon._reloadSpeed;
		}
	}
	private static Weapon _weapon;

	public static float reloadSpeed;

	public static bool Reloading
	{
		get { return reloading; }
		set
		{
			reloading = value;
			AmmoUI.UpdateAmmo(ammo, reloading);
			if (reloading)
			{
				if (ammo != 0) { AmmoUI.UpdateAmmo(ammo - 1, reloading); }
				DOTween.To(() => Ammo, x => Ammo = x, ammoCapacity, reloadSpeed);
			}
		}
	}
	private static bool reloading = false;

	public static int Ammo
	{
		get { return ammo; }
		set
		{
			ammo = Mathf.Clamp(value, 0, ammoCapacity);
			if (ammo <= 0) { Reloading = true; }
			else if (ammo == ammoCapacity) { Reloading = false; _weapon.finishReload(); }
			AmmoUI.UpdateAmmo(ammo, reloading);

		}
	}
	private static int ammo;

	public static int AmmoCapacity
	{
		get { return ammo; }
		set
		{
			ammoCapacity = value;
			AmmoUI.UpdateAmmoCapacity(ammoCapacity);
		}
	}
	private static int ammoCapacity;

	public static float Health
	{
		get { return health; }
		set
		{
			health = Mathf.Clamp(value, 0, maxHealth);
			UI_health.setHealth(health);
			SpeedManager.updateSpeeds(health / maxHealth);
			if (health <= 0) deathUI.reveal(deathUI.instance);

		}
	}
	private static float health;

	public static float MaxHealth
	{
		get { return maxHealth; }
		set
		{
			maxHealth = value;
			UI_health.setMaxHealth(maxHealth);
			if (health > maxHealth) { Health = maxHealth; }
		}
	}
	private static float maxHealth;

	public static void UpdateWeapon()
	{
		UIManager.weapon = _weapon;
	}

	public static int enemiesLeft
	{
		set { waveUI.setLeft(value); }
		get { return waveUI._left; }

	}

	public static int enemiesTotal
	{
		set { waveUI.setTotal(value); }
		get { return waveUI._total; }
	}

	public static void DamageText(Vector3 position, float amount)
	{
		GameObject floatingText = Instantiate(Resources.Load("TextPopup")) as GameObject;

		//For healing text
		//if (amount >= 0) floatingText.GetComponent<TMP_Text>().color += Color.green;

		floatingText.transform.position = position;

		amount = Mathf.Abs(amount);
		string text = amount.ToString();
		floatingText.GetComponent<DamagePopup>().displayText = text;
	}

	public static void showPowerups(PowerUpType[] _powerUpSelectionList)
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		UpgradeUI.powerUpSelectionList = _powerUpSelectionList;
		//UpgradeUI.instance.gameObject.SetActive(true);
		UpgradeUI.init();
	}

	public static int powerSelection = -1;
}
