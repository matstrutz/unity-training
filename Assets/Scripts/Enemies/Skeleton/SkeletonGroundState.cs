using UnityEngine;

public class SkeletonGroundState : EnemyState {
    protected EnemySkeleton enemy;
    protected Transform player;
    
    public int groundDistanceCheck = 2;

    public SkeletonGroundState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName) {
        enemy = _enemy;
    }

    public override void Enter() {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }

    public override void Exit() {
        base.Exit();
    }

    public override void Update() {
        base.Update();

        if(enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < groundDistanceCheck){
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
