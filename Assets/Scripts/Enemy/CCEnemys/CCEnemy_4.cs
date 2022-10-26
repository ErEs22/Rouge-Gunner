using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 持双刀型敌人，冲刺型攻击，攻击力中等，敏捷高。
/// </summary>
public class CCEnemy_4 : EnemyController
{
    [SerializeField] float waitForPreparedTime = 0.5f;

    [SerializeField] Collider weaponCollider;

    WaitForSeconds waitForPrepared;

    WaitForSeconds waitForFinished;

    protected override void Awake()
    {
        base.Awake();
        waitForPrepared = new WaitForSeconds(waitForPreparedTime);
        waitForFinished = new WaitForSeconds(enemyStatsManager.ATKInteval);
    }

    private void OnTriggerEnter(Collider other)
    {
        weaponCollider.enabled = false;
        IDamageable damageable;
        if (other.TryGetComponent<IDamageable>(out damageable))
        {
            damageable.TakeDamage(enemyStatsManager.ATK);
        }
    }

    protected override IEnumerator AttackCoroutine()
    {
        yield return waitForPrepared;
        weaponCollider.enabled = true;
        ai.isStopped = false;
        SetCurrentTargetPos();
        ai.destination = currentTargetPos + (transform.position - currentTargetPos).normalized * 1.5f;
        ai.maxSpeed *= 2;
        anim.CrossFade(attackName, 0.1f);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length - 1.5f);
        weaponCollider.enabled = false;
        ai.isStopped = true;
        ai.maxSpeed *= 0.5f;
        yield return waitForFinished;
        isAttackFinished = true;
    }
}
