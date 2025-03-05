using UnityEngine;

public class SkeletonDeathState : EnemyState {
    private EnemySkeleton enemy;
    public SkeletonDeathState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton enemy) : base(_enemyBase, _stateMachine, _animBoolName) {
        this.enemy = enemy;
    }

    public override void Enter() {
        base.Enter();

        enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        enemy.anim.speed = 0;
        enemy.cd.enabled = false;

        stateTimer = 0.1F;
    }

    public override void Update() {
        base.Update();

        if(stateTimer > 0){
            rb.linearVelocity = new Vector2(0, 10);
        }
    }
}
