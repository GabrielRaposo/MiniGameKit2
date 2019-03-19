using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Bailarinas
{
    public class ChegadaScript : MonoBehaviour {

        private BailarinaScript closerPlayer;        

        public BailarinaScript playerLeft;
        public BailarinaScript playerRight;

		bool endGame = false;

        private void Start()
        {
            playerLeft.onFall += () => OnPlayerFall(playerLeft);
            playerRight.onFall += () => OnPlayerFall(playerRight);
        }

        private void Update()
        {
            //closerPlayer = CheckCloser();
        }

		private void LateUpdate()
		{
			if(endGame == false)
			{			

				if(playerLeft.winning && playerRight.winning)
				{
					EndMinigame(PlayersManager.Result.Draw);
					endGame = true;
				}

				if (playerLeft.winning)
				{
					EndMinigame(PlayersManager.Result.LeftWin);
					endGame = true;
				}
				else if (playerRight.winning)
				{
					EndMinigame(PlayersManager.Result.RightWin);
					endGame = true;
				}
			}
		}

		private void OnTriggerEnter(Collider other)
        {	
            if (other.GetComponent<BailarinaScript>() == playerLeft)
            {
				playerLeft.winning = true;				
                //EndMinigame(PlayersManager.Result.LeftWin);
            }

            if (other.GetComponent<BailarinaScript>() == playerRight)
            {
				playerRight.winning = true;
                //EndMinigame(PlayersManager.Result.RightWin);
            }
        }        

		

        BailarinaScript CheckCloser()
        {
            if (playerLeft.transform.position.z > playerRight.transform.position.z)
            {
                return playerLeft;
            }
            else if (playerRight.transform.position.z > playerLeft.transform.position.z)
            {
                return playerRight;
            }
            else
            {
                return null;
            }
        }

        void EndMinigame(PlayersManager.Result result)
        {
            Debug.Log(result.ToString());

            if (result == PlayersManager.Result.LeftWin)
            {
                PlayersManager.result = PlayersManager.Result.LeftWin;
                playerLeft.Win();
                playerRight.Die();
				MoveCameraToPlayer(playerLeft.transform);
            }
            else if (result == PlayersManager.Result.RightWin)
            {
                PlayersManager.result = PlayersManager.Result.RightWin;
                playerLeft.Die();
                playerRight.Win();
				MoveCameraToPlayer(playerRight.transform);
            }
            else
            {
                PlayersManager.result = PlayersManager.Result.Draw;

				if(playerRight.winning && playerLeft.winning)
				{
					playerLeft.Win();
					playerRight.Win();
				}
				else
				{
					Destroy(playerRight);
					Destroy(playerLeft);
				}
            }

            StartCoroutine(DelayToTransition());
        }

		void MoveCameraToPlayer(Transform transform)
		{
			Vector3 targetPosition = transform.position - new Vector3(0, -0.823f, 3.61f);

			Camera.main.transform.DOMove(targetPosition, 1.0f);
		}

        IEnumerator DelayToTransition()
        {
            yield return new WaitForSeconds(4.5f);
            StartCoroutine(ModeManager.TransitionFromMinigame());
        }

        void OnPlayerFall(BailarinaScript bai)
        {
            if(bai == playerRight)
            {
                if (playerLeft.dead)
                {
                    EndMinigame(PlayersManager.Result.Draw);
                }
                else
                {
                    EndMinigame(PlayersManager.Result.LeftWin);
                }
            }
            else
            {
                if (playerRight.dead)
                {
                    EndMinigame(PlayersManager.Result.Draw);
                }
                else
                {
                    EndMinigame(PlayersManager.Result.RightWin);
                }
            }
        }        

    }
}
