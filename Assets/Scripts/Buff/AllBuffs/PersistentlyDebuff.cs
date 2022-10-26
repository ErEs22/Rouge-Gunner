using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentlyDebuff : BasePersistentlyBuff
{
    /// <summary>
    /// 持续性的减益效果
    /// </summary>
    /// <param name="playerStatsManager"></param>
    /// <param name="buffType">数值修改类型</param>
    /// <param name="affectAttribute">修改的属性值</param>
    /// <param name="affectValue">修改值大小(根据bufftype来填写)</param>
    /// <param name="duration">持续时间</param>
    /// <param name="effectInterval">生效间隔</param>
    public PersistentlyDebuff(PlayerStatsManager playerStatsManager,
    eBuffType buffType,
    eAffectAttribute affectAttribute,
    float affectValue,
    float duration,
    float effectInterval)
    {
        this.playerStatsManager = playerStatsManager;
        this.buffType = buffType;
        this.affectAttribute = affectAttribute;
        this.affectValue = affectValue;
        this.duration = duration;
        this.effectInterval = effectInterval;
        this.buffValue = -1;
        this.isActive = true;
        Debug.Log("Add Debuff: Affect " + this.affectAttribute.ToString() + "for" + this.duration + "s");
    }

    /// <summary>
    /// 由buffManager进行持续更新buff效果
    /// </summary>
    public override void UpdateBuff()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            isActive = false;
            return;
        }
        switch (buffType)
        {
            case eBuffType.direct:
                BuffValueByDirect();
                break;
            case eBuffType.percent:
                BuffValueByPercent();
                break;
            default: break;
        }
    }
}
