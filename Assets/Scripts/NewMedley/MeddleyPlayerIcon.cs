using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MeddleyPlayerIcon : MonoBehaviour
{
	public Image icon;
	public Image colorBorder;
	public TextMeshProUGUI text;
	public TextMeshProUGUI scoreText;

	private int playerIndex;
	public int colorIndex;

	public void Init(int i)
	{
		playerIndex = i;
		colorIndex = i;
		RestoreColor();
	}
	
	public void SetColorIndex(int color)
	{
		colorIndex = color;
		PlayersManager.SetPlayerColor(playerIndex, color);
		RestoreColor();
	}

	public void ForceColor(Color color)
	{
		colorBorder.color = color;
		icon.color = color;
	}

	public void RestoreColor()
	{
		Color c = PlayersManager.GetPlayerColor(playerIndex);
		colorBorder.color = c;
		icon.color = c;
	}

	public void SetName(string name)
	{
		text.text = name;
	}

	public void SetScore(int score)
	{
		scoreText.text = score.ToString();
	}

	public void HideScore()
	{
		scoreText.text = "";
	}

}
