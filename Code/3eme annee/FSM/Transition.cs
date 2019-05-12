using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition
{
    #region Variables
    protected State m_nextState;
    #endregion

    #region Events
    public delegate bool TransitionAction();
    public TransitionAction OnComputeCondition;
    #endregion

    #region Properties
    public State NextState { get { return m_nextState; } }
    #endregion

    #region Constructors/Destructors
    public Transition(State state)
    {
        m_nextState = state;
    }
    #endregion

    #region Methods
    public bool ComputeCondition()
    {
        return OnComputeCondition();
    }
    #endregion
}