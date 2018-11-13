using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class TutorialScreen : MonoBehaviour
{
    [Header("Main components")]
	public TextMeshProUGUI title;
    public RawImage image;

    [System.Serializable]
    public struct UIDisplay
    {
        public RectTransform box;
        public Button title;
        public GameObject arrow;
    }

    [Space(20)]
    public UIDisplay rulesUI;
    public TextMeshProUGUI displayRules;

    [Space(20)]
    public UIDisplay controlsUI;
    public GameObject inputLinePrefab;
    public Transform inputLineParent;

    [Space(20)]
    public UIDisplay creditsUI;
    public TextMeshProUGUI displayCredits;

    [Space(10)]
    [Header("Values")]
    public Color colorFront;
    public Color colorBack;

    string codename;
    string gameTitle;
    string rulesText;
    Texture gameImage;

    private enum State { Rules, Controls, Credits }
    private State state;
    private State previousState;
    private List<GameObject> inputLines;
    private int index;
    private bool onInputDelay;

    private void OnEnable()
    {
        state = State.Rules;
        SetDisplays();
    }

    private void OnDisable()
    {
        previousState = state;
    }

    private void Update()
    {
        if (onInputDelay) return;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (horizontalInput > 0) {
            ChangeIndex(1);
        } else 
        if (horizontalInput < 0) {
            ChangeIndex(-1);
        }
    }

    private void ChangeIndex(int diff)
    {
        const int max = 3;
        index += diff;
        if (index < 0) index = max - 1;
        else           index %= max;

        SwitchState(index);
        StartCoroutine(InputDelayTimer());
    }

    private IEnumerator InputDelayTimer()
    {
        onInputDelay = true;
        yield return new WaitForSeconds(.2f);
        onInputDelay = false;
    }

    public void SwitchState(int index)
    {
        this.index = index;
        previousState = state;
        switch (index)
        {
            default:
            case 0: state = State.Rules; break;
            case 1: state = State.Controls; break;
            case 2: state = State.Credits; break;
        }

        SetDisplays();
    }

    private void SetDisplays()
    {
        UIDisplay previousBox = GetBoxByState(previousState);
        UIDisplay currentBox = GetBoxByState(state);

        HighlightVisual(false, previousBox);
        HighlightVisual(true, currentBox);
        SetDrawOrder(currentBox.box.transform, previousBox.box.transform);
    }

    private UIDisplay GetBoxByState(State localState)
    {
        UIDisplay box;
        switch (localState)
        {
            default:
            case State.Rules:    box = rulesUI;    break;
            case State.Controls: box = controlsUI; break;
            case State.Credits:  box = creditsUI;  break;
        }
        return box;
    }

    void HighlightVisual(bool value, UIDisplay display)
    {
        Color color = value ? colorFront : colorBack;
        display.box.GetComponent<RawImage>().color = color;
        display.arrow.SetActive(!value);
        display.title.GetComponent<RawImage>().color = color;
        display.title.interactable = !value;
    }

    void SetDrawOrder(Transform front, Transform back)
    {
        back.SetAsLastSibling();
        front.SetAsLastSibling();
    }  

    public void GetInfo
        (string codename, string gameTitle, string rulesText, TutorialObject.InputTab[] controls, string creditsText, Texture gameImage)
	{
        this.codename = codename;
        this.gameTitle = gameTitle;
        this.rulesText = rulesText;
        this.gameImage = gameImage;

        title.text = gameTitle;
        image.texture = gameImage;

        displayRules.text = rulesText;
        CreateInputLines(controls);
        displayCredits.text = creditsText;

        gameObject.SetActive(true);
    }

    void CreateInputLines(TutorialObject.InputTab[] controls)
    {
        foreach (Transform child in inputLineParent)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < controls.Length; i++)
        {
            GameObject line = Instantiate(inputLinePrefab, inputLineParent);
            line.GetComponent<TutorialInputLine>().SetValues(
                controls[i].inputKey,
                controls[i].inputType,
                controls[i].text
            );
            line.SetActive(true);
        }
    }

    public void StartMinigame()
    {
        if(codename != string.Empty)
        {
            SceneManager.LoadScene("Minigames/" + codename + "/" + codename);
        }
    }
}
