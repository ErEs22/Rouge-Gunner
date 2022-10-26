using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 处理游戏中的数值计算
/// </summary>
public static class GameMath
{
    public static float DefendRate => 0.2f;//防御系数

    /// <summary>
    /// 获取攻击伤害，无附加属性
    /// </summary>
    /// <param name="atk">攻击力</param>
    /// <param name="def">防御力</param>
    /// <returns>伤害</returns>
    public static float GetDamage(float atk, float def) => Mathf.Clamp(atk - (def * DefendRate), 0, atk);

    /// <summary>
    /// 获取攻击伤害，附加攻击倍率
    /// </summary>
    /// <param name="atk">攻击力</param>
    /// <param name="def">防御力</param>
    /// <param name="atkMutiple">攻击力倍率</param>
    /// <returns>伤害</returns>
    public static float GetDamage(float atk, float def, float atkMutiple) => Mathf.Clamp((atk * atkMutiple) - (def * DefendRate), 0, atk * atkMutiple);
}
