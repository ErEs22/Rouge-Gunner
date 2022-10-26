using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class LREnemy_4Projectile : BaseProjectile
{
    [SerializeField] float explodeRange = 2f;

    [SerializeField] float waitForChasingTime = 2f;

    [SerializeField] float waitForExplodeTime = 4f;

    IAstarAI ai;

    Transform playerTrans;

    WaitForSeconds waitForChasing;

    WaitForSeconds waitForExplode;

    private void Awake()
    {
        ai = GetComponent<IAstarAI>();
        ai.isStopped = true;

        playerTrans = GameObject.FindGameObjectWithTag("Player")?.transform;
        waitForChasing = new WaitForSeconds(waitForChasingTime);
        waitForExplode = new WaitForSeconds(waitForExplodeTime);
    }

    /// <summary>
    /// 生成后等待然后开始追踪玩家
    /// </summary>
    /// <returns></returns>
    IEnumerator Start()
    {
        yield return waitForChasing;
        ai.isStopped = false;
        StartCoroutine(nameof(ChasePlayer));
        yield return waitForExplode;
        ai.isStopped = true;
        Explode();
    }

    private void OnEnable()
    {
        StartCoroutine(nameof(Start));
    }

    private void OnDisable()
    {
        StopCoroutine(nameof(ChasePlayer));
    }

    protected override void Move()
    {
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRange, 1 << 6);
        if (colliders.Length > 0)
        {
            IDamageable damageable;
            colliders[0].TryGetComponent<IDamageable>(out damageable);
            damageable.TakeDamage(damageValue);
        }
        gameObject.SetActive(false);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        print("Triggered:Tag is" + other.gameObject.tag + "and name is:" + other.gameObject.name);
        if (other.tag == "Player")
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
            gameObject.SetActive(false);
        }
    }

    IEnumerator ChasePlayer()
    {
        while (true)
        {
            ai.destination = playerTrans.position;
            yield return null;
        }
    }
}
