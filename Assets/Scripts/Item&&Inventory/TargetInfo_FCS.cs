using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInfo_FCS 
{
    public EnemyManager enemy;
    public float distance;

    public TargetInfo_FCS(EnemyManager enemyManager,float distance)
    {
        enemy=enemyManager;
        this.distance =  distance;
    }


}
