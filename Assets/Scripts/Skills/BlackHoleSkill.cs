using UnityEngine;

public class BlackHoleSkill : Skill {

    [SerializeField] private GameObject blackHolePrefab;

    [Space]
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneCooldown;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;

    public override bool CanUseSKill() {
        return base.CanUseSKill();
    }

    public override void UseSkill() {
        base.UseSkill();

        GameObject newBlackHole = Instantiate(blackHolePrefab);

        BlackHoleSkillController newScript = newBlackHole.GetComponent<BlackHoleSkillController>();

        newScript.SetupBlackHole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneCooldown);
    }

    protected override void Start() {
        base.Start();
    }

    protected override void Update() {
        base.Update();
    }
}
