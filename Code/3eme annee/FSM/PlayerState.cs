using UnityEngine;

/*
ATTENTION la classe n'est pas terminée. Je suis en train de travailler dessus.
C'était pour donner un exemple d'utilisation de la FSM (finite state machine)
*/
public class PlayerState : EntityState
{
    #region Variables
    private Player m_player;

    private State m_idle;
    private State m_move;
    private State m_jump;
    private State m_doubleJump;
    private State m_crouch;
    private State m_roll;

    private Transition m_idleToMove;
    private Transition m_moveToIdle;

    private Transition m_idleToJump;
    private Transition m_moveToJump;
    private Transition m_jumpToIdle;

    private Transition m_jumpToDoubleJump;
    private Transition m_doubleJumpToIdle;

    private Transition m_idleToRoll;
    private Transition m_moveToRoll;
    private Transition m_rollToIdle;

    private Transition m_idleToCrouch;
    private Transition m_crouchToIdle;
    #endregion

    #region Constructors/Destructors
    public PlayerState(Player player)
    {
        m_player = player;

        InitStateTransition();
        BindTransitionToState();

        BindEnterAction();
        BindUpdateAction();
        BindFixedUpdateAction();
        BindExitAction();

        BindComputeCondition();

        m_currentState = m_idle;
    }
    #endregion

    #region Methods
    private void InitStateTransition()
    {
        m_idle = new State("Idle");
        m_move = new State("Move");
        m_jump = new State("Jump");
        m_doubleJump = new State();
        m_crouch = new State();
        m_roll = new State();

        m_idleToMove = new Transition(m_move);
        m_moveToIdle = new Transition(m_idle);

        m_idleToJump = new Transition(m_jump);
        m_moveToJump = new Transition(m_jump);
        m_jumpToIdle = new Transition(m_idle);

        m_jumpToDoubleJump = new Transition(m_doubleJump);
        m_doubleJumpToIdle = new Transition(m_idle);

        m_idleToRoll = new Transition(m_roll);
        m_moveToRoll = new Transition(m_roll);
        m_rollToIdle = new Transition(m_idle);

        m_idleToCrouch = new Transition(m_crouch);
        m_crouchToIdle = new Transition(m_idle);
    }

    private void BindTransitionToState()
    {
        m_idle.AddTransition(m_idleToMove);
        m_idle.AddTransition(m_idleToJump);
        m_idle.AddTransition(m_idleToRoll);
        m_idle.AddTransition(m_idleToCrouch);

        m_move.AddTransition(m_moveToIdle);
        m_move.AddTransition(m_moveToJump);
        m_move.AddTransition(m_moveToRoll);

        m_jump.AddTransition(m_jumpToIdle);
        m_jump.AddTransition(m_jumpToDoubleJump);

        m_doubleJump.AddTransition(m_doubleJumpToIdle);

        m_roll.AddTransition(m_rollToIdle);

        m_crouch.AddTransition(m_crouchToIdle);
    }

    private void BindEnterAction()
    {
        m_jump.OnEnter += EnterJumpState;
        m_doubleJump.OnEnter += EnterDoubleJumpState;
        m_roll.OnEnter += EnterRollState;
        m_crouch.OnEnter += EnterCrouchState;
    }

    private void BindUpdateAction()
    {
        m_jump.OnUpdate += UpdateJumpDoubleJumpState;
        m_doubleJump.OnUpdate += UpdateJumpDoubleJumpState;
    }

    private void BindFixedUpdateAction()
    {
        m_move.OnFixedUpdate += FixedUpdateMoveState;
    }

    private void BindExitAction()
    {
        m_crouch.OnExit += ExitCrouchState;
    }

    private void BindComputeCondition()
    {
        //Conditions with move/idle
        m_idleToMove.OnComputeCondition += IdleToMoveCondition;
        m_moveToIdle.OnComputeCondition += MoveToIdleCondition;

        //Conditions with jump/move/idle
        m_idleToJump.OnComputeCondition += ActionToJumpCondition;
        m_moveToJump.OnComputeCondition += ActionToJumpCondition;
        m_jumpToIdle.OnComputeCondition += JumpToActionCondition;

        //Conditions with doubleJump
        m_jumpToDoubleJump.OnComputeCondition += JumpToDoubleJumpCondition;
        m_doubleJumpToIdle.OnComputeCondition += JumpToActionCondition;

        //Conditions with roll
        m_idleToRoll.OnComputeCondition += ActionToRollCondition;
        m_moveToRoll.OnComputeCondition += ActionToRollCondition;
        m_rollToIdle.OnComputeCondition += RollToIdleCondition;

        //Conditions with crouch
        m_idleToCrouch.OnComputeCondition += IdleToCrouchCondition;
        m_crouchToIdle.OnComputeCondition += CrounchToIdleCondition;
    }
    #endregion

    #region TransitionMethods
    private bool IdleToMoveCondition()
    {
        return m_player.IsMoving;
    }

    private bool MoveToIdleCondition()
    {
        return !m_player.IsMoving;
    }

    private bool ActionToJumpCondition()
    {
        return m_player.IsJumping;
    }

    private bool JumpToActionCondition()
    {
        return m_player.IsGrounded;
    }

    private bool JumpToDoubleJumpCondition()
    {
        Debug.Log(m_player.IsDoubleJumping);
        return m_player.IsDoubleJumping;
    }

    private bool ActionToRollCondition()
    {
        return m_player.IsRolling;
    }

    private bool RollToIdleCondition()
    {
        return !m_player.IsRolling;
    }

    private bool IdleToCrouchCondition()
    {
        return m_player.IsCrouch;
    }

    private bool CrounchToIdleCondition()
    {
        return !m_player.IsCrouch;
    }
    #endregion

    #region StateMethods
    private void EnterJumpState()
    {
        PlayerAnimManager.Instance.SetParameter(PlayerAnimManager.Parameters.Speed, 0);
        PlayerAnimManager.Instance.SetParameter(PlayerAnimManager.Parameters.Jump);
        m_player.Jump();
    }

    private void EnterDoubleJumpState()
    {
        PlayerAnimManager.Instance.SetParameter(PlayerAnimManager.Parameters.DoubleJump);
        m_player.Jump();
    }

    private void EnterRollState()
    {
        Debug.Log("Roll");
        PlayerAnimManager.Instance.SetParameter(PlayerAnimManager.Parameters.Roll);
    }

    private void EnterCrouchState()
    {
        PlayerAnimManager.Instance.SetParameter(PlayerAnimManager.Parameters.Crouch, true);
    }

    private void ExitCrouchState()
    {
        PlayerAnimManager.Instance.SetParameter(PlayerAnimManager.Parameters.Crouch, false);
    }

    private void UpdateJumpDoubleJumpState()
    {
        m_player.Move();
        m_player.CheckIsGrounded();
    }

    private void FixedUpdateMoveState()
    {
        m_player.Move();
        PlayerAnimManager.Instance.SetParameter(PlayerAnimManager.Parameters.Speed, Mathf.Abs(m_player.Speed));
    }
    #endregion
}