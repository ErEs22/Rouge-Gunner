using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 所有buff的更新中枢，测试如BuffTest
/// </summary>
public class BuffManager : Singleton<BuffManager>
{
    PlayerStatsManager playerStatsManager;

    protected override void Awake()
    {
        base.Awake();
        playerStatsManager = GetComponent<PlayerStatsManager>();
    }

    Dictionary<int, PermanentlyBuff> permanentlyBuffs = new Dictionary<int, PermanentlyBuff>();//持久生效的buff

    Dictionary<int, PermanentlyDebuff> permanentlyDebuffs = new Dictionary<int, PermanentlyDebuff>();//持久生效的debuff

    Dictionary<int, PersistentlyBuff> persistentlyBuffs = new Dictionary<int, PersistentlyBuff>();//持续间隔生效的buff

    Dictionary<int, PersistentlyDebuff> persistentlyDebuffs = new Dictionary<int, PersistentlyDebuff>();//持续间隔生效的debuff

    private void Update()
    {
        UpdateAllBuffs();//更新所有已添加buff的状态
    }

    /// <summary>
    /// 创建持久性buff
    /// </summary>
    /// <param name="playerStatsManager"></param>
    /// <param name="buffType">数值修改类型</param>
    /// <param name="affectAttribute">修改的属性值</param>
    /// <param name="affectValue">修改值大小(根据bufftype来填写)</param>
    /// <returns></returns>
    public PermanentlyBuff CreatePermanentlyBuff
    (
        PlayerStatsManager playerStatsManager,
        eBuffType buffType,
        eAffectAttribute affectAttribute,
        float affectValue,
        float duration
    )
    {
        PermanentlyBuff permanentlyBuff = new PermanentlyBuff(playerStatsManager, buffType, affectAttribute, affectValue, duration);
        AddPermanentlyBuff(permanentlyBuff);
        ReCaculateAllStats(playerStatsManager);
        return permanentlyBuff;
    }

    /// <summary>
    /// 创建持久性debuff
    /// </summary>
    /// <param name="playerStatsManager"></param>
    /// <param name="buffType">数值修改类型</param>
    /// <param name="affectAttribute">修改的属性值</param>
    /// <param name="affectValue">修改值大小(根据bufftype来填写)</param>
    /// <returns></returns>
    public PermanentlyDebuff CreatePermanentlyDebuff
    (
        PlayerStatsManager playerStatsManager,
        eBuffType buffType,
        eAffectAttribute affectAttribute,
        float affectValue,
        float duration
    )
    {
        PermanentlyDebuff permanentlyDebuff = new PermanentlyDebuff(playerStatsManager, buffType, affectAttribute, affectValue, duration);
        AddPermanentlyDebuff(permanentlyDebuff);
        ReCaculateAllStats(playerStatsManager);
        return permanentlyDebuff;
    }

    /// <summary>
    /// 创建持续间隔性buff
    /// </summary>
    /// <param name="playerStatsManager"></param>
    /// <param name="buffType">数值修改类型</param>
    /// <param name="affectAttribute">修改的属性值</param>
    /// <param name="affectValue">修改值大小(根据bufftype来填写)</param>
    /// <param name="duration">持续时间</param>
    /// <param name="effectInterval">生效间隔</param>
    /// <returns></returns>
    public PersistentlyBuff CreatePersistentlyBuff
    (
        PlayerStatsManager playerStatsManager,
        eBuffType buffType,
        eAffectAttribute affectAttribute,
        float affectValue,
        float duration,
        float effectInterval
    )
    {
        PersistentlyBuff persistentlyBuff = new PersistentlyBuff(playerStatsManager, buffType, affectAttribute, affectValue, duration, effectInterval);
        AddPersistentlyBuff(persistentlyBuff);
        return persistentlyBuff;
    }

