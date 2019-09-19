using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour {

    public RectTransform imageAnchor;
    public static SceneTransition instance;

    private void Awake()
    {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(transform.parent.gameObject);
        } else Destroy(transform.parent.gameObject);
    }

    public void Call(string name)
    {
        StartCoroutine(Animate(name));
    }

    public void Call(int index)
    {
        StartCoroutine(Animate(string.Empty, index));
    }

    public IEnumerator Animate(string name, int index = -1)
    {
        float 
            startingX = 2300f,
            middleX = 0f,
            targetX = -2300f,
            moveSpeed = 200f;

        //Time.timeScale = 0;
        imageAnchor.localPosition = Vector3.right * startingX;
        while(imageAnchor.localPosition.x > middleX)
        {
            yield return new WaitForEndOfFrame();
            imageAnchor.localPosition += Vector3.left * moveSpeed;
        }
        AsyncOperation asyncOperation;
        if(index > -1)
        {
            asyncOperation = SceneManager.LoadSceneAsync(index);
        }
        else 
        {
            asyncOperation = SceneManager.LoadSceneAsync(name);
        }
        asyncOperation.allowSceneActivation = false;
        while (asyncOperation.progress < .9f)
        {
            yield return null;
        }
        yield return new WaitForSecondsRealtime(.5f);
        asyncOperation.allowSceneActivation = true;
        //Time.timeScale = 1;

        while (imageAnchor.localPosition.x > targetX)
        {
            yield return new WaitForEndOfFrame();
            imageAnchor.localPosition += Vector3.left * moveSpeed;
        }
    }

    public static void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    } 

    public static void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    } 
}

