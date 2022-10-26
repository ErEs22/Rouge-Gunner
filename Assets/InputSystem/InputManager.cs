using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerInput inputActions;
    WeaponManager weaponManager;
    PlayerMovement playerMovement;
    PlayerManager playerManager;
    MapManager mapManager;
    UIManager uIManager;
    WeaponWheelManager weaponWheelManager;

    public Vector2 movementInput;
    public Vector2 mouseInput;

    [Header("Map")]
    Vector3 mapStartCursorPos;
    Vector2 mapStartCursorPos_Screen;
    Vector3 mapWorldCursorPos;
    Vector3 cameraStartPos;


    public float horizontal;
    public float vertical;
    public float moveAmount;

    public float mouseX;
    public float mouseY;

    public float mouseSensitivity = 20f;//鼠标灵敏度

    [Header("输入标识")]
    bool weaponWheel_input;
    public bool shootHold_input;
    public bool shootTap_input;
    public bool abilityTap_Input;
    public bool abilityHold_Input;
    public bool interact_Input;
    public bool map_Input;
    public bool escape_Input;
    bool menuInput;

    [SerializeField]
    private int weaponSwitchInput;
    public float xRotation;

    

    private void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerInput();
            inputActions.PlayerMovement.Walk.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed += inputActions => mouseInput = inputActions.ReadValue<Vector2>();
            inputActions.PlayerMovement.Jump.performed += i => OnJump();
            inputActions.PlayerAction.Shoot.started += i => shootTap_input = true;
            inputActions.PlayerAction.Shoot.performed += i => shootHold_input=true;
            inputActions.PlayerAction.Shoot.canceled += i => ResetShootInput();
            inputActions.PlayerAction.Dodge.performed += i => OnDodge();
            inputActions.PlayerAction.Falcula.performed += i => OnFalcula();
            inputActions.PlayerAction.Reload.started += i => OnReload();
            inputActions.PlayerAction.Weapon1.started += inputActions => SwitchWeaponByKey(0);
            inputActions.PlayerAction.Weapon2.started += inputActions => SwitchWeaponByKey(1);
            inputActions.PlayerAction.Weapon3.started += inputActions => SwitchWeaponByKey(2);
            inputActions.PlayerAction.Weapon4.started += inputActions => SwitchWeaponByKey(3);
            inputActions.PlayerAction.Weapon5.started += inputActions => SwitchWeaponByKey(4);
            inputActions.PlayerAction.Weapon6.started += inputActions => SwitchWeaponByKey(5);
            inputActions.PlayerAction.WeaponWheel.started += i => OpenWeaponWheel();
            inputActions.PlayerAction.WeaponWheel.performed += i => weaponWheel_input = true;
            inputActions.PlayerAction.WeaponWheel.canceled += i => CloseWeaponWheel();
            inputActions.PlayerAction.WeaponAbility.started += i => abilityTap_Input = true;
            inputActions.PlayerAction.WeaponAbility.performed +=i=> abilityHold_Input = true;
            inputActions.PlayerAction.WeaponAbility.canceled += i => ResetAbilityInput();
            inputActions.PlayerAction.Interact.performed+=i=>interact_Input = true;
            inputActions.PlayerAction.Interact.canceled+=i=>interact_Input = false;
            inputActions.PlayerAction.Map.performed+=i=>map_Input = true;
            inputActions.PlayerAction.Map.canceled += i => CloseMapUI();
            inputActions.GameAction.Menu.started += i => escape_Input = true;
            inputActions.GameAction.Menu.started += i => escape_Input = false;

        }
        inputActions.Enable();
    }

    private void ResetAbilityInput()
    {
        abilityTap_Input = false;
        abilityHold_Input = false;
    }

    private void Awake()
    {
        weaponManager = GetComponentInChildren<WeaponManager>();
        playerMovement = GetComponent<PlayerMovement>();
        playerManager = GetComponent<PlayerManager>();
        weaponWheelManager = FindObjectOfType<WeaponWheelManager>();
        mapManager = FindObjectOfType<MapManager>();
        uIManager = FindObjectOfType<UIManager>();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }

    //对外调用 放置于playermanager总线即Update()方法中监听
    public void TickInput(float delta)
    {
        if (!menuInput)
        {
            HandleMovement();
            Observe(delta);
            HandleAbilityInput();
            HandleShootInput();
        }
        HandleWeaponWheelInput();
        HandleMapInput();
        HandleEscapeInput();
        HandleEscapeInput();
    }

    public void TickInput_Late(float delta)
    {
        HandleMapCameraInput();
    }

    public void HandleMovement()
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
    }
    void OnJump()
    {
        playerMovement.HandleJump();
    }

    void OnDodge()
    {
        if (!playerManager.isDodgeCD && !playerManager.isFalculaing)
        {
            StartCoroutine(playerMovement.HandleDodge());
        }
    }

    void ResetShootInput()
    {
        shootTap_input = false;
        shootHold_input = false;
    }
    void OnFalcula()
    {
        playerMovement.HandleFalcula();
    }

    void HandleShootInput()
    {
        weaponManager.HandleShoot();
    }

    void HandleAbilityInput()
    {
        //weaponManager.HandleAbility();
    }
    /// <summary>
    /// 处理移动地图的操作放于LateUpdate
    /// </summary>
    void HandleMapCameraInput()
    {
        if (map_Input)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Vector2 cursourInScreen = Mouse.current.position.ReadValue();
            mapWorldCursorPos = mapManager.ConvertToWorldPosition(cursourInScreen);
            if (shootTap_input)
            {
                mapStartCursorPos_Screen= cursourInScreen;
                mapStartCursorPos = mapWorldCursorPos;
                cameraStartPos = mapManager.mapCam.transform.position;
            }
            if (mapManager.isInRect(mapStartCursorPos_Screen))
            {
                if (shootHold_input)
                {
                    Vector3 dir = mapWorldCursorPos - mapStartCursorPos;
                    mapManager.MoveMapCamera(cameraStartPos, dir);
                }
            }
        }
    }

    void HandleMapInput()
    {
        if (map_Input)
        {
            menuInput = true;
            mapManager.EnableMap();
            mapManager.DetectRoom(Mouse.current.position.ReadValue(), shootTap_input);
        }
    }

    void CloseMapUI()
    {
        map_Input = false;
        menuInput = false; 
        mapManager.DisableMap();
        mapStartCursorPos = Vector2.zero;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void OnReload()
    {
        weaponManager.HandleReload();
    }

    void SwitchWeaponByKey(int slotIndex)
    {
        weaponSwitchInput = slotIndex;
        weaponWheelManager.ChooseSlotDirectly(slotIndex);
        weaponManager.SwitchWeapon(weaponSwitchInput);
    }

    void Observe(float delta)
    {
        if (playerManager.canRotate)
        {
            mouseX = mouseInput.x * mouseSensitivity * delta;
            mouseY = mouseInput.y * mouseSensitivity * delta;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -60f, 60f);
        }
  

    }

    public void HandleWeaponWheelInput()
    {
        if (weaponWheel_input)
        {
            menuInput = true;
            Vector3 currentMousePos = Mouse.current.position.ReadValue();
            Vector2 mouseDir = currentMousePos - weaponWheelManager.initialPositition.position;
            float displacement = mouseDir.magnitude;
            float angle=Vector3.SignedAngle(weaponWheelManager.initialPositition.position, mouseDir,Vector3.forward);

            if (displacement >= 20)
            {
                if (angle>60&&angle<120)
                {
                    weaponWheelManager.HoverSlot(0);
                }
                else if (angle<60&&angle>0)
                {
                    weaponWheelManager.HoverSlot(1);
                }
                else if (angle<0&&angle>-60)
                {
                    weaponWheelManager.HoverSlot(2);
                }
                else if (angle<-60&&angle>-120)
                {
                    weaponWheelManager.HoverSlot(3);
                }
                else if (angle<-120&&angle>-180)
                {
                    weaponWheelManager.HoverSlot(4);
                }
                else if (angle>120&&angle<180)
                {
                    weaponWheelManager.HoverSlot(5);
                }
            }
            else
            {
                weaponWheelManager.CancelSelect();
            }
        }
    }

    void OpenWeaponWheel()
    {
        //Adjust time lapse
        weaponWheelManager.OpenWeaponWheelUI();
        playerManager.canRotate = false;
    }

    void CloseWeaponWheel()
    {
        //Adjust time lapse
        weaponWheelManager.CloseWeaponWheelUI();
        weaponWheel_input = false;
        playerManager.canRotate = true;
        menuInput = false;
    }

    void HandleEscapeInput()
    {
        if (escape_Input)
        {
            uIManager.ClosePopUpUI();
        }    

    }
}
