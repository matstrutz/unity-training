using System.Collections;
using UnityEngine;

public class Player : Entity {

    [Header("Attack Details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = 0.2F;

    [Header("Move Info")]
    public float moveSpeed = 7;
    public float jumpForce = 9;
    public float swordReturnImpact = 5;

    [Header("Dash Info")]
    public float dashSpeed = 25;
    public float dashDuration = 0.3F;
    public float dashDir { get; private set; }

    public bool isBusy { get; private set; }

    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; }

    #region States
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState slideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    public PlayerBlackHoleState blackHoleState { get; private set; }
    #endregion 

    protected override void Awake() {
        base.Awake();
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        slideState = new PlayerWallSlideState(this, stateMachine, "Slide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");

        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "Counter");

        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackHoleState = new PlayerBlackHoleState(this, stateMachine, "Jump");
    }

    protected override void Start() {
        base.Start();

        skill = SkillManager.instance;
        stateMachine.Initialize(idleState);
    }

    protected override void Update() {
        base.Update();
        stateMachine.currentState.Update();

        CheckForDashInput();
    }

    private void CheckForDashInput() {
        if (IsWallDetected()) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSKill()) {
            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir == 0) {
                dashDir = facingDir;
            }

            stateMachine.ChangeState(dashState);
        }
    }

    public void AssignNewSword(GameObject _newSword) {
        sword = _newSword;
    }

    public void ClearTheSword() {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public IEnumerator BusyFor(float _seconds) {
        isBusy = true;

        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }

}

