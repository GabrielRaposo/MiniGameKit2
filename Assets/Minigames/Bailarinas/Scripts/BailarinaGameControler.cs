using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bailarinas {

    public class BailarinaGameControler : MonoBehaviour {

        public LayoutTextManager layoutText;

        public GameObject leftP;
        public GameObject rightP;       

        private void Start()
        {
            StartCoroutine(PreGame());
        }

        IEnumerator PreGame()
        {
            StartCoroutine(layoutText.DownText("3", 1.0f));
            yield return new WaitForSeconds(1.0f);
            StartCoroutine(layoutText.DownText("2", 1.0f));
            yield return new WaitForSeconds(1.0f);
            StartCoroutine(layoutText.DownText("1", 1.0f));
            yield return new WaitForSeconds(1.0f);
            StartCoroutine(layoutText.DownText("VAI!", 0.4f));
            yield return new WaitForSeconds(0.4f);
            layoutText.ClearText();

            StartGame();

        }

        void StartGame()
        {
            leftP.GetComponent<BailarinaScript>().enabled = true;
            rightP.GetComponent<BailarinaScript>().enabled = true;

            leftP.GetComponent<Rigidbody>().useGravity = true;
            rightP.GetComponent<Rigidbody>().useGravity = true;           
        }
       

        //IEnumerator StartGame()
        //{

        //}

        //IEnumerator EndGame(PlayersManager.Result endResult)
        //{

        //}

    }

}



