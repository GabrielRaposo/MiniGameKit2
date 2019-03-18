using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pong
{
    public class ScriptFireWall : MonoBehaviour
    {
        private Material material;

        [SerializeField] private float dispersionSpeed = 1;

        // Start is called before the first frame update
        void Start()
        {
            material = GetComponent<SpriteRenderer>().material;
        }

        // Update is called once per frame
        void Update()
        {
            //Animação do fogo se alastrando
            float newPosition = Mathf.Min(1, material.GetFloat("_FirePosition") + dispersionSpeed * Time.deltaTime);
            material.SetFloat("_FirePosition", newPosition);
        }
    }
}
