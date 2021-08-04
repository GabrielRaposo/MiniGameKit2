using UnityEngine;
using System.Collections;
using LetterboxCamera;

namespace LetterboxCamera {

    public class LetterboxGameDemo : MonoBehaviour {

        public ForceCameraRatio cameraManager;
        float letterboxRate;
        Vector2 targetRatio;

        public void Start() {
            targetRatio = new Vector2(16, 10);
        }

        public void Update() {
            cameraManager.ratio = Vector2.Lerp(cameraManager.ratio, targetRatio, letterboxRate * Time.deltaTime);
        }
    }
}