using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Attack : EnemyState
{
    public EnemyState_Attack(string enterAnimName)
    {
        enemyState = eEnemyState.Attack;
        stateName = enterAnimName;
    }
    public override void Enter()
    {
        base.Enter();
        enemyManager.isAttacking = true;
        ai.isStopped = true;
        enemyController.isAttackFinished = false;
        enemyController.Attack();
    }

    public override void Exit()
    {
        base.Exit();
        enemyManager.isAttacking = false;
        ai.isStopped = false;
        enemyController.StopAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (enemyController.isAttackFinished)
        {
            if (enemyController.Attackable() && enemyController.PlayerVisible())
            {
                enemyStateMachine.SwitchState(eEnemyState.Attack);
            }
            else
            {
                enemyStateMachine.SwitchState(eEnemyState.Chase);
            }
        }
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        enemyController.TowardsPlayer();
    }
}
