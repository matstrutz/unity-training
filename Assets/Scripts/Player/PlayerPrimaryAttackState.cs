using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState {

    private int comboCounter;
    private float lastTimeAttacked;
    private float comboWindow = 2;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName) {
    }

    public override void Enter() {
        base.Enter();
        xInput = 0; //AttackDiretion Bug Fix

        if(comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow){
            comboCounter = 0;
        }

        player.anim.SetInteger("ComboCounter", comboCounter);

        float attackDir = player.facingDir;

        if(xInput != 0){
            attackDir = xInput;
        }

        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);

        stateTimer = 0.1F;
    }

    public override void Exit() {
        base.Exit();

        player.StartCoroutine("BusyFor", 0.15F);

        comboCounter++;
        lastTimeAttacked = Time.time;
    }

    public override void Update() {
        base.Update();

        if (stateTimer < 0){
            player.ResetVelocity();
        }

        if(triggetCalled){
            stateMachine.ChangeState(player.idleState);
        }
    }
}
