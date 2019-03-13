using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPS_Projectile : MonoBehaviour
{
    int dir;
    public int element;
    [SerializeField] float speed;

    public RPS_GameManager manager;

    private void Start()
    {
        //manager = GameObject.FindWithTag("GameController").GetComponent<RPS_GameManager>();
    }

    public void Shoot(int direction, int type)
    {
        SpriteRenderer spr = GetComponentInChildren<SpriteRenderer>();

        dir = direction;
        element = type;

        if(dir == -1)
        {
            transform.GetChild(0).transform.eulerAngles = new Vector3(0,0,180);
        }

        switch (type)
        {
            case 0:
                spr.color = manager.rockColor;
                break;
            case 1:
                spr.color = manager.paperColor;
                break;
            case 2:
                spr.color = manager.scissorsColor;
                break;
        }
    }

    private void Update()
    {
        transform.position += Vector3.right * dir * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (dir == -1) manager.TakeDamage(true);
            else manager.TakeDamage(false);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Projectile")
        {
            int otherElement = collision.gameObject.GetComponent<RPS_Projectile>().element;
            if (element == otherElement)
            {
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
            else
            {
                print(element);
                if(element == 0 && otherElement == 2) Destroy(collision.gameObject);
                else if (element > otherElement && !(element==2 && otherElement ==0)) Destroy(collision.gameObject);
            }
        }

    }
}