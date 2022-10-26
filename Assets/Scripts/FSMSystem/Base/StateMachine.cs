using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    protected IState currentState;
    protected Dictionary<eEnemyState, IState> stateDic;
    private void Update()
    {
        currentState.LogicUpdate();
        // print(gameObject.name + ":" + currentState);
    }

    private void FixedUpdate()
    {
        currentState.PhysicUpdate();
    }

    protected void SwitchOn(IState newState)
    {
        currentState = newState;
        currentState.Enter();
    }

    public void SwitchState(IState newState)
    {
        currentState.Exit();
        SwitchOn(newState);
    }
    public void SwitchState(eEnemyState stateType)
    {
        SwitchState(stateDic[stateType]);
    }
}
