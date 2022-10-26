using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Death : EnemyState
{
    public EnemyState_Death(string enterAnimName)
    {
        enemyState = eEnemyState.Death;
        stateName = enterAnimName;
    }
    public override void Enter()
    {
        base.Enter();
        ai.isStopped = true;
        enemyController.Die();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }
}
