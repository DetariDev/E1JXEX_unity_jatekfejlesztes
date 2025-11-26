using UnityEngine;
public enum WeaponType
{
    auto,
    semi,
    shotgun
}

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/New Weapon")]
public class Weapon : ScriptableObject
{
    public WeaponType weaponType;
    public string weaponName;
    public int damage;
    public float fireRate;
    public float range;
    public float projectilespeed;
    public GameObject projectilePrefab;
}
