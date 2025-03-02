using UnityEngine;

public class PlayerCounterAttackState : PlayerState {
    private bool canCreateClone;

    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName) {
    }

    public override void Enter() {
        base.Enter();
        canCreateClone = true;
        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuccessfulCounter", false);
    }

    public override void Exit() {
        base.Exit();
    }

    public override void Update() {
        base.Update();

        player.ResetVelocity();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders) {
            if (hit.GetComponent<Enemy>() != null) {
                if (hit.GetComponent<Enemy>().CanBeStunned()) {
                    stateTimer = 10; //Any Value Bigger than 1
                    player.anim.SetBool("SuccessfulCounter", true);
                    if (canCreateClone) {
                        player.skill.clone.CreateCloneOnCounter(hit.transform);
                        canCreateClone = false;
                    }
                }
            }
        }

        if (stateTimer < 0 || triggetCalled) {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
