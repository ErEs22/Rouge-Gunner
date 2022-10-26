using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentlyBuff : BasePermanentBuff
{
    /// <summary>
    /// 永久性保持的增益效果
    /// </summary>
    /// <param name="playerStatsManager"></param>
    /// <param name="buffType">数值修改类型</param>
    /// <param name="affectAttribute">修改的属性值</param>
    /// <param name="affectValue">修改值大小(根据bufftype来填写)</param>
    public PermanentlyBuff(PlayerStatsManager playerStatsManager,
    eBuffType buffType,
    eAffectAttribute affectAttribute,
    float affectValue,
    float duration)
    {
        this.playerStatsManager = playerStatsManager;
        this.buffType = buffType;
        this.affectAttribute = affectAttribute;
        this.affectValue = affectValue;
        this.duration = duration;
        this.buffValue = 1;
        this.isActive = true;
        this.affectValue = this.affectValue * this.buffValue;
        this.countDownTimer = 0;
        Debug.Log("Add Buff: Affect " + this.affectAttribute.ToString());
    }

    /// <summary>
    /// 由buffManager进行持续更新buff效果
    /// </summary>
    public override void UpdateBuff()
    {
        switch (buffType)
        {
            case eBuffType.direct:
                BuffValueByDirect();
                break;
            case eBuffType.percent:
                BuffValueByPercent();
                break;
        }
    }
}
