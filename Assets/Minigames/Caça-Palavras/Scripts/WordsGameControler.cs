using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

		public UnityEngine.UI.Image clockImage;

		public List<char> alphabet;

		public List<LetterButtonScript> p1Buttons;
		public List<LetterButtonScript> p2Buttons;

		public List<TextMeshProUGUI> wordProgress;

		public TextMeshProUGUI displayTargetWord;

		private void Awake()
		{
			SelectWord();
			SetupGame();

			playerWords = new List<string>();
			playerWords.Add(targetWord);
			playerWords.Add(targetWord);

			clock = maxTime;
		}

		public void Update()
		{
			Clock -= Time.deltaTime;
			if (Clock <= 0)
				GameTimeout();

		}

		public void TryInputLetter(char letter, int player)
		{
			Debug.Log("Try input (" + letter.ToString() + ")");
			if (letter == playerWords[player][0])
				CorrectInput(letter,player);
			else
				IncorrectInput(player);
		}


		public void CorrectInput(char letter,int player)
		{
			Debug.Log("CorrectInput");
			wordProgress[player].text += letter.ToString();
			playerWords[player] = playerWords[player].Remove(0,1);
			Debug.Log(playerWords[player]);
			if (playerWords[player].Length == 0)
				FinishedWord(player);
		}

		public void IncorrectInput(int player)
		{
			Debug.Log("WrongInput");
		}

		void GameTimeout()
		{
			if (playerWords[0].Length > playerWords[1].Length)
				Results(PlayersManager.Result.RightWin);
			else if (playerWords[0].Length < playerWords[1].Length)
				Results(PlayersManager.Result.LeftWin);
			else
				Results(PlayersManager.Result.Draw);
		}

		public void FinishedWord(int player)
		{
			//Alguma firula de gamefeel aqui
			if (player == 0)
				Results(PlayersManager.Result.LeftWin);
			if (player == 1)
				Results(PlayersManager.Result.RightWin);
		}

		public void Results(PlayersManager.Result result)
		{

		}

		void SelectWord()
		{
			string[] wordList = { "abcd", "aaaa", "bcbc", "bccac" };

			targetWord = wordList[Random.Range(0, wordList.Length)];
		}

		void SetupGame()
		{
			wordProgress[0].text = "";
			wordProgress[1].text = "";
			displayTargetWord.text = targetWord;
		}

	}

}



