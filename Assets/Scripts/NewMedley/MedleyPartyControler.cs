using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MedleyPartyControler : MonoBehaviour
{
	[Header("Icon Placement")]
	public GameObject playerIconPrefab;
	public Transform playerLayoutHolder;

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
	public List<int> playerScores;
	public List<int> playerMatchesPlayed;

	private void Awake()
	{
		medleySetup = FindObjectOfType<MedleySetup>();
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
		playerScores = new List<int>();
		playerMatchesPlayed = new List<int>();

		for(int i = 0;i < MedleySetup.nOfPlayers; i++)
		{
			GameObject p = Instantiate(playerIconPrefab, playerLayoutHolder);
			p.GetComponent<MeddleyPlayerIcon>().SetColor(PlayersManager.playerColor[i]);
			p.GetComponent<MeddleyPlayerIcon>().SetName(PlayersManager.playerName[i]);
			playerScores.Add(0);
			playerMatchesPlayed.Add(0);
		}

		SelectActivePlayers();
		SelectMinigame();
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
		}while (r == l) ;

		Debug.Log(l.ToString() + " vs. " + r.ToString());

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

}
