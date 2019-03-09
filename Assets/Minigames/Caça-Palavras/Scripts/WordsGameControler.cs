using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

		private void Awake()
		{
			SelectWord();

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

		public void InputLetter(char letter, int player)
		{
			if (letter == playerWords[player][0])
				CorrectInput(player);
			else
				IncorrectInput(player);
		}


		public void CorrectInput(int player)
		{
			playerWords[player].Remove(0);
			if (playerWords[player].Length == 0)
				FinishedWord(player);
		}

		public void IncorrectInput(int player)
		{

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
			string[] wordList = { "alface", "almoço" };

			targetWord = wordList[Random.Range(0, wordList.Length)];
		}

	}

}



