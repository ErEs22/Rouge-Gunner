using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LREnemy_1Projectile : BaseProjectile
{
    [SerializeField] float explodeRange = 2f;

    protected override void OnTriggerEnter(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRange, 1 << 6);
        if (colliders.Length > 0)
        {
            IDamageable damageable;
            if (colliders[0].TryGetComponent<IDamageable>(out damageable))
            {
                damageable.TakeDamage(damageValue);
            }
        }
        this.enabled = false;
        gameObject.SetActive(false);
    }
}
