using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 炮弹投射型敌人，攻击时，会在头顶聚集三颗炮弹，数秒之后会向玩家发射，炮弹爆炸时，在范围内的玩家会受到伤害；攻击力高，敏捷低。
/// </summary>
public class LREnemy_1 : EnemyController
{
    [SerializeField] GameObject projectilePrefab;

    [SerializeField] Transform[] projectileGeneratePoints;

    [SerializeField] float waitForAttackTime = 1f;

    [SerializeField] float waitForPreparedTime = 0.5f;

    WaitForSeconds waitForAttack;

    WaitForSeconds waitForFinished;

    WaitForSeconds waitForPrepared;

    GameObject[] projectiles = new GameObject[3];

    protected override void Awake()
    {
        base.Awake();
        waitForAttack = new WaitForSeconds(waitForAttackTime);
        waitForFinished = new WaitForSeconds(enemyStatsManager.ATKInteval);
        waitForPrepared = new WaitForSeconds(waitForPreparedTime);
    }

    protected override IEnumerator AttackCoroutine()
    {
        yield return waitForPrepared;
        SetCurrentTargetPos();
        for (int i = 0; i < projectileGeneratePoints.Length; i++)
        {
            projectiles[i] = PoolManager.Release(projectilePrefab, projectileGeneratePoints[i].position, Quaternion.LookRotation((currentTargetPos - projectileGeneratePoints[i].position), Vector3.up));
        }
        yield return waitForAttack;
        for (int i = 0; i < projectiles.Length; i++)
        {
            projectiles[i].GetComponent<BaseProjectile>().enabled = true;
        }
        yield return waitForFinished;
        isAttackFinished = true;
    }
}
