using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyState : IState
{
    protected string stateName;
    protected float transitionDuration = 0.1f;

    public eEnemyState enemyState;

    protected Animator anim;
    protected EnemyStateMachine enemyStateMachine;
    protected EnemyController enemyController;
    protected EnemyManager enemyManager;
    protected IAstarAI ai;
    protected bool IsAnimationFinished => StateDuration >= anim.GetCurrentAnimatorStateInfo(0).length;
    protected float StateDuration => Time.time - stateStartTime;

    float stateStartTime;

    public void Initialize(Animator anim, EnemyStateMachine enemyStateMachine, IAstarAI ai, EnemyController enemyController, EnemyManager enemyManager)
    {
        this.anim = anim;
        this.enemyStateMachine = enemyStateMachine;
        this.ai = ai;
        this.enemyController = enemyController;
        this.enemyManager = enemyManager;
    }
    public virtual void Enter()
    {
        anim.CrossFade(stateName, transitionDuration);
        stateStartTime = Time.time;
    }

    public virtual void Exit()
    {
    }

    public virtual void LogicUpdate()
    {
        if (!enemyController.IsAlive())
        {
            enemyStateMachine.SwitchState(eEnemyState.Death);
        }
    }

    public virtual void PhysicUpdate()
    {
    }

}
