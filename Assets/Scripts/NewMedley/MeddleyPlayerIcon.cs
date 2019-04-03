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
	
	public void SetColor(Color newColor)
	{
		colorBorder.color = newColor;
	}

	public void SetName(string name)
	{
		text.text = name;
	}

}
