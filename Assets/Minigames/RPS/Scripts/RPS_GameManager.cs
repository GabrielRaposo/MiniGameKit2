using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RPS_GameManager : MonoBehaviour
{
    [Header("Attributes")]
    public static float healthPoints = 10f;
    public static float magicPoints = 4f;
    public static float manaRecoveryRate = .25f;


    struct Player
    {
        public float hp;
        public float mp;
        public int currentMagic;
        public float cooldown;

        public Player(bool player)
        {
            this.hp = healthPoints;
            this.mp = magicPoints;
            this.currentMagic = 1;
            this.cooldown = .75f;
        }
    }

    Player p1 = new Player(true);
    Player p2 = new Player(true);

    [Header("Player 1")]

    [SerializeField] Image p1HpBar;
    [SerializeField] Image p1HpFillBar;
    [SerializeField] Image p1MpBar;

    [Space(10)]

    [SerializeField] Transform p1Skills;

    [Space(5)]

    [SerializeField] Transform p1Rock;
    [SerializeField] Image p1RockBorder;
    [Space(5)]
    [SerializeField] Transform p1Paper;
    [SerializeField] Image p1PaperBorder;
    [Space(5)]
    [SerializeField] Transform p1Scissors;
    [SerializeField] Image p1ScissorsBorder;

    [Space(10)]

    [SerializeField] SpriteRenderer p1Sprite;
    [SerializeField] Sprite p1Idle;
    [SerializeField] Sprite p1Attack;
    [SerializeField] GameObject p1Charge;
    [SerializeField] GameObject p1AttackCharge;
    bool p1CanCast = true;
    bool p1CanSwitch = true;

    [Space(20)]

    [Header("Player 2")]

    [SerializeField] Image p2HpBar;
    [SerializeField] Image p2HpFillBar;
    [SerializeField] Image p2MpBar;
    
    [Space(10)]

    [SerializeField] Transform p2Skills;

    [Space(5)]

    [SerializeField] Transform p2Rock;
    [SerializeField] Image p2RockBorder;
    [Space(5)]
    [SerializeField] Transform p2Paper;
    [SerializeField] Image p2PaperBorder;
    [Space(5)]
    [SerializeField] Transform p2Scissors;
    [SerializeField] Image p2ScissorsBorder;

    [Space(10)]

    [SerializeField] SpriteRenderer p2Sprite;
    [SerializeField] Sprite p2Idle;
    [SerializeField] Sprite p2Attack;
    [SerializeField] GameObject p2Charge;
    [SerializeField] GameObject p2AttackCharge;
    bool p2CanCast = true;
    bool p2CanSwitch = true;

    [Header("General")]

    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed;
    [SerializeField] float cooldown;
    public Color rockColor;
    public Color paperColor;
    public Color scissorsColor;
    [Space(5)]
    [SerializeField] Camera cam;
        
    void Attack(bool isP1)
    {
        GameObject shot;
        //Camera Shake Here
        //cam.DOShakePosition(.4f, .02f, 13, 90, true);
        //print("Attack");
        if (isP1 && p1CanCast)
        {
            if (p1.mp > 1f)
            {
                //print("P1");
                p1Sprite.sprite = p1Attack;
                p1AttackCharge.SetActive(true);
                p1CanCast = false;
                //Instantiate(projectile, new Vector3(-.75f, 0, 0));
                shot = Instantiate(projectile, new Vector3(-.65f, .025f, 0), Quaternion.identity, null);
                shot.GetComponent<RPS_Projectile>().manager = this;
                shot.GetComponent<RPS_Projectile>().Shoot(1, p1.currentMagic);
                p1.mp -= 1f;
                UpdateHud(true);
                p1Charge.SetActive(false);
                StartCoroutine(Coooldown(true));
            }
            else
            {
                //print("Shake");
                switch (p1.currentMagic)
                {
                    case 0:
                        //print("Shake Rock");
                        p1Rock.DORewind();
                        p1Rock.DOShakePosition(.5f, 1.5f, 20, 90, true);
                        break;
                    case 1:
                        //print("Shake Paper");
                        p1Paper.DORewind();
                        p1Paper.DOShakePosition(.5f, 1.5f, 20, 90, true);
                        break;
                    case 2:
                        //print("Shake Scissors");
                        p1Scissors.DORewind();
                        p1Scissors.DOShakePosition(.5f, 1.5f, 20, 90, true);
                        break;
                }
            }
        }
        else if(!isP1 && p2CanCast)
        {
            if (p2.mp > 1f)
            {
                //print("P1");
                p2Sprite.sprite = p2Attack;
                p2AttackCharge.SetActive(true);
                p2CanCast = false;
                //Instantiate(projectile, new Vector3(-.75f, 0, 0));
                shot = Instantiate(projectile, new Vector3(.65f, .025f, 0), Quaternion.identity, null);
                shot.GetComponent<RPS_Projectile>().manager = this;
                shot.GetComponent<RPS_Projectile>().Shoot(-1, p2.currentMagic);
                p2.mp -= 1f;
                UpdateHud(false);
                p2Charge.SetActive(false);
                StartCoroutine(Coooldown(false));
            }
            else
            {
                //print("Shake");
                switch (p2.currentMagic)
                {
                    case 0:
                        //print("Shake Rock");
                        p2Rock.DORewind();
                        p2Rock.DOShakePosition(.5f, 1.5f, 20, 90, true);
                        break;
                    case 1:
                        //print("Shake Paper");
                        p2Paper.DORewind();
                        p2Paper.DOShakePosition(.5f, 1.5f, 20, 90, true);
                        break;
                    case 2:
                        //print("Shake Scissors");
                        p2Scissors.DORewind();
                        p2Scissors.DOShakePosition(.5f, 1.5f, 20, 90, true);
                        break;
                }
            }
        }
    }

    public void TakeDamage(bool isP1)
    {
        cam.DOShakePosition(.4f, .02f, 13, 90, true);
        if(isP1)
        {
            p1.hp -= 3f;
            UpdateHud(true);
        }
        else
        {
            p2.hp -= 3f;
            UpdateHud(false);
        }
    }

    IEnumerator Coooldown(bool isP1)
    {
        if (isP1)
        {
            p1RockBorder.fillAmount = 0;
            p1PaperBorder.fillAmount = 0;
            p1ScissorsBorder.fillAmount = 0;

            p1RockBorder.DOFillAmount(1, p1.cooldown);
            p1PaperBorder.DOFillAmount(1, p1.cooldown);
            p1ScissorsBorder.DOFillAmount(1, p1.cooldown);

            yield return new WaitForSeconds(.2f);
            p1AttackCharge.SetActive(false);
            yield return new WaitForSeconds(.2f);
            p1Sprite.sprite = p1Idle;

            yield return new WaitForSeconds(p1.cooldown - .4f);

            p1CanCast = true;
            p1Charge.SetActive(true);
        }
        else
        {
            p2RockBorder.fillAmount = 0;
            p2PaperBorder.fillAmount = 0;
            p2ScissorsBorder.fillAmount = 0;

            p2RockBorder.DOFillAmount(1, p2.cooldown);
            p2PaperBorder.DOFillAmount(1, p2.cooldown);
            p2ScissorsBorder.DOFillAmount(1, p2.cooldown);

            yield return new WaitForSeconds(.2f);
            p2AttackCharge.SetActive(false);
            yield return new WaitForSeconds(.2f);
            p2Sprite.sprite = p2Idle;

            yield return new WaitForSeconds(p2.cooldown - .4f);

            p2CanCast = true;
            p2Charge.SetActive(true);
        }
    }

    IEnumerator P1SwitchSpell(bool isUp)
    {
        p1CanCast = false;
        p1CanSwitch = false;
        if (isUp)
        {
            //p1Skills.DOLocalMoveY(p1Skills.transform.localPosition.y - 64, .25f, false);
            //yield return new WaitForSeconds(.125f);

            switch (p1.currentMagic)
            {
                case 0: //Rock -> Scissors
                    p1RockBorder.gameObject.SetActive(false);
                    //p1Skills.DOLocalMoveY(128, 0, false);
                    p1Skills.localPosition = new Vector3(32, 128, 0);
                    p1Skills.DOLocalMoveY(p1Skills.transform.localPosition.y - 64, .25f, true).SetEase(Ease.OutQuint);

                    yield return new WaitForSeconds(.125f);

                    p1ScissorsBorder.gameObject.SetActive(true);
                    p1.currentMagic = 2;
                    p1Charge.GetComponent<SpriteRenderer>().color = scissorsColor;
                    p1AttackCharge.GetComponent<SpriteRenderer>().color = scissorsColor;

                    yield return new WaitForSeconds(.125f);
                    break;

                case 1: //Paper -> Rock
                    p1PaperBorder.gameObject.SetActive(false);
                    p1Skills.DOLocalMoveY(p1Skills.transform.localPosition.y - 64, .25f, true).SetEase(Ease.OutQuint);

                    yield return new WaitForSeconds(.125f);

                    p1RockBorder.gameObject.SetActive(true);
                    p1.currentMagic = 0;
                    p1Charge.GetComponent<SpriteRenderer>().color = rockColor;
                    p1AttackCharge.GetComponent<SpriteRenderer>().color = rockColor;

                    yield return new WaitForSeconds(.125f);
                    break;

                case 2: //Scissors -> Paper
                    p1ScissorsBorder.gameObject.SetActive(false);
                    p1Skills.localPosition = new Vector3(32, 64, 0);
                    p1Skills.DOLocalMoveY(p1Skills.transform.localPosition.y - 64, .25f, true).SetEase(Ease.OutQuint);

                    yield return new WaitForSeconds(.125f);

                    p1PaperBorder.gameObject.SetActive(true);
                    p1.currentMagic = 1;
                    p1Charge.GetComponent<SpriteRenderer>().color = paperColor;
                    p1AttackCharge.GetComponent<SpriteRenderer>().color = paperColor;

                    yield return new WaitForSeconds(.125f);
                    break;
            }
            
            p1CanCast = true;
            p1CanSwitch = true;
        }
        else
        {
            //p1Skills.DOLocalMoveY(p1Skills.transform.localPosition.y - 64, .25f, false);
            //yield return new WaitForSeconds(.125f);

            switch (p1.currentMagic)
            {
                case 0: //Rock -> Paper
                    p1RockBorder.gameObject.SetActive(false);
                    //p1Skills.DOLocalMoveY(128, 0, false);
                    //p1Skills.localPosition = new Vector3(32, -128, 0);
                    p1Skills.DOLocalMoveY(p1Skills.transform.localPosition.y + 64, .25f, true).SetEase(Ease.OutQuint);

                    yield return new WaitForSeconds(.125f);

                    p1PaperBorder.gameObject.SetActive(true);
                    p1.currentMagic = 1;
                    p1Charge.GetComponent<SpriteRenderer>().color = paperColor;
                    p1AttackCharge.GetComponent<SpriteRenderer>().color = paperColor;

                    yield return new WaitForSeconds(.125f);
                    break;

                case 1: //Paper -> Scissors
                    p1PaperBorder.gameObject.SetActive(false);
                    p1Skills.DOLocalMoveY(p1Skills.transform.localPosition.y + 64, .25f, true).SetEase(Ease.OutQuint);

                    yield return new WaitForSeconds(.125f);

                    p1ScissorsBorder.gameObject.SetActive(true);
                    p1.currentMagic = 2;
                    p1Charge.GetComponent<SpriteRenderer>().color = scissorsColor;
                    p1AttackCharge.GetComponent<SpriteRenderer>().color = scissorsColor;

                    yield return new WaitForSeconds(.125f);
                    break;

                case 2: //Scissors -> Rock
                    p1ScissorsBorder.gameObject.SetActive(false);
                    p1Skills.localPosition = new Vector3(32, -128, 0);
                    p1Skills.DOLocalMoveY(p1Skills.transform.localPosition.y + 64, .25f, true).SetEase(Ease.OutQuint);

                    yield return new WaitForSeconds(.125f);

                    p1RockBorder.gameObject.SetActive(true);
                    p1.currentMagic = 0;
                    p1Charge.GetComponent<SpriteRenderer>().color = rockColor;
                    p1AttackCharge.GetComponent<SpriteRenderer>().color = rockColor;

                    yield return new WaitForSeconds(.125f);
                    break;
            }
            
            p1CanCast = true;
            p1CanSwitch = true;
        }
    }

    IEnumerator P2SwitchSpell(bool isUp)
    {
        p2CanCast = false;
        p2CanSwitch = false;
        if (isUp)
        {
            //p1Skills.DOLocalMoveY(p1Skills.transform.localPosition.y - 64, .25f, false);
            //yield return new WaitForSeconds(.125f);

            switch (p2.currentMagic)
            {
                case 0: //Rock -> Scissors
                    p2RockBorder.gameObject.SetActive(false);
                    //p1Skills.DOLocalMoveY(128, 0, false);
                    p2Skills.localPosition = new Vector3(48, 128, 0);
                    p2Skills.DOLocalMoveY(p2Skills.transform.localPosition.y - 64, .25f, true).SetEase(Ease.OutQuint);

                    yield return new WaitForSeconds(.125f);

                    p2ScissorsBorder.gameObject.SetActive(true);
                    p2.currentMagic = 2;
                    p2Charge.GetComponent<SpriteRenderer>().color = scissorsColor;
                    p2AttackCharge.GetComponent<SpriteRenderer>().color = scissorsColor;

                    yield return new WaitForSeconds(.125f);
                    break;

                case 1: //Paper -> Rock
                    p2PaperBorder.gameObject.SetActive(false);
                    p2Skills.DOLocalMoveY(p2Skills.transform.localPosition.y - 64, .25f, true).SetEase(Ease.OutQuint);

                    yield return new WaitForSeconds(.125f);

                    p2RockBorder.gameObject.SetActive(true);
                    p2.currentMagic = 0;
                    p2Charge.GetComponent<SpriteRenderer>().color = rockColor;
                    p2AttackCharge.GetComponent<SpriteRenderer>().color = rockColor;

                    yield return new WaitForSeconds(.125f);
                    break;

                case 2: //Scissors -> Paper
                    p2ScissorsBorder.gameObject.SetActive(false);
                    p2Skills.localPosition = new Vector3(48, 64, 0);
                    p2Skills.DOLocalMoveY(p2Skills.transform.localPosition.y - 64, .25f, true).SetEase(Ease.OutQuint);

                    yield return new WaitForSeconds(.125f);

                    p2PaperBorder.gameObject.SetActive(true);
                    p2.currentMagic = 1;
                    p2Charge.GetComponent<SpriteRenderer>().color = paperColor;
                    p2AttackCharge.GetComponent<SpriteRenderer>().color = paperColor;

                    yield return new WaitForSeconds(.125f);
                    break;
            }
            
            p2CanCast = true;
            p2CanSwitch = true;
        }
        else
        {
            //p1Skills.DOLocalMoveY(p1Skills.transform.localPosition.y - 64, .25f, false);
            //yield return new WaitForSeconds(.125f);

            switch (p2.currentMagic)
            {
                case 0: //Rock -> Paper
                    p2RockBorder.gameObject.SetActive(false);
                    //p1Skills.DOLocalMoveY(128, 0, false);
                    //p1Skills.localPosition = new Vector3(32, -128, 0);
                    p2Skills.DOLocalMoveY(p2Skills.transform.localPosition.y + 64, .25f, true).SetEase(Ease.OutQuint);

                    yield return new WaitForSeconds(.125f);

                    p2PaperBorder.gameObject.SetActive(true);
                    p2.currentMagic = 1;
                    p2Charge.GetComponent<SpriteRenderer>().color = paperColor;
                    p2AttackCharge.GetComponent<SpriteRenderer>().color = paperColor;

                    yield return new WaitForSeconds(.125f);
                    break;

                case 1: //Paper -> Scissors
                    p2PaperBorder.gameObject.SetActive(false);
                    p2Skills.DOLocalMoveY(p2Skills.transform.localPosition.y + 64, .25f, true).SetEase(Ease.OutQuint);

                    yield return new WaitForSeconds(.125f);

                    p2ScissorsBorder.gameObject.SetActive(true);
                    p2.currentMagic = 2;
                    p2Charge.GetComponent<SpriteRenderer>().color = scissorsColor;
                    p2AttackCharge.GetComponent<SpriteRenderer>().color = scissorsColor;

                    yield return new WaitForSeconds(.125f);
                    break;

                case 2: //Scissors -> Rock
                    p2ScissorsBorder.gameObject.SetActive(false);
                    p2Skills.localPosition = new Vector3(48, -128, 0);
                    p2Skills.DOLocalMoveY(p2Skills.transform.localPosition.y + 64, .25f, true).SetEase(Ease.OutQuint);

                    yield return new WaitForSeconds(.125f);

                    p2RockBorder.gameObject.SetActive(true);
                    p2.currentMagic = 0;
                    p2Charge.GetComponent<SpriteRenderer>().color = rockColor;
                    p2AttackCharge.GetComponent<SpriteRenderer>().color = rockColor;

                    yield return new WaitForSeconds(.125f);
                    break;
            }
            
            p2CanCast = true;
            p2CanSwitch = true;
        }
    }

    void UpdateHud(bool isP1)
    {
        //print("Update HUD");
        if (isP1)
        {
            p1HpBar.DOFillAmount(p1.hp/healthPoints, .1f);
            p1HpFillBar.DOFillAmount(p1.hp/healthPoints, .5f).SetDelay(.25f);
            p1MpBar.DOFillAmount((float)p1.mp/magicPoints, .25f);
            CheckDeath(true);
        }
        else
        {
            p2HpBar.DOFillAmount(p2.hp/healthPoints, .1f);
            p2HpFillBar.DOFillAmount(p2.hp/healthPoints, .5f).SetDelay(.25f);
            p2MpBar.DOFillAmount((float)p2.mp/magicPoints, .25f);
            CheckDeath(false);
        }
    }

    void CheckDeath(bool isP1)
    {
        if (isP1)
        {
            if (p1.hp < 0f)
            {
                p1CanCast = false;
                p1CanSwitch = false;
                p2CanCast = false;
                p2CanSwitch = false;
                print("P2 Wins!");
            }
        }
        else
        {
            if (p2.hp < 0f)
            {
                p1CanCast = false;
                p1CanSwitch = false;
                p2CanCast = false;
                p2CanSwitch = false;
                print("P1 Wins!");
            }
        }
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) Attack(true);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) Attack(false);

        if (Input.GetKeyDown(KeyCode.W) && p1CanSwitch) StartCoroutine(P1SwitchSpell(true));
        if (Input.GetKeyDown(KeyCode.S) && p1CanSwitch) StartCoroutine(P1SwitchSpell(false));

        if (Input.GetKeyDown(KeyCode.UpArrow) && p2CanSwitch) StartCoroutine(P2SwitchSpell(true));
        if (Input.GetKeyDown(KeyCode.DownArrow) && p2CanSwitch) StartCoroutine(P2SwitchSpell(false));

        //Mana Check
        if (p1.mp < magicPoints)
        {
            p1.mp += manaRecoveryRate * Time.deltaTime;
            UpdateHud(true);
        }
        else p1.mp = magicPoints;

        if (p2.mp < magicPoints)
        {
            p2.mp += manaRecoveryRate * Time.deltaTime;
            UpdateHud(false);
        }
        else p2.mp = magicPoints;
    }

}
