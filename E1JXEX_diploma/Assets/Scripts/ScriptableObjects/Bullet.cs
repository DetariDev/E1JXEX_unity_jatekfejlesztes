using UnityEngine;
public enum BulletType
{
    normal,
    fire,
    ice,
    poison
}

[CreateAssetMenu(fileName = "NewBullet", menuName = "Weapons/New Bullet")]
public class Bullet : ScriptableObject
{
    public BulletType bulletType;
    public int damage;
}
