using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("---State---")]
    [DisplayOnly] public bool isGuarding;

    [DisplayOnly] public bool isChasing;

    [DisplayOnly] public bool isAttacking;

    [DisplayOnly] public bool isEscaping;

    EnemyController enemyController;

    EnemyStateMachine enemyStateMachine;

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
        enemyStateMachine = GetComponent<EnemyStateMachine>();
    }
}
