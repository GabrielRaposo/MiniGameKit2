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
		DontDestroyOnLoad(this.gameObject);
	}

	private void Update()
	{
		if (Input.GetKeyDown("g"))
		{
			SelectActivePlayers();
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

		Debug.Log("Left " + PlayersManager.currentLeftPlayer.ToString() + ": Score " + playerScores[PlayersManager.currentLeftPlayer].ToString());
		Debug.Log("Right " + PlayersManager.currentRightPlayer.ToString() + ": Score " + playerScores[PlayersManager.currentRightPlayer].ToString());

		//SelectActivePlayers();
		//SelectMinigame();
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

	public void SelectActivePlayers()
	{
		//TODO fazer seleção considerando partidas já jogadas
		int r, l;

		r = Random.Range(0, MedleySetup.nOfPlayers);

		do
		{
			l = Random.Range(0, MedleySetup.nOfPlayers);
		} while (r == l) ;

		PlayersManager.currentLeftPlayer = l;
		PlayersManager.currentRightPlayer = r;

		DisplayActivePlayers();
	}

	public void SelectMinigame()
	{
		currentGame = games[Random.Range(0, games.Count)];
		DisplayMinigame();
	}

	public void GoToMinigame()
	{
		SceneManager.LoadScene(currentGame.codename);
	}

	void UpdateScores()
	{
		Debug.Log("UpdateScores()");

		switch (PlayersManager.result)
		{
			case PlayersManager.Result.Draw:
				Debug.Log("Draw");
				break;
			case PlayersManager.Result.LeftWin:
				Debug.Log("Left");
				playerScores[PlayersManager.currentLeftPlayer] = playerScores[PlayersManager.currentLeftPlayer] +1;
				break;
			case PlayersManager.Result.RightWin:
				Debug.Log("Righ");
				playerScores[PlayersManager.currentRightPlayer] = playerScores[PlayersManager.currentRightPlayer] +1;
				break;
		}

	}
		
}
