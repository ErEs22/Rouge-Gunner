using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Inventory : ScriptableObject
{
    public string inventoryName;
    public InventoryType inventoryType;
    public EnhanceType enhanceType;
    public int tier;//道具品级

    [Header("触发位置")]
    public bool t_getGold;
    public bool t_getKey;
    public bool t_getBoom;
    public bool t_getdrop;
    public bool t_getHealthDrop;
    public bool t_getAmmo;
    public bool t_attack_Random;

    //Basic properties
    



    /// <summary>
    /// 当道具计算属性直接增加属性(int)
    /// </summary>
    public virtual void DirectTrigger(PlayerStatsManager playerStatsManager)
    {

    }
    /// <summary>
    /// 当道具计算属性直接增加属性(float)
    /// </summary>
    public virtual void PercentTrigger(PlayerStatsManager playerStatsManager)
    {

    }
    /// <summary>
    /// 当道具计算属性直接增加属性(int和float)
    /// </summary>
    public virtual void HybridTrigger(PlayerStatsManager playerStatsManager)
    {

    }
    /// <summary>
    /// 获得道具时触发
    /// </summary>
    public virtual void TriggerWhenGet(InventoryManager inventoryManager,PlayerStatsManager playerStatsManager)
    {

    }

    /// <summary>
    /// 处于金钱触发队列时，在拾取到金钱时触发的道具效果的接口
    /// </summary>
    /// <param name="count"></param>
    /// <param name="inventoryManager"></param>
    /// <param name="playerStatsManager"></param>
    public virtual void GoldQueueTrigger(ref int count,InventoryManager inventoryManager,PlayerStatsManager playerStatsManager)
    {

    }
    /// <summary>
    /// 处于钥匙触发队列时，在拾取到钥匙时触发的道具效果的接口
    /// </summary>
    /// <param name="count"></param>
    /// <param name="inventoryManager"></param>
    /// <param name="playerStatsManager"></param>
    public virtual void KeyQueueTrigger(ref int count, InventoryManager inventoryManager, PlayerStatsManager playerStatsManager)
    {

    }

    /// <summary>
    /// 处于炸弹触发队列时，在拾取到炸弹时触发的道具效果的接口
    /// </summary>
    /// <param name="count"></param>
    /// <param name="inventoryManager"></param>
    /// <param name="playerStatsManager"></param>
    public virtual void BoomQueueTrigger(ref int count,InventoryManager inventoryManager,PlayerStatsManager playerStatsManager)
    {

    }

    /// <summary>
    /// 处于掉落物触发队列时，在拾取到掉落物时触发的道具效果的接口
    /// </summary>
    /// <param name="count"></param>
    /// <param name="inventoryManager"></param>
    /// <param name="playerStatsManager"></param>
    public virtual void DropQueueTrigger(ref int count, InventoryManager inventoryManager, PlayerStatsManager playerStatsManager)
    {

    }

    /// <summary>
    /// 处于恢复掉落物触发队列时，在拾取到恢复掉落物时触发的道具效果的接口
    /// </summary>
    /// <param name="count"></param>
    /// <param name="inventoryManager"></param>
    /// <param name="playerStatsManager"></param>
    public virtual void HealthQueueTrigger(ref int count, InventoryManager inventoryManager, PlayerStatsManager playerStatsManager)
    {

    }
    /// <summary>
    /// 处于弹药掉落物触发队列时，在拾取到弹药掉落物时触发的道具效果的接口
    /// </summary>
    /// <param name="count"></param>
    /// <param name="inventoryManager"></param>
    /// <param name="playerStatsManager"></param>
    public virtual void AmmoQueueTrigger(ref int count, InventoryManager inventoryManager, PlayerStatsManager playerStatsManager)
    {

    }

    /// <summary>
    /// 随机触发时所用接口
    /// </summary>
    public virtual void Trigger()
    {

    }
}
