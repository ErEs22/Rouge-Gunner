using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/EnemySO", fileName = "EnemySO")]
public class EnemySO : ScriptableObject
{
    public float baseMaxHealth;

    public float ATK;

    public float ATKInteval;
}
