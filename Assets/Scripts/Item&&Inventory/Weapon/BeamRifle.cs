using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Weapon/")]
public class BeamRifle : WeaponSO
{
    public int lockCount;


    public override bool AbilityCheck()
    {
        return base.AbilityCheck();
    }

    public override void AbilityIn(WeaponManager weaponManager, Weapon usingWeapon, PlayerStatsManager playerStatsManager)
    {
        base.AbilityIn(weaponManager, usingWeapon, playerStatsManager);
    }

    public override void AbilityPerform(WeaponManager weaponManager, Weapon usingWeapon, PlayerStatsManager playerStatsManager)
    {

    }

    public override void AbilityOut(WeaponManager weaponManager, Weapon usingWeapon, PlayerStatsManager playerStatsManager)
    {
        base.AbilityOut(weaponManager, usingWeapon, playerStatsManager);
    }
}
