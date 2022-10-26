using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自爆型敌人，在自爆范围内会自动爆炸，攻击力高，敏捷低。
/// </summary>
public class CCEnemy_5 : EnemyController
{
    [SerializeField] float waitForPreparedTime = 0.5f;

    [SerializeField] float explodeRange = 3f;

    WaitForSeconds waitForPrepared;

    WaitForSeconds waitForFinished;

    protected override void Awake()
    {
        base.Awake();
        waitForPrepared = new WaitForSeconds(waitForPreparedTime);
        waitForFinished = new WaitForSeconds(enemyStatsManager.ATKInteval);
    }

    bool IsPlayerInExplodeRange() => Physics.CheckSphere(transform.position, explodeRange, playerMask);

    protected override IEnumerator AttackCoroutine()
    {
        yield return waitForPrepared;
        anim.CrossFade(attackName, 0.1f);
        if (IsPlayerInExplodeRange())
        {
            IDamageable damageable;
            if (playerTrans.gameObject.TryGetComponent<IDamageable>(out damageable))
            {
                damageable.TakeDamage(enemyStatsManager.ATK);
            }
        }
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length - 1f);
        enemyStatsManager.currentHealth = 0;
        yield return waitForFinished;
        isAttackFinished = true;
    }
}
