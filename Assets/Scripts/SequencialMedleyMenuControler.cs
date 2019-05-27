using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


//Raposo
//Sendo utilizado, controla os menus do PartyMode
public class SequencialMedleyMenuControler : MonoBehaviour
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
    [SerializeField] private Menu titleMenu;
    [SerializeField] private Menu colorSelectionMenu;
    [SerializeField] private Menu gameSelectionMenu;

    [Space(10)]
    [Header("Overlays")]

    [SerializeField] private Overlay infoOverlay;
    [SerializeField] private Overlay settingsOverlay;
    [SerializeField] private Overlay tutorialOverlay;

    private bool hasActiveOverlay = false;

    static bool hasSetupControllers;
    static public string FirstScreen = "title";

	public SequencialMedleyControler medleyControler;
	public SequencialMedleySettings medleySettings;

	[Header("Game menu")]
	public GameObject gamePoolMenuButtonShuffle;
	public GameObject gamePoolMenuButtonPlaylist;
	public GameObject gamePageBackground;
	public GameObject playlistBar;
	bool gameMenuReady = false;

    private void Start()
    {
        //FirstScreen = "freeplay";
        SwitchMenu(FirstScreen);
        ModeManager.State = ModeManager.GameState.Party;
		medleyControler = FindObjectOfType<SequencialMedleyControler>();
		medleySettings = FindObjectOfType<SequencialMedleySettings>();
    }

    void Update()
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

    public void SwitchMenu(string menu)
    {
        if (menu == "")
            return;

        currentMenu.menuTransform.gameObject.SetActive(false);
        switch (menu)
        {
            case "title":
                currentMenu = titleMenu;
                break;
            case "color":
                currentMenu = colorSelectionMenu;
                break;
            case "games":
                currentMenu = gameSelectionMenu;
				if (!gameMenuReady)
					SetUpGameMenu();

                break;
            case "main menu":
                CallScene("main menu");
                break;
            default:
                return;
        }
        eventSystem.SetSelectedGameObject(null);
        currentMenu.menuTransform.gameObject.SetActive(true);
        eventSystem.SetSelectedGameObject(currentMenu.firstButton);
    }

    public void MenuReturn()
    {
        SwitchMenu(currentMenu.previousMenu);
    }

	public void SetUpGameMenu()
	{

		TutorialObject[] tutorialObjects;
		tutorialObjects = Resources.LoadAll<TutorialObject>("Tutorials");

		if(medleySettings.gameMode == MedleyGameType.Shuffle)
		{

			foreach(TutorialObject TO in tutorialObjects)
			{
				GameObject button;
				GamePoolMenuButton buttonScript;

				button = Instantiate(gamePoolMenuButtonShuffle, gamePageBackground.transform);
				buttonScript = button.GetComponent<GamePoolMenuButton>();

				buttonScript.SetupButton(TO);

			}

		}

		else
		{
			foreach (TutorialObject TO in tutorialObjects)
			{
				GameObject button;
				GamePoolMenuButtonPlaylist buttonScript;

				button = Instantiate(gamePoolMenuButtonPlaylist, gamePageBackground.transform);
				buttonScript = button.GetComponent<GamePoolMenuButtonPlaylist>();

				if(buttonScript == null)
				{
					Debug.Log("buttonScript null");
					Debug.Break();
				}

				buttonScript.SetupButton(TO);
				buttonScript.menuControler = this;

			}
		}

		//gameMenuReady = true;


	}

    public void EnableOverlay(string overlay)
    {
        switch (overlay)
        {
            case "info":
                currentOverlay = infoOverlay;
                break;

            case "settings":
                currentOverlay = settingsOverlay;
                break;

            case "tutorial":
                currentOverlay = tutorialOverlay;
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

    public void CallScene(string scene)
    {
        switch (scene)
        {
            case "main menu":
                FirstScreen = "title";
                ModeManager.State = ModeManager.GameState.Menu;
                MenuController.FirstScreen = "main";
                StartCoroutine(ModeManager.TransitionFromMinigame());
                break;

            default:
                Debug.Log("Titulo de overlay incorreto.");
                return;
        }
    }

	public void ReciveGameToAdd(GamePoolMenuButtonPlaylist button)
	{
		SequencialMedleyControler.currentGameList.Add(button.game);
		Instantiate(button, playlistBar.transform);
	}

}
