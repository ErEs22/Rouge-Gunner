using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Character/CharacterSO", fileName = "CharacterSO")]
public class CharacterSO : ScriptableObject
{
    public float baseMaxHealth;
    public float skillCD;
    public int shield;

    [Header("移动与跳跃")]
    public float baseMoveSpeed = 4f;//走路速度
    //public float baseSprintSpeed = 10f;//跑步速度
    public float jumpHeight = 1.5f;//跳跃高度

    public float dodgeSpeed = 15f;//闪避速度
    public float baseDodgeCD = 2f;//闪避CD
    public float falculaSpeed = 20f;//抓钩速度
    public float baseFalculaCD = 5f;//抓钩CD


    [Header("人物属性")]
    public float baseMaxShield = 100;
    public int jumpsFrequency = 2;
}
