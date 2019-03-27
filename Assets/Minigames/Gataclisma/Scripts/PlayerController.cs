using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace GataclismaNaPista
{
    [System.Serializable]
    public class ScoreEvent : UnityEvent<ScoreType> { }

    [RequireComponent(typeof(ArrowSequence))]
    public class PlayerController : MonoBehaviour
    {
        /* Perfect > Great > Good > Almost */
        /*  1/4       1/4     1/4     1/4   */
        /*
         *      ____________________
         *     |       Almost       |
         *     |       Good         |
         *     |       Great        |
         *     |       Perfect      |
         *     |       Perfect      |
         *     |       Great        |
         *     |       Good         |
         *     |       Almost       |
         *      ‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
         * 
         * */
        public ScoreEvent onScoreChange;


        public Text scoreText;
        //Gambiarra esse Canvas publico aqui
        public Canvas canvas;

        public float Score { get; private set; }
        private ArrowSequence sequence;
        private float boxSize;

        private float perfectDistance;
        private float greatDistance;
        private float goodDistance;

        /*ele só é publico por gambiarra na arrowsequence*/
        public static float almostDistance { get; private set; }

        //jogador da esquerda (player.side = false) ou direita (player.side = true)
        private PlayerInfo player;
        public TextMeshProUGUI streakText;
        private Vector3 TextPos;
        private TMP_ColorGradient FirstStreakColor;

        public int consecutiveHits { get; private set; }

        private void Awake()
        {
            player = transform.parent.GetComponent<PlayerInfo>();
            sequence = GetComponent<ArrowSequence>();
            boxSize = gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
        }

        void Start()
        {
            perfectDistance = (boxSize / 2 + ArrowSequence.arrowSize / 2) / 8;
            greatDistance = (boxSize / 2 + ArrowSequence.arrowSize / 2) / 4;
            goodDistance = (boxSize / 2 + ArrowSequence.arrowSize / 2) / 2;
            almostDistance = (boxSize / 2 + ArrowSequence.arrowSize / 2);
            onScoreChange.AddListener(sequence.DestroyPeek);
            Debug.Log("Perfect = " + (this.transform.position.y + perfectDistance));
            Debug.Log("Great = " + (this.transform.position.y + greatDistance));
            Debug.Log("Good = " + (this.transform.position.y + goodDistance));
            Debug.Log("Almost = " + (this.transform.position.y + almostDistance));
            TextPos = streakText.transform.position;
            FirstStreakColor = streakText.colorGradientPreset;

            consecutiveHits = 0;
        }

        void Update()
        {
            checkInputDirection();
        }
        
        private void checkInputDirection()
        {
            if (Input.GetButtonDown(player.playerButtons.horizontal) || Input.GetButtonDown(player.playerButtons.vertical))
            {
                if (sequence.ArrowQueue.Count > 0)
                {
                    float distance = Mathf.Abs(this.transform.position.y - sequence.ArrowQueue.Peek().transform.position.y);
                    ScoreType score;
                    if (Input.GetAxisRaw(player.playerButtons.horizontal) == 1 && sequence.peekArrowScript.direction == Direction.right ||
                        Input.GetAxisRaw(player.playerButtons.vertical) == -1 && sequence.peekArrowScript.direction == Direction.down ||
                        Input.GetAxisRaw(player.playerButtons.horizontal) == -1 && sequence.peekArrowScript.direction == Direction.left ||
                        Input.GetAxisRaw(player.playerButtons.vertical) == 1 && sequence.peekArrowScript.direction == Direction.up)
                    {
                        score = CalculateScore(distance);

                        sequence.peekArrowScript.clicked = true;
                        consecutiveHits++;

                        if(consecutiveHits > 9)
                        {
                            streakText.text = consecutiveHits.ToString();
                            if (consecutiveHits == 10)
                            {
                                streakText.transform.position = TextPos - Vector3.up * 0.2f;
                                streakText.GetComponent<RectTransform>().DOMoveY(TextPos.y, 0.3f);
                                streakText.DOFade(0, 0f);
                                streakText.DOFade(1, 0.3f);
                            }
                            else
                            {
                                streakText.GetComponent<RectTransform>().DOScale(streakText.fontScale * 1.2f, 0.15f);
                                streakText.GetComponent<RectTransform>().DOScale(streakText.fontScale / 1.2f, 0.15f);
                            }
                            if(consecutiveHits == 30)
                            {
                                Color[] clr = new Color[4];
                                ColorUtility.TryParseHtmlString("#E7DC00FF", out clr[0]);
                                ColorUtility.TryParseHtmlString("#FAF9B2FF", out clr[1]);
                                ColorUtility.TryParseHtmlString("#FFEC00FF", out clr[2]);
                                ColorUtility.TryParseHtmlString("#A14811FF", out clr[3]);
                                TMP_ColorGradient grd = ScriptableObject.CreateInstance("TMP_ColorGradient") as TMP_ColorGradient;
                                grd.topLeft = clr[0];
                                grd.topRight = clr[1];
                                grd.bottomLeft = clr[2];
                                grd.bottomRight = clr[3];
                                streakText.colorGradientPreset = grd;
                            }
                            if (consecutiveHits == 50)
                            {
                                Color[] clr = new Color[4];
                                ColorUtility.TryParseHtmlString("#36AB14FF", out clr[0]);
                                ColorUtility.TryParseHtmlString("#FFFB00FF", out clr[1]);
                                ColorUtility.TryParseHtmlString("#46FF00FF", out clr[2]);
                                ColorUtility.TryParseHtmlString("#023F2CFF", out clr[3]);
                                TMP_ColorGradient grd = ScriptableObject.CreateInstance("TMP_ColorGradient") as TMP_ColorGradient;
                                grd.topLeft = clr[0];
                                grd.topRight = clr[1];
                                grd.bottomLeft = clr[2];
                                grd.bottomRight = clr[3];
                                streakText.colorGradientPreset = grd;
                            }
                        }

                        //Animacao
                        transform.parent.GetComponentInChildren<Animator>().SetInteger("consecutiveHits", consecutiveHits);
                        transform.parent.GetComponentInChildren<Animator>().SetBool("failed", false);
                        transform.parent.GetChild(0).GetChild(0).GetComponentInChildren<Animator>().SetInteger("consecutiveHits", consecutiveHits);
                        transform.parent.GetChild(0).GetChild(0).GetComponentInChildren<Animator>().SetBool("failed", false);
                        
                    }
                    else
                    {
                        streakText.colorGradientPreset = FirstStreakColor;
                        FailArrow();
                        Debug.Log("Wrong Arrow!");
                        score = ScoreType.wrongArrow;

                        streakText.GetComponent<RectTransform>().DOScale(streakText.rectTransform.localScale * 1.2f, 0.1f);
                        //streakText.GetComponent<RectTransform>().DOScale(0, 0.2f);
                        streakText.DOFade(0, 0.2f);
                        streakText.GetComponent<RectTransform>().localScale = Vector3.one;
                        streakText.GetComponent<RectTransform>().DOScale(Vector3.one, 0f);
                        streakText.transform.position = TextPos;
                        //streakText.text = "";
                        //Animacao
                        consecutiveHits = 0;
                        transform.parent.GetComponentInChildren<Animator>().SetInteger("consecutiveHits", consecutiveHits);
                        transform.parent.GetComponentInChildren<Animator>().SetBool("failed", true);
                        transform.parent.GetChild(0).GetChild(0).GetComponentInChildren<Animator>().SetInteger("consecutiveHits", consecutiveHits);
                        transform.parent.GetChild(0).GetChild(0).GetComponentInChildren<Animator>().SetBool("failed", true);
                    }

                    //InstantiateScoreText(score);
                    onScoreChange.Invoke(score);
                }
            }
        }
        private void LateUpdate()
        {
            if (sequence.unqueuedDeadArrow != null)
            {
                if (sequence.unqueuedDeadArrow.GetComponent<Arrow>().clicked == false )//&& consecutiveHits > 9)
                {
                    Debug.Log("MISS");
                    consecutiveHits = 0;
                    streakText.colorGradientPreset = FirstStreakColor;

                    streakText.GetComponent<RectTransform>().DOScale(streakText.rectTransform.localScale * 1.2f, 0.1f);
                    streakText.DOFade(0, 0.2f);
                    streakText.GetComponent<RectTransform>().localScale = Vector3.one;
                    streakText.GetComponent<RectTransform>().DOScale(Vector3.one, 0f);
                    streakText.transform.position = TextPos;
                    //streakText.text = "";
                    //Animacao
                    transform.parent.GetComponentInChildren<Animator>().SetInteger("consecutiveHits", consecutiveHits);
                    transform.parent.GetComponentInChildren<Animator>().SetBool("failed", true);
                    transform.parent.GetChild(0).GetChild(0).GetComponentInChildren<Animator>().SetInteger("consecutiveHits", consecutiveHits);
                    transform.parent.GetChild(0).GetChild(0).GetComponentInChildren<Animator>().SetBool("failed", true);
                }
            }
        }

        //####################
        private ScoreType CalculateScore(float distance)
        {
            float points;
            ScoreType score;
            if (distance < almostDistance)
            {

                if (distance < perfectDistance)
                {
                    Debug.Log("perfect!");
                    points = 5;
                    score = ScoreType.perfect;
                }
                else if (distance < greatDistance)
                {
                    Debug.Log("great!");
                    points = 3;
                    score = ScoreType.great;
                }
                else if (distance < goodDistance)
                {
                    Debug.Log("good!");
                    points = 2;
                    score = ScoreType.good;
                }
                else
                {
                    Debug.Log("almost!");
                    points = 1;
                    score = ScoreType.almost;
                }
                this.Score += points;
            }
            else
            {
                score = FailArrow();
            }
            return score;
        }

        private ScoreType FailArrow()
        {
            Debug.Log("fail!");
            float points = -2;
            if (this.Score + points < 1)
            {
                this.Score = 1;
            }
            else
            {
                this.Score += points;
            }
            return ScoreType.fail;
        }

        private void InstantiateScoreText(ScoreType score)
        {
            string textString;
            Color textColor;
            switch (score)
            {
                case ScoreType.perfect:
                    textString = "PURRFECT";
                    textColor = Color.magenta;
                    break;
                case ScoreType.great:
                    textString = "OTIMIAU";
                    textColor = Color.cyan;
                    break;
                case ScoreType.good:
                    textString = "BOM";
                    textColor = Color.green;
                    break;
                case ScoreType.almost:
                    textString = "QUASE";
                    textColor = Color.yellow;
                    break;
                default:
                    textString = "ERROU";
                    textColor = Color.red;
                    break;
            }
            /*Text textObj = Instantiate(scoreText, canvas.transform) as Text;
             * #####
            if (transform.position.x > 0)
            {
                textObj.transform.position *= new Vector2(-1, 1);
                textObj.alignment = TextAnchor.MiddleRight;
            }
            textObj.text = textString;
            textObj.color = textColor;
            textObj.DOFade(0, 0.8f);
            textObj.GetComponent<RectTransform>().DOMoveY(textObj.transform.position.y + 0.1f, 0.5f);
            Destroy(textObj, 0.7f);*/
        }

        //define propriedades da variável "player"
        private void SetPlayer()
        {

        }


    }
}
