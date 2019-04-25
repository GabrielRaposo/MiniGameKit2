using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DebugerGame
{

	public class DebugerPlayer : PlayerInfo
	{
		public bool playerSide;

		private void Awake()
		{
			base.Start();
			GetComponent<Image>().color = color;
		}

		private void Update()
		{
			if (Input.GetButtonDown(playerButtons.action))
			{
				if (playerSide)
				{
					PlayersManager.result = PlayersManager.Result.RightWin;					
				}
				else
				{
					PlayersManager.result = PlayersManager.Result.LeftWin;
				}
				StartCoroutine(ModeManager.TransitionFromMinigame());
			}

			if (Input.GetKeyDown("q"))
			{
				PlayersManager.result = PlayersManager.Result.Draw;
				StartCoroutine(ModeManager.TransitionFromMinigame());
			}
		}

	}

}

