using System;
using UnityEngine;
using JoA.Scenes;

namespace JoA.Adam {
	
	public class AdamCharacter : MonoBehaviour {

		[SerializeField] private float maxSpeed = 10f;
		[SerializeField] private float jumpForce = 400f;
		[SerializeField] [Range(0, 1)] private float crouchSpeed = .36f;
		[SerializeField] private bool airControl = false;
		[SerializeField] private LayerMask whatIsGround;
		[SerializeField] private GameObject cameraFollowed;
		[SerializeField] private BatteryManager batteryManager;
		[SerializeField] private DeadZone[] deadZones;

		private Animator animator;
		private Rigidbody2D rigidBody;
		private float direction;

		// check ground
		private Transform groundCheck;
		const float groundRadius = .2f;
		private bool isGrounded;

		// check ceiling
		private Transform ceilingCheck;
		const float ceilingRadius = .01f;

		public event EventHandler AdamDead;

		void Awake () {
			transform.localPosition = Globe.birthPosition;
			cameraFollowed.transform.localPosition = Globe.birthPosition;
			animator = GetComponent<Animator> ();
			rigidBody = GetComponent<Rigidbody2D> ();
			direction = transform.localScale.x;
			groundCheck = transform.Find ("GroundCheck");
			ceilingCheck = transform.Find ("CeilingCheck");
			batteryManager.BatteryConsumed += OnBatteryConsumed;
			if (deadZones.Length > 0) {
				foreach (DeadZone dz in deadZones) {
					dz.DeadZoneEnter += OnDeadZoneEnter;
				}
			}
		}

		void FixedUpdate () {
			isGrounded = false;
			Collider2D[] colliders = Physics2D.OverlapCircleAll (groundCheck.position, groundRadius, whatIsGround);
			foreach (Collider2D collider in colliders)
				if (collider.gameObject != gameObject)
					isGrounded = true;
			animator.SetBool ("ground", isGrounded);
			animator.SetFloat ("vSpeed", rigidBody.velocity.y);
		}

		void OnBatteryConsumed (object sender, EventArgs e) {
			CharacterDead ();
			batteryManager.BatteryConsumed -= OnBatteryConsumed;
		}

		void OnDeadZoneEnter (object sender, EventArgs e) {
			CharacterDead ();
			foreach (DeadZone dz in deadZones) {
				dz.DeadZoneEnter -= OnDeadZoneEnter;
			}
		}

		void CharacterDead () {
			Globe.allowControl = false;
			Move (0f, false, false);
			animator.SetBool ("dead", true);
			if (AdamDead != null)
				AdamDead (this, EventArgs.Empty);
		}

		public void Move (float move, bool crouch, bool jump) {
			if (!crouch && animator.GetBool ("crouch"))
				if (Physics2D.OverlapCircle (ceilingCheck.position, ceilingRadius, whatIsGround))
					crouch = true;
			animator.SetBool ("crouch", crouch);

			if (isGrounded || airControl) {
				move = crouch ? move * crouchSpeed : move;
				animator.SetFloat ("hSpeed", Mathf.Abs(move));
				rigidBody.velocity = new Vector2 (move * maxSpeed, rigidBody.velocity.y);

				float turnTo = Input.GetAxis("Horizontal");
				if (!animator.GetBool ("dead") && turnTo * direction < 0) {
					Vector3 scale = transform.localScale;
					scale.x *= -1;
					direction = scale.x;
					transform.localScale = scale;
				}
			}

			if (isGrounded && jump && animator.GetBool ("ground")) {
				isGrounded = false;
				animator.SetBool ("ground", false);
				rigidBody.AddForce (new Vector2 (0f, jumpForce));
			}
		}

	}

}