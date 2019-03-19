using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Bailarinas
{
	public class TextScript : MonoBehaviour
	{
		public TextMeshProUGUI text;
		Animator animator;

		public bool startedGame = false;

		private void Awake()
		{
			animator = GetComponent<Animator>();
		}

		public void AUXDAUNITYEUMERDASetText(string newText)
		{
			text.text = newText;
		}

		public void AUXDAUNITYEUMERDAToggleStartGame()
		{
			startedGame = true;
		}

		public void StartCoutDownAnimation()
		{
			animator.SetTrigger("Start");
		}
	}
}


