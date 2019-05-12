using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum EStateInput
{
    Down = 0,
    Pressed,
    Released,
    Trigger,
    Axis
}

[Serializable]
public class InputData
{
    #region Variables
    public string inputName;
    public EStateInput stateButton = EStateInput.Down;

    [HideInInspector]
    public Command m_command = new Command();
    [HideInInspector]
    public bool isTriggerUse = false;
    #endregion

    #region Methods
    public void BindExecute(Command.CommandAction action)
    {
        m_command.OnExecute += action;
    }
    #endregion
}