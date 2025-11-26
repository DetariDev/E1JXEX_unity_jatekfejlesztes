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
    public void Shoot()
    {
        GameObject Projectile =Instantiate(currentWeapon.projectilePrefab,weaponPos.position,weaponPos.rotation,null);
        Rigidbody prb = Projectile.GetComponent<Rigidbody>();
        prb.AddForce(weaponPos.forward * currentWeapon.projectilespeed);
        Destroy(Projectile, 5f);
    }

}
