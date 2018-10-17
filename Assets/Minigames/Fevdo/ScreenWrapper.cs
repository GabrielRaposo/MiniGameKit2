using UnityEngine;

namespace Fevdo
{
    public class ScreenWrapper: MonoBehaviour{

        Rect screen;

        void Start(){
            var min = Camera.main.ScreenToWorldPoint(Vector2.zero);
            var max = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
            screen = new Rect(min, (max - min));
            Debug.Log(screen);
        }

        void FixedUpdate(){
            if(transform.position.x < screen.xMin){
                transform.position = new Vector2(screen.xMax, transform.position.y);
            }
            if(transform.position.x > screen.xMax){
                transform.position = new Vector2(screen.xMin, transform.position.y);
            }
            if(transform.position.y > screen.yMax){
                transform.position = new Vector2(transform.position.x, screen.yMin);
            }
            if(transform.position.y < screen.yMin){
                transform.position = new Vector2(transform.position.x, screen.yMax);
            }
        }

        void OnDrawGizmosSelected(){
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(screen.center, screen.size);
        }
    }

}