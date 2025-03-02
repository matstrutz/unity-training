using UnityEngine;

public class CrystalSkillControler : MonoBehaviour {
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();

    private float crystalExitTimer;
    private bool canExplode;
    private bool canMove;
    private float moveSpeed;

    private bool canGrow;
    private float growSpeed;

    private Transform closestEnemy;
    
    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed, float _growSpeed, Transform _closestEnemy){
        crystalExitTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        growSpeed = _growSpeed;
        closestEnemy = _closestEnemy;
    }

    private void Update() {
        crystalExitTimer -= Time.deltaTime;
        if(crystalExitTimer < 0) {
            FinishCrystal();
        }

        if(canMove){
            transform.position = Vector2.MoveTowards(transform.position, closestEnemy.position, moveSpeed * Time.deltaTime);
            if(Vector2.Distance(transform.position, closestEnemy.position) < 1){
                FinishCrystal();
                canMove = false;
            }
        }

        if(canGrow){
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3,3), growSpeed * Time.deltaTime);
        }
    }

    public void FinishCrystal() {
        if (canExplode) {
            canGrow = true;
            anim.SetTrigger("Explode");
        } else {
            SelfDestroy();
        }
    }

    private void AnimationExplodeEvent(){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach(var hit in colliders){
            if(hit.GetComponent<Enemy>() != null){
                hit.GetComponent<Enemy>().Damage();
            }
        }
    }

    public void SelfDestroy() => Destroy(gameObject);
}
