using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Words
{
	public class WordsPlayer : PlayerInfo
	{
		private Color visibleColor;

		public Color VisibleColor
		{
			get
			{
				return color;
			}

			set
			{
				//color = value;
			}
		}

		private void Awake()
		{			
			base.Start();
			//base.Awake();
		}

		new private void Start()
		{
			
		}


	}
}
