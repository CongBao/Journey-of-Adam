using UnityEngine;
using System;
using System.Collections;

namespace JoA.Adam {

	[RequireComponent (typeof (Animator))]
	public class BatteryManager : MonoBehaviour {

		[SerializeField] private float idleConsumeSpeed = 0.00002f;
		[SerializeField] private float activeConsumeSpeed = 0.00005f;
		[SerializeField] private float chargeSpeed = 0.0005f;

		private Animator powerAnim;
		private Animator adamAnim;

		private bool isConsuming = true;
		private bool isCharging = false;

		public event EventHandler BatteryConsumed;
		public event EventHandler BatteryCharged;

		public void ChargeBattery () {
			if (isCharging) {
				return;
			}
			isCharging = true;
			isConsuming = !isCharging;
		}

		public void StopChargeBattery () {
			if (!isCharging) {
				return;
			}
			isCharging = false;
			isConsuming = !isCharging;
		}

		void Awake () {
			powerAnim = GetComponent<Animator> ();
			adamAnim = GameObject.FindGameObjectWithTag ("Player").GetComponent<Animator> ();
		}

		void FixedUpdate () {
			if (isConsuming) {
				if (Globe.batteryPower > 0f) {
					ConsumePower ();
				} else {
					Globe.batteryPower = 0f;
					if (BatteryConsumed != null) {
						BatteryConsumed (this, EventArgs.Empty);
					}
				}
			} else if (isCharging) {
				if (Globe.batteryPower < 1f) {
					ChargePower ();
				} else {
					Globe.batteryPower = 1f;
					if (BatteryCharged != null) {
						BatteryCharged (this, EventArgs.Empty);
					}
				}
			}
			UpdateBatteryPower ();
		}

		private void ConsumePower () {
			if (adamAnim.GetBool ("ground") && adamAnim.GetFloat ("hSpeed") < 0.01f) {
				Globe.batteryPower -= idleConsumeSpeed;
			} else {
				Globe.batteryPower -= activeConsumeSpeed;
			}
		}

		private void ChargePower () {
			Globe.batteryPower += chargeSpeed;
		}

		private void UpdateBatteryPower () {
			powerAnim.SetFloat ("Power", Globe.batteryPower);
		}

	}

}