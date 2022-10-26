using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Idle : EnemyState
{
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Debug.Log("Idle");
        if (ai.velocity != Vector3.zero)
        {
            enemyStateMachine.SwitchState(eEnemyState.Guard);
        }
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }
}
