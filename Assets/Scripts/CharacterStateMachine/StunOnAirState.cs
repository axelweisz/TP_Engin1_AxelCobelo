using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunOnAirState : CharacterState
{
    public override void OnEnter()
    {
        Debug.Log("Enter state: \n");
    }

    public override void OnExit()
    {
        Debug.Log("Exit state: \n");
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
    }

    public override bool CanEnter(CharacterState currentState)
    {
        return false;
    }

    public override bool CanExit()
    {
        return false;
    }
}
