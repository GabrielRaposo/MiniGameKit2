using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MedleyPlayerLayout : MonoBehaviour
{
    public Transform transform => transform;

    public List<Transform> iconSlots;
    public List<GameObject> lines;

    public void AnimateEntrance(List<MeddleyPlayerIcon> icons)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(lines[0].transform.DOMove(lines[0].transform.position + lines[0].transform.up * -900, 1f));
        sequence.Append(lines[1].transform.DOMove(lines[1].transform.position + lines[1].transform.up * 900, 1f));
        sequence.Append(iconSlots[0].transform.DOLocalMoveY(-620, 0.6f));
        sequence.Append(iconSlots[1].transform.DOLocalMoveY(620, 0.6f));
        
        ColorChildren(lines[0].transform, icons[1].GetColor());
        ColorChildren(lines[1].transform, icons[0].GetColor());

        sequence.Play();
    }

    public void SetIcons(List<MeddleyPlayerIcon> icons)
    {
        for (int i = 0; i < icons.Count; i++)
        {
            icons[i].transform.SetParent(iconSlots[i]);
            icons[i].transform.localPosition = Vector3.zero;
        }
    }

    private void ColorChildren(Transform image, Color c)
    {
        Image img = image.GetComponent<Image>();
        
        img.color = c;

        for (int i = 0; i < image.transform.childCount; i++)
        {
            img = image.transform.GetChild(i).GetComponent<Image>();
            if (img)
                img.color = c;
        }
        
    }
    
}
