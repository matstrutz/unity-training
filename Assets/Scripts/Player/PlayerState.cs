using UnityEngine;

public class PlayerState {
    protected PlayerStateMachine stateMachine;
    protected Player player;

    protected Rigidbody2D rb;

    protected float xInput;
    protected float yInput;
    private string animBoolName;

    protected float stateTimer;
    protected bool triggetCalled;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) {
        player = _player;
        stateMachine = _stateMachine;
        animBoolName = _animBoolName;
    }

    public virtual void Enter() {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        triggetCalled = false;
    }

    public virtual void Update() {
        stateTimer -= Time.deltaTime;

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        player.anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    public virtual void Exit() {
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger(){
        triggetCalled = true;
    }
}