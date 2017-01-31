using UnityEngine;
using System.Collections;

namespace JoA.Audio.Behaviour {

	public class CrouchBehaviour : StateMachineBehaviour {

		private AudioSource crouchSfx;

		void Awake () {
			crouchSfx = GameObject.FindGameObjectWithTag ("Audio").transform.Find ("AdamCrouch").GetComponent<AudioSource> ();
		}

		public override void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			if (!crouchSfx.isPlaying) {
				crouchSfx.Play ();
			}
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			if (crouchSfx.isPlaying) {
				crouchSfx.Stop ();
			}
		}

	}

}