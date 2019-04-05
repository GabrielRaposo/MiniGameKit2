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

	public void SetColor(Color newColor)
	{
		colorBorder.color = newColor;
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
