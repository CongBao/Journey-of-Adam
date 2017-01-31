using UnityEngine;
using System;
using System.Collections;

namespace JoA.Scenes.Prologue {

	[RequireComponent (typeof (GUITexture))]
	public class SceneFader : MonoBehaviour {

		private GUITexture fader;

		public float FadeSpeed { get; set; }
		public bool SceneStarting { get; set; }
		public bool SceneClosing { get; set; }

		public event EventHandler SceneStarted;
		public event EventHandler SceneClosed;

		void Awake () {
			fader = GetComponent<GUITexture> ();
			FadeSpeed = 0.5f;
			SceneStarting = true;
			SceneClosing = false;

			fader.pixelInset = new Rect (0f, 0f, Screen.width, Screen.height);
		}

		void Update () {
			if (SceneStarting) {
				StartScene ();
			}
			if (SceneClosing) {
				EndScene ();
			}
		}

		public void StartScene () {
			FadeToClear ();
			if (fader.color.a < 0.05f) {
				fader.color = Color.clear;
				fader.enabled = false;
				SceneStarting = false;
				SceneStarted (this, EventArgs.Empty);
			}
		}

		public void EndScene () {
			fader.enabled = true;
			FadeToBlack ();
			if (fader.color.a > 0.95f) {
				fader.color = Color.black;
				SceneClosing = false;
				SceneClosed (this, EventArgs.Empty);
				fader.enabled = false;
			}
		}

		private void FadeToClear () {
			fader.color = Color.Lerp (fader.color, Color.clear, FadeSpeed * Time.deltaTime);
		}

		private void FadeToBlack () {
			fader.color = Color.Lerp (fader.color, Color.black, FadeSpeed * Time.deltaTime);
		}

	}

}