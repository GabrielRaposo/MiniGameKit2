using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersManager : MonoBehaviour {

    public static Color[] playerDefaultColor = new Color[] 
    {
        new Color(0f, 0.682353f, 1f),    // Azul Claro
        new Color(1f, 0.6f, 0.1058824f), // Laranja
        new Color(.957f, .259f, .259f),  // Vermelho
        new Color(.137f, .627f, .184f)   // Verde Escuro
    };

    public static Color[] playerColor = playerDefaultColor;

    public static int currentLeftPlayer = 0;
    public static int currentRightPlayer = 1;

    public enum Result { Draw, LeftWin, RightWin }
    public static Result result;
}
