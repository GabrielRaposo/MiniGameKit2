using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersManager : MonoBehaviour {

	public static Color[] playerDefaultColor = new Color[]
	{
		new Color(0f, 0.682353f, 1f),    // Azul 
        new Color(1f, 0.6f, 0.1058824f), // Laranja
        new Color(.9451f, .1372f, .2470f),  // Vermelho
        new Color(.9333f, .8784f, .3568f),   // Amarelo
		new Color(.1922f, .7019f, .1686f), //Verde
		new Color(.5059f, .8666f, .8980f), //Azul Claro
		new Color(.7765f, .0784f, .7176f), //Fuxia
		new Color(.9529f, .2118f, .6235f), //Rosa
		new Color(.1765f, .1725f, .1922f), //Basicamente Preto
		new Color(.6039f, .4039f, .1804f), //Marrom
		new Color(.0392f, .1137f, .4157f), //Azul Escuro
		new Color(.5019f, .5019f, .5019f) //Literalmente cinza
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
