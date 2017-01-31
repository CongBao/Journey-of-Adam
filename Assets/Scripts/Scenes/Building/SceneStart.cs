using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using JoA.UI;
using JoA.CrossPlatformInput;

namespace JoA.Scenes.Building {

	public class SceneStart : CinematicsController {

		[SerializeField] private Image fader;
		[SerializeField] private Image textPanel;

		private DialogManager dialogManager;
		private Text dialog;

		private bool panelIsClear = false;
		private bool bufLock = false;
		private bool textEnd = false;

		private int textId = 0;
		private int[] cutId = { 8, 15, 19, 28, 34, 38, 48 };
		private int cutPtr = 0;

		public event EventHandler SceneStarted;

		protected override void Awake () {
			if (Globe.storyLineMap ["C1S1"]) {
				targetImage.gameObject.SetActive (false);
				textPanel.gameObject.SetActive (false);
				fader.gameObject.SetActive (false);
				Globe.allowControl = true;
				return;
			}
			base.Awake ();
			Globe.allowControl = false;
			GameObject.FindGameObjectWithTag ("Menu").transform.Find ("MenuBtn").gameObject.SetActive (false);
			GameObject.FindGameObjectWithTag ("Battery").transform.Find ("Power").gameObject.SetActive (false);
			dialogManager = textPanel.GetComponentInChildren<DialogManager> (true);
			dialog = textPanel.GetComponentInChildren<Text> (true);
			CinematicsEnd += OnCinematicsEnd;
		}

		protected override void Update () {
			if (Globe.storyLineMap ["C1S1"]) {
				return;
			}
			base.Update ();
			if (!panelIsClear && textPanel.gameObject.activeInHierarchy) {
				if (textPanel.color.a < 0.4f) {
					ToClear (textPanel);
				} else {
					panelIsClear = true;
					StartCoroutine (BufferedGetText (GetText (++textId)));
				}
			}
			if (!panelIsClear) {
				return;
			}
			if (bufLock && CrossPlatformInputManager.GetButtonDown ("Confirm")) {
				if (textId == cutId [cutId.Length - 1]) {
					bufLock = false;
					textEnd = true;
					return;
				}
				if (textId++ != cutId [cutPtr]) {
					StartCoroutine (BufferedGetText (GetText (textId)));
				} else {
					dialog.text = "";
					StartCoroutine (BufferedGetText (GetText (textId)));
					cutPtr++;
				}
			}
			if (textEnd) {
				dialog.text = "";
				if (textPanel.color.a > 0.02f || fader.color.a > 0.02f) {
					ToDark (textPanel);
					ToDark (fader);
				} else {
					textPanel.gameObject.SetActive (false);
					fader.gameObject.SetActive (false);
					GameObject.FindGameObjectWithTag ("Dialog").transform.Find ("DialogPanel").gameObject.SetActive (true);
					textEnd = false;
					Globe.storyLineMap ["C1S1"] = true;
					SceneStarted (this, EventArgs.Empty);
				}
			}
		}

		void OnCinematicsEnd (object sender, EventArgs e) {
			targetImage.gameObject.SetActive (false);
			textPanel.gameObject.SetActive (true);
			CinematicsEnd -= OnCinematicsEnd;
		}

		private string GetText (int id) {
			return dialogManager.GetDialogContent (id, Globe.language) + "\n";
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