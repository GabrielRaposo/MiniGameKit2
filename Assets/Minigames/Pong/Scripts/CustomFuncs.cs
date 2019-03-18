using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pong
{
    public static class CustomFuncs
    {
        /// <summary>
        /// Checa interseção ignorando a componente z da posição.
        /// </summary>
        public static bool MyIntersects(Bounds b1, Bounds b2)
        {
            b1 = new Bounds(new Vector3(b1.center.x, b1.center.y, 0), b1.size);
            b2 = new Bounds(new Vector3(b2.center.x, b2.center.y, 0), b2.size);
            return b1.Intersects(b2);
        }

        /// <summary>
        /// Retorna true ou false aleatóriamente
        /// </summary>
        public static bool RandomBool()
        {
            return Random.value > 0.5;
        }

        /// <summary>
        /// Um PlayClipAtPoint que retorna o game object com o audio source criado
        /// </summary>
        public static GameObject MyPlayClipAtPoint(AudioClip clip, Vector3 position)
        {
            GameObject newObject = new GameObject("customSource");
            AudioSource source = newObject.AddComponent<AudioSource>();
            source.GetComponent<AudioSource>().clip = clip;
            source.Play();
            MonoBehaviour.Destroy(source, clip.length);
            return newObject;
        }
    }
}
