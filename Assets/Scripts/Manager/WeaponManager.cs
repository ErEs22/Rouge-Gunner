using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    WeaponWheelManager weaponWheelManager;
    InputManager inputManager;
    PlayerAnimatorManager playerAnimatorManager;
    CurrentWeaponInfo currentWeaponInfo;
    public UIManager uiManager;
    public Camera FpsCam;
    public WeaponSO currentWeaponSO;
    public Weapon currentWeapon;
    public WeaponSO[] weaponList;

    public Transform weaponParent;
    private int availableSlot = 0;


    bool isReloading;
    [SerializeField]
    bool readyToShoot;
    bool allowInvoke=true;

    [Header("射击历史")]
    int bulletsShot;

    [Header("准星设置")]
    [SerializeField] Transform[] viewFrustumPoints;
    Vector3[] viewFrustumVertexs;
    Vector3[] rectCorners = new Vector3[4];
    [SerializeField] RectTransform crossHair;
    [SerializeField] MeshCollider viewFrustum;
    public float maxViewDistance;
    Mesh viewFrustumMesh;


    public LayerMask checkLayer;


    private Dictionary<WeaponSO, Weapon> dic_Weapon = new Dictionary<WeaponSO, Weapon>();
    private Dictionary<Weapon,int> dic_WeaponWithSlot = new Dictionary<Weapon,int>();

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        weaponWheelManager = FindObjectOfType<WeaponWheelManager>();
        inputManager = GetComponentInParent<InputManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        currentWeaponInfo = FindObjectOfType<CurrentWeaponInfo>();        
        readyToShoot = true;
    }

    private void Start()
    {
        GetNewWeapon(currentWeaponSO);
        InitCorners();
    }
    public void HandleShoot()
    {
        if (currentWeaponSO != null&&!PlayerManager.Instance.isInteracting)
        {
            Vector3 targetPoint=HandleAim();//独立模块
            if (currentWeapon.auto)
            {

                PlayerManager.Instance.isShooting = inputManager.shootHold_input;
            }
            else
            {
                PlayerManager.Instance.isShooting = inputManager.shootTap_input;
            }

            //在弹匣子弹打光时仍继续射击则自动换弹
            //为火控系统再增添一个状态值
            if (readyToShoot && PlayerManager.Instance.isShooting && !isReloading && currentWeapon.currentAmmoInMag <= 0) Reload();

            if (readyToShoot && PlayerManager.Instance.isShooting && !isReloading && currentWeapon.currentAmmoInMag > 0)
            {
                readyToShoot = false;
                //Set bullets shot to 0
                bulletsShot = 0;
                if (currentWeapon.isFireControlSys)
                {
                    var targets=FireControlDetecter.Instance.GetTargets(currentWeaponSO.bulletPerTap);
                    Shoot(targets);
                }
                else
                {
                    Shoot(targetPoint);

                }
            }
        }
    }

    public Vector3 HandleAim()
    {
        //道具check
        //一个固定的瞄准类道具check
        Vector3 targetPoint=Vector3.zero;
        if (currentWeapon.isFireControlSys)
        {
            CreateViewFrustum(FpsCam);
        }
        else
        {
            Ray ray = FpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            //检测是否射线是否触碰到物体

            if (Physics.Raycast(ray, out hit))
                targetPoint = hit.point;
            else
                targetPoint = ray.GetPoint(75); //Just a point far away from the player
        }

        if (currentWeaponSO.interactionType==AbilityInteractionType.Click&& inputManager.abilityTap_Input&&currentWeapon.currentCD<=0)
        {
            
            PlayerManager.Instance.usingAbility = true;
        }
        else if (currentWeaponSO.interactionType == AbilityInteractionType.Hold&&currentWeapon.currentCD <= 0)
        {
            if (inputManager.abilityTap_Input)
            {
                currentWeaponSO.AbilityIn(this, currentWeapon, null);
            }
            if (inputManager.abilityHold_Input)
            {
                PlayerManager.Instance.usingAbility = true;
            }
        }

        if (PlayerManager.Instance.usingAbility)
        {
            HandleAbilityPerformed();

            if (currentWeaponSO.interactionType == AbilityInteractionType.Click && !inputManager.abilityTap_Input)
            {
                HandleAbilityCanceled();
            }
            else if (currentWeaponSO.interactionType == AbilityInteractionType.Hold && !inputManager.abilityHold_Input)
            {
                HandleAbilityCanceled();
            }
        }
        return targetPoint;
  
     
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 halfExtend = new Vector3(crossHair.rect.width / 2, crossHair.rect.height / 2, 5);
        Vector3 center = FpsCam.transform.position;
        center.z += 5;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(center, halfExtend);
    }
    public void HandleReload()
    {
        if (currentWeapon.currentAmmoInMag<currentWeapon.currentmagSize&&!isReloading)
        {
            Reload();
        }
    }

    private void Shoot(Vector3 targetPoint)
    {
        Vector3 directionWithoutSpread = targetPoint - currentWeapon.muzzle.position;

        //projectile move direction
        Vector3 directionWithSpread = HandleSpread(directionWithoutSpread);
        var bullet=PoolManager.Release(currentWeaponSO.projectilePrefab, currentWeapon.muzzle.position, Quaternion.identity);
        bullet.transform.forward = directionWithSpread.normalized;
        playerAnimatorManager.PlayTargetAnimation("Shooting", false);
        //未来添加抛物线
        //currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);
        //枪口火光

        currentWeapon.currentAmmoInMag-= currentWeapon.ammoCostPerTap;
        currentWeapon.currentAmmoCarried-= currentWeapon.ammoCostPerTap;
        UpdateAmmoInfo(dic_WeaponWithSlot[currentWeapon], currentWeapon);

        bulletsShot++;
        if (allowInvoke)
        {
            //Calculate Speed
            Invoke("ResetShot", currentWeapon.interval);
            allowInvoke = false;
            //添加后坐力
        }
        if (bulletsShot<currentWeaponSO.bulletPerTap&&currentWeapon.currentAmmoInMag > 0)
            Invoke("Shoot", currentWeaponSO.timeBetweenShots);

    }

    private void Shoot(List<TargetInfo_FCS> targets)
    {
        Vector3 directionWithoutSpread = targets[bulletsShot].enemy.transform.position - currentWeapon.muzzle.position;
        //projectile move direction
        Vector3 directionWithSpread = HandleSpread(directionWithoutSpread);
        var bullet = PoolManager.Release(currentWeaponSO.projectilePrefab, currentWeapon.muzzle.position, Quaternion.identity);
        bullet.transform.forward = directionWithSpread.normalized;
        playerAnimatorManager.PlayTargetAnimation("Shooting", false);
        //未来添加抛物线
        //currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);
        //枪口火光

        currentWeapon.currentAmmoInMag -= currentWeapon.ammoCostPerTap;
        currentWeapon.currentAmmoCarried -= currentWeapon.ammoCostPerTap;
        UpdateAmmoInfo(dic_WeaponWithSlot[currentWeapon], currentWeapon);

        bulletsShot++;
        if (allowInvoke)
        {
            //Calculate Speed
            Invoke("ResetShot", currentWeapon.interval);
            allowInvoke = false;
            //添加后坐力
        }
        if (bulletsShot < targets.Count && currentWeapon.currentAmmoInMag > 0)
            Invoke("Shoot", currentWeaponSO.timeBetweenShots);
    }

    #region 火控系统
    public void InitCorners()
    {
        rectCorners[0] = new Vector3(crossHair.transform.position.x-crossHair.rect.width/2, crossHair.transform.position.x + crossHair.rect.height / 2);
        rectCorners[1] = new Vector3(crossHair.transform.position.x + crossHair.rect.width / 2, crossHair.transform.position.x + crossHair.rect.height / 2);
        rectCorners[2] = new Vector3(crossHair.transform.position.x + crossHair.rect.width / 2, crossHair.transform.position.x - crossHair.rect.height / 2); 
        rectCorners[3] = new Vector3(crossHair.transform.position.x - crossHair.rect.width / 2, crossHair.transform.position.x - crossHair.rect.height / 2); 
    }

    public void CreateViewFrustum(Camera cam)
    {
        viewFrustumVertexs = new Vector3[8];
        RaycastHit hit;

        for (int i = 0; i < 4; i++)
        {
            Ray ray = cam.ScreenPointToRay(rectCorners[i]);
            Physics.Raycast(ray, out hit, maxViewDistance, checkLayer);
            viewFrustumPoints[i].position = ray.origin;
            viewFrustumPoints[i + 4].position = ray.GetPoint(75);
            viewFrustumVertexs[i] = viewFrustumPoints[i].localPosition;
            viewFrustumVertexs[i + 4] = viewFrustumPoints[i + 4].localPosition;
        }
        viewFrustumMesh = new Mesh();
        viewFrustumMesh.vertices = viewFrustumVertexs;
        viewFrustumMesh.triangles = new int[] { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 };

        viewFrustum.sharedMesh = viewFrustumMesh;
        viewFrustum.enabled = true;
    }

 

    #endregion

    private Vector3 HandleSpread(Vector3 directionWithoutSpread)
    {
        float currentSpread = currentWeaponSO.spread;//+角色道具属性
        float x = Random.Range(-currentSpread, currentSpread);
        float y = Random.Range(-currentSpread, currentSpread);
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);
        return directionWithSpread;
    }

    public void SwitchWeapon(int index)
    {

        if (weaponList[index] != null)
        {
            if (dic_Weapon.TryGetValue(weaponList[index], out Weapon weapon))
            {
                currentWeaponSO = weaponList[index];
                currentWeapon = weapon;
                LoadWeaponModel(weapon);
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError("This Weapon didn't Add to Dictionary index: " + index);
#endif
            }

        }
    }


    /// <summary>
    /// 获得新武器时，更新可用槽位同时加载武器模型
    /// </summary>
    /// <param name="newWeapon"></param>
    public void GetNewWeapon(WeaponSO newWeapon)
    {
        if (availableSlot > -1 && availableSlot <= weaponList.Length)
        {
            weaponList[availableSlot] = newWeapon;
            var weapon=InstantiateWeaponModel(newWeapon);
            InitializeWeaponStatus(weapon, newWeapon);
            weaponWheelManager.UpdateWheelSlot(weapon, newWeapon, availableSlot);
            availableSlot = FindAvailableIndex();
        }
    }

    /// <summary>
    /// 初始化武器模型
    /// </summary>
    /// <param name="weapon"></param>
    private Weapon InstantiateWeaponModel(WeaponSO weapon)
    {
        GameObject weaponModel = weapon.LoadWeaponModel(weaponParent);
        var newWeapon = weaponModel.GetComponent<Weapon>();
        if (currentWeapon==null)
        {
            currentWeapon = newWeapon;
        }
        weaponModel.SetActive(false);
        if (!dic_Weapon.ContainsKey(weapon))
        {
            dic_Weapon.Add(weapon, newWeapon);
            dic_WeaponWithSlot.Add(newWeapon,availableSlot);
        }
        return newWeapon;
    }

    /// <summary>
    /// 加载当前武器模型
    /// </summary>
    private void LoadWeaponModel(Weapon weapon)
    {
        if (currentWeapon!=null)
        {
            RemoveCurrentWeaponModel();
        }
        weapon.gameObject.SetActive(true);
    }

    /// <summary>
    /// 移除当前武器模型
    /// </summary>
    private void RemoveCurrentWeaponModel()
    {
        dic_Weapon[currentWeaponSO].gameObject.SetActive(false);
    }

    /// <summary>
    /// 寻找可用位置
    /// </summary>
    /// <returns></returns>
    private int FindAvailableIndex()
    {
        int result = -1;
        for (int i = 0; i < weaponList.Length; i++)
        {
            if (weaponList[i] == null)
            {
                result = i;
                return result;
            }
        }
        return result;
    }

    /// <summary>
    /// 当切换武器时清除该武器的射击历史
    /// 例如使用该武器射击了多少次
    /// </summary>
    private void ResetShootHistory()
    {

    }

    private void ResetShot()
    {

        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        isReloading = true;
        playerAnimatorManager.PlayTargetAnimation("Reloading", false);
        //Invoke("ReloadFinished", currentWeaponSO.reloadTime);//未来调整为动画
    }

    /// <summary>
    /// 动画事件
    /// </summary>
    private void ReloadFinished()
    {
        //填装子弹
        currentWeapon.currentAmmoCarried -= currentWeapon.currentmagSize;
        if (currentWeapon.currentAmmoCarried>= currentWeapon.currentmagSize)
        {
            currentWeapon.currentAmmoInMag = currentWeapon.currentmagSize;
        }
        else
        {
            currentWeapon.currentAmmoInMag = Mathf.Abs(currentWeapon.currentAmmoCarried);
        }
        currentWeaponInfo.UpdateAmmoInfo(currentWeapon.currentAmmoInMag, currentWeapon.currentmagSize);
        isReloading = false;
    }

    /// <summary>
    /// 初始化武器数据
    /// </summary>
    private void InitializeWeaponStatus(Weapon weapon,WeaponSO weaponSO)
    {
        weapon.currentmagSize = weaponSO.baseMagSize;
        weapon.currentAmmoInMag = weapon.currentmagSize;
        weapon.currentMaxAmmoCarried = weaponSO.baseAmmoAmount;
        weapon.currentAmmoCarried = weapon.currentMaxAmmoCarried;
        weapon.ammoCostPerTap = weaponSO.ammoCostPerTap;
        weapon.interval = weaponSO.interval;
        weapon.auto = weaponSO.auto;
        weapon.currentCD = 0;
        weapon.maxCD = weaponSO.abilityCD;
    }

    public WeaponSO ExposeWeaponInfoToUI(int index)
    {
        if (weaponList[index]!=null)
        {
            return weaponList[index];
        }
        return null;
    }

    public void UpdateAmmoInfo(int index,Weapon weapon)
    {
        weaponWheelManager.UpdateSlotInfo(index, weapon);
        currentWeaponInfo.UpdateAmmoInfo(weapon.currentAmmoInMag, weapon.currentmagSize);
    }

    public void HandleAbilityPerformed()
    {
        currentWeaponSO.AbilityPerform(this,currentWeapon,null);
    }

    public void HandleAbilityCanceled()
    {
        PlayerManager.Instance.usingAbility = false;
        currentWeaponSO.AbilityOut(this, currentWeapon, null);
    }
}
