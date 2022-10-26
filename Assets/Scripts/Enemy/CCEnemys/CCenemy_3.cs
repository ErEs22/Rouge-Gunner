using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 持刀型敌人，在攻击范围内才会对玩家进行攻击，并且拥有较高的血量；攻击力高，敏捷低。
/// </summary>
public class CCenemy_3 : EnemyController
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
        anim.CrossFade(attackName, 0.1f);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length - 1f);
        weaponCollider.enabled = false;
        yield return waitForFinished;
        isAttackFinished = true;
    }
}
