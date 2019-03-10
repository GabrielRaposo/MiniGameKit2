using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ExtendedDetachableButton : DetachableButton
{
    [Header("Color")]
    [SerializeField] private Image[] imageRenderers;
    [Space(10)]
    [SerializeField] private Color highlightColor;
    [SerializeField] private Color hideColor;

    public override void Highlight()
    {
        base.Highlight();

        foreach (Image image in imageRenderers)
        {
            image.color = highlightColor;
        }
    }

    public override void Hide()
    {
        base.Hide();

        foreach(Image image in imageRenderers)
        {
            image.color = hideColor;
        }
    }
}
