using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    PlayerStatsManager playerStatsManager;
    WeaponManager weaponManager;
    [Header("上限")]
    const int _GoldLimit = 99;
    const int _KeyLimit = 99;
    const int _BoomLimit = 99;
    [Header("资源")]
    public int goldAmount;
    public int keyAmount;
    public int boomAmount;

    public List<PassiveInventory> inventories;
    public ActiveInventory currentActive;
    [Header("特殊队列")]
    [SerializeField] List<PassiveInventory> directQueue;
    [SerializeField] List<PassiveInventory> percentQueue;
    [SerializeField] List<PassiveInventory> hybridtQueue;
    [SerializeField] List<PassiveInventory> goldQueue;//对于拾取金钱掉落物能够触发效果的物品队列
    [SerializeField] List<PassiveInventory> keyQueue;//对于拾取钥匙掉落物能够触发效果的物品队列
    [SerializeField] List<PassiveInventory> boomQueue;//对于拾取炸弹掉落物能够触发效果的物品队列
    [SerializeField] List<PassiveInventory> dropQueue;//对于拾取炸弹掉落物能够触发效果的物品队列
    [SerializeField] List<PassiveInventory> healthQueue;//对于拾取恢复掉落物能够触发效果的物品队列
    [SerializeField] List<PassiveInventory> AmmoQueue;//对于拾取弹药掉落物能够触发效果的物品队列
    [SerializeField] List<PassiveInventory> attackRandomTriggerQueue;//对于攻击时会概率触发的物品队列
    public Dictionary<PassiveInventory,EntityRecorder_Inventory> entityTriggerQueue;//对于会生成实体的道具的物品队列(例：X秒概率触发一次闪电)


    /// <summary>
    /// 获取被动道具
    /// </summary>
    /// <param name="inventory"></param>
    public void GetPassiveInventory(PassiveInventory pInventory)
    {
        inventories.Add(pInventory);
        pInventory.TriggerWhenGet(this, playerStatsManager);
        if (pInventory.enhanceType == EnhanceType.direct)
        {
            directQueue.Add(pInventory);
        }
        else if (pInventory.enhanceType == EnhanceType.percent)
        {
            percentQueue.Add(pInventory);
        }
        else if (pInventory.enhanceType == EnhanceType.hybrid)
        {
            hybridtQueue.Add(pInventory);
        }
        if (pInventory.t_getGold)
        {
            goldQueue.Add(pInventory);
        }
        if (pInventory.t_getKey)
        {
            keyQueue.Add(pInventory);
        }
        if (pInventory.t_getBoom)
        {
            boomQueue.Add(pInventory);
        }
        if (pInventory.t_getHealthDrop)
        {
            healthQueue.Add(pInventory);
        }
        if (pInventory.t_getAmmo)
        {
            AmmoQueue.Add(pInventory);
        }
        if (pInventory.t_getdrop)
        {
            dropQueue.Add(pInventory);
        }
        if (pInventory.t_attack_Random)
        {
            attackRandomTriggerQueue.Add(pInventory);
        }
        CalculateAttribute();
    }

    public void GetActiveInventory(ActiveInventory aInventory)
    {
        currentActive = aInventory;
        currentActive.TriggerWhenGet(this, playerStatsManager);
        CalculateAttribute();
    }

    #region 掉落物
    /// <summary>
    /// 
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>决定是否销毁掉落物</returns>
    public bool GetGold(int amount)
    {
        if (goldAmount < _GoldLimit)
        {
            EventManager.current.DropCollected();
            DropQueueCheck(ref amount);        
            GoldQueueCheck(ref amount);
            goldAmount += amount;
            if (goldAmount >= _GoldLimit)
            {
                goldAmount = _GoldLimit;

            }
            return true;
        }
        return false;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>决定是否销毁掉落物</returns>

    public bool GetBoom(int amount)
    {
        if (boomAmount < _BoomLimit)
        {
            EventManager.current.DropCollected();
            DropQueueCheck(ref amount);
            BoomQueueCheck(ref amount);
            boomAmount += amount;
            if (boomAmount >= _BoomLimit)
            {
                boomAmount = _BoomLimit;
            }
            return true;
        }
        return false;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>决定是否销毁掉落物</returns>
    public bool GetKey(int amount)
    {
        if (keyAmount < _KeyLimit)
        {
            EventManager.current.DropCollected();
            DropQueueCheck(ref amount);
            KeyQueueCheck(ref amount);
            keyAmount += amount;
            if (keyAmount >= _KeyLimit)
            {
                keyAmount = _KeyLimit;
            }
            return true;
        }
        return false;
    }

    public bool GetHealth(int amount)
    {
        if (playerStatsManager.currentHealth < playerStatsManager.currentMaxHealth)
        {
            EventManager.current.DropCollected();
            DropQueueCheck(ref amount);
            HealthQueueCheck(ref amount);
            playerStatsManager.currentHealth += amount;//未来改为playerStats的Heal接口
            if (playerStatsManager.currentHealth>= playerStatsManager.currentMaxHealth)
            {
                playerStatsManager.currentHealth = playerStatsManager.currentMaxHealth;
            }
            return true;
        }
        return false;
    }

    public bool GetAmmo(int amount)
    {
        if (weaponManager.currentWeapon.currentAmmoCarried < weaponManager.currentWeapon.currentMaxAmmoCarried)
        {
            EventManager.current.DropCollected();
            DropQueueCheck(ref amount);
            AmmoQueueCheck(ref amount);
            weaponManager.currentWeapon.currentAmmoCarried += amount;//未来改为WeaponManager的Supply接口
            if (weaponManager.currentWeapon.currentAmmoCarried > weaponManager.currentWeapon.currentMaxAmmoCarried)
            {
                weaponManager.currentWeapon.currentAmmoCarried = weaponManager.currentWeapon.currentMaxAmmoCarried;
            }
            return true;
        }
        return false;
    }

    #endregion
    #region 快速道具队列检测
    void GoldQueueCheck(ref int amount)
    {
        if (goldQueue!=null)
        {
            foreach (var gq in goldQueue)
            {
                gq.GoldQueueTrigger(ref amount, this, playerStatsManager);
            }
        }
    }

    void KeyQueueCheck(ref int amount)
    {
        if (keyQueue != null)
        {
            foreach (var kq in keyQueue)
            {
                kq.GoldQueueTrigger(ref amount, this, playerStatsManager);
            }
        }
    }

    void BoomQueueCheck(ref int amount)
    {
        if (boomQueue != null)
        {
            foreach (var bq in boomQueue)
            {
                bq.GoldQueueTrigger(ref amount, this, playerStatsManager);
            }
        }
    }

    void DropQueueCheck(ref int amount)
    {
        if (dropQueue != null)
        {
            foreach (var dq in dropQueue)
            {
                dq.DropQueueTrigger(ref amount, this, playerStatsManager);
            }
        }
    }

    void HealthQueueCheck(ref int amount)
    {
        if (healthQueue != null)
        {
            foreach (var dq in healthQueue)
            {
                dq.HealthQueueTrigger(ref amount, this, playerStatsManager);
            }
        }
    }

    void AmmoQueueCheck(ref int amount)
    {
        if (AmmoQueue != null)
        {
            foreach (var dq in AmmoQueue)
            {
                dq.AmmoQueueTrigger(ref amount, this, playerStatsManager);
            }
        }
    }
    #endregion


    /// <summary>
    /// 删除物品，并从快速队列中删除
    /// </summary>
    /// <param name="index"></param>
    public void RemovePassiveInventory(int index)
    {
        var inventory = inventories[index];
        if (inventory != null)
        {
            inventories.Remove(inventory);
            if (directQueue.Contains(inventory))
            {
                directQueue.Remove(inventory);
            }
            if (percentQueue.Contains(inventory))
            {
                percentQueue.Remove(inventory);
            }
            if (goldQueue.Contains(inventory))
            {
                percentQueue.Remove(inventory);
            }
            if (keyQueue.Contains(inventory))
            {
                keyQueue.Remove(inventory);
            }
            if (boomQueue.Contains(inventory))
            {
                boomQueue.Remove(inventory);
            }
            if (dropQueue.Contains(inventory))
            {
                dropQueue.Remove(inventory);
            }
            if (healthQueue.Contains(inventory))
            {
                healthQueue.Remove(inventory);
            }
            if (AmmoQueue.Contains(inventory))
            {
                AmmoQueue.Remove(inventory);
            }
            if (attackRandomTriggerQueue.Contains(inventory))
            {
                attackRandomTriggerQueue.Remove(inventory);
            }
            if (entityTriggerQueue.ContainsKey(inventory))
            {
                entityTriggerQueue[inventory].Remove();
                entityTriggerQueue.Remove(inventory);
            }
            //将物品弹到地上
            CalculateAttribute();
        }
    }

    public void RemoveActiveInventory()
    {
        currentActive = null;
        //把该道具弹到地上
        CalculateAttribute();
    }
    
    /// <summary>
    /// 计算来自道具给予的属性
    /// </summary>
    public void CalculateAttribute()
    {
        foreach (var dq in directQueue)
        {
            dq.DirectTrigger(playerStatsManager);
        }
        foreach (var hq in hybridtQueue)
        {
            hq.HybridTrigger(playerStatsManager);
        }
        currentActive.CalculateTrigger(playerStatsManager);
        foreach (var pq in percentQueue)
        {
            pq.PercentTrigger(playerStatsManager);
        }
    }

    /// <summary>
    /// 使用主动道具
    /// </summary>
    public void UseActiveInventory()
    {
        if (currentActive == null)
        currentActive.Use(this,playerStatsManager);
    }

    public void TriggerPassiveEffect()
    {
        if (inventories!=null)
        {
            foreach (var inventory in inventories)
            {
                inventory.Trigger();
            }
        }
    }

}
