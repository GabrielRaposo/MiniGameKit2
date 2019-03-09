using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pilhas
{
    public class StackableRock : MonoBehaviour
    {
        public bool taken;

        private Transform target; 

        private Rigidbody2D rb2D;

        void Start()
        {
            rb2D = GetComponent<Rigidbody2D>();
        }

        public void SetTarget (Transform target, bool side)
        {
            this.target = target;
            taken = true;
        }

        public void ReleaseTarget()
        {
            if (target)
            {
                target = null;
                taken = false;
                rb2D.velocity = Vector2.zero;
            }
        }

        void Update()
        {
            if (target)
            {
                //Vector2 movement = (target.position - transform.position);
                //rb2D.velocity += movement;
                transform.position = target.position;
            }
        }
    }

}
