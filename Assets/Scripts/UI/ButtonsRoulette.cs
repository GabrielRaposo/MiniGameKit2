using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class ButtonsRoulette : MonoBehaviour
{
    [Header("Roulette")]
    [SerializeField] private float scrollTime;
    [SerializeField] private Transform buttonsAxis;
    [SerializeField] private GameObject baseButton;
    [SerializeField] private TutorialObject[] tutorialObjects;

    [Header("UI References")]
    [SerializeField] private ChargeableButton exitButton;
    [SerializeField] private SubmenuSelectorDisplay selectorDisplay;
    [Space(10)]
    [SerializeField] private TextMeshProUGUI minigameLabel;
    [SerializeField] private RectTransform infoTab;
    [SerializeField] private TextMeshProUGUI infoLabel;
    [SerializeField] private TextMeshProUGUI infoDisplay;
    [SerializeField] private RawImage gameArt;
    [Space(10)]
    [SerializeField] private GameObject inputArrowUp;
    [SerializeField] private GameObject inputArrowDown;

    private RectTransform rectTransform;
    private Vector2 scrollMovement = new Vector2(75, 256) * 1.2f;
    private GameObject[] buttons;

    private int index;


    private void OnEnable()
    {
        exitButton.enabled = true;
        if(buttons != null && buttons.Length > 0)
        {
            buttons[index].GetComponent<Button>().Select();
        }
    }

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        buttons = new GameObject[tutorialObjects.Length];

        for (int i = 0; i < tutorialObjects.Length; i++)
        {
            GameObject button = Instantiate(baseButton, buttonsAxis);
            button.GetComponent<RectTransform>().anchoredPosition = (Vector2.left * 315) - (scrollMovement * i);

            GameRouletteButton rouletteButton = button.GetComponent<GameRouletteButton>();
            if (rouletteButton)
            {
                rouletteButton.Init(this, tutorialObjects[i]);
            }

            if (i == 0)
            {
                button.GetComponent<Button>().Select();
                button.transform.localScale = Vector3.one * 2.0f;
            }
            buttons[i] = button;
        }

        baseButton.SetActive(false);

        SetInfoScreen(tutorialObjects[index]);
    }

    private void Update()
    {
        if (exitButton.enabled)
        {
            exitButton.charging = Input.GetAxisRaw("Horizontal") < 0;
        }
    }

    public void ScrollUp()
    {
        if(index > 0)
        {
            buttons[index].transform.DOScale(Vector3.one * 1.5f, scrollTime);
            rectTransform.DOAnchorPos((Vector2.left * 300) + (scrollMovement * --index), scrollTime);
            buttons[index].transform.DOScale(Vector3.one * 2.0f, scrollTime);

            SetInfoScreen(tutorialObjects[index]);
        }
    }

    public void ScrollDown()
    {
        if(index < tutorialObjects.Length - 1)
        {
            buttons[index].transform.DOScale(Vector3.one * 1.5f, scrollTime);
            rectTransform.DOAnchorPos((Vector2.left * 300) + (scrollMovement * ++index), scrollTime);
            buttons[index].transform.DOScale(Vector3.one * 2.0f, scrollTime);

            SetInfoScreen(tutorialObjects[index]);
        }
    }

    public void ScrollRight()
    {
        selectorDisplay.AddValue(1);
        UpdateTextDisplay();
    }

    public void ScrollLeft()
    {
        selectorDisplay.AddValue(-1);
        UpdateTextDisplay();
    }

    private void UpdateTextDisplay()
    {
        switch (selectorDisplay.index)
        {
            default:
            case 0:
                infoLabel.text = "Rules";
                infoDisplay.text = tutorialObjects[index].gameRules;
                break;

            case 1:
                infoLabel.text = "Controls";
                string s = string.Empty;
                foreach(TutorialObject.InputTab inputTab in tutorialObjects[index].controls)
                {
                    s += inputTab.inputKey.ToString();
                    if (inputTab.inputType != TutorialObject.InputType.None) s += " (" + inputTab.inputType.ToString() + ")";
                    s += " - " + inputTab.text;
                    s += "\n";
                }
                infoDisplay.text = s;
                break;

            case 2:
                infoLabel.text = "Credits";
                infoDisplay.text = tutorialObjects[index].credits;
                break;
        }

        infoTab.localPosition += new Vector3(100, 0);
        infoTab.DOLocalMove(Vector3.zero, .1f);
    }

    private void UpdateInputArrows()
    {
        if (index > 0)
            inputArrowUp.SetActive(true);
        else
            inputArrowUp.SetActive(false);

        if (index < tutorialObjects.Length - 1)
            inputArrowDown.SetActive(true);
        else
            inputArrowDown.SetActive(false);
    }

    public void SetInfoScreen(TutorialObject tutorialObject)
    {
        UpdateInputArrows();

        minigameLabel.text = tutorialObject.codename;
        //rulesLabel.text = tutorialObject.gameRules;
        gameArt.texture = tutorialObject.image;

        selectorDisplay.ResetValue();
        UpdateTextDisplay();
    }
}
