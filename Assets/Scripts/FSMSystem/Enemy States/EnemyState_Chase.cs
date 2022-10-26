using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Chase : EnemyState
{
    public EnemyState_Chase(string enterStateName)
    {
        enemyState = eEnemyState.Chase;
        stateName = enterStateName;
    }
    public override void Enter()
    {
        base.Enter();
        enemyManager.isChasing = true;
    }

    public override void Exit()
    {
        base.Exit();
        enemyManager.isChasing = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (enemyController.CheckPlayerAround())
        {
            if (enemyController.PlayerVisible())
            {
                if (enemyController.Attackable())
                {
                    enemyStateMachine.SwitchState(eEnemyState.Attack);
                }
                else
                {
                    ai.isStopped = false;
                    enemyController.ChasePlayer();
                }
            }
            else
            {
                enemyStateMachine.SwitchState(eEnemyState.Guard);
            }
        }
        else
        {
            enemyStateMachine.SwitchState(eEnemyState.Guard);
        }
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }
}
