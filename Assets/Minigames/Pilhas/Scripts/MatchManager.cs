using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pilhas
{
    public class MatchManager : MonoBehaviour
    {
        void Start()
        {
        
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                CheckScore();
            }
        }
        
        private void CheckScore()
        {
            Debug.Log("Checking");
            Time.timeScale = 0;
        }
    }
}
