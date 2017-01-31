using UnityEngine;
using System.Collections;

namespace JoA.Audio.Behaviour {

	public class RunBehaviour : StateMachineBehaviour {

		private AudioSource runSfx;

		void Awake () {
			runSfx = GameObject.FindGameObjectWithTag ("Audio").transform.Find ("AdamRun").GetComponent<AudioSource> ();
		}

		public override void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			if (!runSfx.isPlaying) {
				runSfx.Play ();
			}
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			if (runSfx.isPlaying) {
				runSfx.Stop ();
			}
		}

	}

}