using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestTanks
{
    public class Bullet : MonoBehaviour
    {
        private float time;
        private float speed = 5f;
        private TanksController ownerTank;

        public void Init(TanksController tc)
        {
            ownerTank = tc;
        }
        void Update()
        {
            transform.Translate(Vector3.right*speed);
            SelfDestroy();
        }

        private void SelfDestroy()
        {
            time += Time.deltaTime;
            if (time > 5f)
                Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("OnTriggerEnter2D");
            var tank = other.gameObject.GetComponent<TanksController>();
            if(tank == null) return;
            if(tank == ownerTank) return;
            
            tank.Die();
        }

    }    
}


