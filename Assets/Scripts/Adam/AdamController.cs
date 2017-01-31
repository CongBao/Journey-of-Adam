using System;
using UnityEngine;
using JoA.CrossPlatformInput;

namespace JoA.Adam {

	[RequireComponent (typeof (AdamCharacter))]
	public class AdamController : MonoBehaviour {

		private AdamCharacter character;
		private bool jump;

		private void Awake () {
			character = GetComponent<AdamCharacter> ();
		}

		private void Update () {
			if (!jump)
				jump = CrossPlatformInputManager.GetButtonDown ("Jump");
		}

		private void FixedUpdate () {
			if (!Globe.allowControl) {
				character.Move (0f, false, false);
				return;
			}
			bool crouch = Input.GetKey (KeyCode.LeftControl);
			float h = CrossPlatformInputManager.GetAxis ("Horizontal");
			character.Move (h, crouch, jump);
			jump = false;
		}

	}

}