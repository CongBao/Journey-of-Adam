using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using JoA.CrossPlatformInput;
using JoA.UI;

namespace JoA.Scenes.Building {

	public class DialogController : MonoBehaviour {
	
		private SceneStart sceneStart;
		private GameObject dialogObj;
		private DialogManager dialogManager;
		private Text dialog;

		private bool sceneStarted = false;
		private bool bufLock = false;

		private int textId = 0;
		private int stopId = 26;

		void Awake () {
			sceneStart = GetComponent<SceneStart> ();
			dialogObj = GameObject.FindGameObjectWithTag ("Dialog");
			dialogManager = dialogObj.GetComponentInChildren<DialogManager> (true);
			dialog = dialogObj.GetComponentInChildren<Text> (true);
			sceneStart.SceneStarted += OnSceneStarted;
		}

		void Update () {
			if (!sceneStarted) {
				return;
			}
			if (bufLock && CrossPlatformInputManager.GetButtonDown ("Confirm")) {
				if (textId == stopId) {
					bufLock = false;
					GameObject.FindGameObjectWithTag ("Dialog").transform.Find ("DialogPanel").gameObject.SetActive (false);
					GameObject.FindGameObjectWithTag ("Menu").transform.Find ("MenuBtn").gameObject.SetActive (true);
					GameObject.FindGameObjectWithTag ("Battery").transform.Find ("Power").gameObject.SetActive (true);
					Globe.batteryPower = 0.2f;
					Globe.allowControl = true;
					return;
				}
				dialog.text = "";
				StartCoroutine (BufferedGetText (GetText (++textId)));
			}
		}

		void OnSceneStarted (object sender, EventArgs e) {
			sceneStarted = true;
			StartCoroutine (BufferedGetText (GetText (++textId)));
			sceneStart.SceneStarted -= OnSceneStarted;
		}

		private string GetText (int id) {
			return dialogManager.GetDialogContent (id, Globe.language);
		}

		private IEnumerator BufferedGetText (string content) {
			bufLock = false;
			foreach (char c in content.ToCharArray ()) {
				yield return new WaitForSeconds (Globe.textSpeed);
				dialog.text += c;
			}
			bufLock = true;
		}

	}

}