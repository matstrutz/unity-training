using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSkillController : MonoBehaviour {

    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private bool canGrow = true;
    private bool canShrink;

    private bool canCreateHotkeys = true;
    private bool cloneAttackRelease;
    private int amountOfAttacks = 4;
    private float cloneAttackCooldown = 0.3F;
    private float cloneAttackTimer;
    private float blackHoleTimer;
    private bool playerCanDisapear = true;

    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotkey = new List<GameObject>();

    public bool playerCanExitState { get; private set; }

    public void SetupBlackHole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _cloneAttackCooldown, float _blackHoleDuration) {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackHoleTimer = _blackHoleDuration;
    }

    private void Update() {
        cloneAttackTimer -= Time.deltaTime;
        blackHoleTimer -= Time.deltaTime;

        if(blackHoleTimer < 0){
            blackHoleTimer = Mathf.Infinity;
            if(targets.Count > 0){
                ReleaseAbility();
            } else {
                FinishAbility();
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            ReleaseAbility();
        }

        CloneAttackMechanic();

        if (canGrow && !canShrink) {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink) {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0) {
                Destroy(gameObject);
            }
        }
    }

    private void ReleaseAbility() {
        if(targets.Count <= 0){
            return;
        }

        DestroyHotkeys();
        cloneAttackRelease = true;
        canCreateHotkeys = false;

        if(playerCanDisapear){
            playerCanDisapear = false;
            PlayerManager.instance.player.MakeTransparent(true);
        }
    }

    private void CloneAttackMechanic() {
        if (cloneAttackTimer < 0 && cloneAttackRelease && amountOfAttacks > 0) {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range(0, targets.Count);
            float xOffset;

            if (Random.Range(0, 100) > 50) {
                xOffset = 1.5F;
            } else {
                xOffset = -1.5F;
            }

            SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0));
            amountOfAttacks--;

            if (amountOfAttacks <= 0) {
                Invoke("FinishAbility", 0.5F);
            }
        }
    }

    private void FinishAbility() {
        DestroyHotkeys();
        playerCanExitState = true;
        canShrink = true;
        cloneAttackRelease = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<Enemy>() != null) {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotkey(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.GetComponent<Enemy>() != null) {
            collision.GetComponent<Enemy>().FreezeTime(false);
        }
    }

    private void DestroyHotkeys() {
        if (createdHotkey.Count <= 0) {
            return;
        }

        for (int i = 0; i < createdHotkey.Count; i++) {
            Destroy(createdHotkey[i]);
        }
    }

    private void CreateHotkey(Collider2D collision) {
        if (keyCodeList.Count <= 0) {
            return;
        }

        if (!canCreateHotkeys) {
            return;
        }

        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotkey.Add(newHotKey);

        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        // keyCodeList.Remove(choosenKey);

        BlackHoleHotkeyController newHotkeyScript = newHotKey.GetComponent<BlackHoleHotkeyController>();
        newHotkeyScript.SetupHotKey(choosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
