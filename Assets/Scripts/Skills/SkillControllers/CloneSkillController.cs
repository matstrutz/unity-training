using UnityEngine;

public class CloneSkillController : MonoBehaviour {

    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorLoosingSpeed;
    private float cloneTimer;

    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = 0.8F;
    private Transform closestEnemy;
    private bool canDuplicateClone;
    private int facingDir = 1;
    private float duplicateCloneChance;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update() {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0) {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLoosingSpeed));

            if(sr.color.a < 0){
                Destroy(gameObject);
            }
        }
    }

    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack, Vector3 _offset, Transform _closestEnemy, bool _canDuplicateClone, float _duplicateCloneChance) {
        if (_canAttack) {
            anim.SetInteger("AttackNumber", UnityEngine.Random.Range(1, 3));
        }

        transform.position = _newTransform.position + _offset;
        closestEnemy = _closestEnemy;
        cloneTimer = _cloneDuration;
        canDuplicateClone = _canDuplicateClone;
        duplicateCloneChance = _duplicateCloneChance;

        FaceClosestTarget();
    }

    private void AnimationTrigger(){
        cloneTimer = -0.1F;
    }

    private void AttackTrigger(){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach(var hit in colliders){
            if(hit.GetComponent<Enemy>() != null){
                hit.GetComponent<Enemy>().Damage();

                if(canDuplicateClone){
                    if(Random.Range(0,100) < duplicateCloneChance){
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(0.5F * facingDir, 0));
                    }
                }
            }
        }
    }

    private void FaceClosestTarget(){
        if(closestEnemy != null){
            if(transform.position.x > closestEnemy.position.x){
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
