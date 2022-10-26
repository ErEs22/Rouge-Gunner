using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Guard : EnemyState
{
    public EnemyState_Guard(string enterStateName)
    {
        enemyState = eEnemyState.Guard;
        stateName = enterStateName;
    }

    public override void Enter()
    {
        base.Enter();
        enemyManager.isGuarding = true;
        ai.maxSpeed = ai.maxSpeed / 2f;
        ai.isStopped = true;
        enemyController.StartGuardCoroutine(transitionDuration, ai);
    }

    public override void Exit()
    {
        base.Exit();
        enemyManager.isGuarding = false;
        enemyController.StopGuardCoroutine();
        ai.maxSpeed = ai.maxSpeed * 2f;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        // Debug.Log(enemyController.gameObject.name + ": Guard");
        if (enemyController.CheckPlayerAroundInGuard())
        {
            if (enemyController.PlayerVisible())
            {
                if (enemyController.attack)
                {
                    enemyStateMachine.SwitchState(eEnemyState.Chase);
                }
                else
                {
                    enemyStateMachine.SwitchState(eEnemyState.Escape);
                }
            }
        }
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }

}
