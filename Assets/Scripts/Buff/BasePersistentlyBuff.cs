using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePersistentlyBuff : BaseBuff
{
    protected float duration;

    protected float effectInterval;

    protected float effectTimer = 1;

    #region Direct
    /// <summary>
    /// 根据affectvalue的值持续增加血量
    /// </summary>
    protected override void HealthBuffDirect()
    {
        effectTimer += Time.deltaTime;
        if (effectTimer >= effectInterval)
        {
            effectTimer = 0;
            playerStatsManager.currentHealth += affectValue * buffValue;
            if (playerStatsManager.currentHealth >= playerStatsManager.currentMaxHealth)
            {
                playerStatsManager.currentHealth = playerStatsManager.currentMaxHealth;
            }
        }
    }

    protected override void HealthBuffDirectExit()
    {
        isActive = false;
    }

    /// <summary>
    /// 根据affectvalue的值持续增加护盾
    /// </summary>
    protected override void ShieldBuffDirect()
    {
        effectTimer += Time.deltaTime;
        if (effectTimer >= effectInterval)
        {
            effectTimer = 0;
            playerStatsManager.currentShield += affectValue * buffValue;
            if (playerStatsManager.currentShield >= playerStatsManager.currentMaxShield)
            {
                playerStatsManager.currentShield = playerStatsManager.currentMaxShield;
            }
        }
    }

    protected override void ShieldBuffDirectExit()
    {
        isActive = false;
    }

    /// <summary>
    /// 根据affectvalue的值持续增加攻击力(如不需要可不写)
    /// </summary>
    protected override void ATKBuffDirect()
    {
    }

    protected override void ATKBuffDirectExit()
    {
        isActive = false;
    }

    /// <summary>
    /// 根据affectvalue的值持续增加移动速度(如不需要可不写)
    /// </summary>
    protected override void MoveSpeedBuffDirect()
    {
    }

    protected override void MoveSpeedBuffDirectExit()
    {
        isActive = false;
    }

    /// <summary>
    /// 根据affectvalue的值持续增加闪避CD(如不需要可不写)
    /// </summary>
    protected override void DodgeCDBuffDirect()
    {
    }

    protected override void DodgeCDBuffDirectExit()
    {
        isActive = false;
    }

    /// <summary>
    /// 根据affectvalue的值持续增加抓钩CD(如不需要可不写)
    /// </summary>
    protected override void FalculaCDBuffDirect()
    {
    }

    protected override void FalculaCDBuffDirectExit()
    {
        isActive = false;
    }

    /// <summary>
    /// 根据affectvalue的值持续增加幸运值(如不需要可不写)
    /// </summary>
    protected override void LuckyBuffDirect()
    {
    }

    protected override void LuckyBuffDirectExit()
    {
        isActive = false;
    }

    #endregion
    #region Percent
    /// <summary>
    /// 根据affectvalue的值持续增加血量
    /// </summary>
    protected override void HealthBuffPercent()
    {
        effectTimer += Time.deltaTime;
        if (effectTimer >= effectInterval)
        {
            effectTimer = 0;
            playerStatsManager.currentHealth += affectValue * playerStatsManager.currentMaxHealth * buffValue;
            if (playerStatsManager.currentHealth >= playerStatsManager.currentMaxHealth)
            {
                playerStatsManager.currentHealth = playerStatsManager.currentMaxHealth;
            }
        }
    }

    protected override void HealthBuffPercentExit()
    {
        isActive = false;
    }

    /// <summary>
    /// 根据affectvalue的值持续增加护盾
    /// </summary>
    protected override void ShieldBuffPercent()
    {
        effectTimer += Time.deltaTime;
        if (effectTimer >= effectInterval)
        {
            effectTimer = 0;
            playerStatsManager.currentShield += affectValue * playerStatsManager.currentMaxShield * buffValue;
            if (playerStatsManager.currentShield >= playerStatsManager.currentMaxShield)
            {
                playerStatsManager.currentShield = playerStatsManager.currentMaxShield;
            }
        }
    }

    protected override void ShieldBuffPercentExit()
    {
        isActive = false;
    }

    /// <summary>
    /// 根据affectvalue的值持续增加攻击力(如不需要可不写)
    /// </summary>
    protected override void ATKBuffPercent()
    {
    }

    protected override void ATKBuffPercentExit()
    {
        isActive = false;
    }

    /// <summary>
    /// 根据affectvalue的值持续增加移动速度(如不需要可不写)
    /// </summary>
    protected override void MoveSpeedBuffPercent()
    {
    }

    protected override void MoveSpeedBuffPercentExit()
    {
        isActive = false;
    }

    /// <summary>
    /// 根据affectvalue的值持续增加闪避CD(如不需要可不写)
    /// </summary>
    protected override void DodgeCDBuffPercent()
    {
    }

    protected override void DodgeCDBuffPercentExit()
    {
        isActive = false;
    }

    /// <summary>
    /// 根据affectvalue的值持续增加抓钩CD(如不需要可不写)
    /// </summary>
    protected override void FalculaCDBuffPercent()
    {
    }

    protected override void FalculaCDBuffPercentExit()
    {
        isActive = false;
    }

    /// <summary>
    /// 根据affectvalue的值持续增加幸运值(如不需要可不写)
    /// </summary>
    protected override void LuckyBuffPercent()
    {
    }

    protected override void LuckyBuffPercentExit()
    {
        isActive = false;
    }

    #endregion
}