    /// <summary>
    /// 创建持续间隔性debuff
    /// </summary>
    /// <param name="playerStatsManager"></param>
    /// <param name="buffType">数值修改类型</param>
    /// <param name="affectAttribute">修改的属性值</param>
    /// <param name="affectValue">修改值大小(根据bufftype来填写)</param>
    /// <param name="duration">持续时间</param>
    /// <param name="effectInterval">生效间隔</param>
    /// <returns></returns>
    public PersistentlyDebuff CreatePersistentlyDebuff
    (
        PlayerStatsManager playerStatsManager,
        eBuffType buffType,
        eAffectAttribute affectAttribute,
        float affectValue,
        float duration,
        float effectInterval
    )
    {
        PersistentlyDebuff persistentlyDebuff = new PersistentlyDebuff(playerStatsManager, buffType, affectAttribute, affectValue, duration, effectInterval);
        AddPersistentlyDebuff(persistentlyDebuff);
        return persistentlyDebuff;
    }

    /// <summary>
    /// 添加持久性buff
    /// </summary>
    /// <param name="permanentlyBuff"></param>
    void AddPermanentlyBuff(PermanentlyBuff permanentlyBuff)
    {
        permanentlyBuff.index = permanentlyBuffs.Count;
        permanentlyBuffs.Add(permanentlyBuff.index, permanentlyBuff);
    }

    /// <summary>
    /// 添加持久性debuff
    /// </summary>
    /// <param name="permanentlyDebuff"></param>
    void AddPermanentlyDebuff(PermanentlyDebuff permanentlyDebuff)
    {
        permanentlyDebuff.index = permanentlyDebuffs.Count;
        permanentlyDebuffs.Add(permanentlyDebuff.index, permanentlyDebuff);
    }

    /// <summary>
    /// 添加持续间隔buff
    /// </summary>
    /// <param name="persistentlyBuff"></param>
    void AddPersistentlyBuff(PersistentlyBuff persistentlyBuff)
    {
        persistentlyBuff.index = persistentlyBuffs.Count;
        persistentlyBuffs.Add(persistentlyBuff.index, persistentlyBuff);
    }

    /// <summary>
    /// 添加持续间隔debuff
    /// </summary>
    /// <param name="persistentlyDebuff"></param>
    void AddPersistentlyDebuff(PersistentlyDebuff persistentlyDebuff)
    {
        persistentlyDebuff.index = persistentlyDebuffs.Count;
        persistentlyDebuffs.Add(persistentlyDebuff.index, persistentlyDebuff);
    }

    #region 移除buff
    /// <summary>
    /// 移除buff
    /// </summary>
    /// <param name="index">索引</param>
    public void RemovePermanentlyBuff(int index)
    {
        PermanentlyBuff permanentlyBuff;
        permanentlyBuffs.TryGetValue(index, out permanentlyBuff);
        permanentlyBuff.ExitBuff();
        permanentlyBuffs.Remove(index);
    }

    public void RemovePermanentlyDebuff(int index)
    {
        PermanentlyDebuff permanentlyDebuff;
        permanentlyDebuffs.TryGetValue(index, out permanentlyDebuff);
        permanentlyDebuff.ExitBuff();
        permanentlyDebuffs.Remove(index);
    }

    public void RemovePersistentlyBuff(int index)
    {
        PersistentlyBuff persistentlyBuff;
        persistentlyBuffs.TryGetValue(index, out persistentlyBuff);
        persistentlyBuff.ExitBuff();
        persistentlyBuffs.Remove(index);
    }

    public void RemovePersistentlyDebuff(int index)
    {
        PersistentlyDebuff persistentlyDebuff;
        persistentlyDebuffs.TryGetValue(index, out persistentlyDebuff);
        persistentlyDebuff.ExitBuff();
        persistentlyDebuffs.Remove(index);
    }
    #endregion

    /// <summary>
    /// 持续更新所有buff队列中的buffs
    /// </summary>
    void UpdateAllBuffs()
    {
        UpdatePermanentlyBuffs();
        UpdatePermanentlyDebuffs();
        UpdatePersistentlyBuffs();
        UpdatePersistentlyDebuffs();
    }

