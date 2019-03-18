using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pong
{
    public class LinhaDeAviso : MonoBehaviour
    {
        public Color corFogo;
        public Color corGelo;

        [SerializeField] private AudioClip sound = null;

        private void Start()
        {
            AudioSource.PlayClipAtPoint(sound, Vector3.zero);
        }
    }
}
