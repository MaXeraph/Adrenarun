using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerStats : MonoBehaviour
{

    [Header("UI References")]
    [SerializeField] private UI_health healthBar;
    [SerializeField] private AmmoUI AmmoBar;

    [Header("Player Variables")]
    [SerializeField] public float reloadSpeed = 1f;
    [SerializeField] public float fireRate = 0.15f;

    public bool Reloading
    {
        get { return reloading; }
        set
        {
            reloading = value;
            AmmoBar.UpdateAmmo(ammo, reloading);
            if (reloading)
            {
                if (ammo != 0) { AmmoBar.UpdateAmmo(ammo - 1, reloading); }
                DOTween.To(() => Ammo, x => Ammo = x, ammoCapacity, reloadSpeed); }
        }
    }
    private bool reloading = false;

    public float Speed
    {
        get { return speed; }
        set
        {
            speed = value;
            Movement.speed = speed;
        }
    }
    [SerializeField] private float speed = 12f;

    public float DashSpeed
    {
        get { return dashSpeed; }
        set
        {
            dashSpeed = value;
            Movement.dashSpeed = dashSpeed;
        }
    }
    [SerializeField] private float dashSpeed = 5f;

    public float JumpHeight
    {
        get { return jumpHeight; }
        set
        {
            jumpHeight = value;
            Movement.jumpHeight = jumpHeight;
        }
    }
    [SerializeField] private float jumpHeight = 3f;

    public int Ammo
    {
        get { return ammo; }
        set
        {
            ammo = Mathf.Clamp(value, 0, ammoCapacity);
            if (ammo <= 0) { Reloading = true;}
            else if (ammo == ammoCapacity) { Reloading = false; }
            AmmoBar.UpdateAmmo(ammo, reloading);

        }
    }
    private int ammo;

    public int AmmoCapacity
    {
        get { return ammo; }
        set
        {
            ammoCapacity = value;
            AmmoBar.UpdateAmmoCapacity(ammoCapacity);
        }
    }
    [SerializeField] private int ammoCapacity = 10;

    public float Health
    {
        get { return health; }
        set
        {
            health = Mathf.Clamp(value, 0, maxHealth);
            healthBar.setHealth(health);
        }
    }
    private float health;

    public float MaxHealth
    {
        get { return maxHealth; }
        set
        {
            maxHealth = value;
            healthBar.setMaxHealth(maxHealth);
            if (health > maxHealth){ Health = maxHealth; }
        }
    }
    [SerializeField] float maxHealth = 100f;

    void Awake()
    {
        MaxHealth = maxHealth;
        Health = maxHealth;
        AmmoCapacity = ammoCapacity;
        Ammo = ammoCapacity;
        Speed = speed;
        JumpHeight = jumpHeight;
        DashSpeed = dashSpeed;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Equals)) { Health += 10f; }
        if (Input.GetKeyDown(KeyCode.Minus)) { Health -= 10f; }
        if (Input.GetKeyDown(KeyCode.UpArrow)) { Speed += 10f; }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { Speed -= 10f; }
        if (Input.GetKeyDown(KeyCode.R)) { Reloading = true; }
    }
}
