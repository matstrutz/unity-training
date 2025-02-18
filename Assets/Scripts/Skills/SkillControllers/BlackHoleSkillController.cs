using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSkillController : MonoBehaviour {

    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    public float maxSize;
    public float growSpeed;
    public bool canGrow;

    private List<Transform> targets = new List<Transform>();

    private void Update() {
        if(canGrow){
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.GetComponent<Enemy>() != null) {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotkey(collision);
        }
    }

    private void CreateHotkey(Collider2D collision) {
        if(keyCodeList.Count <= 0){
            return;
        }

        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);

        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        // keyCodeList.Remove(choosenKey);

        BlackHoleHotkeyController newHotkeyScript = newHotKey.GetComponent<BlackHoleHotkeyController>();
        newHotkeyScript.SetupHotKey(choosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
