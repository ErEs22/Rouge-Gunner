using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    [SerializeField] protected float speed = 10f;

    [SerializeField] protected float damageValue = 10f;

    private void Update()
    {
        Move();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        //TODO 爆炸效果
        IDamageable damageable;
        if (other.TryGetComponent<IDamageable>(out damageable))
        {
            damageable.TakeDamage(damageValue);
        }
        print("Hit:" + other.gameObject.name);
        gameObject.SetActive(false);
    }

    protected virtual void Move()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
    }
}
