using UnityEngine;
using VInspector;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance { get; private set; }

    [Foldout("Fegyver")]
    public Weapon currentWeapon;
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

    public void Shoot(GameObject owner)
    {
        for (int i = 0; i < currentWeapon.projectileCount; i++)
        {
            Quaternion spread = weaponPos.rotation;
            if (currentWeapon.projectileCount > 1)
            {
                spread = Quaternion.Euler(0, Random.Range(-15f, 15f), 0) * spread;
            }
            GameObject Projectile = Instantiate(currentWeapon.projectilePrefab, weaponPos.position, spread, null);
            Projectile projectilecomponent = Projectile.GetComponent<Projectile>();
            projectilecomponent.damage = currentWeapon.damage;
            projectilecomponent.bulletType = currentWeapon.bulletType;
            projectilecomponent.owner = owner;
            Rigidbody prb = Projectile.GetComponent<Rigidbody>();
            prb.AddForce(spread * Vector3.forward * currentWeapon.projectilespeed, ForceMode.Impulse);
            Destroy(Projectile, 5f);
        }
    }
}
