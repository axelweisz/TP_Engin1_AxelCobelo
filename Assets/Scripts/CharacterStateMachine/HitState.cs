using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HitState : CharacterState
{

    private const float STATE_EXIT_TIMER = 1f;
    private float m_currentStateTimer = 0.0f;

    public override void OnEnter()
    {
        Debug.Log("Enter state: HitState\n");
        m_stateMachine.GotHit();
    }

    public override void OnExit()
    {
        Debug.Log("Exit state: HitState\n");
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
        m_currentStateTimer -= Time.deltaTime;
    }

    public override bool CanEnter(CharacterState currentState)
    {
        var freeState = currentState as FreeState;
        if (freeState != null)
        {
            return Input.GetKeyDown(KeyCode.X);
        }
        return false;
    }

    public override bool CanExit()
    {
        return m_currentStateTimer <= 0;
    }
}
