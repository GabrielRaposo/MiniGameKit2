using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectButtonOnEnable : MonoBehaviour
{
    public GameObject firstSelection;

    private void OnEnable()
    {
        StartCoroutine(WaitForAFrame());
    }

    private IEnumerator WaitForAFrame()
    {
        yield return new WaitForFixedUpdate();

        if (firstSelection != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelection);
        }
    }
}
