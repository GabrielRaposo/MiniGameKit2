using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SequencialMedleySettings : MonoBehaviour
{

    [Header("Color config")]
    public Image p1Color;
    public Image p2Color;

    public int p1ColorIndex;
    public int p2ColorIndex;

    private SequencialMedleyControler sequencialMedleyControler;

    public MedleyGameType gameMode;
    public TextMeshProUGUI gameModeText;

    public MedleyGameType GameMode
    {
        get
        {
            return gameMode;
        }

        set
        {
            gameMode = value;
            sequencialMedleyControler.gameMode = gameMode;

            switch (gameMode)
            {
                case MedleyGameType.Shuffle:
                    gameModeText.text = "Shuffle";
                    break;
                case MedleyGameType.Playlist:
                    gameModeText.text = "Playlist";
                    break;
            }
        }
    }

    private void Awake()
    {
        sequencialMedleyControler = FindObjectOfType<SequencialMedleyControler>();
        p1ColorIndex = 0;
        p2ColorIndex = 1;
        GameMode = MedleyGameType.Shuffle;
    }

    void Update()
    {
        // p1Color.color = PlayersManager.playerDefaultColor[p1ColorIndex];
        // p2Color.color = PlayersManager.playerDefaultColor[p2ColorIndex];
    }

    
    //ARRUMAR ISSO AQUI
    public void ChangeP1Color(int i)
    {
        p1ColorIndex += i;

        if(p1ColorIndex < 0)
        {
            //p1ColorIndex = PlayersManager.playerDefaultColor.Length - 1;
        }
        // else if(p1ColorIndex >= PlayersManager.playerDefaultColor.Length - 1)
        // {
        //     p1ColorIndex = 0;
        // }

        if(p1ColorIndex == p2ColorIndex)
        {
            if (i < 0)
                ChangeP1Color(-1);
            if (i > 0)
                ChangeP1Color(1);
        }

    }

    public void ChangeP2Color(int i)
    {
        p2ColorIndex += i;

        // if (p2ColorIndex < 0)
        // {
        //     p2ColorIndex = PlayersManager.playerDefaultColor.Length - 1;
        // }
        // else if (p2ColorIndex >= PlayersManager.playerDefaultColor.Length - 1)
        // {
        //     p2ColorIndex = 0;
        // }

        if (p2ColorIndex == p1ColorIndex)
        {
            if (i < 0)
                ChangeP2Color(-1);
            if (i > 0)
                ChangeP2Color(1);
        }
    }

    public void ChangeGameMode()
    {
        if (GameMode == MedleyGameType.Shuffle)
            GameMode = MedleyGameType.Playlist;
        else
            GameMode = MedleyGameType.Shuffle;
    }

    public void SelectColors()
    {
        // PlayersManager.playerColor[0] = PlayersManager.playerDefaultColor[p1ColorIndex];
        // PlayersManager.playerColor[1] = PlayersManager.playerDefaultColor[p2ColorIndex];       
    }
}
