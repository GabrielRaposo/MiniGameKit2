using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ToggleUIButton : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI buttonLabel;
    public Image indicator;
    public Image valueDisplay;

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
            valueDisplay.enabled = on;
        }
    }

    private void OnEnable()
    {
        Hide();
    }

    public void Highlight()
    {
        valueDisplay.color = indicator.color = buttonLabel.color = selectedColor;
    }

    public void Hide()
    {
        valueDisplay.color = indicator.color = buttonLabel.color = deselectedColor;
    }

}
