using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform muzzle;
    public int currentmagSize;
    public int currentAmmoInMag;
    public int currentAmmoCarried;
    public int currentMaxAmmoCarried;
    public int damage;
    public float interval;
    public bool auto;

    public int ammoCostPerTap;//每一枪所消耗的子弹

    [Header("武器能力")]
    public float currentCD;
    public float maxCD;

    [Header("武器Flag")]
    public bool isFireControlSys;
    public bool targetRequired;
    public bool allowAbility=true;

    private void Update()
    {
        float delta = Time.deltaTime;
        Cooling(delta);
    }


    private void Cooling(float delta)
    {
        if (currentCD>0&&allowAbility)
        currentCD -= delta;
        else
        {
            currentCD = 0;
        }
    }

}
