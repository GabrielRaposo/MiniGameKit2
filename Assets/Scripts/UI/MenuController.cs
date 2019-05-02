using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
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
		//public GameObject firstButton;
		public string previousMenu;
	}
	
	[System.Serializable]
	private struct Overlay
	{
		public Transform overlayTransform;
		//public GameObject firstButton;
	}

	[SerializeField] Menu currentMenu;
	[SerializeField] Overlay currentOverlay;

	[Header("Menus")]

    [SerializeField] private Menu startUpMenu;
	[SerializeField] private Menu mainMenu;
	[FormerlySerializedAs("medleyMenu")]
	[SerializeField] private Menu partyModeMenu;
	[SerializeField] private Menu freeplayMenu;
    [SerializeField] private Menu controllerMenu;

	[Space(10)]

    [Header("Overlays")]
	
	[SerializeField] private Overlay controllerOverlay;
	[SerializeField] private Overlay confirmOverlay;
	[FormerlySerializedAs("medleySettingOverlay")]
	[SerializeField] private Overlay partyModeSettingOverlay;
	[FormerlySerializedAs("medleyGameOverlay")]
	[SerializeField] private Overlay partyModeGameOverlay;
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
        StartMenu(FirstScreen);
        ModeManager.State = ModeManager.GameState.FreePlay;

        if (PlayerPrefs.HasKey("MUTE"))
        {
            AudioListener.volume = (PlayerPrefs.GetInt("MUTE") == 1) ? 0 : 1;
        }
        GetComponent<AudioSource>().Play();
    }
	
	void Update ()
	{
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

    private void StartMenu(string menu)
    {
        currentMenu.menuTransform.gameObject.SetActive(false);

        switch (menu)
        {
            case "startup":
                currentMenu = startUpMenu;
                currentMenu.menuTransform.gameObject.SetActive(true);
                eventSystem.SetSelectedGameObject(startUpMenu.menuTransform.GetComponent<SelectButtonOnEnable>().firstSelection);
                break;

            case "main":
                FirstScreen = "main";
                currentMenu = mainMenu;
                currentMenu.menuTransform.gameObject.SetActive(true);
                eventSystem.SetSelectedGameObject(mainMenu.menuTransform.GetComponent<SelectButtonOnEnable>().firstSelection);
                break;

            case "freeplay":
                currentMenu = freeplayMenu;
                currentMenu.menuTransform.gameObject.SetActive(true);
                ModeManager.State = ModeManager.GameState.FreePlay;
                break;

            case "controller":
                mainMenu.menuTransform.GetComponent<SelectButtonOnEnable>().firstSelection = lastSelectedObject;
                currentMenu = controllerMenu;
                currentMenu.menuTransform.gameObject.SetActive(true);
                break;
        }
    }

	public void SwitchMenu(string menu)
	{
		switch (menu)
		{
			case "startup":
                StartCoroutine(WipeAnimation(startUpMenu));
                eventSystem.SetSelectedGameObject(startUpMenu.menuTransform.GetComponent<SelectButtonOnEnable>().firstSelection);
                break;

			case "main":
                StartCoroutine(WipeAnimation(mainMenu));
                FirstScreen = "main";
                //if (!hasSetupControllers)
                break;

			case "freeplay":
                StartCoroutine(WipeAnimation(freeplayMenu));
                ModeManager.State = ModeManager.GameState.FreePlay;
                FirstScreen = "freeplay";
                break;

            case "controller":
                mainMenu.menuTransform.GetComponent<SelectButtonOnEnable>().firstSelection = lastSelectedObject;
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
		lastSelectedObject = eventSystem.currentSelectedGameObject;
        currentOverlay.overlayTransform.gameObject.SetActive(true);
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
        lastSelectedObject = eventSystem.currentSelectedGameObject;
        optionsPanel.gameObject.SetActive(true);
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
            case "party":
                ModeManager.State = ModeManager.GameState.Party;
                StartCoroutine(ModeManager.TransitionFromMinigame());
                break;

            default:
                Debug.Log("Titulo de overlay incorreto.");
                return;
        }
    }
}
