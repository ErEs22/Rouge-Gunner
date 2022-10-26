using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Rifle")]
public class Rifle : WeaponSO
{
    public int sniperModeCost;
    public int damageIncrease;
    public float intervalIncreased;
    public bool single=true;

    public override void AbilityIn(WeaponManager weaponManager, Weapon usingWeapon, PlayerStatsManager playerStatsManager)
    {
        usingWeapon.ammoCostPerTap += sniperModeCost;
        usingWeapon.damage += damageIncrease;
        usingWeapon.interval+=intervalIncreased;
        usingWeapon.auto = !single;
    }

    public override void AbilityPerform(WeaponManager weaponManager, Weapon usingWeapon, PlayerStatsManager playerStatsManager)
    {
        weaponManager.uiManager.EnableScope();
    }

    public override void AbilityOut(WeaponManager weaponManager, Weapon usingWeapon, PlayerStatsManager playerStatsManager)
    {
        usingWeapon.ammoCostPerTap -= sniperModeCost;
        usingWeapon.damage -= damageIncrease;
        usingWeapon.interval -= intervalIncreased;
        usingWeapon.auto = single;
        weaponManager.uiManager.DisableScope();
    }
}
