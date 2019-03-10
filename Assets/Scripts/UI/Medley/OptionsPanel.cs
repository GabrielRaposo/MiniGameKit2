using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OptionsPanel : MonoBehaviour
{
    [Header("Buttons")]
    public ToggleUIButton fullscreenButton;
    public ToggleUIButton muteButton;

    [Header("Transition")]
    [SerializeField] private float transitionTime;
    [SerializeField] private RectTransform movementAnchor;
    [SerializeField] private RawImage fadeScreen;

    private Vector3 startingPosition;
    private Color startingColor;

    private void Awake()
    {
        startingPosition = movementAnchor.anchoredPosition;
        startingColor = fadeScreen.color;

        movementAnchor.anchoredPosition += Vector2.right * 840;
        fadeScreen.color = Vector4.zero;
    }

    public void TransitionIn()
    {
        movementAnchor.DOAnchorPosX(startingPosition.x, transitionTime);
        fadeScreen.DOColor(startingColor, transitionTime);
    }

    public void TransitionOut()
    {
        movementAnchor.DOAnchorPosX(840, transitionTime);
        fadeScreen.DOColor(Vector4.zero, transitionTime).onComplete = (() => gameObject.SetActive(false));
    }

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
