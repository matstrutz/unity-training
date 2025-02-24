using UnityEngine;

public class BlackHoleSkill : Skill {

    [SerializeField] private GameObject blackHolePrefab;

    [Space]
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneCooldown;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private float blackHoleDuration;

    private BlackHoleSkillController currentBlackHole;

    public override bool CanUseSKill() {
        return base.CanUseSKill();
    }

    public override void UseSkill() {
        base.UseSkill();

        GameObject newBlackHole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity);

        currentBlackHole = newBlackHole.GetComponent<BlackHoleSkillController>();

        currentBlackHole.SetupBlackHole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneCooldown, blackHoleDuration);
    }

    protected override void Start() {
        base.Start();
    }

    protected override void Update() {
        base.Update();
    }

    public bool AbilityCompleted(){
        if(!currentBlackHole){
            return false;
        }

        if(currentBlackHole.playerCanExitState){
            currentBlackHole = null;
            return true;
        }

        return false;
    }
}
