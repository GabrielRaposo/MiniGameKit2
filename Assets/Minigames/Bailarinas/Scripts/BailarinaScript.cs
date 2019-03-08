using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using Random = UnityEngine.Random;

namespace Bailarinas
{

	public class BailarinaScript : PlayerInfo
    {
        private bool lockedMovement;

        public bool dead = false;

        public float impulse;
        public float speed;
        private Rigidbody rb;

        private PezinhoScript pezinho;

        public Action onFall;
        private float angle;

        public Animator anim;
        public Transform meshTransform;

		public GameObject balanceBar;
		public GameObject arrow;
		public GameObject arrowPivot;

		public float Angle
		{
			get
			{
				return angle;
			}

			set
			{
				angle = value;
				arrowPivot.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, angle / 2);
			}
		}

		private void Awake()
        {
            base.Start();
            //base.Awake();
            SetColor();
        }

        override public void Start()
        {
            base.Start();
            rb = GetComponent<Rigidbody>();
            Unbalance(4.0f, 1);
            pezinho = GetComponentInChildren<PezinhoScript>();

        }
                
        void Update()
        {                        
            speed = rb.velocity.z;
            if (!dead)
            {
                anim.SetFloat("Blend", Angle);
                anim.SetFloat("Speed", rb.velocity.magnitude);
            }
        }

        private void FixedUpdate()
        {
            FixMovement();
            Move();            
            Rebalance();
            FixMovement();

            Angle = transform.rotation.eulerAngles.z;
            if(Angle > 180)
            {
                Angle -= 360;
            }

            if (Mathf.Abs(Angle) > 50.0f)
            {                
                Die();
                StartCoroutine(CallOnFall());
            }
        }

        void FixMovement()
        {
            transform.position = transform.position - (new Vector3(pezinho.transform.position.x - pezinho.xAxis, 0, 0));
        }


        void Move()
        {

            if(Input.GetAxisRaw(playerButtons.vertical) >= 1.0f && lockedMovement == false)
            {
                Step();
            }

            if(Input.GetAxisRaw(playerButtons.vertical) == 0 && lockedMovement == true)
            {
                lockedMovement = false;
            }

        }

        void Step()
        {
            rb.AddForce(Vector3.forward * impulse, ForceMode.Acceleration);
            Unbalance();
            lockedMovement = true;
        }

        void Unbalance()
        {
            float force = Random.Range(5f, 7f);
            int direction;

            if(transform.rotation.z >= 0)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }

            rb.AddTorque(Vector3.forward * direction * force * speed, ForceMode.Acceleration);
        }

        void Unbalance(float force, int direction)
        {

            rb.AddTorque(Vector3.forward * direction * force, ForceMode.Acceleration);

        }

        void Rebalance()
        {
            rb.AddTorque(Vector3.forward * Input.GetAxisRaw(playerButtons.horizontal) * 2.5f *-1, ForceMode.Acceleration);
            
        }

        public void Die()
        {
            dead = true;
            rb.constraints = RigidbodyConstraints.None;
            this.enabled = false;

			SetTransparent(balanceBar);
			SetTransparent(arrow);
			
			meshTransform.GetComponent<PositionConstraint>().enabled = false;
            
            anim.enabled = false;
            meshTransform.SetParent(transform);
        }

        public void Win()
        {
            transform.rotation = Quaternion.identity;
            transform.position.Set(transform.position.x, transform.position.y + 3.0f, transform.position.z);

			SetTransparent(balanceBar);
			SetTransparent(arrow);

            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;

        }

        public void SetColor()
        {
            meshTransform.Find("Mesh").GetComponent<SkinnedMeshRenderer>().materials[3].color = color;
			arrow.GetComponent<UnityEngine.UI.Image>().color = color;

			float h = 1, s = 1, v = 1;
			Color lowSatColor;

			Color.RGBToHSV(color, out h, out s, out v);

			s *= 0.5f;
			lowSatColor = Color.HSVToRGB(h, s, v);


			balanceBar.GetComponent<UnityEngine.UI.Image>().color = lowSatColor;

			

			//GetComponentInChildren<MeshRenderer>().materials[3].color = color;
		}

        public IEnumerator CallOnFall()
        {
            yield return new WaitForSeconds(1.0f);
            onFall();
        }

		void SetTransparent(GameObject go)
		{
			go.GetComponent<UnityEngine.UI.Image>().color = Color.clear;
		}

	}



}


