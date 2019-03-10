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

		public List<LetterButtonScript> p0Buttons;
		public List<LetterButtonScript> p1Buttons;

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
				CorrectInput(letter, player);
			else
				IncorrectInput(player);
		}


		public void CorrectInput(char letter, int player)
		{
			wordProgress[player].text += letter.ToString();
			playerWords[player] = playerWords[player].Remove(0, 1);

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
			PlayersManager.result = result;
		}

		void SelectWord()
		{
			string[] wordList = { "taxidermista", "ornitorrinco", "raposinho", "desenvolvimento", "asfaltamento", "tropicalidade" };

			targetWord = wordList[Random.Range(0, wordList.Length)];
		}

		void SetupGame()
		{
			wordProgress[0].text = "";
			wordProgress[1].text = "";
			displayTargetWord.text = targetWord;

			//shuffle list
			Shuffle<char>(alphabet);

			for (int i = 0; i < p0Buttons.Count; i++)
			{
				p0Buttons[i].letter = alphabet[i];
				p0Buttons[i].player = 0;
				p0Buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = alphabet[i].ToString();
			}

			// shuffle de novo
			Shuffle<char>(alphabet);

			for (int i = 0; i < p1Buttons.Count; i++)
			{
				p1Buttons[i].player = 1;
				p1Buttons[i].letter = alphabet[i];
				p1Buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = alphabet[i].ToString();
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

	}


}



