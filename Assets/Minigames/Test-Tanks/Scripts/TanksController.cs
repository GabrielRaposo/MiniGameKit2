using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestTanks
{
    public class TanksController : MonoBehaviour
    {
        private float speed = 3.45f;
        private float rangeLimit = 135f;
        
        public GameObject bulletPrefab;
        void Update()
        {
            Movement();
            Shoot();
        }

        private void Shoot()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var go = Instantiate(bulletPrefab, transform.position, transform.rotation);
                go.GetComponent<Bullet>().Init(this);
            }
        }

        public void Die()
        {
            Debug.Log("Die()");
            Destroy(gameObject);
        }

            private void Movement()
        {
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.down * speed);
            }

            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.up * speed);
            }

            var position = transform.position;
            if (transform.position.y > rangeLimit)
            {
                position = new Vector3(position.x, rangeLimit, position.z);
                transform.position = position;
            }

            if (transform.position.y < -rangeLimit)
            {
                position = new Vector3(position.x, -rangeLimit, position.z);
                transform.position = position;
            }
        }
    }
}


