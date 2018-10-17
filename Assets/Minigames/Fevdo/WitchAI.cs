using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
namespace Fevdo{
    public class WitchAI: MonoBehaviour{
        enum State{Flying, Vulnerable}
        State state;

        [SerializeField] int maxLives = 3;
        int lives;
        bool shield;
        [SerializeField] float shieldCooldown;
        float shieldTimer;

        Tweener tween;
        Vector2 screenMid;
        [SerializeField][Range(0f,1f)]
        float lowerBound;
        List<Coroutine> patterns;
        [Header("Random Movement Parameters")]
        Rigidbody2D rb;
        Vector2 velDirection;
        float flightTimer = 0.0f;
        [SerializeField] float startDirectionChangeFrequency = 2f;
        [SerializeField] float finalDirectionChangeFrequency = 1f;
        float dirChangeFreDecay;
        float directionChangeFreq = 0f;
        
        float flightSpeed= 50f;


        void Start(){
            screenMid = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height * lowerBound));
            Debug.Log(screenMid);
            rb = GetComponent<Rigidbody2D>();
            directionChangeFreq = startDirectionChangeFrequency;
            
        }

        Coroutine currentMovement;

        void FixedUpdate(){
            RamdomFlight();

            if(Input.GetKeyDown(KeyCode.E)){
                if(currentMovement != null) StopCoroutine(currentMovement);
                currentMovement =  StartCoroutine(DoSpiral(5f, 10f, -1));
            }
            if(Input.GetKeyDown(KeyCode.F)){
                if(currentMovement != null) StopCoroutine(currentMovement);
                currentMovement = StartCoroutine(DoSpiral(5f, 10f, 1));
            }

        }

        IEnumerator DoSpiral(float linearSpeed, float duration, int direction){
            Vector2 radiusDir = new Vector2(screenMid.x, screenMid.y);
            float radialSpeed = (radiusDir.magnitude/duration)*direction;
            Vector2 radius = (radialSpeed < 0)? radiusDir : Vector2.zero; 
            radiusDir = radiusDir.normalized;
            
            float timer = 0.0f;

            while(timer < duration){
                float x = (radius.x) * Mathf.Cos(timer * linearSpeed);
                float y = (radius.y) * Mathf.Sin(timer * linearSpeed) + screenMid.y;

                var newPosition = new Vector3(x, y, 0);
                var diff = transform.position - newPosition;
                var angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), 1);
                rb.MovePosition(newPosition);
                
                radius += radiusDir * radialSpeed * Time.smoothDeltaTime;

                timer += Time.smoothDeltaTime;

                yield return null;
            }
        }

        void RamdomFlight(){
            flightTimer += Time.deltaTime;
            Debug.Log("deltaTime = " + Time.deltaTime);
            rb.velocity = -transform.right * flightSpeed;

            //Vector2 v = rb.velocity;
			//var angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
			//transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), 1);

            if(flightTimer >= directionChangeFreq){
                flightTimer = 0;
                transform.DORotate(Vector3.forward * Random.Range(0, 360), directionChangeFreq);
                
            }            
        }

        void OnDrawGizmos(){
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(Camera.main.ScreenToWorldPoint(new Vector2(Screen.width/2, Screen.height/(lowerBound*2))),.1f);
        }

        void OnCollisionEnter2D(Collision2D coll){
            if(coll.transform.GetComponent<Arrow>()){
                
            } else if(coll.gameObject.name == "Bounds"){
                rb.velocity = -rb.velocity;
            }
        }
        void OnTriggerEnter2D(Collider2D coll){
            if(coll.gameObject.name == "Bounds"){
                rb.velocity = -rb.velocity;
            }
        }

        IEnumerator shieldRecharge(){
            shield = false;
            for(float i = 0; i < shieldCooldown; i+= Time.smoothDeltaTime){
                yield return null;
            }
            shield = true;
        }
    }
}