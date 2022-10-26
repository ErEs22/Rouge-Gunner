using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : ScriptableObject
{
    public string equipmentName;
    public float atk;
    public float firingRate;//射速
    public int maxBullet;//子弹容量
    public int currentBullet;//当前子弹数

}
