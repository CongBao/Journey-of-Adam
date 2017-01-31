using UnityEngine;
using System;
using System.Collections;

namespace JoA.Scenes {

	[RequireComponent (typeof (Collider2D))]
	public class DeadZone : MonoBehaviour {

		public event EventHandler DeadZoneEnter;

		void OnCollisionEnter2D (Collision2D coll) {
			if (coll.gameObject.tag == "Player" && DeadZoneEnter != null) {
				DeadZoneEnter (this, EventArgs.Empty);
			}
		}

	}

}