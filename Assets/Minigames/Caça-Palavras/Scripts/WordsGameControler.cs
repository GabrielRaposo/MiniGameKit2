﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Words
{
	public class WordsGameControler : MonoBehaviour
	{
		public string targetWord;

		public List<string> playerWords;

		public float maxTime;
		private float clock;

		public float Clock
		{
			get
			{
				return clock;
			}

			set
			{
				clock = value;
				if (clock > maxTime)
					clock = maxTime;
				if (clock < 0)
					clock = 0;

				clockImage.fillAmount = clock / maxTime;
			}
		}

		float clockSpeed = 0;

		public Image clockImage;

		public List<char> alphabet;

		public List<LetterButtonScript> p0Buttons;
		public List<LetterButtonScript> p1Buttons;

		public WordsPlayer player0;
		public WordsPlayer player1;

		public Image boxBorderP0;
		public Image boxBorderP1;

		public List<TextMeshProUGUI> wordProgress;

		public TextMeshProUGUI displayTargetWord;

		public AudioSource audioTyping;
		public AudioSource audioMain;
		public AudioSource audioWin;

		public float endgameAnimationDelay = 2.0f;
		public float animationEndgameDuration = 2.2f;

		private void Awake()
		{
						
		}

		private void Start()
		{
			SelectWord();
			SetupGame();

			//audioMain.Pause();
			clock = maxTime;
		}

		public void Update()
		{
			Clock -= Time.deltaTime * clockSpeed;
			if (Clock <= 0 && clockSpeed > 0)
			{
				clockSpeed = 0;
				GameTimeout();
			}

		}

		private void LateUpdate()
		{
			boxBorderP0.color = KeepColorTone(boxBorderP0.color, player0.VisibleColor);
			boxBorderP1.color = KeepColorTone(boxBorderP1.color, player1.VisibleColor);
		}

		public Color KeepColorTone(Color target, Color tone)
		{
			target = new Color(tone.r, tone.g, tone.b, target.a);

			return target;
		}

		public void TryInputLetter(char letter, int player)
		{
			if (playerWords[player] == "")
				return;

			if (letter == playerWords[player][0])
				CorrectInput(letter, player);
			else
				IncorrectInput(player);
		}


		public void CorrectInput(char letter, int player)
		{
			wordProgress[player].text += letter.ToString();
			playerWords[player] = playerWords[player].Remove(0, 1);

			wordProgress[player].GetComponent<AudioSource>().Play();

			if (playerWords[player].Length == 0)
				FinishedWord(player);
		}

		public void IncorrectInput(int player)
		{

		}

		void GameTimeout()
		{
			if (playerWords[0].Length > playerWords[1].Length)
			{
				Results(1);
				StartCoroutine(TypeAnything(targetWord, wordProgress[1]));
			}
			else if (playerWords[0].Length < playerWords[1].Length)
			{
				Results(0);
				StartCoroutine(TypeAnything(targetWord, wordProgress[0]));
			}
			else
				Results(PlayersManager.Result.Draw);
		}

		public void FinishedWord(int player)
		{
            //Alguma firula de gamefeel aqui
            //if (audioWin != null)
            //{
			    //audioWin.Play();
			    //audioMain.volume = 0.1f;
            //}
			Results(player);
		}

		public void Results(int result)
		{
			clockSpeed = 0;
			player0.GetComponent<MyEventSystem>().enabled = false;
			player1.GetComponent<MyEventSystem>().enabled = false;			

			StartCoroutine(EndGameAnimations(result));
			
		}

		IEnumerator EndGameAnimations(int result)
		{
			yield return new WaitForSeconds(endgameAnimationDelay);

			switch (result)
			{
				case 0:
					WinnerAnimation(0);
					LoserAnimation(1);
					break;
				case 1:
					WinnerAnimation(1);
					LoserAnimation(0);
					break;
				case -1:
					LoserAnimation(1);
					LoserAnimation(0);
					break;
			}

			yield return new WaitForSeconds(animationEndgameDuration);

			if (result == 0)
				PlayersManager.result = PlayersManager.Result.LeftWin;
			else if (result == 1)
				PlayersManager.result = PlayersManager.Result.RightWin;
			else
				PlayersManager.result = PlayersManager.Result.Draw;

			StartCoroutine(ModeManager.TransitionFromMinigame());
			

		}

		public void Results(PlayersManager.Result result)
		{
			if(result == PlayersManager.Result.Draw)
			{
				player0.GetComponent<MyEventSystem>().enabled = false;
				player1.GetComponent<MyEventSystem>().enabled = false;

				StartCoroutine(EndGameAnimations(-1));
			}
		}

		public void WinnerAnimation(int player)
		{
			player0.GetComponent<Animator>().enabled = true;
			player1.GetComponent<Animator>().enabled = true;
			if (player == 0)
			{				
				player0.GetComponent<Animator>().SetTrigger("Win");
			}
			else
			{				
				player1.GetComponent<Animator>().SetTrigger("Win");
			}
		}

		public void LoserAnimation(int player)
		{
			player0.GetComponent<Animator>().enabled = true;
			player1.GetComponent<Animator>().enabled = true;
			wordProgress[player].color = new Vector4(wordProgress[player].color.r, wordProgress[player].color.g, wordProgress[player].color.b, 0.5f);

			if (player == 0)
			{
				wordProgress[player].color = LowSatColor(player0.VisibleColor);
				player0.GetComponent<Animator>().SetTrigger("Loss");
			}
			else
			{
				wordProgress[player].color = LowSatColor(player1.VisibleColor);
				player1.GetComponent<Animator>().SetTrigger("Loss");
			}
		}

		void SelectWord()
		{
			string[] wordList = { "taxidermista", "ornitorrinco", "mononucleose", "desenvolvimento", "asfaltamento", "tropicalidade", "pluralidade", "aristotelismo", "espectroscopia", "infravermelho", "espalhamento",
									"encaminhamento", "endocrinologia", "quilometragem", "sepultamento", "comunidades", "cavalheirismo", "esmerilhadeira", "prestidigitador", "estequiometria"};

			targetWord = wordList[Random.Range(0, wordList.Length)];
		}

		void SetupGame()
		{		
			wordProgress[0].text = "";
			wordProgress[1].text = "";

			playerWords = new List<string>();
			playerWords.Add("");
			playerWords.Add("");

			//displayTargetWord.text = targetWord;

			//shuffle list
			Shuffle<char>(alphabet);

			wordProgress[0].color = player0.VisibleColor;
			boxBorderP0.color = new Color(player0.VisibleColor.r, player0.VisibleColor.g, player0.VisibleColor.b, 0.6f);

			player0.gameObject.GetComponent<MyInputModule>().horizontalAxis = player0.playerButtons.horizontal;
			player0.gameObject.GetComponent<MyInputModule>().verticalAxis = player0.playerButtons.vertical;
			player0.gameObject.GetComponent<MyInputModule>().submitButton = player0.playerButtons.action;

			for (int i = 0; i < p0Buttons.Count; i++)
			{
				p0Buttons[i].gameObject.GetComponent<Image>().color = player0.VisibleColor;
				p0Buttons[i].letter = alphabet[i];
				p0Buttons[i].player = 0;
				p0Buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = alphabet[i].ToString();
			}

			// shuffle de novo
			//Shuffle<char>(alphabet);

			wordProgress[1].color = player1.VisibleColor;
			boxBorderP1.color = new Color(player1.VisibleColor.r, player1.VisibleColor.g, player1.VisibleColor.b, 0.6f);

			player1.gameObject.GetComponent<MyInputModule>().horizontalAxis = player1.playerButtons.horizontal;
			player1.gameObject.GetComponent<MyInputModule>().verticalAxis = player1.playerButtons.vertical;
			player1.gameObject.GetComponent<MyInputModule>().submitButton = player1.playerButtons.action;

			for (int i = 0; i < p1Buttons.Count; i++)
			{
				p1Buttons[i].gameObject.GetComponent<Image>().color = player1.VisibleColor;
				p1Buttons[i].player = 1;
				p1Buttons[i].letter = alphabet[i];
				p1Buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = alphabet[i].ToString();
			}

			audioMain.Play();
			StartCoroutine(IntroTypeWord());

		}

		IEnumerator IntroTypeWord()
		{

			displayTargetWord.text = "";

			displayTargetWord.gameObject.GetComponent<MainTextScript>().enabled = false;

			yield return new WaitForSeconds(1.1f);

			for(int i = 0; i < targetWord.Length; i++)
			{
				displayTargetWord.text += targetWord[i];
				audioTyping.Play();
				yield return  new  WaitForSeconds((1.7f)/targetWord.Length);
			}

			clockSpeed = 1;
						
			displayTargetWord.gameObject.GetComponent<MainTextScript>().enabled = true;


			playerWords[0] = targetWord;
			playerWords[1] = targetWord;

		}

		IEnumerator TypeAnything(string word, TextMeshProUGUI targetText)
		{
			float totalDuration = 1.7f;
			int n = word.Length - targetText.text.Length;

			for(int i = 0; i < n; i++)
			{
				targetText.text += word[word.Length - n + i];
				audioTyping.Play();
				yield return new WaitForSeconds(totalDuration / n);
			}

		}

		public void Shuffle<T>(IList<T> list)
		{
			int n = list.Count;			

			while (n > 1)
			{
				n--;
				int k = Random.Range(0, n);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		public Color LowSatColor(Color color)
		{
			float h= 0 , s = 0, v = 0;

			Color.RGBToHSV(color, out h, out s, out v);

			s = s / 2;
			v = v / 2;

			return Color.HSVToRGB(h, s, v);

		}

	}
}



