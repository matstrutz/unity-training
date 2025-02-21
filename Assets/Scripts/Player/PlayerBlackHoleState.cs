using UnityEngine;

public class PlayerBlackHoleState : PlayerState {

    private float flyTime = 0.4F;
    private bool skillUsed;
    private float defaultGravity;

    public PlayerBlackHoleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName) {
    }

    public override void AnimationFinishTrigger() {
        base.AnimationFinishTrigger();
    }

    public override void Enter() {
        base.Enter();

        defaultGravity = player.rb.gravityScale;

        skillUsed = false;
        stateTimer = flyTime;
        rb.gravityScale = 0;
    }

    public override void Exit() {
        base.Exit();

        player.rb.gravityScale = defaultGravity;
        player.MakeTransparent(false);
    }

    public override void Update() {
        base.Update();

        if (stateTimer > 0) {
            rb.linearVelocity = new Vector2(0, 15);
        }

        if (stateTimer < 0) {
            rb.linearVelocity = new Vector2(0, -0.1F);

            if (!skillUsed) {
                if (player.skill.blackHole.CanUseSKill()){
                    skillUsed = true;
                }
            }
        }
    }
}
