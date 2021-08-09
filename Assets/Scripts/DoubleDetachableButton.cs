using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoubleDetachableButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform rightTab;
    [SerializeField] private RectTransform leftTab;
    [SerializeField] private Image labelMask;
    [SerializeField] private TextMeshProUGUI centerText;

    [Header("Values")]
    [Range(0f, 1f)] [SerializeField] protected float transitionTime;
    [SerializeField] private float width;
 
    public virtual void Highlight()
    {
        StopAllCoroutines();
        StartCoroutine(HighlightRoutine());
    }

    public virtual void Hide()
    {
        StopAllCoroutines();
        StartCoroutine(HideRoutine());
    }

    private IEnumerator HighlightRoutine()
    {
        yield return new WaitForEndOfFrame();
        rightTab.DOLocalMove(Vector3.right*width, transitionTime).SetEase(Ease.InOutBounce);
        leftTab.DOLocalMove(Vector3.left*width, transitionTime).SetEase(Ease.InOutBounce);
        //centerText.DOFade(0, transitionTime / 2);
        //labelMask.DOFillAmount(1, transitionTime);
    }

    private IEnumerator HideRoutine()
    {
        yield return new WaitForEndOfFrame();
        rightTab.DOLocalMove(Vector3.zero, transitionTime);
        leftTab.DOLocalMove(Vector3.zero, transitionTime);
        //centerText.DOFade(1, transitionTime);
        //labelMask.DOFillAmount(0, transitionTime);
    }
    
}
