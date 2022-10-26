using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSO : ScriptableObject
{
    public string weaponName;
    public Sprite weaponIcon;

    public GameObject prefab;
    public GameObject projectilePrefab;
    public Sprite scope;
    public AmmoType ammoType;
    public LayerMask targetLayer;

    [Header("武器属性&&规则")]
    public int damage;
    public float interval;//射击间隔
    public float timeBetweenShots;//每发弹丸的间隔 用于处理霰弹枪
    public float projectileSpeed;
    public float criticalMultiplier;
    public int baseMagSize;
    public int baseAmmoAmount;//基础弹药总量
    public int bulletPerTap;//当武器为火控系统武器时该属性等于目标数
    public int ammoCostPerTap;//每一枪所消耗的弹量
    public float spread;//散布（值大约在0~1.5之间）
    public bool auto;
    public bool isfireControlSys;
    

    [Header("武器能力")]
    public string abilityName;
    public Sprite abilityIcon;
    public float abilityCD;
    public bool needAbilityCheck;
    public bool targetRequired;
    public AbilityInteractionType interactionType;

    [TextArea]
    public string abilityDescription;

    public GameObject LoadWeaponModel(Transform weaponParent)
    {
        return Instantiate(prefab, weaponParent);
    }


    /// <summary>
    /// 能力启动时预先处理
    /// </summary>
    public virtual void AbilityIn(WeaponManager weaponManager,Weapon usingWeapon,PlayerStatsManager playerStatsManager)
    {

    }

    /// <summary>
    /// 启用能力
    /// </summary>
    /// <param name="weaponManager"></param>
    /// <param name="enemyManager"></param>
    /// <param name="usingWeapon"></param>
    /// <param name="playerStatsManager"></param>
    public virtual void AbilityPerform(WeaponManager weaponManager,Weapon usingWeapon,PlayerStatsManager playerStatsManager)
    {

    }

    /// <summary>
    /// 关闭能力
    /// </summary>
    /// <param name="weaponManager"></param>
    /// <param name="enemyManager"></param>
    /// <param name="usingWeapon"></param>
    /// <param name="playerStatsManager"></param>
    public virtual void AbilityOut(WeaponManager weaponManager, Weapon usingWeapon, PlayerStatsManager playerStatsManager)
    {

    }

    public virtual bool AbilityCheck()
    {
        return true;
    }

    public virtual void FireControlSystem()
    {
       
    }
}

