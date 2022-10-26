using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 小怪生成型敌人，可以生成定量的小怪，小怪会冲向玩家，造成伤害；本体无攻击力，敏捷低；小怪攻击力低，敏捷高。
/// </summary>
public class LREnemy_4 : EnemyController
{
    [SerializeField] float waitForPreparedTime = 0.5f;

    [SerializeField] float waitForBetweenTime = 1f;

    [SerializeField] Transform[] muzzlePoints = new Transform[4];

    [SerializeField] GameObject projectilePrefab;

    WaitForSeconds waitForPrepared;

    WaitForSeconds waitForFinished;

    WaitForSeconds waitForBetween;

    protected override void Awake()
    {
        base.Awake();
        waitForPrepared = new WaitForSeconds(waitForPreparedTime);
        waitForFinished = new WaitForSeconds(enemyStatsManager.ATKInteval);
        waitForBetween = new WaitForSeconds(waitForBetweenTime);
    }

    protected override IEnumerator AttackCoroutine()
    {
        yield return waitForPrepared;
        SetCurrentTargetPos();
        for (int i = 0; i < muzzlePoints.Length; i++)
        {
            PoolManager.Release(projectilePrefab, muzzlePoints[i].position);
            yield return waitForBetween;
        }
        yield return waitForFinished;
        isAttackFinished = true;
    }
}
