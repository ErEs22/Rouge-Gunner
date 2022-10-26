using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 持盾型敌人，追击玩家会持盾前进，攻击时才会有破绽；攻击力中等，敏捷低。
/// </summary>
public class CCEnemy_2 : EnemyController
{
    [SerializeField] float waitForPreparedTime = 0.5f;

    [SerializeField] Collider shieldCollider;

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
        shieldCollider.enabled = false;
        IDamageable damageable;
        if (other.TryGetComponent<IDamageable>(out damageable))
        {
            damageable.TakeDamage(enemyStatsManager.ATK);
        }
    }

    protected override IEnumerator AttackCoroutine()
    {
        yield return waitForPrepared;
        shieldCollider.enabled = true;
        anim.CrossFade(attackName, 0.1f);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length - 1f);
        shieldCollider.enabled = false;
        yield return waitForFinished;
        isAttackFinished = true;
    }
}
