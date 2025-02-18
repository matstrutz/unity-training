using TMPro;
using UnityEngine;

public class BlackHoleHotkeyController : MonoBehaviour {

    private SpriteRenderer sr;
    private KeyCode hotKey;
    private TextMeshProUGUI text;
    private Transform enemy;
    private BlackHoleSkillController blackHoleController;

    void Update() {
        if(Input.GetKeyDown(hotKey)){
            blackHoleController.AddEnemyToList(enemy);

            text.color = Color.clear;
            sr.color = Color.clear;
        }
    }

    public void SetupHotKey(KeyCode _hotKey, Transform _enemy, BlackHoleSkillController _blackHoleController){
        sr = GetComponent<SpriteRenderer>();
        text = GetComponentInChildren<TextMeshProUGUI>();

        enemy = _enemy;
        blackHoleController = _blackHoleController;

        hotKey = _hotKey;
        text.text = _hotKey.ToString();
    }
}
