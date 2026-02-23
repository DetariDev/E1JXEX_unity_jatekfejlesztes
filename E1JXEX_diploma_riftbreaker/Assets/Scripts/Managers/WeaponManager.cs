using System;
using UnityEngine;
using VInspector;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance { get; private set; }
    public event Action<Weapon> OnWeaponChanged;



    [Foldout("Fegyver")]
    public Weapon currentWeapon;
    public Weapon StartingWeapon;

    public Weapon CurrentWeapon
    {
        get { return currentWeapon; }
        set 
        {
            currentWeapon = value;
            OnWeaponChanged?.Invoke(currentWeapon);
        }
    }

    public Transform weaponPos;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void Start()
    {
        CurrentWeapon = StartingWeapon;
    }

    public void Shoot(GameObject owner)
    {
        for (int i = 0; i < CurrentWeapon.projectileCount; i++)
        {
            Quaternion spread = weaponPos.rotation;
            if (CurrentWeapon.projectileCount > 1)
            {
                spread = Quaternion.Euler(0, UnityEngine.Random.Range(-15f, 15f), 0) * spread;
            }
            GameObject Projectile = Instantiate(CurrentWeapon.projectilePrefab, weaponPos.position, spread, null);
            Projectile projectilecomponent = Projectile.GetComponent<Projectile>();
            projectilecomponent.damage = CurrentWeapon.damage;
            projectilecomponent.bulletType = CurrentWeapon.bulletType;
            projectilecomponent.owner = owner;
            Rigidbody prb = Projectile.GetComponent<Rigidbody>();
            prb.AddForce(spread * Vector3.forward * CurrentWeapon.projectilespeed, ForceMode.Impulse);
            Destroy(Projectile, 5f);
        }
    }
}
