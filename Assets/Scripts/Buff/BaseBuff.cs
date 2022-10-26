using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuff
{
    public bool isActive;

    public int index;

    protected int buffValue;//值为1则为增益buff，为-1则为减益debuff

    public float affectValue;//根据bufftype填写，直接增减则填写增减量，百分比则填写0~1之间的值(不需要填符号，buffvalue会影响是增益还是减益)

    public eAffectAttribute affectAttribute;

    public eBuffType buffType;

    protected PlayerStatsManager playerStatsManager;

    public virtual void UpdateBuff() { }

    public virtual void ExitBuff()
    {
        switch (buffType)
        {
            case eBuffType.direct:
                BuffValueByDirectExit();
                break;
            case eBuffType.percent:
                BuffValueByPercentExit();
                break;
            default: break;
        }
    }

    /// <summary>
    /// 根据影响的属性修改属性值(直接增减)
    /// </summary>
    protected void BuffValueByDirect()
    {
        switch (affectAttribute)
        {
            case eAffectAttribute.health:
                HealthBuffDirect();
                break;
            case eAffectAttribute.shield:
                ShieldBuffDirect();
                break;
            case eAffectAttribute.atk:
                ATKBuffDirect();
                break;
            case eAffectAttribute.moveSpeed:
                MoveSpeedBuffDirect();
                break;
            case eAffectAttribute.dodgeCD:
                DodgeCDBuffDirect();
                break;
            case eAffectAttribute.falculaCD:
                FalculaCDBuffDirect();
                break;
            case eAffectAttribute.lucky:
                LuckyBuffDirect();
                break;
            default: break;
        }
    }

    protected virtual void LuckyBuffDirect()
    {

    }

    protected virtual void FalculaCDBuffDirect()
    {

    }

    protected virtual void DodgeCDBuffDirect()
    {

    }

    protected virtual void MoveSpeedBuffDirect()
    {

    }

    protected virtual void ATKBuffDirect()
    {

    }

    protected virtual void ShieldBuffDirect()
    {

    }

    protected virtual void HealthBuffDirect()
    {

    }

    /// <summary>
    /// 根据影响的属性修改属性值(百分比)
    /// </summary>
    protected void BuffValueByPercent()
    {
        switch (affectAttribute)
        {
            case eAffectAttribute.health:
                HealthBuffPercent();
                break;
            case eAffectAttribute.shield:
                ShieldBuffPercent();
                break;
            case eAffectAttribute.atk:
                ATKBuffPercent();
                break;
            case eAffectAttribute.moveSpeed:
                MoveSpeedBuffPercent();
                break;
            case eAffectAttribute.dodgeCD:
                DodgeCDBuffPercent();
                break;
            case eAffectAttribute.falculaCD:
                FalculaCDBuffPercent();
                break;
            case eAffectAttribute.lucky:
                LuckyBuffPercent();
                break;
            default: break;
        }
    }

    protected virtual void LuckyBuffPercent()
    {

    }

    protected virtual void FalculaCDBuffPercent()
    {

    }

    protected virtual void DodgeCDBuffPercent()
    {

    }

    protected virtual void MoveSpeedBuffPercent()
    {

    }

    protected virtual void ATKBuffPercent()
    {

    }

    protected virtual void ShieldBuffPercent()
    {

    }

    protected virtual void HealthBuffPercent()
    {

    }

    /// <summary>
    /// buff效果移除，将数值修正(直接增减)
    /// </summary>
    protected void BuffValueByDirectExit()
    {
        switch (affectAttribute)
        {
            case eAffectAttribute.health:
                HealthBuffDirectExit();
                break;
            case eAffectAttribute.shield:
                ShieldBuffDirectExit();
                break;
            case eAffectAttribute.atk:
                ATKBuffDirectExit();
                break;
            case eAffectAttribute.moveSpeed:
                MoveSpeedBuffDirectExit();
                break;
            case eAffectAttribute.dodgeCD:
                DodgeCDBuffDirectExit();
                break;
            case eAffectAttribute.falculaCD:
                FalculaCDBuffDirectExit();
                break;
            case eAffectAttribute.lucky:
                LuckyBuffDirectExit();
                break;
            default: break;
        }
    }

    protected virtual void LuckyBuffDirectExit()
    {

    }

    protected virtual void FalculaCDBuffDirectExit()
    {

    }

    protected virtual void DodgeCDBuffDirectExit()
    {

    }

    protected virtual void MoveSpeedBuffDirectExit()
    {

    }

    protected virtual void ATKBuffDirectExit()
    {

    }

    protected virtual void ShieldBuffDirectExit()
    {

    }

    protected virtual void HealthBuffDirectExit()
    {

    }

    /// <summary>
    /// buff效果移除，将数值修正(百分比)
    /// </summary>
    protected void BuffValueByPercentExit()
    {
        switch (affectAttribute)
        {
            case eAffectAttribute.health:
                HealthBuffPercentExit();
                break;
            case eAffectAttribute.shield:
                ShieldBuffPercentExit();
                break;
            case eAffectAttribute.atk:
                ATKBuffPercentExit();
                break;
            case eAffectAttribute.moveSpeed:
                MoveSpeedBuffPercentExit();
                break;
            case eAffectAttribute.dodgeCD:
                DodgeCDBuffPercentExit();
                break;
            case eAffectAttribute.falculaCD:
                FalculaCDBuffPercentExit();
                break;
            case eAffectAttribute.lucky:
                LuckyBuffPercentExit();
                break;
            default: break;
        }
    }

    protected virtual void LuckyBuffPercentExit()
    {

    }

    protected virtual void FalculaCDBuffPercentExit()
    {

    }

    protected virtual void DodgeCDBuffPercentExit()
    {

    }

    protected virtual void MoveSpeedBuffPercentExit()
    {

    }

    protected virtual void ATKBuffPercentExit()
    {

    }

    protected virtual void ShieldBuffPercentExit()
    {

    }

    protected virtual void HealthBuffPercentExit()
    {

    }
}
