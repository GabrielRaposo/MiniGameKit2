using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePoolMenuButtonPlaylist : GamePoolMenuButton
{
	public SequencialMedleyMenuControler menuControler;

	public override void ButtonPress()
	{
		AddToPlaylist();
	}

	void AddToPlaylist()
	{
		menuControler.ReciveGameToAdd(this);
	}

}
