using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Escape : EnemyState
{
    public EnemyState_Escape(string enterStateName)
    {
        enemyState = eEnemyState.Escape;
        stateName = enterStateName;
    }
    public override void Enter()
    {
        base.Enter();
        enemyManager.isEscaping = true;
        ai.isStopped = false;
        enemyController.SetEscapePoint();
    }

    public override void Exit()
    {
        base.Exit();
        enemyManager.isEscaping = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (ai.velocity == Vector3.zero && StateDuration >= 0.3f)
        {
            if (enemyController.CheckPlayerAround() && enemyController.PlayerVisible())
            {
                enemyStateMachine.SwitchState(eEnemyState.Escape);
            }
            else
            {
                anim.CrossFade("Idle", 0.1f);
            }
        }
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }
}
