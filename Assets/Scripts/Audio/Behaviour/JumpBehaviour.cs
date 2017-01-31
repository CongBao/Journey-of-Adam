using UnityEngine;
using System.Collections;

namespace JoA.Audio.Behaviour {

	public class JumpBehaviour : StateMachineBehaviour {

		private AudioSource jumpSfx;

		void Awake () {
			jumpSfx = GameObject.FindGameObjectWithTag ("Audio").transform.Find ("AdamJump").GetComponent<AudioSource> ();
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			if (!jumpSfx.isPlaying) {
				jumpSfx.PlayDelayed (0.065f * Mathf.Abs (animator.GetFloat ("vSpeed")));
			}
		}

	}

}