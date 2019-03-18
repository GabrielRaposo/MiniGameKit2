using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pong
{
    public class CutsceneFinal : MonoBehaviour
    {
        [SerializeField] private Color fireMessageColor = Color.white;
        [SerializeField] private Color iceMessageColor = Color.white;
        [SerializeField] private Color standardMessageColor = Color.white;
        [SerializeField] private Canvas canvas = null;

        [SerializeField] private float timeScreenApparition = 1; //tempo até a tela aparecer depois que o ponto for feito
        [SerializeField] private float timeFinishGame = 1; //tempo até o jogo se encerrar depois que a tela parecer

        [SerializeField] private AudioClip endingSoundNormal = null;
        [SerializeField] private AudioClip endingSoundFire = null;
        [SerializeField] private AudioClip endingSoundIce = null;

        private void Start()
        {
            StartCoroutine(EndGame());
        }

        private IEnumerator EndGame()
        {
            yield return new WaitForSeconds(timeScreenApparition);


            //Cria tela
            canvas.gameObject.SetActive(true);
            Color chosenColor;

            switch (GetComponent<GameManager>().gameState)
            {
                case GameState.FIRE: chosenColor = fireMessageColor; break;
                case GameState.ICE: chosenColor = iceMessageColor; break;
                case GameState.NORMAL: chosenColor = standardMessageColor; break;
                default: chosenColor = Color.white; break;
            }

            canvas.GetComponentInChildren<Image>().color = chosenColor;
            canvas.GetComponentInChildren<Image>().GetComponentInChildren<Text>().color = chosenColor;
            canvas.GetComponentInChildren<Image>().GetComponentInChildren<Text>().text = GetComponent<GameManager>().winnerSide == Lado.DIR ?
                "DIREITA VENCEU" : "ESQUERDA VENCEU";

            //Som
            switch (GetComponent<GameManager>().gameState)
            {
                case GameState.FIRE:  AudioSource.PlayClipAtPoint(endingSoundFire, Vector3.zero); break;
                case GameState.ICE: AudioSource.PlayClipAtPoint(endingSoundIce, Vector3.zero); break;
                case GameState.NORMAL: AudioSource.PlayClipAtPoint(endingSoundNormal, Vector3.zero); break;
                default: break;
            }

            yield return new WaitForSeconds(timeFinishGame);

            GetComponent<GameManager>().MenuTransition();
        }
    }
}