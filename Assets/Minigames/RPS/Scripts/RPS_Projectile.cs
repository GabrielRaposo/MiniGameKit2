using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPS_Projectile : MonoBehaviour
{
    int dir;
    int element;
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
        print(collision.gameObject.name);
        manager.TakeDamage(true);
        Destroy(gameObject);
    }
}
