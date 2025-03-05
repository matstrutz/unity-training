using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill {
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;

    [Header("Crystal Explode")]
    [SerializeField] private bool canExplode;
    [SerializeField] private float growSpeed = 5;

    [Header("Crystal Move")]
    [SerializeField] private bool canMove;
    [SerializeField] private float moveSpeed;

    //TODO FIX COOLDOWN OF MULTICRYSTAL AND CHECK COOLDOWN IN GENERAL
    [Header("Multi Crystal")]
    [SerializeField] private bool canMultiCrystal;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    private List<GameObject> crystalLeft = new List<GameObject>();

    [Header("Crystal Mirage")]
    [SerializeField] private bool canClone;

    protected override void Start() {
        base.Start();

        InitMultiCrystal();
    }

    public override void UseSkill() {
        base.UseSkill();
        if (CanUseMultiCrystal()) {
            return;
        }

        if (currentCrystal == null) {
            CreateCrystal();
        } else {
            if (canMove) {
                return;
            }
            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if(canClone){
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            } else {
                currentCrystal.GetComponent<CrystalSkillControler>()?.FinishCrystal();
            }
        }
    }

    public void CreateCrystal() {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        CrystalSkillControler currentCrystalScript = currentCrystal.GetComponent<CrystalSkillControler>();
        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMove, moveSpeed, growSpeed, FindClosestEnemy(currentCrystal.transform));
        currentCrystalScript.ChooseRandomEnemy();
    }

    public void CrystalChooseRandomTarget() => currentCrystal.GetComponent<CrystalSkillControler>().ChooseRandomEnemy();

    private bool CanUseMultiCrystal() {
        if (canMultiCrystal) {
            if (crystalLeft.Count > 0) {

                if (crystalLeft.Count == amountOfStacks) {
                    Invoke(nameof(ResetAbility), useTimeWindow);
                }

                cooldown = 0;
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);

                crystalLeft.Remove(crystalToSpawn);
                newCrystal.GetComponent<CrystalSkillControler>().SetupCrystal(crystalDuration, canExplode, canMove, moveSpeed, growSpeed, FindClosestEnemy(newCrystal.transform));

                if (crystalLeft.Count <= 0) {
                    cooldown = multiStackCooldown;
                    RefilCrystal();
                }

                return true;
            }
        }

        return false;
    }

    private void RefilCrystal() {
        int amountToAdd = amountOfStacks - crystalLeft.Count;
        for (int i = 0; i < amountToAdd; i++) {
            crystalLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility() {
        if (cooldownTimer > 0) {
            return;
        }
        cooldownTimer = multiStackCooldown;
        RefilCrystal();
    }

    private void InitMultiCrystal() {
        for (int i = 0; i < amountOfStacks; i++) {
            crystalLeft.Add(crystalPrefab);
        }
    }
}
