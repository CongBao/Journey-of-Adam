using UnityEngine;
using System.Collections;
using JoA.Adam;

namespace JoA.Scenes.Building {

	public class Charger : MonoBehaviour {

		[SerializeField] private BatteryManager batteryManager;

		private Animator chargeAnim;

		void Awake () {
			chargeAnim = GetComponent<Animator> ();
		}

		void OnTriggerEnter2D (Collider2D coll) {
			batteryManager.ChargeBattery ();
			chargeAnim.SetBool ("charge", true);
		}

		void OnTriggerExit2D (Collider2D coll) {
			batteryManager.StopChargeBattery ();
			chargeAnim.SetBool ("charge", false);
		}

	}

}