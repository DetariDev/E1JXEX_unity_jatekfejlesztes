using UnityEngine;
public class PlayerAttack : MonoBehaviour
{
    bool isFiring= false;
    private float nextFireTime;

    private void Update()
    {
        Weapon currentWeapon = WeaponManager.instance.currentWeapon;
        if (currentWeapon == null) return;
        switch (currentWeapon.weaponType)
        {
            case WeaponType.auto:
                isFiring = InputManager.instance.input.Player.Attack.IsPressed();
                break;
            case WeaponType.semi:
                isFiring = InputManager.instance.input.Player.Attack.WasPerformedThisFrame();
                break;
            case WeaponType.shotgun:
                isFiring = InputManager.instance.input.Player.Attack.WasPerformedThisFrame();
                break;
            default:
                break;
        }
        if (isFiring && Time.time >= nextFireTime)
        {
            WeaponManager.instance.Shoot();
            nextFireTime = Time.time + currentWeapon.fireRate;
        }
    }
}