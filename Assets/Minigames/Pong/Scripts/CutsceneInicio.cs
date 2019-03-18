using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pong
{
    public class CutsceneInicio : MonoBehaviour
    {
        [SerializeField] private GameObject ballsUpperHalf = null;
        [SerializeField] private GameObject ballsLowerHalf = null;
        [SerializeField] private GameObject spriteBall = null;
        [SerializeField] private AudioClip movementSound = null;
        [SerializeField] private AudioClip unionSound = null;
        [SerializeField] private AudioClip startSound = null;



        [SerializeField] private float appearingTime = 0.5f;    //tempo que leva para as metades aparecerem na tela e começarem a se mover
        [SerializeField] private float meetingTime = 3;   //tempo que leva para as metades se encontrarem após o inicio do movimento
        [SerializeField] private float unionTime = 1;    //tempo que leva para as metades se unirem após se encontrarem
        [SerializeField] private float gameStartTime = 1;    //tempo que leva para a partida começar após as metades se unirem

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Animation());
        }

        private IEnumerator Animation()
        {
            yield return new WaitForSeconds(appearingTime);

            //Cria metades
            float cameraY = Camera.main.transform.position.y;
            Vector3 cameraVectX = new Vector3(Camera.main.transform.position.x, 0, 0);
            float initialY = Camera.main.orthographicSize * 2 + cameraY;
            GameObject upperHalf = Instantiate(ballsUpperHalf, cameraVectX + Vector3.up * initialY, Quaternion.identity);
            GameObject lowerHalf = Instantiate(ballsLowerHalf, cameraVectX + Vector3.down * initialY, Quaternion.Euler(0, 0, 180));

            //Som
            GameObject movementSoundGameObject = CustomFuncs.MyPlayClipAtPoint(movementSound, Vector3.zero);

            //Desce (ou sobe) peças
            float speed = initialY / meetingTime;

            while (upperHalf.transform.position.x > cameraY || lowerHalf.transform.position.y < cameraY)
            {
                upperHalf.transform.position += speed * Vector3.down * Time.fixedDeltaTime;
                lowerHalf.transform.position += speed * Vector3.up * Time.fixedDeltaTime;

                yield return new WaitForFixedUpdate();
            }
            upperHalf.transform.position = new Vector3(upperHalf.transform.position.x, cameraY, upperHalf.transform.position.z);
            lowerHalf.transform.position = new Vector3(lowerHalf.transform.position.x, cameraY, lowerHalf.transform.position.z);

            //Som
            Destroy(movementSoundGameObject);

            yield return new WaitForSeconds(unionTime);

            //Junta metades
            Destroy(upperHalf);
            Destroy(lowerHalf);
            GameObject fusionBall = Instantiate(spriteBall, new Vector3(cameraVectX.x, cameraY, 0), Quaternion.identity);

            //Som
            AudioSource.PlayClipAtPoint(unionSound, Vector3.zero);

            yield return new WaitForSeconds(gameStartTime);

            //Começa partida
            Destroy(fusionBall);

            GetComponent<GameManager>().StartsGameAfterCutScene();

            //Som
            AudioSource.PlayClipAtPoint(startSound, Vector3.zero);
        }
    }
}