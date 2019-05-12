using System.Collections.Generic;

public class State 
{
    #region Variables
    protected List<Transition> m_transitions;
    private string m_name;
    #endregion

    #region Events
    public delegate void StateAction();
    public StateAction OnEnter;
    public StateAction OnUpdate;
    public StateAction OnFixedUpdate;
    public StateAction OnLateUpdate;
    public StateAction OnExit;
    #endregion

    #region Properties
    public string Name { get { return m_name; } }
    #endregion

    #region Constructors/Destructors
    public State(string name = "DefaultNameState")
    {
        m_transitions = new List<Transition>();
        m_name = name;
    }
    #endregion

    #region Methods
    public void AddTransition(Transition transition)
    {
        m_transitions.Add(transition);
    }

    public State ComputeTransition()
    {
        //Check conditions to change state
        foreach (Transition transition in m_transitions)
        {
            if (transition.ComputeCondition())
                return transition.NextState;
        }
        return this;
    }

    //When enter the state
    public void Enter()
    {
        OnEnter?.Invoke();
    }

    //When update the state
    public State Update()
    {
        OnUpdate?.Invoke();
        return ComputeTransition();
    }

    //When fixedUpdate the state
    public void FixedUpdate()
    {
        OnFixedUpdate?.Invoke();
    }

    //When lateUpdate the state
    public void LateUpdate()
    {
        OnLateUpdate?.Invoke();
    }

    //When exit the state
    public void Exit()
    {
        OnExit?.Invoke();
    }
    #endregion
}