using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pong
{
    public class ShatteredIceWall : MonoBehaviour
    {
        [SerializeField] private float travelDistance = 1;
        [SerializeField] private float animationInterval = 0.5f;
        [SerializeField] private Sprite secondSprite = null;
        [SerializeField] private Lado screenSide = Lado.ESQ;

        private void Start()
        {
            StartCoroutine(MoveAndDestroy());
        }

        IEnumerator MoveAndDestroy()
        {
            yield return new WaitForSeconds(animationInterval);

            GetComponent<SpriteRenderer>().sprite = secondSprite;
            int side = screenSide == Lado.DIR ? 1 : -1;
            transform.position += new Vector3(side * travelDistance, 0, 0);

            yield return new WaitForSeconds(animationInterval);

            Destroy(gameObject);
        }
    }
}
