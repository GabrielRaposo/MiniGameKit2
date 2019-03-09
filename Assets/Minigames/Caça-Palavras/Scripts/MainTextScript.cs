using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainTextScript : MonoBehaviour
{
	private AudioSource audio;
	private TextMeshProUGUI tmpro;

	public float clock = 0 , speed = 1, tickRate;

	public List<TMP_FontAsset> fonts;
	private int fontIndex;

	public int FontIndex
	{
		get
		{
			return fontIndex;
		}

		set
		{
			Debug.Log(value.ToString() + " > " + (fonts.Count-1).ToString());
			if (value >= fonts.Count)
			{
				fontIndex = (value % fonts.Count);
			}
			else
				fontIndex = value;

			tmpro.font = fonts[fontIndex];
		}
	}

	private void Awake()
	{
		audio = GetComponent<AudioSource>();
		tmpro = GetComponent<TextMeshProUGUI>();

		FontIndex = Random.Range(0, fonts.Count);
	}

	private void Update()
	{
		clock += Time.deltaTime * speed;

		if(clock >= tickRate)
		{
			clock = 0;
			audio.Play();
			FontIndex++;
		}
	}
}
