using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制型敌人，发射物可以让玩家减速或停顿，发射物直接击中停顿，溅射范围内减速，攻击力低，敏捷一般。
/// </summary>
public class LREnemy_3 : EnemyController
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
