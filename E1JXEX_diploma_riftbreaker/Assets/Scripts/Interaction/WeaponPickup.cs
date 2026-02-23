using TMPro;
using UnityEngine;

public class WeaponPickup : MonoBehaviour, IInteractable
{
    public TMP_Text title;
    public Weapon weapon;
    void Start()
    {
        title.text = weapon.weaponName;
    }
    public void Interact()
    {
        Weapon temp = WeaponManager.instance.CurrentWeapon;
        WeaponManager.instance.CurrentWeapon = weapon;
        weapon = temp;
        title.text = weapon.weaponName;
    }
}