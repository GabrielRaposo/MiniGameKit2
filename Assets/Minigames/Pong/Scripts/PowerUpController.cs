using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pong
{
    public enum PowerUpType
    {
        FIRE,
        ICE
    }

    public class PowerUpController : MonoBehaviour
    {
        private Collider2D myCollider;
        private Collider2D ballCollider;

        private PowerUpType type;
        private GameManager gameManager;

        [SerializeField] private BolaController ball = null;
        [SerializeField] private ScriptFundoFogo fundoFogo = null;
        [SerializeField] private HalfFireWall paredeFogoEsquerda = null;
        [SerializeField] private HalfFireWall paredeFogoDireita = null;
        [SerializeField] private ScriptFundoGelo fundoGelo = null;
        [SerializeField] private ShatteredIceWall pedacosGeloEsquerda = null;
        [SerializeField] private ShatteredIceWall pedacosGeloDireita = null;
        [SerializeField] private Material paredeFogoMaterial = null;
        [SerializeField] private AudioClip startFireSound = null;
        [SerializeField] private AudioClip startIceSound = null;
        [SerializeField] private AudioClip spreadFireSound = null;
        [SerializeField] private AudioClip spreadIceSound = null;
        [SerializeField] private AudioClip iceBreakSound = null;

        private void Start()
        {
            myCollider = GetComponent<Collider2D>();
            ballCollider = (ball != null) ? ball.GetComponent<Collider2D>() : null;
            gameManager = FindObjectOfType<GameManager>();

            if (type == PowerUpType.FIRE)
            {
                GetComponent<SpriteRenderer>().material = paredeFogoMaterial;
                gameObject.GetComponent<ScriptFireWall>().enabled = true;
                GetComponent<Animator>().SetInteger("PowerUpType", (int)type);

                //Som
                AudioSource.PlayClipAtPoint(startFireSound, Vector3.zero);
            }

            else if (type == PowerUpType.ICE)
            {
                GetComponent<Animator>().SetInteger("PowerUpType", (int)type);

                //Som
                AudioSource.PlayClipAtPoint(startIceSound, Vector3.zero);
            }
        }


        private void Update()
        {
            //ativa efeito do power up
            if (ballCollider != null)
            {
                if (CustomFuncs.MyIntersects(myCollider.bounds, ballCollider.bounds))
                {
                    gameManager.ActivateEffect(type);
                    Destroy(gameObject);

                    //Animações
                    if (type == PowerUpType.FIRE)
                    {
                        fundoFogo.gameObject.SetActive(true);
                        Instantiate(paredeFogoEsquerda, transform.position, Quaternion.identity);
                        Instantiate(paredeFogoDireita, transform.position, Quaternion.identity);

                        //Som
                        AudioSource.PlayClipAtPoint(spreadFireSound, Vector3.zero);
                    }

                    if (type == PowerUpType.ICE)
                    {
                        fundoGelo.gameObject.SetActive(true);
                        Instantiate(pedacosGeloDireita, transform.position, Quaternion.identity);
                        Instantiate(pedacosGeloEsquerda, transform.position, Quaternion.identity);

                        //Som
                        AudioSource.PlayClipAtPoint(iceBreakSound, Vector3.zero);
                        AudioSource.PlayClipAtPoint(spreadIceSound, Vector3.zero);
                    }
                }
            }
        }

        /// <summary>
        /// Ativa o objeto do power up e atribui um dado tipo para ele
        /// </summary>
        public void TurnOn(PowerUpType powerType)
        {
            gameObject.SetActive(true);
            type = powerType;
        }
    }
}
