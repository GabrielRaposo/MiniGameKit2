using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class DetachableButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform rightTab;
    [SerializeField] private Image labelMask;
    [SerializeField] private Image icon;

    [Header("Values")]
    [Range(0f, 1f)] [SerializeField] private float transitionTime;
    [SerializeField] private float width;

    private Vector3 rightTabStartingPosition;

    private void OnEnable()
    {
        rightTabStartingPosition = rightTab.localPosition;
    }

    public void Highlight()
    {
        rightTab.DOLocalMove(rightTabStartingPosition + (new Vector3(1f, .5f) * width), transitionTime);
        icon.DOFade(0, transitionTime / 2);
        labelMask.DOFillAmount(1, transitionTime);
    }

    public void Hide()
    {
        rightTab.DOLocalMove(rightTabStartingPosition, transitionTime);
        icon.DOFade(1, transitionTime);
        labelMask.DOFillAmount(0, transitionTime);
    }
}
