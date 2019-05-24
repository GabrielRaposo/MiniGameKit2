using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public enum MedleyGameType { Shuffle, Playlist};
public class SequencialMedleyControler : MonoBehaviour
{
    public List<TutorialObject> allgames;
    public static List<TutorialObject> currentGameList;

    public static int p1Score;
    public static int p2Socre;

    public TextMeshProUGUI p1ScoreText;
    public TextMeshProUGUI p2ScoreText;

    public Image p1BG;
    public Image p2BG;

    public static bool medleyInProgress;

    public static int currentRound;
    public static int maxRounds = 3;

    public Canvas betwenGamesCanvas;
    public Canvas otherCanvas;

    private void Start()
    {
        if (!medleyInProgress)
        {
            allgames = new List<TutorialObject>(Resources.LoadAll<TutorialObject>("Tutorials"));
            ModeManager.State = ModeManager.GameState.Medley;
            currentGameList = allgames;
        }
        else
        {
            betwenGamesCanvas.enabled = true;
            otherCanvas.enabled = false;

            p1BG.color = PlayersManager.playerColor[0];
            p2BG.color = PlayersManager.playerColor[1];

            ReturnFromGame();
        }
    }

    public void GoToRandomGame()
    {
        if (!medleyInProgress)
            medleyInProgress = true;

        int i = Random.Range(0, currentGameList.Count);
        string gameToLoad = currentGameList[i].codename;

        currentGameList.RemoveAt(i);

        if (currentGameList.Count == 0)
        {
            if(allgames == null)
            {
                allgames = new List<TutorialObject>(Resources.LoadAll<TutorialObject>("Tutorials"));
            }
            currentGameList = allgames;
        }

        SceneManager.LoadScene(gameToLoad);

    }

    public IEnumerator GoToGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GoToRandomGame();
    }

    public void ReturnFromGame()
    {
        switch (PlayersManager.result)
        {
            case PlayersManager.Result.Draw:
                break;
            case PlayersManager.Result.LeftWin:
                p1Score++;
                break;
            case PlayersManager.Result.RightWin:
                p2Socre++;
                break;
        }

        p1ScoreText.text = p1Score.ToString();
        p2ScoreText.text = p2Socre.ToString();

        currentRound++;

        if(currentRound >= maxRounds)
        {
            Debug.Log("ACBAOU");
        }
        else
        {
            StartCoroutine(GoToGameAfterDelay(5.0f));
        }

    }

    private void Awake()
    {
        
    }
    
}