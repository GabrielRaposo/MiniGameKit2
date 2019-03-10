using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ChargeableButton : MonoBehaviour
{
    const int MAX_CHARGE = 1000;

    [SerializeField] private int speed;
    [SerializeField] private Image fillDisplay;

    private int charge;
    public bool charging;

    void OnEnable()
    {
        charge = 0;
        UpdateFillDisplay();
    }

    void Update()
    {
        if (charging)
        {
            charge += speed;

            if (charge > MAX_CHARGE)
            {
                charge = MAX_CHARGE;
                GetComponent<Button>().onClick.Invoke();
                enabled = false;
            }

            transform.DOScale(Vector3.one * 1.8f, .2f);
        }
        else
        {
            if(charge > 0)
            {
                charge -= speed * 2;
            }
            else
            {
                charge = 0;
            }

            transform.DOScale(Vector3.one * 1.5f, .2f);
        }

        UpdateFillDisplay();
    }

    private void UpdateFillDisplay()
    {
        fillDisplay.fillAmount = (float) charge / MAX_CHARGE;
    }
}