    /// <summary>
    /// 更新持久性buff
    /// </summary>
    void UpdatePermanentlyBuffs()
    {
        List<int> keys = new List<int>();
        foreach (var buff in permanentlyBuffs)
        {
            if (buff.Value.isActive == false)
            {
                keys.Add(buff.Key);
            }
            else
            {
                buff.Value.UpdateBuff();
            }
        }

        foreach (var key in keys)
        {
            permanentlyBuffs.Remove(key);
        }
        if (keys.Count > 0)
        {
            ReCaculateAllStats(playerStatsManager);
        }
        keys.Clear();
    }

    /// <summary>
    /// 更新持久性debuff
    /// </summary>
    void UpdatePermanentlyDebuffs()
    {
        List<int> keys = new List<int>();
        foreach (var buff in permanentlyDebuffs)
        {
            if (buff.Value.isActive == false)
            {
                keys.Add(buff.Key);
            }
            else
            {
                buff.Value.UpdateBuff();
            }
        }

        foreach (var key in keys)
        {
            permanentlyBuffs.Remove(key);
        }
        if (keys.Count > 0)
        {
            ReCaculateAllStats(playerStatsManager);
        }
        keys.Clear();
    }

    /// <summary>
    /// 更新持续间隔性buff
    /// </summary>
    void UpdatePersistentlyBuffs()
    {
        List<int> keys = new List<int>();
        foreach (var buff in persistentlyBuffs)
        {
            if (buff.Value.isActive == false)
            {
                keys.Add(buff.Key);
            }
            else
            {
                buff.Value.UpdateBuff();
            }
        }

        foreach (var key in keys)
        {
            permanentlyBuffs.Remove(key);
        }
        if (keys.Count > 0)
        {
            ReCaculateAllStats(playerStatsManager);
        }
        keys.Clear();
    }

    /// <summary>
    /// 更新持续间隔性debuff
    /// </summary>
    void UpdatePersistentlyDebuffs()
    {
        List<int> keys = new List<int>();
        foreach (var buff in persistentlyDebuffs)
        {
            if (buff.Value.isActive == false)
            {
                keys.Add(buff.Key);
            }
            else
            {
                buff.Value.UpdateBuff();
            }
        }

        foreach (var key in keys)
        {
            permanentlyBuffs.Remove(key);
        }
        if (keys.Count > 0)
        {
            ReCaculateAllStats(playerStatsManager);
        }
        keys.Clear();
    }

    /// <summary>
    /// 根据影响的属性值修改当前属性
    /// </summary>
    /// <param name="affectAttribute"></param>
    (float, int) ReCaculateStatChanges(eAffectAttribute affectAttribute)
    {
        float percent = 0f;
        int amount = 0;

        //计算增益
        foreach (var buff in permanentlyBuffs)
        {
            if (buff.Value.affectAttribute == affectAttribute)
            {
                if (buff.Value.buffType == eBuffType.direct)
                {
                    amount += (int)buff.Value.affectValue;
                }
                else if (buff.Value.buffType == eBuffType.percent)
                {
                    percent += buff.Value.affectValue;
                }
            }
        }

        //计算减益
        foreach (var debuff in permanentlyDebuffs)
        {
            if (debuff.Value.affectAttribute == affectAttribute)
            {
                if (debuff.Value.buffType == eBuffType.direct)
                {
                    amount += (int)debuff.Value.affectValue;
                }
                else if (debuff.Value.buffType == eBuffType.percent)
                {
                    percent += debuff.Value.affectValue;
                }
            }
        }
        return (percent, amount);
    }

