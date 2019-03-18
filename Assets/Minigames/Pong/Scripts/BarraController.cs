using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pong
{
    public enum Lado
    {
        ESQ,
        DIR
    };

    public class BarraController : PlayerInfo
    {
        private const float normalSpeed = 10f;
        private const float frozenSpeed = 5f;
        private const float endingDeceleration = 20f;

        private GameManager gameManager;

        private float speed = normalSpeed;
        private float minY;
        private float maxY;

        private string input;

        [Range(0, 1)] [SerializeField] private float saturationFactorAtFreeze = 0.5f; //é multiplicado pela saturação da cor da sprite

        public Lado lado; //esquerda ou direita;
        public Vector3 normal { get; private set; }
        public Vector3 velocity { get; private set; }

        public override void Start()
        {
            base.Start();

            gameManager = FindObjectOfType<GameManager>();

            //define cor
            GetComponent<SpriteRenderer>().color = base.color;

            //calcula minY e maxY
            Sprite sprite = GetComponent<SpriteRenderer>().sprite;
            maxY = Camera.main.orthographicSize - sprite.bounds.extents.y * transform.localScale.y;
            minY = -Camera.main.orthographicSize + sprite.bounds.extents.y * transform.localScale.y;

            //calcula normal e define tipo de input
            if (lado == Lado.DIR)
            {
                normal = Vector3.left;
            }
            else if (lado == Lado.ESQ)
            {
                normal = Vector3.right;
            }
            else Debug.LogError("Lado não definido");

            input = base.playerButtons.vertical;
        }

        private void FixedUpdate()
        {
            //Get input
            if (Input.GetAxisRaw(input) == 1 && gameManager.gameState != GameState.CUTSCENE)
            {
                velocity = Vector3.up * speed;
            }
            else if (Input.GetAxisRaw(input) == -1 && gameManager.gameState != GameState.CUTSCENE)
            {
                velocity = Vector3.down * speed;
            }
            else
            {
                velocity = Vector3.zero;
            }

            //Set position
            float newY = transform.position.y + velocity.y * Time.fixedDeltaTime;
            newY = Mathf.Min(newY, maxY);
            newY = Mathf.Max(newY, minY);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }

        /// <summary>
        /// Congela a raquete
        /// </summary>
        public void IceUp()
        {
            speed = frozenSpeed;
            float presentH, presentS, presentV;
            Color.RGBToHSV(GetComponent<SpriteRenderer>().color, out presentH, out presentS, out presentV);

            GetComponent<SpriteRenderer>().color = Color.HSVToRGB(presentH, presentS * saturationFactorAtFreeze, presentV);
        }
    }
}