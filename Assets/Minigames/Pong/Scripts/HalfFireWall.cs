using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pong
{
    public class HalfFireWall : MonoBehaviour
    {
        [SerializeField] private Lado lado = Lado.ESQ;
        [SerializeField] private float speed = 1;   //velocidade imaginando uma escala na qual a coordenada x da tela vai de -0.5 a 0.5
        private float scaledSpeed;  //velocidade modificada para a escala normal

        private void Start()
        {
            //converte velocidade para a escala normal
            scaledSpeed = speed * 2 * Camera.main.orthographicSize * Camera.main.aspect;
        }

        void Update()
        {
            transform.position += lado == Lado.DIR ? new Vector3(scaledSpeed * Time.deltaTime, 0, 0) : new Vector3(-scaledSpeed * Time.deltaTime, 0, 0);

            //Saída da tela
            if (Mathf.Abs(transform.position.x - Camera.main.transform.position.x) > Camera.main.orthographicSize * Camera.main.aspect)
            {
                Destroy(gameObject);
            }
        }
    }
}