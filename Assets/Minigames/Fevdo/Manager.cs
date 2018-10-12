using UnityEngine;

namespace Fevdo{
    public class Manager: MonoBehaviour{

        public static float elapsedTime{get; private set;}
        public static float elapsedTimeinMinutes
        {
            get{
                return elapsedTime/60f;
            }
        }
        void Start(){
            elapsedTime = 0.0f;
        }
        void Update(){
            elapsedTime += Time.unscaledDeltaTime;
        }
    }
}