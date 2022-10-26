using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsManager : MonoBehaviour, IDamageable
{
    [SerializeField] EnemySO enemySO;

    [SerializeField] StateBar stateBar;

    [DisplayOnly] public float currentHealth;

    [DisplayOnly] public float baseMaxHealth;

    [DisplayOnly] public float currentMaxHealth;

    [DisplayOnly] public float ATK;

    [DisplayOnly] public float ATKInteval;

    private void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        this.currentHealth = enemySO.baseMaxHealth;
        this.baseMaxHealth = enemySO.baseMaxHealth;
        this.currentMaxHealth = enemySO.baseMaxHealth;
        this.ATK = enemySO.ATK;
        this.ATKInteval = enemySO.ATKInteval;
    }

    /// <summary>
    /// 此方法为接口IDamageable的实现，根据伤害量更新属性和状态条
    /// </summary>
    /// <param name="damage">伤害量</param>
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            //TODO 死亡
        }

        stateBar.UpdateStateBar(currentHealth, currentMaxHealth);
    }
}
