﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public enum MedleyModes { NumberOfGames, NumberOfVictories};

public class MedleySetup : MonoBehaviour
{	
	public TextMeshProUGUI playerNumbersDisplay;
	public TextMeshProUGUI gameTypeDisplay;
	public TextMeshProUGUI numberOfVictoriesDisplay;

	public static int nOfPlayers;
	public static int nOfVictories;
	public MedleyModes mode;

	public MeddleyPlayerIcon[] meddleyPlayerIcons;
	
	private void Awake()
	{
		nOfPlayers = 2;
		nOfVictories = 1;
		mode = MedleyModes.NumberOfGames;

		UpdatePlayerNumberDisplay();
		UpdateGameTypeDisplay();
		UpdateNOfVictoriesDisplay();
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
		//TODO matemática para equilibrar numeros de jogos
		nOfVictories += i;
		if (nOfVictories < 1)
			nOfVictories = 1;
		UpdateNOfVictoriesDisplay();
	}

	public void SwitchMode(int i)
	{
		int value = (int)mode;
		value += i;

		int lenght = System.Enum.GetValues(typeof(MedleyModes)).Length;

		mode = (MedleyModes)(value % lenght);

		//if( value >= System.Enum.GetValues(typeof(MedleyModes)).Length)
		//{
		//	mode = 0;
		//}
		//if(value == -1)
		//{
		//	mode = (MedleyModes)((System.Enum.GetValues(typeof(MedleyModes)).Length)-1);
		//}


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