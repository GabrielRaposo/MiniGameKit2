using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    struct Player
    {
        public int hp;
        public int mp;
        public int currentMagic;
        public float cooldown;

        public Player(bool player)
        {
            this.hp = 20;
            this.mp = 4;
            this.currentMagic = 1;
            this.cooldown = 1f;
        }
    }

    Player p1 = new Player(true);
    Player p2 = new Player(true);

    [Header("Player 1")]

    [SerializeField] Image p1HpBar;
    [SerializeField] Image p1MpBar;
    
    [Space(10)]

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

    [Space(20)]

    [Header("Player 2")]

    [SerializeField] Image p2HpBar;
    [SerializeField] Image p2MpBar;
    
    [Space(10)]

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

    [Header("General")]

    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed;
    [SerializeField] float cooldown;
    [SerializeField] Color rockColor;
    [SerializeField] Color paperColor;
    [SerializeField] Color scissorsColor;
        
    void Attack(bool isP1)
    {
        print("Attack");
        if(isP1 && p1CanCast && p1.mp > 0)
        {
            print("P1");
            p1Sprite.sprite = p1Attack;
            p1AttackCharge.SetActive(true);
            p1CanCast = false;
            //Instantiate(projectile, new Vector3(-.75f, 0, 0));
            p1.mp--;
            UpdateHud(true);
            p1Charge.SetActive(false);
            StartCoroutine(Coooldown(true));
        }
        else if(!isP1 && p2CanCast && p2.mp > 0)
        {
            print("P2");
            p2Sprite.sprite = p2Attack;
            p2AttackCharge.SetActive(true);
            p2CanCast = false;
            //Instantiate(projectile, new Vector3(.75f, 0, 0));
            p2.mp--;
            UpdateHud(false);
            p2Charge.SetActive(false);
            StartCoroutine(Coooldown(false));
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

    void UpdateHud(bool isP1)
    {
        print("Update HUD");
        if (isP1)
        {
            p1HpBar.DOFillAmount(p1.hp/20f, .25f);
            p1MpBar.DOFillAmount((float)p1.mp/4f, .25f);
        }
        else
        {
            p2HpBar.DOFillAmount(p2.hp/20f, .25f);
            p2MpBar.DOFillAmount((float)p2.mp/4f, .25f);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //print("Attack");
            Attack(true);
        }
    }

}
