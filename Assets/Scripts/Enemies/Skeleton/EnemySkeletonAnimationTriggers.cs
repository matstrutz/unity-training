using UnityEngine;

public class EnemySkeletonAnimationTriggers : MonoBehaviour {

    private EnemySkeleton enemy => GetComponentInParent<EnemySkeleton>();

    private void AnimationTriggers() {
        enemy.AnimationFinishTriggers();
    }

    private void AttackTrigger() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in colliders) {
            if (hit.GetComponent<Player>() != null) {
                hit.GetComponent<Player>().Damage();
            }
        }
    }

    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();

}
