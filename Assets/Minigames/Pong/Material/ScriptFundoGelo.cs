using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pong
{
    public class ScriptFundoGelo : MonoBehaviour
    {
        private Material material;

        [SerializeField] private float dispersionInterval = 1f;     //tempo entre expansões do gelo
        [SerializeField] private float dispertionFraction = 0.25f;  //fração adicional que o gelo consome a cada expansão

        // Start is called before the first frame update
        void Start()
        {
            material = GetComponent<SpriteRenderer>().material;
            StartCoroutine(IceExpansion());
        }



        private IEnumerator IceExpansion()
        {
            float icePosition = material.GetFloat("_IcePosition");

            while (icePosition < 1)
            {
                float newPosition = Mathf.Min(1, material.GetFloat("_IcePosition") + dispertionFraction);
                material.SetFloat("_IcePosition", newPosition);
                yield return new WaitForSeconds(dispersionInterval);
            }
        }
    }
}
