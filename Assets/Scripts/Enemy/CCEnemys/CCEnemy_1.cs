using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 持刀型敌人，在攻击范围内才会对玩家进行攻击；攻击力中等，敏捷高。
/// </summary>
public class CCEnemy_1 : EnemyController
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
        print("HitSomething");
    }

    protected override IEnumerator AttackCoroutine()
    {
        yield return waitForPrepared;
        weaponCollider.enabled = true;
        anim.CrossFade(attackName, 0.1f);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length - 1.3f);
        weaponCollider.enabled = false;
        yield return waitForFinished;
        isAttackFinished = true;
    }
}
