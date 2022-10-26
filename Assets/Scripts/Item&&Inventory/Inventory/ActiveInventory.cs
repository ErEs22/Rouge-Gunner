using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveInventory : Inventory
{
    public float cd;

    public virtual void CalculateTrigger(PlayerStatsManager playerStatsManager)
    {

    }

    /// <summary>
    /// 当物品能够使用时调用的接口
    /// </summary>
    public virtual void Use(InventoryManager inventoryManager,PlayerStatsManager playerStatsManager)
    {

    }
}
