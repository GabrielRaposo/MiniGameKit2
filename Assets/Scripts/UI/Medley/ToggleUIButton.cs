using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ToggleUIButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Header("References")]
    public RawImage backRectangle;
    public TextMeshProUGUI valueDisplay;

    [Header("Values")]
    public Color selectedColor;
    public Color deselectedColor;

    private bool on;
    public bool On
    {
        get
        {
            return on;
        }

        set
        {
            on = value;
            valueDisplay.text = (on) ? "ON" : "OFF";
        }
    }

    private void OnEnable()
    {
        OnDeselect(new BaseEventData(EventSystem.current));
    }

    public void OnSelect(BaseEventData eventData)
    {
        backRectangle.color = selectedColor;
        valueDisplay.color = deselectedColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        backRectangle.color = deselectedColor;
        valueDisplay.color = selectedColor;
    }
}
