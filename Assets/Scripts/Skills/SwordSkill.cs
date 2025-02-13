using System;
using UnityEngine;

public enum SwordType {
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class SwordSkill : Skill {

    public SwordType swordType = SwordType.Regular;

    [Header("Bounce Info")]
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;

    [Header("Pierce Info")]
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;

    [Header("Spin Info")]
    [SerializeField] private float hitCooldown = 0.35F;
    [SerializeField] private float maxTravelDistance = 7;
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float spinGravity = 1;

    [Header("Skill Info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;

    [Header("Aim Dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBeetwenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private Vector2 finalDir;

    private GameObject[] dots;

    protected override void Start() {
        base.Start();

        GenerateDots();

        SetupGravity();
    }

    private void SetupGravity() {
        if(swordType == SwordType.Bounce){
            swordGravity = bounceGravity;
        } else if (swordType == SwordType.Pierce){
            swordGravity = pierceGravity;
        } else if (swordType == SwordType.Spin){
            swordGravity = spinGravity;
        }
    }

    protected override void Update() {
        if (Input.GetKeyUp(KeyCode.Mouse1)) {
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);
        }

        if (Input.GetKey(KeyCode.Mouse1)) {
            for (int i = 0; i < dots.Length; i++) {
                dots[i].transform.position = DotsPosition(i * spaceBeetwenDots);
            }
        }
    }

    public void CreateSword() {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        SwordSkillControler newSwordScript = newSword.GetComponent<SwordSkillControler>();

        if(swordType == SwordType.Bounce){
            newSwordScript.SetupBounce(true, bounceAmount);
        } else if(swordType == SwordType.Pierce){
            newSwordScript.SetupPierce(pierceAmount);
        } else if(swordType == SwordType.Spin){
            newSwordScript.SetupSpin(true, maxTravelDistance, spinDuration, hitCooldown);
        }

        newSwordScript.SetupSword(finalDir, swordGravity, player);

        player.AssignNewSword(newSword);
        DotsActive(false);
    }

    #region Aiming
    public Vector2 AimDirection() {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }

    public void DotsActive(bool _isActive) {
        for (int i = 0; i < dots.Length; i++) {
            dots[i].SetActive(_isActive);
        }
    }

    private void GenerateDots() {
        dots = new GameObject[numberOfDots];

        for (int i = 0; i < numberOfDots; i++) {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t) {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + t * t * 0.5F * (Physics2D.gravity * swordGravity);

        return position;
    }
    #endregion
}
