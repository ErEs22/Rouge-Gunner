using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour, IDamageable
{
    [SerializeField]
    CharacterSO characterSO;

    HealthBar healthBar;

    ShieldBar shieldBar;

    private void Awake()
    {
        healthBar = FindObjectOfType<HealthBar>();
        shieldBar = FindObjectOfType<ShieldBar>();
        InitializeStatus();
    }
    public float currentHealth;
    public float currentMaxHealth;
    public float baseMaxHealth;
    public float currentShield;
    public float currentMaxShield;
    public float baseMaxShield;

    public float currentWalkSpeed;
    //public float currentSprintSpeed;
    public float currentJumpHeight;

    public float DodgeCD;
    public float currenDodgeSpeed;
    public float FalculaCD;
    public float currenFalculaSpeed;

    public int currentJumpsFrequency;
    public int jumpsFrequency;

    private void InitializeStatus()
    {
        currentHealth = characterSO.baseMaxHealth;
        currentMaxHealth = characterSO.baseMaxHealth;
        baseMaxHealth = characterSO.baseMaxHealth;
        currentShield = characterSO.shield;
        currentMaxShield = characterSO.shield;
        baseMaxShield = characterSO.shield;

        currentWalkSpeed = characterSO.baseMoveSpeed;
        //currentSprintSpeed = characterSO.baseSprintSpeed;
        currentJumpHeight = characterSO.jumpHeight;

        DodgeCD = characterSO.baseDodgeCD;
        currenDodgeSpeed = characterSO.dodgeSpeed;
        FalculaCD = characterSO.baseFalculaCD;
        currenFalculaSpeed = characterSO.falculaSpeed;

        jumpsFrequency = characterSO.jumpsFrequency;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("TakeDamage:" + gameObject.name);
        if (currentShield > 0)
        {
            currentShield -= damage;

            if (currentShield <= 0)
            {
                currentShield = 0;
                //TODO 死亡
            }

            shieldBar.UpdateStateBar(currentShield, currentMaxShield);
            return;
        }
        else
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                //TODO 死亡
            }

            healthBar.UpdateStateBar(currentHealth, currentMaxHealth);
            return;
        }
    }
}
