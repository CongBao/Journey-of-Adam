using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using JoA.CrossPlatformInput;
using JoA.UI;
using JoA.Video;

namespace JoA.Scenes.Prologue {

	[RequireComponent (typeof (DialogManager))]
	public class TextProcess : MonoBehaviour {

		[SerializeField] private SceneFader fader;
		[SerializeField] private AudioSource bgm;
		[SerializeField] private VideoController videoPlayer;

		private ApplicationManager appManager;
		private DialogManager dialogManager;
		private Text dialog;

		private int textId = 0;
		private int[] cutId = { 7, 13, 19, 26, 38, 44, 49, 51 };
		private int cutPtr = 0;

		private bool bufLock = false;

		void Awake () {
			appManager = GetComponent<ApplicationManager> ();
			dialogManager = GetComponent<DialogManager> ();
			dialog = GetComponent<Text> ();
			fader.SceneStarted += OnSceneStarted;
			fader.SceneClosed += OnSceneClosed;
			videoPlayer.VideoStopped += OnVideoStopped;
			//fader.SceneStarting = true;
		}

		void Update () {
			if (bufLock && CrossPlatformInputManager.GetButtonDown ("Confirm")) {
				if (textId == cutId [cutId.Length - 1]) {
					bufLock = false;
					fader.SceneClosing = true;
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
		}

		void OnSceneStarted (object sender, EventArgs e) {
			StartCoroutine (BufferedGetText (GetText (++textId)));
			Image panelImg = GetComponentInParent<Image> ();
			Color color = panelImg.color;
			color.a = 0.4f;
			panelImg.color = color;
			fader.SceneStarted -= OnSceneStarted;
		}

		void OnSceneClosed (object sender, EventArgs e) {
			if (bgm.isPlaying) {
				bgm.Stop ();
			}
			videoPlayer.PlayVideo ();
			fader.SceneClosed -= OnSceneClosed;
		}

		void OnVideoStopped (object sender, EventArgs e) {
			appManager.LoadScene ("Building");
			videoPlayer.VideoStopped -= OnVideoStopped;
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