using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pong
{
    public enum GameState
    {
        NORMAL,
        FIRE,
        ICE,
        CUTSCENE
    };

    public class GameManager : MonoBehaviour
    {
        private const int bouncesTillPowerUp = 6;   //number of times the ball bounces on the pads until power up spawns
        private const int bouncesTillWarningLine = 3;   //number of times the ball bounces on the pads until the warning line appears
        private const float ballScreenFractionToActivatePowerup = 0.2f;    //define uma condição para que o power up possa spawnar

        private int PowerUpBounces = 0;

        private BarraController[] paddles;
        [SerializeField] private BolaController ball = null;
        [SerializeField] private LinhaDeAviso linhaDeAviso = null; //linha que indica qual será o power up
        [SerializeField] private PowerUpController powerUp = null;

        private float lastBounceX;    //coordenada x na qual a bola levou a última raquetada para liberar o power up
        private bool powerupActivated = false;
        private PowerUpType powerUpType;
        public GameState gameState { get; private set; }
        public Lado winnerSide { get; private set; }

        private void Start()
        {
            paddles = FindObjectsOfType<BarraController>();

            //Inicia cutscene
            gameState = GameState.CUTSCENE;
            gameObject.GetComponent<CutsceneInicio>().enabled = true;
        }

        private void Update()
        {
            if (PowerUpBounces == bouncesTillPowerUp)
            {
                /*Ativa o power up quando a bola passar sobre uma certa fração do lado oposto à última raquete
                 com a qual a bola colidiu.*/
                if (PowerUpBounces == bouncesTillPowerUp && ball != null)
                {
                    float relativePositionX = ball.transform.position.x - Camera.main.transform.position.x;
                    float relativeLastBounceX = lastBounceX - Camera.main.transform.position.x;

                    //Ativa o power up
                    if (Mathf.Sign(relativePositionX) != Mathf.Sign(relativeLastBounceX) &&
                        Mathf.Abs(relativePositionX) > Camera.main.orthographicSize * Camera.main.aspect * ballScreenFractionToActivatePowerup &&
                        !powerupActivated)
                    {
                        Destroy(linhaDeAviso.gameObject);
                        powerUp.TurnOn(powerUpType);
                        powerupActivated = true;
                    }
                }
            }
        }

        /// <summary>
        /// Incrementa 'PowerUpBounces'. Quando atingi um certo número de 'bounces',
        /// escolhe qual será o powerup e ativa a linha de aviso.
        /// </summary>
        public void BounceOcurred()
        {
            PowerUpBounces++;

            if (PowerUpBounces == bouncesTillWarningLine)
            {
                powerUpType = CustomFuncs.RandomBool() ? PowerUpType.FIRE : PowerUpType.ICE;
                //powerUpType = PowerUpType.FIRE;
                //powerUpType = PowerUpType.ICE;

                linhaDeAviso.gameObject.SetActive(true);
                linhaDeAviso.GetComponent<SpriteRenderer>().color = powerUpType == PowerUpType.FIRE ? linhaDeAviso.corFogo : linhaDeAviso.corGelo;
            }

            if (PowerUpBounces == bouncesTillPowerUp)
            {
                lastBounceX = ball.transform.position.x;

            }

        }

        /// <summary>
        /// Ativa efeito de fogo ou gelo e seta 'gameState'
        /// </summary>
        public void ActivateEffect(PowerUpType type)
        {
            if (type == PowerUpType.FIRE)
            {
                ball.FireUp();
                gameState = GameState.FIRE;
            }
            else if (type == PowerUpType.ICE)
            {
                foreach (BarraController paddle in paddles)
                {
                    paddle.IceUp();
                    gameState = GameState.ICE;
                }
            }
            else
            {
                Debug.LogError("Tipo de power up não existente");
            }
        }

        /// <summary>
        /// Começa o jogo após o fim da cutscene inicial
        /// </summary>
        public void StartsGameAfterCutScene()
        {
            gameState = GameState.NORMAL;
            ball.gameObject.SetActive(true);
        }

        /// <summary>
        /// Começa cutscene de encerramento
        /// </summary>
        public void MatchEnd(Lado winner)
        {
            winnerSide = winner;
            GetComponent<CutsceneFinal>().enabled = true;
        }

        //Define o vencedor para o gdpack e termina de vez o jogo, indo para o menu do gdpack.
        public void MenuTransition()
        {
            PlayersManager.result = winnerSide == Lado.ESQ ? PlayersManager.Result.LeftWin : PlayersManager.Result.RightWin;
            StartCoroutine(ModeManager.TransitionFromMinigame());
        }
    }
}