    /// <summary>
    /// 重计算血量
    /// </summary>
    /// <param name="playerStatsManager"></param>
    void ReCaculateHealthAmount(PlayerStatsManager playerStatsManager)
    {
        float currentPercent = 0f;
        float percent = 0f;
        int amount = 0;
        (percent, amount) = ReCaculateStatChanges(eAffectAttribute.health);
        currentPercent = playerStatsManager.currentHealth / playerStatsManager.currentMaxHealth;
        playerStatsManager.currentMaxHealth = playerStatsManager.baseMaxHealth + amount;
        playerStatsManager.currentMaxHealth += playerStatsManager.currentMaxHealth * percent;
        playerStatsManager.currentHealth = playerStatsManager.currentMaxHealth * currentPercent;
    }

    /// <summary>
    /// 重计算护盾
    /// </summary>
    /// <param name="playerStatsManager"></param>
    void ReCaculateShieldAmount(PlayerStatsManager playerStatsManager)
    {
        float currentPercent = 0f;
        float percent = 0f;
        int amount = 0;
        (percent, amount) = ReCaculateStatChanges(eAffectAttribute.shield);
        currentPercent = playerStatsManager.currentShield / playerStatsManager.currentMaxShield;
        playerStatsManager.currentMaxShield = playerStatsManager.baseMaxShield + amount;
        playerStatsManager.currentMaxShield += playerStatsManager.currentMaxShield * percent;
        playerStatsManager.currentShield = playerStatsManager.currentMaxShield * currentPercent;
    }

    /// <summary>
    /// 重计算攻击力
    /// </summary>
    /// <param name="playerStatsManager"></param>
    void ReCaculateATKAmount(PlayerStatsManager playerStatsManager)
    {
        float percent = 0f;
        int amount = 0;
        (percent, amount) = ReCaculateStatChanges(eAffectAttribute.atk);
        //TODO 重计算攻击力
    }

    /// <summary>
    /// 重计算移动速度
    /// </summary>
    /// <param name="playerStatsManager"></param>
    void ReCaculateMoveSpeedAmount(PlayerStatsManager playerStatsManager)
    {
        float percent = 0f;
        int amount = 0;
        (percent, amount) = ReCaculateStatChanges(eAffectAttribute.moveSpeed);
        //TODO 重计算移动速度
    }

    /// <summary>
    /// 重计算闪避CD
    /// </summary>
    /// <param name="playerStatsManager"></param>
    void ReCaculateDodgeCDAmount(PlayerStatsManager playerStatsManager)
    {
        float percent = 0f;
        int amount = 0;
        (percent, amount) = ReCaculateStatChanges(eAffectAttribute.dodgeCD);
        //TODO 重计算闪避CD
    }

    /// <summary>
    /// 重计算抓钩CD
    /// </summary>
    /// <param name="playerStatsManager"></param>
    void ReCaculateFalculaCDAmount(PlayerStatsManager playerStatsManager)
    {
        float percent = 0f;
        int amount = 0;
        (percent, amount) = ReCaculateStatChanges(eAffectAttribute.falculaCD);
        //TODO 重计算抓钩CD
    }

    /// <summary>
    /// 重计算幸运值
    /// </summary>
    /// <param name="playerStatsManager"></param>
    void ReCaculateLuckyAmount(PlayerStatsManager playerStatsManager)
    {
        float percent = 0f;
        int amount = 0;
        (percent, amount) = ReCaculateStatChanges(eAffectAttribute.lucky);
        //TODO 重计算幸运值
    }

    /// <summary>
    /// 重计算所有属性
    /// </summary>
    /// <param name="playerStatsManager"></param>
    void ReCaculateAllStats(PlayerStatsManager playerStatsManager)
    {
        ReCaculateHealthAmount(playerStatsManager);
        ReCaculateShieldAmount(playerStatsManager);
        ReCaculateMoveSpeedAmount(playerStatsManager);
        ReCaculateATKAmount(playerStatsManager);
        ReCaculateDodgeCDAmount(playerStatsManager);
        ReCaculateFalculaCDAmount(playerStatsManager);
        ReCaculateLuckyAmount(playerStatsManager);
    }
}
