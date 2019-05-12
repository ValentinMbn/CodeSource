using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class InputManager
{
    #region Variables
    [SerializeField]
    private int m_maxSizeBuffer = 10;
    [SerializeField, Header("Chrono Settings")]
    private int m_minutes;
    [SerializeField]
    private int m_secondes;

    public Inputs m_inputs;
    private List<ComboInput> m_combos;

    private Dictionary<string, List<Command>> m_inputsButtons;
    private List<InputData> m_inputsBind;

    private Queue<Command> m_currentBuffer;
    private List<string> m_keyBuffer;
    private int m_indexBuffer;

    private Timer m_timer;

    private bool m_isRunning = true;
    #endregion

    #region Properties
    public bool IsRunning
    {
        get { return m_isRunning; }
        set { m_isRunning = value; }
    }

    public Inputs Inputs { get { return m_inputs; } }
    #endregion

    #region Methods
    public void Init()
    {
        m_inputsButtons = new Dictionary<string, List<Command>>();

        m_currentBuffer = new Queue<Command>();
        m_keyBuffer = new List<string>();
        m_timer = new Timer();
        m_combos = new List<ComboInput>();
        m_indexBuffer = 0;

        m_timer.Init(m_minutes, m_secondes);
        m_timer.OnStopTimer += ResetKeyBuffer;

        InitInputDictionnary();
    }

    private void InitInputDictionnary()
    {
        m_inputsBind = new List<InputData>();

        CheckInputForAdd(m_inputs.Attack1);
        CheckInputForAdd(m_inputs.Attack2);
        CheckInputForAdd(m_inputs.Consumable);
        CheckInputForAdd(m_inputs.Dash);
        CheckInputForAdd(m_inputs.Interaction);
        CheckInputForAdd(m_inputs.Jump);
        CheckInputForAdd(m_inputs.Roll);
        CheckInputForAdd(m_inputs.Rewind);
        CheckInputForAdd(m_inputs.Crouch);
        CheckInputForAdd(m_inputs.Move);
    }

    private void CheckInputForAdd(InputData inputData)
    {
        //Check if input is correct
        if(!string.IsNullOrEmpty(inputData.inputName))
            m_inputsBind.Add(inputData);
    }

    Command HandleInput()
    {
        for (int i = 0; i < m_inputsBind.Count; i++)
        {
            Command command = ComputeInputWithState(m_inputsBind[i]);
            if (command != null)
                return command;
        }
        return null;
    }

    private Command ComputeInputWithState(InputData inputData)
    {
        //Check the state of the input
        switch (inputData.stateButton)
        {
            case EStateInput.Down:
                return GetButtonDown(inputData);
            case EStateInput.Pressed:
                return GetButtonPressed(inputData);
            case EStateInput.Released:
                return GetButtonReleased(inputData);
            case EStateInput.Trigger:
                return GetTrigger(inputData);
            case EStateInput.Axis:
                return GetAxis(inputData);
            default:
                return null;
        }
    }

    private Command GetButtonDown(InputData inputData)
    {
        if (Input.GetButtonDown(inputData.inputName))
            return ComputeCommandInput(inputData);
        return null;
    }

    private Command GetButtonPressed(InputData inputData)
    {
        if (Input.GetButton(inputData.inputName))
            return ComputeCommandInput(inputData);
        return null;
    }

    private Command GetButtonReleased(InputData inputData)
    {
        if (Input.GetButtonUp(inputData.inputName))
            return ComputeCommandInput(inputData);
        return null;
    }

    //Method to call axis only one time
    private Command GetTrigger(InputData inputData)
    {
        if (Input.GetAxisRaw(inputData.inputName) == 1 && !inputData.isTriggerUse)
        {
            inputData.isTriggerUse = true;
            return ComputeCommandInput(inputData);
        }
        else if(Input.GetAxisRaw(inputData.inputName) == 0 && inputData.isTriggerUse)
            inputData.isTriggerUse = false;
        return null;
    }

    private Command GetAxis(InputData inputData)
    {
        if (Input.GetAxisRaw(inputData.inputName) != 0)
            return ComputeCommandInput(inputData);
        return null;
    }

    private void AddInputToBuffer()
    {
        Command currentCommand = HandleInput();
        if (currentCommand != null)
        {
            if (m_indexBuffer++ < m_maxSizeBuffer)
                m_currentBuffer.Enqueue(currentCommand);
        }
    }

    public void UpdateInput()
    {
        if (!m_isRunning)
            return;

        m_timer.Update();

        if (m_currentBuffer.Count > 0)
        {
            m_currentBuffer.Dequeue().Execute();
            m_indexBuffer--;
        }
        AddInputToBuffer();
    }

    private Command ComputeCommandInput(InputData inputData)
    {
        m_timer.StartTimer();
        m_keyBuffer.Add(inputData.inputName);
        CheckCombo(inputData);

        if (inputData.m_command != null)
            return inputData.m_command;
        return null;
    }

    //Check if a combo input have been played
    private void CheckCombo(InputData inputData)
    {
        if (m_keyBuffer.Count < 0)
            return;

        //Check all combos that have been defined
        for (int i = 0; i < m_combos.Count; i++)
        {
            if (CheckAssociationCombo(m_combos[i]))
            {
                ResetKeyBuffer();
                return;
            }
            else
                ResetKeyExceptLast();
        }
    }

    private bool CheckAssociationCombo(ComboInput comboInput)
    {
        if (comboInput.inputs.Count != m_keyBuffer.Count)
            return false;

        //Check if the combo is correct
        for(int i = 0; i < comboInput.inputs.Count; i++)
        {
            if (comboInput.inputs[i] != m_keyBuffer[i])
                return false;
        }

        //Call the combo function
        comboInput.action?.Invoke();
        return true;
    }

    private void ResetKeyBuffer()
    {
        m_keyBuffer.Clear();
    }

    private void ResetKeyExceptLast()
    {
        for(int i = 0; i < m_keyBuffer.Count-1; i++)
            m_keyBuffer.RemoveAt(0);
    }

    public void AddCombo(ComboInput comboInput)
    {
        m_combos.Add(comboInput);
    }
    #endregion 
}