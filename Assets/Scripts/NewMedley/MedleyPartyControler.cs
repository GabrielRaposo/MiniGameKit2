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

	public static bool partyInProgress;
	public static bool drawMode;

	public static int round;
	public static bool changeRoundNext;

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
			Debug.Log("Starting New Party()");
			round = 0;
			partyInProgress = true;
			playerScores = new List<int>();
			playerMatchesPlayed = new List<int>();

			for (int i = 0; i < MedleySetup.nOfPlayers; i++)
			{				
				GameObject p = Instantiate(playerIconPrefab, playerLayoutHolder);
				playerIcons.Add(p);
				playerScores.Add(0);
				p.GetComponent<MeddleyPlayerIcon>().SetColor(PlayersManager.playerColor[i]);
				p.GetComponent<MeddleyPlayerIcon>().SetName(PlayersManager.playerName[i]);
				p.GetComponent<MeddleyPlayerIcon>().SetScore(playerScores[i]);
				playerMatchesPlayed.Add(0);
			}

		}

		else
		{
			UpdateScores();
			for (int i = 0;i < MedleySetup.nOfPlayers; i++)
			{
				GameObject p = Instantiate(playerIconPrefab, playerLayoutHolder);
				p.GetComponent<MeddleyPlayerIcon>().SetColor(PlayersManager.playerColor[i]);
				p.GetComponent<MeddleyPlayerIcon>().SetName(PlayersManager.playerName[i]);
				p.GetComponent<MeddleyPlayerIcon>().SetScore(playerScores[i]);
				playerIcons.Add(p);
			}
		}

		if (changeRoundNext)
			ChangeRound();
		
		SelectActivePlayers();
		//SelectMinigame();
	}

	public void ChangeRound()
	{
		round++;
		changeRoundNext = false;
		Debug.Log("ChangingRound");
	}
		
	public void SelectActivePlayers()
	{

		int safe = 0;

		int[] lowPool = GetPlayersThatPlayedEqualTheRound();
		int[] moreThanPool = GetPlayersThatPlayedMoreThanTheRound();

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
				safe++;
				PlayersManager.currentLeftPlayer = lowPool[Random.Range(0, lowPool.Length)];
				if (safe > 500)
					Debug.Break();
			} while (PlayersManager.currentRightPlayer == PlayersManager.currentLeftPlayer);

		}

		DisplayActivePlayers();

		////TODO fazer seleção considerando partidas já jogadas


		//int r, l;

		//r = Random.Range(0, MedleySetup.nOfPlayers);

		//do
		//{
		//	l = Random.Range(0, MedleySetup.nOfPlayers);
		//} while (r == l) ;

		//PlayersManager.currentLeftPlayer = l;
		//PlayersManager.currentRightPlayer = r;


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

		CheckIfGameOver();

	}

	void CheckIfGameOver()
	{

		int n = MedleySetup.nOfVictories;

		switch (MedleySetup.mode)
		{
			case MedleyModes.NumberOfGames:
				Debug.Log("Played the same: "+AllPlayersPlayedTheSame().ToString());
				Debug.Log("Player 0  matches: " + playerMatchesPlayed[0].ToString());
				if (AllPlayersPlayedTheSame() && playerMatchesPlayed[0] == n)
				{
					if (GetHighestScoringPlayers().Length == 1)
						EndGame(GetHighestScoringPlayers());
					else
					{
						Debug.Log("Desempate");
						//Desempate
					}
				}
				else
					Debug.Log("Segue o jogo");
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
							Debug.Log("Desempate");
							//Desempate
						}
					}
					else
					{
						//TODO Alertazinho de pode acabar
					}
				}
				else
				{
					Debug.Log("Segue o jogo");
				}
				break;
		}
	}

	void EndGame(int[] winners)
	{
		Debug.Log("Acabou o jogo ganhou : " + winners.ToString());
	}

	private bool AllPlayersPlayedTheSame()
	{
		int n = playerMatchesPlayed[0];

		foreach(int p in playerMatchesPlayed)
		{
			if (n != p)
				return false;
		}

		return true;
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
		return count;
	}

	private int[] GetPlayersWithSpecifcScore(int n)
	{
		int[] players;
		players = new int[NumberOfPlayersWithSpecificScore(n)];
		if (players.Length == 0)
			return null;

		int j = 0;

		for (int i = 0; i < playerScores.Count; i++)
		{
			if (playerScores[i] < round)
			{
				players[j] = i;
				j++;
			}
		}

		return players;
	}

	private int[] GetHighestScoringPlayers()
	{
		int highest = 0;

		foreach (int p in playerScores)
		{
			if (p > highest)
				highest = p;
		}

		return GetPlayersWithSpecifcScore(highest);

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
				players[j] = i;
				j++;
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
				players[j] = i;
				j++;
			}
		}

		return players;
	}

}
