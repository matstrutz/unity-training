using UnityEngine;

public class SkeletonStunState : EnemyState {

    private EnemySkeleton enemy;

    public SkeletonStunState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName) {
        enemy = _enemy;
    }

    public override void Enter() {
        base.Enter();

        enemy.fx.InvokeRepeating("RedColorBlink", 0, 0.1F);

        stateTimer = enemy.stunDuration;

        rb.linearVelocity = new Vector2(-enemy.facingDir * enemy.stunDirection.x, enemy.stunDirection.y);
    }

    public override void Exit() {
        base.Exit();

        enemy.fx.Invoke("CancelRedColorBlink", 0);
    }

    public override void Update() {
        base.Update();

        if(stateTimer < 0){
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
