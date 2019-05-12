using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityState
{
    #region Variables
    protected State m_currentState;

    private bool m_isEnter = false;
    #endregion

    #region Properties
    public State CurrentState
    {
        get { return m_currentState; }
        set
        {
            m_currentState = value;
            m_isEnter = false;
        }
    }
    #endregion

    #region Methods
    public void Enter()
    {
        if(!m_isEnter)
        {
            m_currentState.Enter();
            m_isEnter = true;
        }
    }

    public void Update()
    {
        Enter();
        State tempState = m_currentState.Update();

        if(tempState != m_currentState)
        {
            m_currentState.Exit();
            CurrentState = tempState;
        }
    }

    public void FixedUpdate()
    {
        m_currentState.FixedUpdate();
    }

    public void LateUpdate()
    {
        m_currentState.LateUpdate();
    }
    #endregion
}