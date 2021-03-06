﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace GataclismaNaPista
{
    public class GameManager : MonoBehaviour
    {
        public int BPM;
        public TextMeshProUGUI text;
        public List<SpriteRenderer> fadeOutSpriteRendererObjects;
        public List<TextMeshProUGUI> fadeOutTextMeshProObjects;
        private AudioSource catHit;

        public float musicStartTime { get; private set; }

        private ArrowSequence[] allArrowSequences;

        [SerializeField] private CatAnimation leftCat;
        [SerializeField] private CatAnimation rightCat;

        //gambiarra
        private ScoreCalculation scoreCalculation;

        private void Awake()
        {
            allArrowSequences = GameObject.FindObjectsOfType<ArrowSequence>();
            scoreCalculation = GameObject.FindObjectOfType<ScoreCalculation>();
            catHit = this.GetComponent<AudioSource>();

            musicStartTime = Time.time;
        }

        private void Start()
        {
            StartCoroutine(CountDown());
        }

        IEnumerator CountDown()
        {
            for(int i = 3; i > 0; i--)
            {
                text.text = i.ToString();
                yield return new WaitForSeconds(60f/BPM);
            }
            text.text = "GO!";
            text.GetComponent<RectTransform>().DOMoveY(0.5f, 0.5f);
            text.DOColor(new Color(1, 1, 1, 0), 0.5f);
            StartCoroutine(SpawnAllArrows());
        }

        IEnumerator SpawnAllArrows()
        {
            yield return new WaitForSeconds(50f);
            StartCoroutine(EndGame());
        }

        IEnumerator CatHit()
        {
            yield return new WaitForSeconds(0.1f);
            catHit.Play();
        }

        IEnumerator EndGame()
        {
            StartCoroutine(CatHit());
            text.GetComponent<RectTransform>().DOMoveY(0.5f, 0.5f);
            text.DOColor(new Color(1, 1, 1, 1), 0.5f);
            foreach(SpriteRenderer sp in fadeOutSpriteRendererObjects)
            {
                sp.DOFade(0, 0.3f);
            }
            foreach(TextMeshProUGUI txt in fadeOutTextMeshProObjects)
            {
                txt.DOFade(0, 0.3f);
            }


            //text.resizeTextForBestFit = true;
            if(scoreCalculation.Winner > 0)
            {
                text.text = "DIREITA VENCE!";
                PlayersManager.result = PlayersManager.Result.RightWin;

                //Animação
                rightCat.GetComponent<Animator>().SetInteger("vencedor", 1);
                leftCat.GetComponent<Animator>().SetInteger("vencedor", -1);
            }
            else if(scoreCalculation.Winner < 0)
            {
                text.text = "ESQUERDA VENCE!";
                PlayersManager.result = PlayersManager.Result.LeftWin;

                //Animação
                //Animação
                rightCat.GetComponent<Animator>().SetInteger("vencedor", -1);
                leftCat.GetComponent<Animator>().SetInteger("vencedor", 1);
            }
            else
            {
                text.text = "EMPATE!";
                PlayersManager.result = PlayersManager.Result.Draw;
            }
            /*aqui deveria esperar um tempo e depois finalizar o jogo*/
            yield return new WaitForSeconds(5);
            
            StartCoroutine(ModeManager.TransitionFromMinigame());
        }
    }
    
}