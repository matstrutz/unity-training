using UnityEngine;

public class PlayerStats : EntityStats {
    private Player player;

    protected override void Start() {
        base.Start();

        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage) {
        base.TakeDamage(_damage);

        player.Damage();
    }

    protected override void Die() {
        base.Die();

        player.Die();
    }
}
