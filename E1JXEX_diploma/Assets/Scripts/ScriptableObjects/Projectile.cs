using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public BulletType bulletType;
    public GameObject owner;
    public GameObject hitEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("map"))
        {
            Destroy(Instantiate(hitEffect, transform.position, hitEffect.transform.rotation,null), 1f);
            Destroy(gameObject);

        }
    }
}