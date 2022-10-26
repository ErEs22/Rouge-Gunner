using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyStateMachine : StateMachine
{
    [SerializeField] protected List<EnemyState> states;

    [SerializeField] List<eEnemyState> allStates;

    Animator anim;

    protected EnemyController enemyController;

    EnemyManager enemyManager;

    IAstarAI ai;

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        enemyController = GetComponent<EnemyController>();
        enemyManager = GetComponent<EnemyManager>();
        ai = GetComponent<IAstarAI>();
        states = new List<EnemyState>();
        Initialize();

        stateDic = new Dictionary<eEnemyState, IState>();
        foreach (var state in states)
        {
            state.Initialize(anim, this, ai, enemyController, enemyManager);
            stateDic.Add(state.enemyState, state);
        }
    }

    private void Start()
    {
        // ai.canMove = false;
        SwitchOn(stateDic[eEnemyState.Guard]);
    }

    protected virtual void Initialize()
    {
        foreach (var state in allStates)
        {
            states.Add(CreateEnemyState(state));
        }
    }

    EnemyState CreateEnemyState(eEnemyState enemyState)
    {
        switch (enemyState)
        {
            case eEnemyState.Guard:
                return new EnemyState_Guard(enemyController.guardStateAnimation);
            case eEnemyState.Chase:
                return new EnemyState_Chase(enemyController.chaseStateAnimation);
            case eEnemyState.Attack:
                return new EnemyState_Attack(enemyController.attackStateAnimation);
            case eEnemyState.Escape:
                return new EnemyState_Escape(enemyController.escapeStateAnimation);
            case eEnemyState.Death:
                return new EnemyState_Death(enemyController.deathStateAnimation);
            default: return null;
        }
    }
}
