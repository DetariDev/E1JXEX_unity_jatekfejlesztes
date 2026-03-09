using UnityEngine;
using UnityEngine.UIElements;

public class Shooting : MonoBehaviour
{
    public void Shoot(GameObject owner,Weapon usedWeapon, Transform target)
    {
        Vector3 shootDirection = owner.transform.forward;
        if (target != null)
        {
            
            shootDirection = (target.position - owner.transform.position).normalized;
            shootDirection.y = 0;
        }
        
        for (int i = 0; i < usedWeapon.projectileCount; i++)
        {
            Quaternion baseRotation = Quaternion.LookRotation(shootDirection);
            if (usedWeapon.projectileCount > 1)
            {
                baseRotation = Quaternion.Euler(0,Random.Range(-15f, 15f), 0) * baseRotation;
            }
            GameObject Projectile = Instantiate(usedWeapon.projectilePrefab, new Vector3(owner.transform.position.x,0.5f,owner.transform.position.z), baseRotation, null);
            Projectile projectilecomponent = Projectile.GetComponent<Projectile>();
            projectilecomponent.damage = usedWeapon.damage;
            projectilecomponent.bulletType = usedWeapon.bulletType;
            projectilecomponent.owner = owner;
            Rigidbody prb = Projectile.GetComponent<Rigidbody>();
            prb.AddForce(baseRotation * Vector3.forward * usedWeapon.projectilespeed, ForceMode.Impulse);
            Destroy(Projectile, 5f);
        }
    }
}
