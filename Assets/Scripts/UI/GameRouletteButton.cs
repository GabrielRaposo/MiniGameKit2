using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameRouletteButton : MonoBehaviour, IMoveHandler
{
    private ButtonsRoulette roulette;
    private TutorialObject tutorialObject;

    public void Init(ButtonsRoulette roulette, TutorialObject tutorialObject)
    {
        this.roulette = roulette;
        this.tutorialObject = tutorialObject;

        Button button = GetComponent<Button>();
        if (button)
        {
            button.onClick.AddListener(() => CallMinigame());
        }
    }

    public void OnMove(AxisEventData eventData)
    {
        switch (eventData.moveDir)
        {
            case MoveDirection.Up:
                roulette.ScrollUp();
                break;

            case MoveDirection.Down:
                roulette.ScrollDown();
                break;
        }
    }

    private void CallMinigame()
    {
        string codename = tutorialObject.codename;

        if (codename != string.Empty)
        {
            SceneManager.LoadScene("Minigames/" + codename + "/" + codename);
        }
    }
}
