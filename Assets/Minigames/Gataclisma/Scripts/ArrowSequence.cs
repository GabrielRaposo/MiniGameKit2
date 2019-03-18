using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GataclismaNaPista
{
    public class ArrowSequence : MonoBehaviour
    {
        /// <summary>
        /// Padrões de ritmo usados no método SpawnSequence
        /// </summary>
        private enum Padroes
        {
            NORMAL, //1-1-1-1
            UM_MEIO //1-1-(1/2)(1/2)-1
        };

        public Queue<GameObject> ArrowQueue { get; private set; }
        public GameObject arrowPrefab;
        public float fallSpeed;
        public float absoluteOffset;    //diferença entre tempo do script e tempo da música
        public static float arrowSize;

        private GameManager gameManager;

        private float firstArrowDelay; //tempo que a primeira seta demora do spawn até a input box em segundos
        private static float deadZone;
        private static float unqueueZone;
        private static float spawnZone;

        /*Essas variáveis aqui são meio gambiarras, eu acho? Tô inseguro*/
        public Arrow peekArrowScript { get; private set; } // Arrow que deve receber o input
        private GameObject unqueuedDeadArrow; // Arrow após passar a área de input

        //Alerta de gambiarra para as setas dos dois jogadores serem iguais:
        private static List<Direction> directionsList = new List<Direction>();
        private int directionListIndex = 0;

        private void Awake()
        {
            ArrowQueue = new Queue<GameObject>();
            arrowSize = arrowPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
            spawnZone = (Camera.main.orthographicSize + arrowSize / 2);
            unqueueZone = this.transform.position.y - (this.GetComponent<SpriteRenderer>().bounds.size.y / 2 + arrowSize / 2);
            deadZone = -spawnZone;

            //Gambiarra para as setas dos dois jogadores serem iguais
            int numbOfDirections = 500;
            for (int i = 0; i < numbOfDirections; i++) directionsList.Add((Direction)Random.Range(0, 4));
        }

        private void Start()
        {
            int BPM = FindObjectOfType<GameManager>().BPM;

            gameManager = FindObjectOfType<GameManager>();

            SpawnSequence(4.8f, 20.042f, BPM, Padroes.NORMAL);
            SpawnSequence(20.042f, 29.134f, BPM, Padroes.UM_MEIO);
            SpawnSequence(32f, 43.311f, BPM, Padroes.NORMAL);
            SpawnSequence(43.311f, 52.409f, BPM, Padroes.UM_MEIO);
        }

        private void Update()
        {
            CheckDeadZone();
        }

        private void FixedUpdate()
        {
            if (Time.time > gameManager.musicStartTime)
            {
                FallAllArrows();
            }
        }

        void FallAllArrows()
        {
            if (unqueuedDeadArrow != null) unqueuedDeadArrow.transform.position += (Vector3.down *
                    fallSpeed*Time.fixedDeltaTime);
            foreach (GameObject arrow in ArrowQueue)
            {
                arrow.transform.position += (Vector3.down * fallSpeed * Time.fixedDeltaTime);
            }
        }

        private void SpawnArrow(Direction direction, float spawnPositionY)
        {
            GameObject newArrow = Instantiate(arrowPrefab, new Vector3(this.transform.position.x, spawnPositionY), Quaternion.identity, this.transform);
            newArrow.GetComponent<Arrow>().Initialize(direction, 1);
            ArrowQueue.Enqueue(newArrow);
            peekArrowScript = ArrowQueue.Peek().GetComponent<Arrow>();
            ArrowQueue.Peek().GetComponent<SpriteRenderer>().color = Color.cyan;
        }

        /// <summary>
        /// Spawna uma sequencia de setas que começam a chegar na input box em um dado 'offset' de tempo
        /// desde o início da música. A sequencia acaba em um tempo 'end' da música
        /// </summary>
        private void SpawnSequence(float offset, float end, float BPM, Padroes padrao)
        {
            float duration = end - offset;
            float arrowGap = fallSpeed * 60 / BPM - arrowSize;
            /* mais fácil de entender a fórmula:
             * fallSpeed = (arrowGap + arrowSize) * BPM / 60f */

            float offsetHeight = (offset + absoluteOffset) * fallSpeed - (spawnZone - transform.position.y);
            
            int numberOfArrows = (int)(duration * BPM / 60);

            //spawna setas
            for (int i = 0; i < numberOfArrows; i++)
            {
                float spawnPositionY = offsetHeight + spawnZone + (arrowGap + arrowSize) * i;

                //Gambiarra paras as setas dos dois jogadores ficarem iguais
                Direction direction = directionsList[directionListIndex];
                directionListIndex++;

                SpawnArrow(direction, spawnPositionY);
            
                if( padrao == Padroes.UM_MEIO && i % 4 == 2)
                {
                    direction = directionsList[directionListIndex];
                    directionListIndex++;

                    SpawnArrow(direction, spawnPositionY + (arrowGap + arrowSize) / 2);
                }
            }
        }

        public void DestroyPeek(ScoreType score)
        {
            /*distance é variável gambiarra aqui:*/
            float distance = Mathf.Abs(this.transform.position.y - peekArrowScript.transform.position.y);
            if (score != ScoreType.fail && /*Gambiarra:*/ distance < PlayerController.almostDistance)
            {
                if (score == ScoreType.wrongArrow)
                {
                    //gambiarra if
                    if (unqueuedDeadArrow != null) Destroy(unqueuedDeadArrow);
                    unqueuedDeadArrow = ArrowQueue.Peek();
                    unqueuedDeadArrow.GetComponent<SpriteRenderer>().color = Color.gray;
                }
                else
                {
                    peekArrowScript.animator.Play("ArrowExplode");
                    Destroy(ArrowQueue.Peek(), 1f);
                }
                ArrowQueue.Dequeue();
                if (ArrowQueue.Count > 0)
                {
                    peekArrowScript = ArrowQueue.Peek().GetComponent<Arrow>();
                    ArrowQueue.Peek().GetComponent<SpriteRenderer>().color = Color.cyan;
                }
            }
        }

        private void CheckDeadZone()
        {
            if (ArrowQueue.Count > 0 && ArrowQueue.Peek().transform.position.y < unqueueZone)
            {
                unqueuedDeadArrow = ArrowQueue.Peek();
                ArrowQueue.Dequeue();
                unqueuedDeadArrow.GetComponent<SpriteRenderer>().color = Color.gray;
                if (ArrowQueue.Count > 0)
                {
                    peekArrowScript = ArrowQueue.Peek().GetComponent<Arrow>();
                    ArrowQueue.Peek().GetComponent<SpriteRenderer>().color = Color.cyan;
                }
            }
            if (unqueuedDeadArrow != null && unqueuedDeadArrow.transform.position.y < deadZone)
            {
                Destroy(unqueuedDeadArrow);
            }
        }

        // Apenas para mostrar a linha de spawn e de destroy no Gizmos o3o
        private void OnDrawGizmosSelected()
        {
            float cameraWidth = Camera.main.orthographicSize * Camera.main.aspect;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(new Vector3(-cameraWidth, spawnZone), new Vector3(cameraWidth, spawnZone));
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(-cameraWidth, deadZone), new Vector3(cameraWidth, deadZone));
        }

        
    }
}
