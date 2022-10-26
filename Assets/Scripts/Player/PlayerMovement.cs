using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerStatsManager playerStatsManager;
    InputManager inputManager;
    PlayerAnimatorManager playerAnimatorManager;
    [SerializeField] FalculaControl falculaControl;

    public CharacterController controller;
    private Transform mainCamera;
    [SerializeField] Transform rotateHand;


    private float speed;
    public Vector3 verticalVelocity;//该速率受人为控制影响
    private Vector3 environVelocity;//仅自然因素对垂直方向的影响
    [SerializeField]
    private Vector3 displacementVelocity;//位移手段导致的移动
    public Vector3 characterVelocity;//水平方向人为控制


    private float gravity = -18.62f;//重力加速度

    [Header("地面检测")]
    [SerializeField] private Transform groundCheck;//地面检测
    [SerializeField] private float groundDistance = 0.01f;//检测半径
    [SerializeField] private LayerMask groundMask;//地面层

    [Header("斜坡")]
    [SerializeField] private float slopeForce = 10f;//走斜坡时施加的力度
    [SerializeField] private float slopeFprceRayLength = 2f;//斜坡射线长度

    [Header("抓钩")]
    [SerializeField] Transform falcula;
    [SerializeField] LayerMask catchableLayer;//抓钩能抓的层
    [SerializeField] LayerMask playerLayer;//玩家层


    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        inputManager = GetComponent<InputManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        controller = GetComponent<CharacterController>();
    }
    void Start()
    {
        playerAnimatorManager.Initialize();
        Cursor.lockState = CursorLockMode.Locked;

    }

    public void CheckGround()
    {
        playerManager.isGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (playerManager.isGround && verticalVelocity.y < 0)
        {
            playerManager.isJump = false;
            playerStatsManager.currentJumpsFrequency = playerStatsManager.jumpsFrequency;
            verticalVelocity.y = -2f;
            if (!falculaControl.grappling)
            {
                falculaControl.airVelocity = Vector3.zero;
            }
        }
    }

    public void Move(float delta)
    {
        if (!playerManager.isFalculaing)
        {
            HandleEnviromentVelocity();

            HandleHorizontalVelocity();
            Gravity(delta);
        }
        HandleDisplacementVelocity();
        controller.Move((characterVelocity + displacementVelocity + verticalVelocity + environVelocity) * Time.deltaTime);

        CheckFalcula();
        CheckGround();
    }

    public void DisableGravity()
    {
        gravity = 0f;
    }

    public void EnableGravity()
    {
        gravity = -18.62f;
    }

    public void Gravity(float delta)
    {
        verticalVelocity.y += gravity * delta;
    }

    /// <summary>
    /// 斜坡检测
    /// </summary>
    /// <returns></returns>
    public bool OnSlpe()
    {
        if (playerManager.isJump) return false;

        RaycastHit hit;
        //向下打出射线（检测是否在斜坡上）
        if (Physics.Raycast(transform.position, Vector3.down, out hit, controller.height / 2 * slopeFprceRayLength))
        {
            //如果接触到的点的法线不在（0，1，0）的方向上，那么人物就在斜坡上
            if (hit.normal != Vector3.up) return true;
        }
        return false;
    }

    public void HandleRotation()
    {
        if (playerManager.canRotate)
        {
            rotateHand.localRotation = Quaternion.Euler(inputManager.xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * inputManager.mouseX);
        }

    }

    public void HandleJump()
    {
        if (playerStatsManager.currentJumpsFrequency > 0)
        {
            playerManager.isJump = true;
            playerStatsManager.currentJumpsFrequency--;
            verticalVelocity.y = Mathf.Sqrt(playerStatsManager.currentJumpHeight * -2f * gravity);
            // playerAnimatorManager.PlayTargetAnimation("Jump", true);
        }
    }

    public IEnumerator HandleDodge()
    {
        if (inputManager.horizontal + inputManager.vertical == 0) yield break;
        displacementVelocity = (transform.right * inputManager.horizontal + transform.forward * inputManager.vertical)
            * playerStatsManager.currenDodgeSpeed;
        yield return new WaitForSeconds(0.2f);
        displacementVelocity = Vector3.zero;
        playerManager.isDodgeCD = true;
        yield return new WaitForSeconds(playerStatsManager.DodgeCD);
        playerManager.isDodgeCD = false;
    }



    public void HandleFalcula()
    {
        // falcula.gameObject.SetActive(true);
        falculaControl.StartGrappling();
    }

    public Vector3 FalculaVelocity()
    {
        return falculaControl.airVelocity;
        // playerManager.isFalculaing = true;
        // Vector3 falculaDirection;
        // falculaDirection = falculaControl.hitPositon - transform.position;
        // return falculaDirection.normalized * playerStatsManager.currenFalculaSpeed;
    }

    private void CheckFalcula()
    {
        if (playerManager.isFalculaing)
        {
            if (Physics.CheckSphere(falculaControl.hitPositon, 0.75f, playerLayer))
            {
                displacementVelocity = Vector3.zero;
                playerManager.isFalculaing = false;
            }
        }
    }

    private Vector3 HandleMovement()
    {
        Vector3 moveDirection = Vector3.zero;
        speed = playerStatsManager.currentWalkSpeed;
        if (!falculaControl.grappling)
        {
            playerAnimatorManager.UpdateAnimatorValues(inputManager.moveAmount, 0);
            moveDirection = transform.right * inputManager.horizontal + transform.forward * inputManager.vertical;
        }
        else
        {
            playerAnimatorManager.UpdateAnimatorValues(0, 0);
        }
        return moveDirection * speed;
    }

    /// <summary>
    /// 玩家水平面上的位移
    /// </summary>
    private void HandleHorizontalVelocity()
    {
        characterVelocity = HandleMovement();
    }

    /// <summary>
    /// 环境，自然因素导致的位移
    /// </summary>
    private void HandleEnviromentVelocity()
    {
        Vector3 slopeVeclocity;
        if (OnSlpe())
        {
            slopeVeclocity = Vector3.down * controller.height / 2 * slopeForce;
        }
        else
        {
            slopeVeclocity = Vector3.zero;
        }
        environVelocity = slopeVeclocity;
    }

    /// <summary>
    /// 处理玩家行为（钩爪，道具）带来的位移
    /// </summary>
    private void HandleDisplacementVelocity()
    {
        displacementVelocity = FalculaVelocity();
    }

    public void ClearVelocity()
    {
        characterVelocity = Vector3.zero;
        displacementVelocity = Vector3.zero;
        verticalVelocity = Vector3.zero;
    }

}
