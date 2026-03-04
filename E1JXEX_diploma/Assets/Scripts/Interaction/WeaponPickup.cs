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
        WeaponManager.instance.availableWeapons.Add(weapon);
        title.text = "";
    }
}