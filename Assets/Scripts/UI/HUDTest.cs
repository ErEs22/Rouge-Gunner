using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDTest : MonoBehaviour
{
    HealthBar healthBar;
    ShieldBar shieldBar;
    private void Awake()
    {
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>();
        shieldBar = GameObject.FindGameObjectWithTag("ShieldBar").GetComponent<ShieldBar>();
    }
    public void HPDamage()
    {
        healthBar.UpdateStateBar(20f, 100f);
    }
    public void IncreaseHP()
    {
        healthBar.UpdateStateBar(100f, 100f);
    }
    public void ShieldDamage()
    {
        shieldBar.UpdateStateBar(50f, 100f);
    }
    public void IncreaseShield()
    {
        shieldBar.UpdateStateBar(100f, 100f);
    }
}
