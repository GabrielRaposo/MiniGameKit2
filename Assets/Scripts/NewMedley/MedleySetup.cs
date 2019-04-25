using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public enum MedleyModes { NumberOfGames, NumberOfVictories};

public class MedleySetup : MonoBehaviour
{
	public static MedleySetup i;
	public TextMeshProUGUI playerNumbersDisplay;
	public TextMeshProUGUI gameTypeDisplay;
	public TextMeshProUGUI numberOfVictoriesDisplay;

	public static int nOfPlayers;
	public static int nOfVictories;
	public static MedleyModes mode;

	public MeddleyPlayerIcon[] meddleyPlayerIcons;
	
	private void Awake()
	{
		if (i == null)
		{
			i = this;
			nOfPlayers = 2;
			nOfVictories = 1;
			mode = MedleyModes.NumberOfGames;
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

		if (mode == MedleyModes.NumberOfGames && (nOfPlayers % 2 != 0))
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

		int lenght = System.Enum.GetValues(typeof(MedleyModes)).Length;

		mode = (MedleyModes)(value % lenght);

		if (mode == MedleyModes.NumberOfGames && nOfPlayers % 2 != 0 && nOfVictories % 2 != 0)
		{
			nOfVictories++;
			UpdateNOfVictoriesDisplay();
		}

		UpdateGameTypeDisplay();
	}	

	public void SettupPlayerDisplay()
	{
		for (int i = 0; i < meddleyPlayerIcons.Length; i++)
		{
			MeddleyPlayerIcon mpi = meddleyPlayerIcons[i];

			if (i >= nOfPlayers)
				mpi.gameObject.SetActive(false);
			else
			{
				mpi.gameObject.SetActive(true);
				mpi.SetColor(PlayersManager.playerColor[i]);
				mpi.SetName(PlayersManager.playerName[i]);
				mpi.HideScore();
			}
		}
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
			case MedleyModes.NumberOfGames:
				gameTypeDisplay.text = "Numero de jogos";
				break;
			case MedleyModes.NumberOfVictories:
				gameTypeDisplay.text = "Numero de Vitorias";
				break;
		}

	}
}