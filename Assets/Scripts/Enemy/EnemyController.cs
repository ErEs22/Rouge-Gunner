using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(EnemyManager))]
[RequireComponent(typeof(EnemyStatsManager))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] bool drawGizmos = true;
    //如果此敌人会攻击玩家，则设为true
    public bool attack = true;
    [Header("---Idle---")]
    public string idleName = "Idle";

    [Header("---Death---")]
    public string deathStateAnimation;

    public string deathName = "Death";

    [Header("---Escape---")]
    public string escapeStateAnimation;

    [Header("---Guard---")]
    public string guardStateAnimation;

    public string walkName = "Walk";

    [SerializeField] float guardRange = 6f;

    [SerializeField] float idleTime = 2f;

    [SerializeField] Transform[] guardPoints;

    [SerializeField] Transform raycastGuardPoint;

    [SerializeField] LayerMask guardRaycastMask;

    [Header("---Chase---")]
    public string chaseStateAnimation;

    public string runName = "Run";

    [SerializeField] float chaseRange = 10f;

    [SerializeField] protected LayerMask playerMask;

    [SerializeField] LayerMask visibleCastMask;

    [Header("---Attack---")]
    public string attackStateAnimation;

    public string attackName = "Attack";

    [SerializeField] float attackRange = 4f;

    [SerializeField] float rotateSpeed = 0.1f;

    [SerializeField] Transform raycastPlayerPoint;

    [HideInInspector] public bool isAttackFinished = false;

    WaitForSeconds waitforGuard;

    Coroutine guardCoroutine;

    protected EnemyManager enemyManager;

    protected EnemyStatsManager enemyStatsManager;

    protected Animator anim;

    protected Transform playerTrans;

    RaycastHit[] hits = new RaycastHit[4];//巡逻射线检测结果数组

    RaycastHit playerHit;//检测玩家结果

    Vector3[] guardWayPoints = new Vector3[4];//巡逻点

    protected Vector3 currentTargetPos;

    protected IAstarAI ai;

    RaycastHit escapeHit;

    int guardWayIndex = 0;

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        enemyManager = GetComponent<EnemyManager>();
        enemyStatsManager = GetComponent<EnemyStatsManager>();
        waitforGuard = new WaitForSeconds(idleTime);
        playerTrans = GameObject.FindGameObjectWithTag("PlayerDetect")?.transform;
        ai = GetComponent<IAstarAI>();
    }

    private void Start()
    {
        for (int i = 0; i < hits.Length; i++)
        {
            //通过射线确定巡逻的点，避免高低不平的地面导致AI走不到该去的地方
            Physics.Raycast(raycastGuardPoint.position, (guardPoints[i].position - raycastGuardPoint.position), out hits[i], 10f, guardRaycastMask);
            guardWayPoints[i] = hits[i].point;
        }
    }

    public void TakeDamage(float damage)
    {
        if (enemyStatsManager.currentHealth <= damage)
        {
            enemyStatsManager.currentHealth = 0;
            //TODO 死亡
        }
        enemyStatsManager.currentHealth -= damage;
    }

    public bool IsAlive()
    {
        if (enemyStatsManager.currentHealth <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void Die()
    {
        Destroy(gameObject, 10f);
    }

    /// <summary>
    /// 向玩家朝向反向发射射线，射线会有一定的随机量，射线如果击中,则设定该点为目标点，否则重新发射射线，重新计算超过一定次数后，则会向玩家方向发射射线，表示侧方无可用目标点
    /// </summary>
    public void SetEscapePoint()
    {
        EscapeRayCast(100, 10, false);
        int recaculateCount = 0;
        while (escapeHit.collider == null)
        {
            while (recaculateCount >= 5)
            {
                EscapeRayCast(30, 25, true);
                if (escapeHit.collider != null)
                {
                    ai.destination = escapeHit.point;
                    return;
                }
            }
            recaculateCount++;
            EscapeRayCast(100, 10, false);
        }
        ai.destination = escapeHit.point;
    }

    /// <summary>
    /// 射线获取逃离目标点
    /// </summary>
    /// <param name="horizontalRandom">水平随机旋转</param>
    /// <param name="verticalRotation">垂直旋转</param>
    /// <param name="towardsPlayer">是否面向玩家</param>
    void EscapeRayCast(int horizontalRandom, int verticalRotation, bool towardsPlayer)
    {
        Transform raycastPoint = raycastGuardPoint;
        if (towardsPlayer)
        {
            raycastPoint.rotation = Quaternion.LookRotation((playerTrans.position - transform.position), Vector3.up);
        }
        else
        {
            raycastPoint.rotation = Quaternion.LookRotation((transform.position - playerTrans.position), Vector3.up);
        }
        raycastPoint.Rotate(new Vector3(0, Random.Range(-horizontalRandom, horizontalRandom), 0), Space.World);
        raycastPoint.Rotate(new Vector3(verticalRotation, 0, 0), Space.Self);
        Physics.Raycast(raycastPoint.position, raycastPoint.forward, out escapeHit, 100f, 1 << 13);
    }

    /// <summary>
    /// 敌人的攻击实现
    /// </summary>
    public void Attack()
    {
        StartCoroutine(nameof(AttackCoroutine));
    }

    public void StopAttack()
    {
        StopCoroutine(nameof(AttackCoroutine));
    }

    protected virtual IEnumerator AttackCoroutine()
    {
        yield break;
    }

    public void SetCurrentTargetPos()
    {
        currentTargetPos = playerTrans.position;
    }

    /// <summary>
    /// 检查玩家是否在周围
    /// </summary>
    /// <param name="playerMask">玩家遮罩层</param>
    /// <param name="guardDistance">检测距离</param>
    /// <returns></returns>
    public bool CheckPlayerAround()
    {
        bool result = Physics.CheckSphere(transform.position, chaseRange, playerMask);
        return result;
    }

    /// <summary>
    /// 巡逻时检查玩家是否在周围
    /// </summary>
    /// <param name="playerMask">玩家遮罩层</param>
    /// <param name="guardDistance">检测距离</param>
    /// <returns></returns>
    public bool CheckPlayerAroundInGuard()
    {
        bool result = Physics.CheckSphere(transform.position, guardRange, playerMask);
        return result;
    }

    /// <summary>
    /// 检查玩家是否可见
    /// </summary>
    /// <param name="castMask">射线可检测的物体遮罩层</param>
    /// <param name="visibleDistance">可见距离</param>
    /// <returns></returns>
    public bool PlayerVisible()
    {
        Physics.Raycast(raycastPlayerPoint.position, (playerTrans.position - raycastPlayerPoint.position), out playerHit, chaseRange + 1, visibleCastMask);
        Debug.DrawLine(raycastPlayerPoint.position, playerTrans.position, Color.red);
        if (playerHit.collider != null && (playerMask & (1 << playerHit.collider.gameObject.layer)) != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 检测是否可攻击玩家
    /// </summary>
    /// <param name="attackDistance">攻击距离</param>
    /// <returns></returns>
    public bool Attackable()
    {
        if (Vector3.Distance(transform.position, playerTrans.position) <= attackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void TowardsPlayer()
    {
        Vector3 lookDir = new Vector3(playerTrans.position.x, 0, playerTrans.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDir), rotateSpeed);
    }

    public void SetTarget(Vector3 position)
    {
        ai.destination = position;
    }

    public void ChasePlayer()
    {
        ai.destination = playerTrans.position;
    }

    public void StartGuardCoroutine(float transitionDuration, IAstarAI ai)
    {
        guardCoroutine = StartCoroutine(GuardCoroutine(transitionDuration, ai));
    }

    public void StopGuardCoroutine()
    {
        StopCoroutine(guardCoroutine);
    }

    /// <summary>
    /// 警戒巡逻状态携程
    /// </summary>
    /// <param name="idleStateName">空闲状态动画名称</param>
    /// <param name="walkStateName">走路状态动画名称</param>
    /// <param name="transitionDuration">动画片段过度时间</param>
    /// <param name="ai">敌人的IAstarAI接口</param>
    /// <returns></returns>
    IEnumerator GuardCoroutine(float transitionDuration, IAstarAI ai)
    {
        while (true)
        {
            anim.CrossFade(idleName, transitionDuration);
            yield return waitforGuard;
            ai.isStopped = false;
            // ai.canMove = true;
            anim.CrossFade(walkName, transitionDuration);
            SetTarget(guardWayPoints[guardWayIndex]);
            yield return new WaitForSeconds(0.2f);
            while (ai.velocity != Vector3.zero)
            {
                yield return null;
            }
            ai.isStopped = true;
            if (guardWayIndex == guardPoints.Length - 1)
            {
                guardWayIndex = 0;
            }
            else
            {
                guardWayIndex++;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!drawGizmos)
        {
            return;
        }
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, guardRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
