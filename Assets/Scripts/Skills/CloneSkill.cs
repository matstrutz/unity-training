using System.Collections;
using UnityEngine;

public class CloneSkill : Skill {

    [Header("Clone Info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [Space]
    [SerializeField] private bool canAttack;
    [SerializeField] private float duplicateCloneChance = 10F;

    [Header("Skill Tree")]
    [SerializeField] private bool canCreateOnDashStart;
    [SerializeField] private bool canCreateOnDashEnd;
    [SerializeField] private bool canCreateOnCounter;
    [SerializeField] private bool canDuplicateClone;

    [Header("Crystal Info")]
    public bool canCrystal;

    public void CreateClone(Transform _clonePosition, Vector3 _offset) {
        if(canCrystal){
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<CloneSkillController>().SetupClone(_clonePosition, cloneDuration, canAttack, _offset, FindClosestEnemy(newClone.transform), canDuplicateClone, duplicateCloneChance);
    }

    public void CreateCloneOnDashStart(){
        if(canCreateOnDashStart){
            CreateClone(player.transform, Vector3.zero);
        }
    }

    public void CreateCloneOnDashEnd(){
        if(canCreateOnDashEnd){
            CreateClone(player.transform, Vector3.zero);
        }
    }

    public void CreateCloneOnCounter(Transform _transform){
        if(canCreateOnCounter){
            StartCoroutine(CreateCloneWithDelay(_transform, new Vector3(2 * player.facingDir, 0)));
        }
    }

    private IEnumerator CreateCloneWithDelay(Transform _transform, Vector3 _offset){
        yield return new WaitForSeconds(0.4F); 
        CreateClone(_transform, _offset);
    }
}
