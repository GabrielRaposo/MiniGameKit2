using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pilhas
{
    public class PlayerCursor : PlayerInfo
    {
        const float X_LIMIT = 9.5f;
        const float Y_LIMIT = 5.5f;

        [SerializeField] private float movementSpeed;

        private StackableRock currentRock;
        private new Collider2D collider2D;

        override public void Start()
        {
            base.Start();

            collider2D = GetComponent<Collider2D>();
        }

        void Update()
        {
            float verticalMovement = Input.GetAxisRaw(playerButtons.vertical) * movementSpeed;
            float horizontalMovement = Input.GetAxisRaw(playerButtons.horizontal) * movementSpeed;

            if (Mathf.Abs(transform.position.y + verticalMovement) > Y_LIMIT) verticalMovement = 0;
            if (Mathf.Abs(transform.position.x + horizontalMovement) > X_LIMIT) horizontalMovement = 0;
            if(playerId == 1)
            {
                if(transform.position.x + horizontalMovement > 3) horizontalMovement = 0;
            }
            else
            {
                if (transform.position.x + horizontalMovement < -3) horizontalMovement = 0;
            }
            transform.position += Vector3.up * verticalMovement + Vector3.right * horizontalMovement;

            if (Input.GetButtonDown(playerButtons.action))
            {
                collider2D.enabled = true;
            }
            else
            {
                collider2D.enabled = false;
            }

            if (Input.GetButtonUp(playerButtons.action))
            {
                if (currentRock)
                {
                    currentRock.ReleaseTarget();
                    currentRock = null;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Item"))
            {
                StackableRock stackableRock = collision.GetComponent<StackableRock>();
                if (stackableRock && !stackableRock.taken && !currentRock)
                {
                    currentRock = stackableRock;
                    stackableRock.SetTarget(transform, playerId == 2);
                }
            }
        }
    }
}
