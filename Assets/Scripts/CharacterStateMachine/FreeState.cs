using UnityEngine;

public class FreeState : CharacterState
{
    public override void OnEnter()  { Debug.Log("Enter state: FreeState\n"); }

    public override void OnUpdate()  { }

    public override void OnFixedUpdate()
    {
        Vector3 vectorOnFloor = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            vectorOnFloor = m_stateMachine.Camera.transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            vectorOnFloor = -1 * m_stateMachine.Camera.transform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            vectorOnFloor = -1 * m_stateMachine.Camera.transform.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            vectorOnFloor = m_stateMachine.Camera.transform.right;
        }

        m_stateMachine.RB.AddForce(vectorOnFloor * m_stateMachine.AccelerationValue, ForceMode.Acceleration);

        Vector3 velocity = m_stateMachine.RB.velocity;
        float mag = m_stateMachine.RB.velocity.magnitude;
        float vx = m_stateMachine.RB.velocity.x;
        float vz = m_stateMachine.RB.velocity.z;  

        if (vz > 0)
        {
            //forward diagonal
            if (vx > 0.3 || vx < -0.3)
            {
                if (mag > m_stateMachine.MaxVelocityDiagUp)
                {
                    float scalingFactor = m_stateMachine.MaxVelocityDiagUp/mag;
                    velocity *= scalingFactor;
                    m_stateMachine.RB.velocity = velocity;
                }
            }
            //straight forward 
            if (mag > m_stateMachine.MaxVelocityFwd)
            {
                float scalingFactor = m_stateMachine.MaxVelocityFwd/mag;
                velocity *= scalingFactor;
                m_stateMachine.RB.velocity = velocity;
            }
        }
        //going backwards
        if (vz < 0)
        {
            //backwards diagonal
            if (vx > 0.3 || vx < -0.3)
            {
                if (m_stateMachine.RB.velocity.magnitude > m_stateMachine.MaxVelocityDiagDown)
                {
                    float scalingFactor = m_stateMachine.MaxVelocityDiagDown / m_stateMachine.RB.velocity.magnitude;
                    velocity *= scalingFactor;
                    m_stateMachine.RB.velocity = velocity;
                }
            }
            //straight backwards
            if (m_stateMachine.RB.velocity.magnitude > m_stateMachine.MaxVelocityBckwd)
            {
                float scalingFactor = m_stateMachine.MaxVelocityBckwd / m_stateMachine.RB.velocity.magnitude;
                velocity *= scalingFactor;
                m_stateMachine.RB.velocity = velocity;
            }
        }
        //going sideways
        if (vz == 0 && vx != 0)
        {
            if (m_stateMachine.RB.velocity.magnitude > m_stateMachine.MaxVelocitySides)
            {
                float scalingFactor = m_stateMachine.MaxVelocitySides / m_stateMachine.RB.velocity.magnitude;
                velocity *= scalingFactor;
                m_stateMachine.RB.velocity = velocity;
            }
        }
        float forwardComponent = Mathf.Clamp(vz, -1.0f, 1.0f);
        float horizontalComponent = Mathf.Clamp(vz, -1.0f, 1.0f);
        Vector2 animDir = new Vector2(horizontalComponent, forwardComponent);
        m_stateMachine.UpdateAnimatorValues(animDir);

    }
    public override void OnExit(){ Debug.Log("Exit state: FreeState\n"); }
    public override bool CanEnter(CharacterState currentState)
    {
        //Je ne peux entrer dans le FreeState que si je touche le sol
        return m_stateMachine.IsInContactWithFloor();
    }
    public override bool CanExit() { return true; }
}



//TODO 31 AO�T:
//Appliquer les d�placements relatifs � la cam�ra dans les 3 autres directions
//Avoir des vitesses de d�placements maximales diff�rentes vers les c�t�s et vers l'arri�re
//Lorsqu'aucun input est mis, d�c�l�rer le personnage rapidement