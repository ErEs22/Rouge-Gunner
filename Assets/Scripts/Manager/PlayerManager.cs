using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    WeaponManager weaponManager;
    PlayerMovement playerMovement;
    InputManager inputManager;
    CameraManager cameraManager;
    Animator anim;

    public static PlayerManager Instance;

    [Header("人物状态")]
    //public bool isRun;
    public bool isWalk;
    public bool isJump;
    public bool isGround;
    public bool isDodgeCD;
    public bool isFalculaing;
    public bool isInteracting;
    public bool isShooting;
    //一个状态值控制人物在使用火控系统时不能换弹
    public bool usingAbility;
    public bool canRotate = true;
    public bool isTeleporting;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        weaponManager = GetComponent<WeaponManager>();
        playerMovement = GetComponent<PlayerMovement>();
        inputManager = GetComponent<InputManager>();
        anim = GetComponentInChildren<Animator>();
        cameraManager = FindObjectOfType<CameraManager>();
    }
    private void Start()
    {
        PrepareTeleporting();
    }

    private void Update()
    {
        float delta = Time.deltaTime;

        inputManager.TickInput(delta);
        if (!isTeleporting)
        playerMovement.Move(delta);
        playerMovement.HandleRotation();
        cameraManager.CheckForInteractableObject();
        isWalk = Mathf.Abs(inputManager.horizontal) > 0 || Mathf.Abs(inputManager.vertical) > 0 ? true : false;
    }

    private void LateUpdate()
    {
        float delta = Time.deltaTime;
        inputManager.TickInput_Late(delta);
        isInteracting = anim.GetBool("isInteracting");
        inputManager.shootTap_input = false;
        inputManager.abilityTap_Input = false;

    }


    public void SpawnCharacter(Vector3 pos)
    {
        transform.position = pos;
    }

    public void TraverseOtherRoom(Vector3 pos)
    {
        transform.position = pos;
    }

    public void PrepareTeleporting()
    {
        isTeleporting = true;
        playerMovement.ClearVelocity();
    }
}
