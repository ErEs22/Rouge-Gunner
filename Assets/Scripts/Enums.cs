using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Gold,
    Key,
    Boom,
    Health,
    Ammo
}

public enum InventoryType
{
    Usable,
    Passive,
    Active
}

public enum RoomType
{
    Normal,
    Treasure,
    Shop,
    Devil,
    Angel,
    Start,
    End
}

public enum AmmoType
{
    Standard,
    Energy,
    Division,
}

public enum AbilityInteractionType
{
    Click,
    Hold
}

public enum eEnemyState
{
    Guard,
    Chase,
    Attack,
    Escape,
    Death
}

public enum eBuffType
{
    direct,
    percent
}

public enum eAffectAttribute
{
    health,
    shield,
    atk,
    moveSpeed,
    dodgeCD,
    falculaCD,
    lucky
}

public enum EnhanceType
{
    none,//无直接数值强化效果
    direct,//加减法
    percent,//乘法百分比
    hybrid,//混合包含加减乘除
}
public class Enums
{

}
