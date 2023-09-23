using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerStateMachine : MonoBehaviour
{
    public Camera Camera { get; private set; }
    [field:SerializeField]
    public Rigidbody RB { get; private set; }
    [field:SerializeField]
    private Animator Animator { get; set; }

    [field: SerializeField]
    public float JumpIntensity { get; private set; } = 1000.0f;
    [field: SerializeField]
    public float AccelerationValue { get; private set; }
    [field: SerializeField]
    public float MaxVelocity { get; private set; }

    [field: SerializeField]
    public float MaxVelocityFwd { get;  set; } = 8f;
    [field: SerializeField]
    public float MaxVelocityBckwd { get;  set; } = 6f;
    [field: SerializeField]
    public float MaxVelocitySides { get;  set; } = 5f;
    public float MaxVelocityDiagUp { get; set; } = 9.5f;
    public float MaxVelocityDiagDown { get; set; } = 7.82f; 

    [SerializeField]
    private CharacterFloorTrigger m_floorTrigger;
    private CharacterState m_currentState; //IMPORTANT  
    private List<CharacterState> m_possibleStates;
    
    //creat states n add them 2 the m_possibleStates list
    private void Awake()
    {
        m_possibleStates = new List<CharacterState>();
        m_possibleStates.Add(new FreeState());
        m_possibleStates.Add(new JumpState());
        m_possibleStates.Add(new HitState());
    }
    //onStart we set the cam and go to the 1st state onEnter()
    void Start()
    {
        Camera = Camera.main;

        foreach (CharacterState state in m_possibleStates)
        {
            state.OnStart(this);
        }
        m_currentState = m_possibleStates[0];
        m_currentState.OnEnter();
    }
    //we do currentState.onUpdate and we're always trying 2 transition
    private void Update()
    {
        m_currentState.OnUpdate();
        TryStateTransition();
    }
    //we do currentState.onUpdate
    void FixedUpdate()
    {
        m_currentState.OnFixedUpdate();
    }
    //here we only do stuff if m_currentStateCanExit()
    private void TryStateTransition()
    {
        if (!m_currentState.CanExit())
            return;
        //Je PEUX quitter le state actuel -
        //we loop every state until we find one where canEnter() returns true
        foreach (var state in m_possibleStates)
        {
            if (m_currentState == state)//transition from state to itself
               continue;

            if (state.CanEnter(m_currentState))
            {
                //Quitter le state actuel
                m_currentState.OnExit();
                m_currentState = state;
                //Rentrer dans le state state
                m_currentState.OnEnter();
                return;
            }
        }
    }

    public bool IsInContactWithFloor()
    {
        return m_floorTrigger.IsOnFloor;
    }

    public void UpdateAnimatorValues(Vector2 movementVecValue)
    {
        //Aller chercher ma vitesse actuelle
        //Communiquer directement avec mon Animator

        movementVecValue = new Vector2(movementVecValue.x, movementVecValue.y / MaxVelocity);

        Animator.SetFloat("MoveX", movementVecValue.x);
        Animator.SetFloat("MoveY", movementVecValue.y);
    }

    public void GotHit()
    {
        Animator.SetBool("Hit", true);
    }
}
