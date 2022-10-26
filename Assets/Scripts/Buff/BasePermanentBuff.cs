using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePermanentBuff : BaseBuff
{
    bool disable = false;//第一次增加后设为true，防止在update中持续增加

    protected float duration;

    protected float countDownTimer;

    #region Direct
    /// <summary>
    /// 根据affectvalue的值增加血量
    /// </summary>
    protected override void HealthBuffDirect()
    {
        if (duration == 0)
        {
            return;
        }
        else
        {
            if (countDownTimer >= duration)
            {
                isActive = false;
            }
            else
            {
                countDownTimer += Time.deltaTime;
            }
        }
    }

    protected override void HealthBuffDirectExit()
    {
        isActive = false;
    }

    /// <summary>
    /// 根据affectvalue的值增加护盾
    /// </summary>
    protected override void ShieldBuffDirect()
    {
        if (duration == 0)
        {
            return;
        }
        else
        {
            if (countDownTimer >= duration)
            {
                isActive = false;
            }
            else
            {
                countDownTimer += Time.deltaTime;
            }
        }
    }

    protected override void ShieldBuffDirectExit()
    {
        isActive = false;
        playerStatsManager.currentMaxShield -= affectValue * buffValue;
    }

    /// <summary>
    /// 根据affectvalue的值增加攻击力(如不需要可不写)
    /// </summary>
    protected override void ATKBuffDirect()
    {
        if (duration == 0)
        {
            return;
        }
        else
        {
            if (countDownTimer >= duration)
            {
                isActive = false;
            }
            else
            {
                countDownTimer += Time.deltaTime;
            }
        }
    }

    protected override void ATKBuffDirectExit()
    {
        isActive = false;
    }

    /// <summary>
    /// 根据affectvalue的值增加移动速度(如不需要可不写)
    /// </summary>
    protected override void MoveSpeedBuffDirect()
    {
        if (duration == 0)
        {
            return;
        }
        else
        {
            if (countDownTimer >= duration)
            {
                isActive = false;
            }
            else
            {
                countDownTimer += Time.deltaTime;
            }
        }
    }

    protected override void MoveSpeedBuffDirectExit()
    {
        isActive = false;
    }

    /// <summary>
    /// 根据affectvalue的值增加闪避CD(如不需要可不写)
    /// </summary>
    protected override void DodgeCDBuffDirect()
    {
        if (duration == 0)
        {
            return;
        }
        else
        {
            if (countDownTimer >= duration)
            {
                isActive = false;
            }
            else
            {
                countDownTimer += Time.deltaTime;
            }
        }
    }

    protected override void DodgeCDBuffDirectExit()
    {
        isActive = false;
    }

    /// <summary>
    /// 根据affectvalue的值增加抓钩CD(如不需要可不写)
    /// </summary>
    protected override void FalculaCDBuffDirect()
    {
        if (duration == 0)
        {
            return;
        }
        else
        {
            if (countDownTimer >= duration)
            {
                isActive = false;
            }
            else
            {
                countDownTimer += Time.deltaTime;
            }
        }
    }

    protected override void FalculaCDBuffDirectExit()
    {
        isActive = false;
    }

    /// <summary>
    /// 根据affectvalue的值增加幸运值(如不需要可不写)
    /// </summary>
    protected override void LuckyBuffDirect()
    {
        if (duration == 0)
        {
            return;
        }
        else
        {
            if (countDownTimer >= duration)
            {
                isActive = false;
            }
            else
            {
                countDownTimer += Time.deltaTime;
            }
        }
    }

    protected override void LuckyBuffDirectExit()
    {
        isActive = false;
    }

    #endregion
    #region Percent
    /// <summary>
    /// 根据affectvalue的值增加血量
    /// </summary>
    protected override void HealthBuffPercent()
    {
        if (duration == 0)
        {
            return;
        }
        else
        {
            if (countDownTimer >= duration)
            {
                isActive = false;
            }
            else
            {
                countDownTimer += Time.deltaTime;
            }
        }
    }

    protected override void HealthBuffPercentExit()
    {
        isActive = false;
    }

    /// <summary>
    /// 根据affectvalue的值增加护盾
    /// </summary>
    protected override void ShieldBuffPercent()
    {
        if (duration == 0)
        {
            return;
        }
        else
        {
            if (countDownTimer >= duration)
            {
                isActive = false;
            }
            else
            {
                countDownTimer += Time.deltaTime;
            }
        }
    }

    protected override void ShieldBuffPercentExit()
    {
        isActive = false;
    }

    /// <summary>
    /// 根据affectvalue的值增加攻击力(如不需要可不写)
    /// </summary>
    protected override void ATKBuffPercent()
    {
        if (duration == 0)
        {
            return;
        }
        else
        {
            if (countDownTimer >= duration)
            {
                isActive = false;
            }
            else
            {
                countDownTimer += Time.deltaTime;
            }
        }
    }

    protected override void ATKBuffPercentExit()
    {
        isActive = false;
    }

    /// <summary>
    /// 根据affectvalue的值增加移动速度(如不需要可不写)
    /// </summary>
    protected override void MoveSpeedBuffPercent()
    {
        if (duration == 0)
        {
            return;
        }
        else
        {
            if (countDownTimer >= duration)
            {
                isActive = false;
            }
            else
            {
                countDownTimer += Time.deltaTime;
            }
        }
    }

    protected override void MoveSpeedBuffPercentExit()
    {
        isActive = false;
    }

    /// <summary>
    /// 根据affectvalue的值增加闪避CD(如不需要可不写)
    /// </summary>
    protected override void DodgeCDBuffPercent()
    {
        if (duration == 0)
        {
            return;
        }
        else
        {
            if (countDownTimer >= duration)
            {
                isActive = false;
            }
            else
            {
                countDownTimer += Time.deltaTime;
            }
        }
    }

    protected override void DodgeCDBuffPercentExit()
    {
        isActive = false;
    }

    /// <summary>
    /// 根据affectvalue的值增加抓钩CD(如不需要可不写)
    /// </summary>
    protected override void FalculaCDBuffPercent()
    {
        if (duration == 0)
        {
            return;
        }
        else
        {
            if (countDownTimer >= duration)
            {
                isActive = false;
            }
            else
            {
                countDownTimer += Time.deltaTime;
            }
        }
    }

    protected override void FalculaCDBuffPercentExit()
    {
        isActive = false;
    }

    /// <summary>
    /// 根据affectvalue的值增加幸运值(如不需要可不写)
    /// </summary>
    protected override void LuckyBuffPercent()
    {
        if (duration == 0)
        {
            return;
        }
        else
        {
            if (countDownTimer >= duration)
            {
                isActive = false;
            }
            else
            {
                countDownTimer += Time.deltaTime;
            }
        }
    }

    protected override void LuckyBuffPercentExit()
    {
        isActive = false;
    }

    #endregion
}
