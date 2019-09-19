using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameRouletteButton : MonoBehaviour, IMoveHandler
{
    [SerializeField] private RawImage icon;

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

        if(icon && tutorialObject.icon != null)
        {
            icon.texture = tutorialObject.icon;
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

            case MoveDirection.Right:
                roulette.ScrollRight();
                break;

            case MoveDirection.Left:
                roulette.ScrollLeft();
                break;
        }
    }

    private void CallMinigame()
    {
        string codename = tutorialObject.codename;

        if (codename != string.Empty)
        {
            //SceneManager.LoadScene("Minigames/" + codename + "/" + codename);

            SceneTransition sceneTransition = SceneTransition.instance;
            if(sceneTransition != null)
                sceneTransition.Call(codename);
            else
                SceneTransition.LoadScene(codename);
        }
    }
}
