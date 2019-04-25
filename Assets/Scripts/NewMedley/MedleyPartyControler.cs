using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MedleyPartyControler : MonoBehaviour
{
	public static MedleyPartyControler i;

	[Header("Icon Placement")]
	public GameObject playerIconPrefab;
	public Transform playerLayoutHolder;
	List<GameObject> playerIcons;

	public GameObject currentLeftPlayerDisplay;
	public GameObject currentRightPlayerDisplay;

	public GameObject currentMinigameDisplay;

	[Header("Games")]
	public List<TutorialObject> games;
	public TutorialObject currentGame;

	MedleySetup medleySetup;

	[Header("Game Info Display")]
	public GameObject gameInfoDisplay;
	public TextMeshProUGUI gameName;
	public TextMeshProUGUI gameRules;
	
	[Header("Player Match Info")]
	public static List<int> playerScores;
	public static List<int> playerMatchesPlayed;
	public static List<int> playersInDraw;

	public static bool partyInProgress;
	public static bool drawMode;

	[Header("UI Elements")]
	public static int round;
	public TextMeshProUGUI roundText;
	public static bool changeRoundNext;
	public GameObject endGameScreen;
	public TextMeshProUGUI endGameText;

	private void Awake()
	{
		if(i == null)
		{
			i = this;
		}
		else
		{
			if(i!= this)
			{
				Destroy(this);
			}
		}

		medleySetup = FindObjectOfType<MedleySetup>();
		//DontDestroyOnLoad(this.gameObject);
	}

	private void Update()
	{
		if (Input.GetKeyDown("g"))
		{			
			SelectMinigame();
		}

		if (Input.GetKeyDown("h"))
		{
			GoToMinigame();
		}
	}


	public void OpenParty()
	{
		Debug.Log("OpenParty()");
		playerIcons = new List<GameObject>();

		if (!partyInProgress)
		{
			//Start New party
			round = 0;
			playerScores = new List<int>();
			playerMatchesPlayed = new List<int>();
		}
		else
		{
			UpdateScores();
		}

		for (int i = 0; i < MedleySetup.nOfPlayers; i++)
		{				
			if (!partyInProgress)
			{
				playerScores.Add(0);
				playerMatchesPlayed.Add(0);
			}

			GameObject p = Instantiate(playerIconPrefab, playerLayoutHolder);//
			playerIcons.Add(p);//
			p.GetComponent<MeddleyPlayerIcon>().SetColor(PlayersManager.playerColor[i]);//
			p.GetComponent<MeddleyPlayerIcon>().SetName(PlayersManager.playerName[i]);//
			p.GetComponent<MeddleyPlayerIcon>().SetScore(playerScores[i]);//
		}

		if(!partyInProgress)
			partyInProgress = true;

		if (changeRoundNext)
			ChangeRound();
		
		SelectActivePlayers();
		SelectMinigame();
	}

	public void ChangeRound()
	{
		//Animar isto
		round++;
		if (drawMode)
			roundText.text = "DRAW";
		else
			roundText.text = "R: " + round.ToString();
		changeRoundNext = false;
		Debug.Log("ChangingRound");
	}
		
	public void SelectActivePlayers()
	{
		int[] lowPool = GetPlayersThatPlayedEqualTheRound();
		int[] moreThanPool = GetPlayersThatPlayedMoreThanTheRound();

		if (!drawMode)
		{
			foreach(int p in moreThanPool)
			{
				playerIcons[p].GetComponent<MeddleyPlayerIcon>().SetColor(new Color(0.5f, 0.5f, 0.5f));
			}
		}
		else
		{
			foreach(GameObject player in playerIcons)
			{
				//player.GetComponent<MeddleyPlayerIcon>().SetColor(new Color(0.5f, 0.5f, 0.5f));
			}
			foreach(int p in playersInDraw)
			{
				playerIcons[p].GetComponent<MeddleyPlayerIcon>().SetColor(PlayersManager.playerColor[p]);
			}
		}

		if(lowPool.Length <= 2 && lowPool.Length > 0)
		{
			Debug.Log("ChangeRoundNext");
			changeRoundNext = true;
			if(lowPool.Length == 2)
			{
				if (Random.Range(0, 2) == 0)
				{
					PlayersManager.currentLeftPlayer = lowPool[0];
					PlayersManager.currentRightPlayer = lowPool[1];
				}
				else
				{
					PlayersManager.currentLeftPlayer = lowPool[1];
					PlayersManager.currentRightPlayer = lowPool[0];
				}
			}
			if(lowPool.Length == 1)
			{
				if (Random.Range(0, 2) == 0)
				{
					PlayersManager.currentLeftPlayer = lowPool[0];
					PlayersManager.currentRightPlayer = moreThanPool[Random.Range(0, moreThanPool.Length)];
				}
				else
				{
					PlayersManager.currentRightPlayer = lowPool[0];
					PlayersManager.currentLeftPlayer = moreThanPool[Random.Range(0, moreThanPool.Length)];
				}
			}
		}

		else
		{
			PlayersManager.currentRightPlayer = lowPool[Random.Range(0, lowPool.Length)];

			do
			{
				PlayersManager.currentLeftPlayer = lowPool[Random.Range(0, lowPool.Length)];
			} while (PlayersManager.currentRightPlayer == PlayersManager.currentLeftPlayer);

		}

		DisplayActivePlayers();

	}
			
	public void SelectMinigame()
	{
		currentGame = games[Random.Range(0, games.Count)];
		DisplayMinigame();
	}

	public void GoToMinigame()
	{		
		playerMatchesPlayed[PlayersManager.currentLeftPlayer]++;
		playerMatchesPlayed[PlayersManager.currentRightPlayer]++;
		SceneManager.LoadScene(currentGame.codename);
	}

	public void DisplayActivePlayers()
	{
		//Colocar animações e etc aqui
		currentLeftPlayerDisplay.GetComponent<Image>().color = PlayersManager.playerColor[PlayersManager.currentLeftPlayer];
		currentRightPlayerDisplay.GetComponent<Image>().color = PlayersManager.playerColor[PlayersManager.currentRightPlayer];
	}

	public void DisplayMinigame()
	{
		//Colocar animações aqui
		currentMinigameDisplay.GetComponent<RawImage>().texture = currentGame.icon;

		gameInfoDisplay.SetActive(true); //Animar isso aqui descendo
		gameName.text = currentGame.minigameName;
		gameRules.text = currentGame.gameRules;

	}



	void UpdateScores()
	{		
		switch (PlayersManager.result)
		{
			case PlayersManager.Result.Draw:
				break;
			case PlayersManager.Result.LeftWin:
				playerScores[PlayersManager.currentLeftPlayer] = playerScores[PlayersManager.currentLeftPlayer] +1;
				break;
			case PlayersManager.Result.RightWin:
				playerScores[PlayersManager.currentRightPlayer] = playerScores[PlayersManager.currentRightPlayer] +1;
				break;
		}

		if (!drawMode)
			CheckIfGameOver();
		else
			CheckIfDrawOver();
	}

	void DebugPrintPlayerMatches()
	{
		//for (int i = 0; i < playerMatchesPlayed.Count; i++)
		//{
		//	Debug.Log("Player " + i.ToString() + " matches played:" + playerMatchesPlayed[i].ToString());
		//}
	}

	void CheckIfGameOver()
	{
		int n = MedleySetup.nOfVictories;
				
		switch (MedleySetup.mode)
		{
			case MedleyModes.NumberOfGames:
				if (AllPlayersPlayedTheSame() && playerMatchesPlayed[0] == n)
				{
					if (GetHighestScoringPlayers().Length == 1)
						EndGame(GetHighestScoringPlayers());
					else
					{
						DrawModeSetup();
					}
				}
				else
				{
					//Debug.Log("AllPlayersPlayedTheSame(): " + AllPlayersPlayedTheSame().ToString() + " playerMatchesPlayed[0]: " + playerMatchesPlayed[0].ToString() + " n: " + n.ToString());
					//Debug.Log("Segue o jogo");
				}
				break;
			case MedleyModes.NumberOfVictories:
				if(NumberOfPlayersWithSpecificScore(n) > 0)
				{
					Debug.Log("Tem alguem arriscando ganhar aí");
					if (AllPlayersPlayedTheSame())
					{
						if (GetHighestScoringPlayers().Length == 1)
							EndGame(GetHighestScoringPlayers());
						else
						{
							DrawModeSetup();
						}
					}
					else
					{
						//TODO Alertazinho de pode acabar
						//animar isto
					}
				}
				else
				{
					Debug.Log("Segue o jogo");
				}
				break;
		}
	}

	void CheckIfDrawOver()
	{
		if(playersInDraw.Count == 2)
		{
			if (playerScores[playersInDraw[0]] > playerScores[playersInDraw[1]])
				EndGame(new int[] { playersInDraw[0] });
			if(playerScores[playersInDraw[1]] > playerScores[playersInDraw[0]])
				EndGame(new int[] { playersInDraw[1] });
			if (playerScores[playersInDraw[1]] == playerScores[playersInDraw[0]])
				Debug.Log("Ta empatado ainda segue o jogo");
		}
		else
		{
			int highScore = GetHighestScore();
			List<int> playersToRemove = new List<int>();

			foreach(int playerIndex in playersInDraw)
			{
				if (playerScores[playerIndex] < highScore)
					playersToRemove.Add(playerIndex);
			}

			foreach (int playerIndex in playersToRemove)
			{
				playersInDraw.Remove(playerIndex);
				Debug.Log("Jogador " + playerIndex.ToString() + "rodou");
			}
		}
	}

	void DrawModeSetup()
	{
		Debug.Log("EMPATE");
		drawMode = true;
		playersInDraw = new List<int>(GetHighestScoringPlayers());

		string jogadoresEmpatados = "";

		foreach(int p in playersInDraw)
		{
			jogadoresEmpatados += p.ToString() + ", ";
		}
		Debug.Log("Empatados: " + jogadoresEmpatados);

	}

	void EndGame(int[] winners)
	{
		endGameScreen.SetActive(true);
		if(winners.Length == 1)
		{
			endGameText.text = PlayersManager.playerName[winners[0]] +" GANHOU!!!1!11!";
			endGameText.color = PlayersManager.playerColor[winners[0]];
		}
		else
		{
			endGameText.text = "empate entre algumas pessoas aí";
		}
	}

	private bool AllPlayersPlayedTheSame()
	{
		int n = playerMatchesPlayed[0];

		foreach(int p in playerMatchesPlayed)
		{
			if (n != p)
			{
				return false;
			}
		}		
		return true;
	}

	private bool AllPlayersInTheDrawPlayedTheSame()
	{
		int n = playerMatchesPlayed[playersInDraw[0]];

		foreach(int p in playersInDraw)
		{
			if(n != playerMatchesPlayed[p])
			{
				return false;
			}
		}

		return true;

	}

	private bool IsPlayerInDraw(int player)
	{
		foreach (int p in playersInDraw)
		{
			if (p == player)
			{
				return true;
			}
		}
		return false;
	}

	private int NumberOfPlayersWithSpecificScore(int n)
	{		
		int count = 0;

		foreach(int p in playerScores)
		{
			if (p == n)
			{
				count++;
			}
		}
		Debug.Log("Finding players with " + n.ToString() + " points: " + count.ToString() + " players");
		return count;
	}

	private int[] GetPlayersWithSpecifcScore(int n)
	{
		Debug.Log("Trying to get players that scored " + n.ToString());
		int[] players;
		players = new int[NumberOfPlayersWithSpecificScore(n)];
		if (players.Length == 0)
		{
			return null;
		}

		int j = 0;

		for (int i = 0; i < playerScores.Count; i++)
		{
			if (playerScores[i] == n)
			{
				players[j] = i;
				j++;
			}
		}

		return players;
	}

	private int GetHighestScore()
	{
		int highest = 0;

		foreach (int p in playerScores)
		{
			if (p > highest)
				highest = p;
		}

		return highest;

	}

	private int[] GetHighestScoringPlayers()
	{
		return GetPlayersWithSpecifcScore(GetHighestScore());
	}

	private int[] GetPlayersThatPlayedLessThanTheRound()
	{
		int n = 0;

		foreach(int p in playerMatchesPlayed)
		{
			if (p < round)
				n++;
		}

		int[] players;
		players = new int[n];

		int j = 0;

		for(int i = 0; i < playerMatchesPlayed.Count; i++)
		{
			if (playerMatchesPlayed[i] < round)
			{
				players[j] = i;
				j++;
			}
		}

		return players;
	}

	private int[] GetPlayersThatPlayedEqualTheRound()
	{
		int n = 0;

		foreach (int p in playerMatchesPlayed)
		{
			if (p == round)
				n++;
		}

		int[] players;
		players = new int[n];

		int j = 0;

		for (int i = 0; i < playerMatchesPlayed.Count; i++)
		{
			if (playerMatchesPlayed[i] == round)
			{
				if((drawMode && IsPlayerInDraw(i)) || (!drawMode))
				{
					players[j] = i;
					j++;
				}
			}
		}

		return players;
	}

	private int[] GetPlayersThatPlayedMoreThanTheRound()
	{
		int n = 0;

		foreach (int p in playerMatchesPlayed)
		{
			if (p > round)
				n++;
		}

		int[] players;
		players = new int[n];

		int j = 0;

		for (int i = 0; i < playerMatchesPlayed.Count; i++)
		{
			if (playerMatchesPlayed[i] > round)
			{
				if((drawMode && IsPlayerInDraw(i)) || (!drawMode))
				{
					players[j] = i;
					j++;
				}

			}
		}

		return players;
	}

}
