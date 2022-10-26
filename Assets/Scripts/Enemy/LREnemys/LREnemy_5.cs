using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 普通多向攻击敌人，发射物击中的区域会造成灼烧伤害，直接命中玩家会扣血并造成持续伤害；攻击力低，敏捷一般。
/// </summary>
public class LREnemy_5 : EnemyController
{
    [SerializeField] float waitForPreparedTime = 0.5f;

    [SerializeField] Transform[] muzzlePoints = new Transform[4];

    [SerializeField] GameObject projectilePrefab;

    WaitForSeconds waitForPrepared;

    WaitForSeconds waitForFinished;

    protected override void Awake()
    {
        base.Awake();
        waitForPrepared = new WaitForSeconds(waitForPreparedTime);
        waitForFinished = new WaitForSeconds(enemyStatsManager.ATKInteval);
    }

    protected override IEnumerator AttackCoroutine()
    {
        yield return waitForPrepared;
        SetCurrentTargetPos();
        for (int i = 0; i < muzzlePoints.Length; i++)
        {
            PoolManager.Release(projectilePrefab, muzzlePoints[i].position, muzzlePoints[i].rotation);
        }
        yield return waitForFinished;
        isAttackFinished = true;
    }
}
