using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using VInspector;
using VInspector.Libs;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance { get; private set; }
    public event Action<Weapon> OnWeaponChanged;



    [Foldout("Fegyver")]
    public List<Weapon> availableWeapons = new List<Weapon>();
    public List<WeaponRecipe> weaponRecipes = new List<WeaponRecipe>();
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
        if (!availableWeapons.Contains(StartingWeapon) && StartingWeapon != null)
        {
            availableWeapons.Add(StartingWeapon);
        }
        InputManager.instance.input.Player.ChangeWeapon.performed += ChangeWeapon;
    }
    private int currentWeaponIndex = 0;

    public void ChangeWeapon(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (availableWeapons.Count <= 1) return;
        float scrollValue = context.ReadValue<Vector2>().y;

        if (scrollValue > 0)
        {
            currentWeaponIndex = (currentWeaponIndex + 1) % availableWeapons.Count;
        }
        else if (scrollValue < 0)
        {
            currentWeaponIndex = (currentWeaponIndex - 1 + availableWeapons.Count) % availableWeapons.Count;
        }
        CurrentWeapon = availableWeapons[currentWeaponIndex];

    }
}
