﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Raposo
	//Sendo utilizado, controla os menus
public class MedleyController : MonoBehaviour
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
    [SerializeField] private Menu mainMenu;
	[SerializeField] private Menu settingsMenu;
	[SerializeField] private Menu playerSettingsMenu;

    [Space(10)]
    [Header("Overlays")]

    [SerializeField] private Overlay infoOverlay;
    [SerializeField] private Overlay settingsOverlay;
    [SerializeField] private Overlay tutorialOverlay;

    private bool hasActiveOverlay = false;

    static bool hasSetupControllers;
    static public string FirstScreen = "title";

	public MedleyPartyControler partyControler;

	private void Start()
    {
        //FirstScreen = "freeplay";
        SwitchMenu(FirstScreen);
        ModeManager.State = ModeManager.GameState.Medley;		
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
            case "main":
                currentMenu = mainMenu;
                FirstScreen = "main";
				partyControler.OpenParty();
                break;
			case "settings":
				MedleySetup.nOfVictories = 2;
				FindObjectOfType<MedleySetup>().UpdateNOfVictoriesDisplay();
				currentMenu = settingsMenu;
				break;
            case "main menu":
                CallScene("main menu");
                break;
			case "playerDisplay":
				currentMenu = playerSettingsMenu;
				FindObjectOfType<MedleySetup>().SettupPlayerDisplay();
				break;
            default:
                return;
        }
        eventSystem.SetSelectedGameObject(null);
        currentMenu.menuTransform.gameObject.SetActive(true);
        eventSystem.SetSelectedGameObject(currentMenu.firstButton);
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
}
