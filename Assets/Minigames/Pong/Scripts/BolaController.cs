using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pong
{
    public class BolaController : MonoBehaviour
    {
        private const float padTransmission = 0.3f; //elasticidade da colisão entre raquete e bola
        private const float padBumping = 1f; //horizontal speed added by the pad to the ball;
        private const float initialTerminalSpeed = 15f;
        private const float fireTerminalSpeed = 25f;
        private const float fireSpeedIncrement = 5f;

        private float minY;
        private float maxY;
        private float terminalSpeed = initialTerminalSpeed;  //velocidade maxima da bola
        private GameManager gameManager;
        private Collider2D myCollider;
        private List<Collider2D> padColliders = new List<Collider2D>();
        [SerializeField] private Color fireColor = Color.white;
        [SerializeField] private AudioClip normalBounceSound = null;
        [SerializeField] private AudioClip fireBounceSound = null;
        [SerializeField] private AudioClip iceBounceSound = null;
        [SerializeField] private AudioClip pointSound = null;

        public Vector3 velocity { get; private set; }

        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
            myCollider = GetComponent<Collider2D>();

            foreach (Collider2D collider in FindObjectsOfType<Collider2D>())
            {
                if (collider.gameObject.GetComponent<BarraController>() != null)
                {
                    padColliders.Add(collider);
                }
            }

            //calcula minY e maxY
            Sprite sprite = GetComponent<SpriteRenderer>().sprite;
            maxY = Camera.main.orthographicSize - sprite.bounds.extents.y * transform.localScale.y;
            minY = -Camera.main.orthographicSize + sprite.bounds.extents.y * transform.localScale.y;

            //Joga bola
            velocity = (CustomFuncs.RandomBool()) ? Vector3.left * terminalSpeed * 0.5f : Vector3.right * terminalSpeed * 0.5f;
        }

        private void Update()
        {
            //Termina partida se bola sair do campo e declara vencedor, terminando a partida
            Vector3 point = Camera.main.WorldToViewportPoint(transform.position);
            if (point.x < 0 || point.x > 1)
            {
                if (point.x < 0)
                {
                    gameManager.MatchEnd(Lado.DIR);

                }
                else
                {
                    gameManager.MatchEnd(Lado.ESQ);

                }

                Destroy(gameObject);

                //Som
                AudioSource.PlayClipAtPoint(pointSound, Vector3.zero);
            }
        }

        private void FixedUpdate()
        {

            bool collisionHappened = false;
            Vector3 newPosition = transform.position + velocity * Time.fixedDeltaTime;

            //Reflexão nas paredes
            if (newPosition.y > maxY)
            {
                //Torna colisão pixel perfect
                Vector3 diffVect = newPosition - transform.position;
                transform.position += diffVect * Mathf.Abs((maxY - transform.position.y) / diffVect.y);

                velocity = Vector3.Reflect(velocity, Vector3.down);

                collisionHappened = true;
            }
            if (newPosition.y < minY)
            {
                //Torna colisão pixel perfect
                Vector3 diffVect = newPosition - transform.position;
                transform.position += diffVect * Mathf.Abs((minY - transform.position.y) / diffVect.y);

                velocity = Vector3.Reflect(velocity, Vector3.up);

                collisionHappened = true;
            }
            //Reflexão nas raquetes
            foreach (Collider2D collider in padColliders)
            {
                Bounds newBounds = new Bounds(newPosition, myCollider.bounds.size);

                if (CustomFuncs.MyIntersects(newBounds, collider.bounds))
                {

                    BarraController paddle = collider.gameObject.GetComponent<BarraController>();

                    //Calcula a maior (ou menor) posição x que a bola chegar na qual a raquete ainda possa salvar o ponto
                    Vector3 diffVect = newPosition - transform.position;
                    int paddleSide = (diffVect.x > 0) ? 1 : -1; //diz se a bola está indo para a direita ou esquerda
                    float paddleHalfLenght = collider.bounds.extents.x;
                    float ballHalfLenght = myCollider.bounds.extents.x;
                    float xSafe = paddle.transform.position.x + (paddleHalfLenght + ballHalfLenght) * paddleSide;

                    //Torna a colisão pixel perfect
                    float xLimit = paddle.transform.position.x - (ballHalfLenght + paddleHalfLenght) * paddleSide;
                    transform.position += (paddleSide * transform.position.x < xLimit * paddleSide) ?
                                            diffVect * Mathf.Abs((xLimit - transform.position.x) / diffVect.x) :
                                            new Vector3(xLimit - transform.position.x, 0, 0);

                    //Efeito da colisão
                    velocity = Vector3.Reflect(velocity, paddle.normal);
                    velocity += paddle.velocity * padTransmission + paddle.normal * padBumping;

                    //Som da raquetada
                    switch (gameManager.gameState)
                    {
                        case GameState.NORMAL: AudioSource.PlayClipAtPoint(normalBounceSound, Vector3.zero); break;
                        case GameState.FIRE: AudioSource.PlayClipAtPoint(fireBounceSound, Vector3.zero); break;
                        case GameState.ICE: AudioSource.PlayClipAtPoint(iceBounceSound, Vector3.zero); break;
                        default: break;
                    }

                    gameManager.BounceOcurred();
                    collisionHappened = true;
                    break;
                }
            }

            if (!collisionHappened)
            {
                transform.position = newPosition;
            }

            //Limita velocidade
            float speed = Mathf.Min(velocity.magnitude, terminalSpeed);
            velocity = velocity.normalized * speed;
        }

        /// <summary>
        /// Bola começa a pegar fogo
        /// </summary>
        public void FireUp()
        {
            terminalSpeed = fireTerminalSpeed;
            velocity += velocity.normalized * fireSpeedIncrement;

            GetComponent<SpriteRenderer>().color = fireColor;
        }
    }
}