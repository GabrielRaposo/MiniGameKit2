using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bailarinas {

    public class BailarinaGameControler : MonoBehaviour {

        public TextScript layoutText;

        public GameObject leftP;
        public GameObject rightP;       

        private void Start()
        {
            StartCoroutine(PreGame());
        }

        IEnumerator PreGame()
        {
			layoutText.StartCoutDownAnimation();
			yield return new WaitUntil(() =>  layoutText.startedGame );
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



