using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePoolMenuButton : MonoBehaviour
{
    public Image marker;
	public Image gameLogo;
	private bool isOn;
	public TutorialObject game;

	public bool IsOn
	{
		get
		{
			return isOn;
		}

		set
		{
			isOn = value;

			if (isOn)
			{
				marker.enabled = true;
			}
			else
			{
				marker.enabled = false;
			}

		}
	}

	public virtual void ButtonPress()
	{
		ToggleButton();
	}

	public void ToggleButton()
	{
		if (IsOn)
			IsOn = false;
		else
			IsOn = true;
	}

	public void SetupButton(TutorialObject inputGame)
	{
		game = inputGame;
		gameLogo.sprite = Sprite.Create(inputGame.icon, new Rect(0,0,inputGame.icon.width, inputGame.icon.height), new Vector2(0.5f, 0.5f));
		IsOn = true;
		// gameLogo = inputGame.icon;

	}

}
