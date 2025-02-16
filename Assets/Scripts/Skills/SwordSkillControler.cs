using System;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillControler : MonoBehaviour {

    [SerializeField] private float returnSpeed = 12F;
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;
    private bool isReturning;

    private bool canRotate = true;

    private float freezedTimeDuration = 0.7F;

    [Header("Bounce Info")]
    [SerializeField] private float bounceSpeed = 20;
    private bool isBouncing;
    private int bounceAmount;
    private List<Transform> enemyTarget;
    private int targetIndex;

    [Header("Pierce Info")]
    private float pierceAmount = 20;

    [Header("Spin Info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;

    private float hitTimer;
    private float hitCooldown;

    private float spinDirection;
    private float timeForSwordDestroy = 8F;


    private void Awake() {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    private void Update() {
        if (canRotate) {
            transform.right = rb.linearVelocity;
        }

        if (isReturning) {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            anim.SetBool("Rotation", true);

            if (Vector2.Distance(transform.position, player.transform.position) < 1) {
                player.ClearTheSword();
            }
        }

        BounceMechanic();
        SpinMechanic();
    }

    private void SpinMechanic() {
        if (isSpinning) {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped) {
                StopWhenHit();
            }

            if (wasStopped) {
                spinTimer -= Time.deltaTime;

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5F * Time.deltaTime);

                if (spinTimer < 0) {
                    isReturning = true;
                    isSpinning = false;
                }

                if (hitTimer < 0) {
                    hitTimer = hitCooldown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (var hit in colliders) {
                        if (hit.GetComponent<Enemy>() != null) {
                            SwordSkillDamage(hit.GetComponent<Enemy>());
                        }
                    }
                }
            }
        }
    }

    private void StopWhenHit() {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    public void ReturnSword() {
        anim.SetBool("Rotation", false);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturning = true;
    }

    public void DestroySword(){
        Destroy(gameObject);
    }

    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player, float _freezedTimeDuration) {
        freezedTimeDuration = _freezedTimeDuration;
        player = _player;
        rb.linearVelocity = _dir;
        rb.gravityScale = _gravityScale;

        spinDirection = Mathf.Clamp(rb.linearVelocity.x, -1, 1);

        anim.SetBool("Rotation", true);

        Invoke("DestroySword", timeForSwordDestroy);
    }

    public void SetupBounce(bool _isBouncing, int _bounceAmount) {
        isBouncing = _isBouncing;
        bounceAmount = _bounceAmount;

        enemyTarget = new List<Transform>();
    }

    public void SetupPierce(int _pierceAmount) {
        pierceAmount = _pierceAmount;
    }

    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown) {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }

    private void BounceMechanic() {
        if (isBouncing && enemyTarget.Count > 0) {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < 0.1F) {
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());
                targetIndex++;
                bounceAmount--;

                if (bounceAmount < 0) {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTarget.Count) {
                    targetIndex = 0;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (isReturning) {
            return;
        }

        if(collision.GetComponent<Enemy>() != null) {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }

        collision.GetComponent<Enemy>()?.Damage();
        BounceCalcTarget(collision);

        StuckInto(collision);
    }

    private void SwordSkillDamage(Enemy enemy) {
        enemy.Damage();
        enemy.StartCoroutine("FreezeTimeFor", freezedTimeDuration);
    }

    private void BounceCalcTarget(Collider2D collision) {
        if (collision.GetComponent<Enemy>() != null) {
            if (isBouncing && enemyTarget.Count <= 0) {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders) {
                    if (hit.GetComponent<Enemy>() != null) {
                        enemyTarget.Add(hit.transform);
                    }
                }
            }
        }
    }

    private void StuckInto(Collider2D collision) {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null) {
            pierceAmount--;
            return;
        }

        if (isSpinning) {
            StopWhenHit();
            return;
        }

        canRotate = false;
        cd.enabled = false;

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0) {
            return;
        }

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}
