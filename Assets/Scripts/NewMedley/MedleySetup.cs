using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public enum PartyMode { NumberOfGames, NumberOfVictories};

public class MedleySetup : MonoBehaviour
{
	public static MedleySetup i;
	public TextMeshProUGUI playerNumbersDisplay;
	public TextMeshProUGUI gameTypeDisplay;
	public TextMeshProUGUI numberOfVictoriesDisplay;

	public static int nOfPlayers;
	public static int nOfVictories;
	public static PartyMode mode;

	public MeddleyPlayerIcon[] meddleyPlayerIcons;

	private List<int> colorsInUse;
	
	[Header("Layouts")]
	
	[SerializeField] private Transform topRow;
	[SerializeField] private Transform bottomRow;
	
	private void Awake()
	{
		if (i == null)
		{
			i = this;
			nOfPlayers = 2;
			nOfVictories = 1;
			mode = PartyMode.NumberOfGames;
		}
		else
		{
			if (i != this)
			{
				Destroy(this.gameObject);
			}
		}

		UpdatePlayerNumberDisplay();
		UpdateGameTypeDisplay();
		UpdateNOfVictoriesDisplay();
		DontDestroyOnLoad(this);
	}

	public void SwitchPlayerNumber(int i)
	{
		nOfPlayers += i;
		if (nOfPlayers > 12)
			nOfPlayers = 12;
		if (nOfPlayers < 2)
			nOfPlayers = 2;
		UpdatePlayerNumberDisplay();
	}

	public void SwitchVictoryN(int i)
	{

		if (mode == PartyMode.NumberOfGames && (nOfPlayers % 2 != 0))
			i = i * 2;

		nOfVictories += i;
		if (nOfVictories < 1)
			nOfVictories = 1;

		if (nOfVictories < 2 && (nOfPlayers % 2 != 0))
			nOfVictories = 2;

		UpdateNOfVictoriesDisplay();
	}

	public void SwitchMode(int i)
	{
		int value = (int)mode;
		value += i;

		int enumLenght = System.Enum.GetNames(typeof(PartyMode)).Length;
		
		if (value < 0)
			mode = (PartyMode) (enumLenght - 1);
		else if(value >= enumLenght)
			mode = (PartyMode) 0;
		else
			mode = (PartyMode) value;

		if (mode == PartyMode.NumberOfGames && nOfPlayers % 2 != 0 && nOfVictories % 2 != 0)
		{
			nOfVictories++;
			UpdateNOfVictoriesDisplay();
		}

		UpdateGameTypeDisplay();
	}	

	public void SetupPlayerDisplay()
	{
		colorsInUse = new List<int>();
		
		if(nOfPlayers<=6)
			bottomRow.gameObject.SetActive(false);
		else
			bottomRow.gameObject.SetActive(true);
		
		for (int i = 0; i < meddleyPlayerIcons.Length; i++)
		{
			MeddleyPlayerIcon mpi = meddleyPlayerIcons[i];

			if (i >= nOfPlayers)
				mpi.gameObject.SetActive(false);
			else
			{
				mpi.gameObject.SetActive(true);
				mpi.Init(i);
				colorsInUse.Add(i);
				mpi.SetName(PlayersManager.playerName[i]);
				mpi.HideScore();

				var index = i;
				
				var buttonClickedEvent = mpi.GetComponent<Button>().onClick;
				buttonClickedEvent.RemoveAllListeners();
				buttonClickedEvent.AddListener(()=>SwapColor(index));
				
			}
		}
	}

	private void SwapColor(int player)
	{
		if(colorsInUse.Count >= PlayersManager.ColorCount) return;
		
		Debug.Log("---");

		string debugS = "";
		foreach (var i1 in colorsInUse)
		{
			debugS += i1.ToString() + ", ";
		}
		Debug.Log(debugS);
		
		var icon = meddleyPlayerIcons[player];

		int starting = icon.colorIndex;
		int i = icon.colorIndex;
		
		i++;

		if (i >= PlayersManager.ColorCount)
			i = 0;
		
		Debug.Log($"i={i}");

		while (colorsInUse.Contains(i))
		{
			i++;
			if (i >= PlayersManager.ColorCount)
				i = 0;
			Debug.Log($"i={i}");
		}

		icon.SetColorIndex(i);
		colorsInUse.Remove(starting);
		colorsInUse.Add(i);
	}

	public void UpdatePlayerNumberDisplay()
	{
		playerNumbersDisplay.text = nOfPlayers.ToString();
	}
	public void UpdateNOfVictoriesDisplay()
	{
		numberOfVictoriesDisplay.text = nOfVictories.ToString();
	}
	public void UpdateGameTypeDisplay()
	{
		switch (mode)
		{
			case PartyMode.NumberOfGames:
				gameTypeDisplay.text = "Numero de jogos";
				break;
			case PartyMode.NumberOfVictories:
				gameTypeDisplay.text = "Numero de Vitorias";
				break;
		}

	}
}