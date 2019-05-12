using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Timer
{
    #region Variables
    private int m_minutes;
    private int m_secondes;

    private float m_time;
    private float m_currentTime;
    private bool m_isRunning;
    #endregion

    #region Events
    public delegate void TimerAction();
    public TimerAction OnStopTimer;
    #endregion

    #region Properties
    public int Minutes { get { return m_minutes; } }
    public int Secondes { get { return m_secondes; } }
    public bool isRunning { get { return m_isRunning; } }
    #endregion

    #region Methods
    public void Init(int minutes, int secondes)
    {
        m_minutes = minutes;
        m_secondes = secondes;
    }

    public void StartTimer()
    {
        m_time = m_minutes * 60 + m_secondes;
        m_currentTime = m_time;
        m_isRunning = true;
    }

    public void Update()
    {
        if (!m_isRunning)
            return;
        if (m_currentTime > 0)
            m_currentTime -= Time.fixedDeltaTime;
        else
            StopTimer();
    }

    public void StopTimer()
    {
        m_isRunning = false;
        OnStopTimer?.Invoke();
    }
    #endregion
}