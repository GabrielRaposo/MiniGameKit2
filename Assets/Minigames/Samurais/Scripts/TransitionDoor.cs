﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Samurais
{
    public class TransitionDoor : MonoBehaviour
    {
        public RectTransform leftDoor;
        public RectTransform rightDoor;
        public float speed;
        public float deltaPosition;

        public AudioSource slideSound;

        public bool isOpen { get; private set; }

        Vector2 leftOriginalPosition, rightOriginalPosition;

        void Start()
        {
            leftOriginalPosition  = leftDoor.anchoredPosition;
            rightOriginalPosition = rightDoor.anchoredPosition;

            isOpen = false;
        }

        public void ToggleState()
        {
            StopAllCoroutines();
            if (isOpen) {
                StartCoroutine(CloseDoors());
            } else {
                StartCoroutine(OpenDoors());
            }
        }

        IEnumerator OpenDoors()
        {
            float targetLeft  = leftDoor.anchoredPosition.x  - deltaPosition;
            //float targetRight = rightDoor.anchoredPosition.x + deltaPosition;

            slideSound.Play();

            while (leftDoor.anchoredPosition.x > targetLeft)
            {
                yield return new WaitForEndOfFrame();

                leftDoor.anchoredPosition  += Vector2.left  * speed;
                rightDoor.anchoredPosition += Vector2.right * speed;
            }

            //leftDoor.anchoredPosition  = Vector2.left  * targetLeft;
            //rightDoor.anchoredPosition = Vector2.right * targetRight;

            isOpen = true;
        }

        IEnumerator CloseDoors()
        {
            float targetLeft  = leftOriginalPosition.x;
            float targetRight = rightOriginalPosition.x;

            slideSound.Play();

            while (leftDoor.anchoredPosition.x < targetLeft)
            {
                leftDoor.anchoredPosition  -= Vector2.left  * speed * 3;
                rightDoor.anchoredPosition -= Vector2.right * speed * 3;
                yield return new WaitForEndOfFrame();
            }

            //leftDoor.anchoredPosition  = Vector2.left * targetLeft;
            //rightDoor.anchoredPosition = Vector2.right * targetRight;

            isOpen = false;
        }
    }
}
