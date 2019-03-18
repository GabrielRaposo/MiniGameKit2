using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pong
{
    public class ScriptFundoFogo : MonoBehaviour
    {
        private Material material;

        public float dispersionSpeed = 1;
        public float greenDecay = 0.3f;
        public float darkeningRate = 0.1f;
        public Color minimumRed;

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

            //Animação da cor do fogo mudando
            Color color = material.GetColor("_Color");
            float red = Mathf.Max(minimumRed.r, color.r - darkeningRate * Time.deltaTime);
            float green = Mathf.Max(color.g - greenDecay * Time.deltaTime, 0);
            color = new Color(red, green, color.b, color.a);
            material.SetColor("_Color", color);
        }
    }
}
