using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SubmenuSelectorDisplay : MonoBehaviour
{
    [SerializeField] private Transform[] icons;

    public int index { get; private set; }

    void Start()
    {
        ResetValue();
    }

    private void UpdateDisplay()
    {
        for (int i = 0; i < icons.Length; i++)
        {
            if (i == index)
            {
                icons[i].DOScale(Vector3.one * 2, .1f);
            }
            else
            {
                icons[i].DOScale(Vector3.one * 1, .1f);
            }
        }
    }

    public void ResetValue()
    {
        index = 0;

        UpdateDisplay();
    }

    public void AddValue(int value)
    {
        index += value;
        if (index < 0 ) index = icons.Length-1;
        index %= icons.Length;

        UpdateDisplay();
    }
    
}
