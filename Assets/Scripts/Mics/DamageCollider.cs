using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [SerializeField] float damageValue = 10;

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable;
        if (other.TryGetComponent<IDamageable>(out damageable))
        {
            damageable.TakeDamage(damageValue);
        }
    }
}
