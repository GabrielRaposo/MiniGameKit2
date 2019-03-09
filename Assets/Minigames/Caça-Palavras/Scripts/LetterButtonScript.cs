using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Words
{
	public class LetterButtonScript : MonoBehaviour
	{
		public char letter;
		public int player;

		public void SendInput()
		{
			FindObjectOfType<WordsGameControler>().TryInputLetter(letter, player);
		}
	}
}
