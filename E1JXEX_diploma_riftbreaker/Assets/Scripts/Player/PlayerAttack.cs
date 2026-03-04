using UnityEngine;
public class PlayerAttack : MonoBehaviour
{
    bool isFiring= false;
    private float nextFireTime;
    WeaponManager weaponManager;
    PlayerManager playerManager;
    private Weapon currentWeapon;
    private void Awake()
    {
        weaponManager = WeaponManager.instance;
        playerManager = PlayerManager.Instance;
    }
    private void Start()
    {
        currentWeapon = weaponManager.CurrentWeapon;
        weaponManager.OnWeaponChanged += UpdateWeapon;
    }
    private void UpdateWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
    }
    private void Update()
    {
        if (currentWeapon == null ||playerManager.inBuildState || playerManager.inMenu) return;
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
            weaponManager.Shoot(this.gameObject);
            nextFireTime = Time.time + currentWeapon.fireRate;
        }
    }
}