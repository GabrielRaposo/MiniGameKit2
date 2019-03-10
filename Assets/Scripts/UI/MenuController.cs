using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MenuController : MonoBehaviour
{
	[Header("References")]
	
	[SerializeField] EventSystem eventSystem;
	GameObject lastSelectedObject;

	[System.Serializable]
	private struct Menu
	{
		public Transform menuTransform;
		public GameObject firstButton;
		public string previousMenu;
	}
	
	[System.Serializable]
	private struct Overlay
	{
		public Transform overlayTransform;
		public GameObject firstButton;
	}

	[SerializeField] Menu currentMenu;
	[SerializeField] Overlay currentOverlay;

	[Header("Menus")]

    [SerializeField] private Menu startUpMenu;
	[SerializeField] private Menu mainMenu;
	[SerializeField] private Menu medleyMenu;
	[SerializeField] private Menu freeplayMenu;
    [SerializeField] private Menu controllerMenu;

	[Space(10)]

    [Header("Overlays")]
	
	[SerializeField] private Overlay controllerOverlay;
	[SerializeField] private Overlay confirmOverlay;
	[SerializeField] private Overlay medleySettingOverlay;
    [SerializeField] private Overlay medleyGameOverlay;
    [SerializeField] private Overlay tutorialOverlay;
    [SerializeField] private Overlay optionsOverlay;

    [Space(10)]

    [Header("Startup Sequence")]
    [SerializeField] private GameObject startupScreen;
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private Animator wipe;
    [SerializeField] private Animation wipeAnim;

    [Header("Options")]
    [SerializeField] private OptionsPanel optionsPanel;

    private bool hasActiveOverlay = false;

    static bool hasSetupControllers;
    static public string FirstScreen = "startup";
	
	void Start ()
	{
        //SwitchMenu(FirstScreen);
        ModeManager.State = ModeManager.GameState.FreePlay;

        if (PlayerPrefs.HasKey("MUTE"))
        {
            AudioListener.volume = (PlayerPrefs.GetInt("MUTE") == 1) ? 0 : 1;
        }
        GetComponent<AudioSource>().Play();
    }
	
	void Update ()
	{
        if (!hasActiveOverlay && eventSystem.currentSelectedGameObject == null)
        {
            //temp pra lidar com mouse chato
            eventSystem.SetSelectedGameObject(currentMenu.firstButton);
        }

        if (Input.GetButtonDown("Cancel"))
		{
            if (hasActiveOverlay) DisableOverlay();
            else SwitchMenu(currentMenu.previousMenu);
		}
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void SwitchMenu(string menu)
	{
		switch (menu)
		{
			case "startup":
				currentMenu.menuTransform.gameObject.SetActive(false);
				currentMenu = startUpMenu;
				currentMenu.menuTransform.gameObject.SetActive(true);
				eventSystem.SetSelectedGameObject(currentMenu.firstButton);
                //FirstScreen = "main";
                break;
			case "main":
                /*
				currentMenu.menuTransform.gameObject.SetActive(false);
				currentMenu = mainMenu;
				currentMenu.menuTransform.gameObject.SetActive(true);
				eventSystem.SetSelectedGameObject(currentMenu.firstButton);
                */

                StartCoroutine(WipeAnimation(mainMenu));
                //if (!hasSetupControllers)
                //{
                //    EnableOverlay("controller");
                //    hasSetupControllers = true;
                //}
                break;
			case "freeplay":
                //currentMenu.menuTransform.gameObject.SetActive(false);
                //currentMenu = freeplayMenu;
                //currentMenu.menuTransform.gameObject.SetActive(true);
                //eventSystem.SetSelectedGameObject(currentMenu.firstButton);
                StartCoroutine(WipeAnimation(freeplayMenu));
                ModeManager.State = ModeManager.GameState.FreePlay;
                break;

            case "controller":
                StartCoroutine(WipeAnimation(controllerMenu));
                break;
		}
	}

    private IEnumerator WipeAnimation(Menu targetMenu)
    {
        wipe.SetTrigger("Wiping");
        yield return new WaitForSeconds(.25f);
        currentMenu.menuTransform.gameObject.SetActive(false);
        targetMenu.menuTransform.gameObject.SetActive(true);
        currentMenu = targetMenu;
        eventSystem.SetSelectedGameObject(currentMenu.firstButton);
    }

	public void EnableOverlay(string overlay)
	{
		switch (overlay)
		{
			case "controller":
				currentOverlay = controllerOverlay;
				break;

			case "confirm":
				currentOverlay = confirmOverlay;
				break;

            case "tutorial":
				currentOverlay = tutorialOverlay;
				break;

            case "options":
                currentOverlay = optionsOverlay;
                break;

            default:
                Debug.Log("Titulo de overlay incorreto.");
                return;
		}
		hasActiveOverlay = true;
		currentOverlay.overlayTransform.gameObject.SetActive(true);
		lastSelectedObject = eventSystem.currentSelectedGameObject;
		eventSystem.SetSelectedGameObject(currentOverlay.firstButton);
	}

	public void DisableOverlay()
	{
		hasActiveOverlay = false;
		eventSystem.SetSelectedGameObject(lastSelectedObject);
		currentOverlay.overlayTransform.gameObject.SetActive(false);
	}

    public void EnableOptionsPanel()
    {
        hasActiveOverlay = true;
        optionsPanel.gameObject.SetActive(true);
        eventSystem.SetSelectedGameObject(optionsOverlay.firstButton);
        optionsPanel.TransitionIn();
    }

    public void DisableOptionsPanel()
    {
        hasActiveOverlay = false;
        eventSystem.SetSelectedGameObject(lastSelectedObject);
        optionsPanel.TransitionOut();
    }

    public void CallScene(string scene)
    {
        switch (scene)
        {
            case "medley":
                ModeManager.State = ModeManager.GameState.Medley;
                StartCoroutine(ModeManager.TransitionFromMinigame());
                break;

            default:
                Debug.Log("Titulo de overlay incorreto.");
                return;
        }
    }
}
