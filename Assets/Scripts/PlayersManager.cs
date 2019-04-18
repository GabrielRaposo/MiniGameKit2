using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersManager : MonoBehaviour {

	public static Color[] playerDefaultColor = new Color[]
	{
		new Color(0f, 0.682353f, 1f),    // Azul Claro
        new Color(1f, 0.6f, 0.1058824f), // Laranja
        new Color(.957f, .259f, .259f),  // Vermelho
        new Color(.137f, .627f, .184f),   // Verde Escuro
		new Color(0.5f,0.25f,0),
		new Color(0,1,1),
		new Color(1,0,1),
		new Color(1,1,0),
		new Color(0,1,0),
		new Color(0.5f,0.25f,1),
		new Color(0.5f,1,0.5f),
		new Color(0,0.5f,0.75f)
	};

	public static string[] playerDefaultName = new string[]
	{
		"Player Um",
		"Player Dois",
		"Player Tres",
		"Player Quatro",
		"Player Cienco",
		"Player Siex",
		"Player 7",
		"Player 8",
		"Player 9",
		"Player 10",
		"Player 11",
		"Player 12",
	};

    public static Color[] playerColor = playerDefaultColor;

	public static string[] playerName = playerDefaultName;

    public static int currentLeftPlayer = 0;
    public static int currentRightPlayer = 1;

    public enum Result { Draw, LeftWin, RightWin }
    public static Result result;
}
