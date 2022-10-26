using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 普通挟枪敌人；攻击力中等，敏捷一般。
/// </summary>
public class LREnemy_2 : EnemyController
{
    [SerializeField] float waitForPreparedTime = 0.5f;

    [SerializeField] Transform muzzlePoint;

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
        PoolManager.Release(projectilePrefab, muzzlePoint.position, Quaternion.LookRotation((currentTargetPos - muzzlePoint.position), Vector3.up));
        yield return waitForFinished;
        isAttackFinished = true;
    }
}
