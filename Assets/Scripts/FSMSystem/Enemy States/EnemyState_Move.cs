using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Move : EnemyState
{
    public override void Enter()
    {
        base.Enter();
        Debug.Log("StartMove");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (ai.velocity == Vector3.zero)
        {
            enemyStateMachine.SwitchState(eEnemyState.Guard);
        }
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }
}
