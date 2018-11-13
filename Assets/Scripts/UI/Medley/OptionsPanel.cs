using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsPanel : MonoBehaviour {

    public ToggleUIButton fullscreenButton;
    public ToggleUIButton muteButton;

    void Start ()
    {
        fullscreenButton.On = Screen.fullScreen;

        if (PlayerPrefs.HasKey("MUTE"))
        {
            Debug.Log("=== Pre-set ===");
            Debug.Log("PlayerPrefs.GetInt('MUTE'): " + PlayerPrefs.GetInt("MUTE"));

            muteButton.On = (PlayerPrefs.GetInt("MUTE") == 1) ? true : false;
        }
        PlayerPrefs.SetInt("MUTE", muteButton.On ? 1 : 0);
    }
	
	public void SwitchFullscreen()
    {
        fullscreenButton.On = !fullscreenButton.On;
        Screen.fullScreen = fullscreenButton.On;
    }

    public void SwitchMute()
    {
        if(muteButton.On = !muteButton.On)
        {
            AudioListener.volume = 0;
            PlayerPrefs.SetInt("MUTE", 1);
        }
        else
        {
            AudioListener.volume = 1;
            PlayerPrefs.SetInt("MUTE", 0);
        }
    }
}
