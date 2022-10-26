using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FalculaControl : MonoBehaviour
{
    [SerializeField] Transform playerTransform;

    [SerializeField] PlayerMovement playerMovement;

    [SerializeField] Transform hookObject;

    [SerializeField] float maxGrapDistance;

    [SerializeField] float airSpeed = 8f;

    [SerializeField] float finishGrapThreshold = 1f;

    [SerializeField] int cacheVelCount = 3;

    [SerializeField] float airVerticalVelMultiple = 2f;

    [SerializeField] float throwSpeed = 0.1f;

    [SerializeField] LayerMask grappableMask;

    [HideInInspector] public Vector3 hitPositon;

    LineRenderer lr;

    Coroutine grapplingCoroutine;

    Ray grapRay;

    RaycastHit grapHit;

    Vector3[] cacheVelocity;

    Vector3 avgVelocity;

    int cacheVelIndex = 0;

    [DisplayOnly] public bool grappling;

    [DisplayOnly] public Vector3 airVelocity;

    private void Awake()
    {
        lr = GetComponentInChildren<LineRenderer>();
        cacheVelocity = new Vector3[cacheVelCount];
    }

    private void Update()
    {
        if (!grappling)
        {
            avgVelocity = GetAvergeVelocity(playerMovement.characterVelocity);
        }
    }

    /// <summary>
    /// 发射钩爪，如果没有勾中物体，则不会移动和渲染线
    /// </summary>
    public void StartGrappling()
    {
        bool isGrapTarget = false;
        Vector3 grapTarget = Vector3.zero;
        (grapTarget, isGrapTarget) = GetGrapTarget();
        lr.enabled = true;
        lr.SetPosition(0, hookObject.position);
        lr.SetPosition(1, hookObject.position);
        if (grapplingCoroutine != null)
        {
            StopCoroutine(grapplingCoroutine);
        }
        if (isGrapTarget)
        {
            grappling = true;
            grapplingCoroutine = StartCoroutine(GrapplingCoroutine(grapTarget));
        }
        else
        {
            SetAirVelocity(Vector3.zero);
            Invoke(nameof(DisableLine), 0.3f);
        }
    }

    void DisableLine()
    {
        lr.enabled = false;
    }

    /// <summary>
    /// 获取发射钩爪前三帧的平均移动速度
    /// </summary>
    /// <param name="newVelocity">当前帧的移动速度</param>
    /// <returns></returns>
    Vector3 GetAvergeVelocity(Vector3 newVelocity)
    {
        cacheVelocity[cacheVelIndex] = newVelocity;
        cacheVelIndex++;
        cacheVelIndex %= cacheVelCount;
        Vector3 avgVelocity = Vector3.zero;
        foreach (Vector3 item in cacheVelocity)
        {
            avgVelocity += item;
        }
        return (avgVelocity / cacheVelCount) * airVerticalVelMultiple;
    }

    /// <summary>
    /// 获取勾中点
    /// </summary>
    /// <returns>返回射线射中的点；如果没有射中物体，返回false</returns>
    (Vector3, bool) GetGrapTarget()
    {
        grapRay = Camera.main.ScreenPointToRay(new Vector2(960f, 540f));
        if (Physics.Raycast(grapRay, out grapHit, maxGrapDistance, grappableMask))
        {
            return (grapHit.point, true);
        }
        else
        {
            return (grapRay.direction * maxGrapDistance + grapRay.origin, false);
        }
    }

    void SetAirVelocity(Vector3 direction) => airVelocity = direction.normalized * airSpeed;

    /// <summary>
    /// 确定勾中之后，开始渲染线，然后根据前三帧缓存平均速度的方向与线的方向进行比较，如果角度过大则会断，后退时勾中前方的目标也会断
    /// </summary>
    /// <param name="grapTarget">勾中点坐标</param>
    /// <returns></returns>
    IEnumerator GrapplingCoroutine(Vector3 grapTarget)
    {
        Vector3 hookPos = hookObject.position;
        while (true)
        {
            while ((hookPos - hookObject.position).magnitude <= (grapTarget - hookObject.position).magnitude - 0.5f)
            {
                hookPos = Vector3.Lerp(hookPos, grapTarget, throwSpeed);
                lr.SetPosition(1, hookPos);
                lr.SetPosition(0, hookObject.position);
                yield return null;
            }
            if ((hookObject.position - grapTarget).magnitude <= finishGrapThreshold || Vector3.Angle((grapTarget - hookObject.position), avgVelocity) >= 80f)
            {
                SetAirVelocity(avgVelocity);
                playerMovement.EnableGravity();

                grappling = false;
                DisableLine();
                yield break;
            }
            lr.SetPosition(0, hookObject.position);
            playerMovement.DisableGravity();
            SetAirVelocity((grapTarget - playerTransform.position) + avgVelocity);
            print(avgVelocity);
            yield return null;
        }
    }
    // [SerializeField] PlayerMovement playerMovement;
    // Rigidbody rb;

    // float falculaSpeed = 15f;//判定点飞行速度

    // public Transform falculaMuzzle;
    // public LayerMask layer;

    // private void Awake()
    // {
    //     rb = GetComponent<Rigidbody>();
    // }
    // private void OnEnable()
    // {
    //     transform.position = falculaMuzzle.position;
    //     transform.forward = falculaMuzzle.forward;
    //     rb.velocity = transform.forward.normalized * falculaSpeed;
    //     StartCoroutine(Destory());
    // }

    // private void Update()
    // {
    //     //判断挂钩是否抓到物体
    //     if (Physics.CheckSphere(transform.position, 0.1f, layer))
    //     {
    //         hitPositon = transform.position;
    //         Initialize();
    //     }       
    // }
    // IEnumerator Destory()
    // {
    //     yield return new WaitForSeconds(1.5f);
    //     Initialize();
    // }

    // void Initialize()
    // {
    //     if (transform.gameObject.activeInHierarchy)
    //     {
    //         transform.gameObject.SetActive(false);
    //         rb.velocity = Vector3.zero;
    //     }
    // }
}
