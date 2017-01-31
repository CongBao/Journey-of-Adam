using UnityEngine;
using System.Collections;

namespace JoA.Scenes.Building {

	public class Deliver : MonoBehaviour {

		[SerializeField] private Transform target;

		private Transform player;
		private Transform cameraRig;
		private Animator timerAn;

		void Awake () {
			player = GameObject.FindGameObjectWithTag ("Player").transform;
			cameraRig = GameObject.FindGameObjectWithTag ("CameraRig").transform;
			timerAn = GetComponentInChildren<Animator> ();
		}

		void OnTriggerEnter2D (Collider2D coll) {
			timerAn.SetBool ("active", true);
			Invoke ("Move", 2.7f);
		}

		void OnTriggerExit2D (Collider2D coll) {
			timerAn.SetBool ("active", false);
			CancelInvoke ();
		}

		void Move () {
			Vector3 pos = target.localPosition;
			player.localPosition = pos;
			cameraRig.localPosition = pos;
		}

	}

}