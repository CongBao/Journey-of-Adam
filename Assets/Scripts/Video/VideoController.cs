using UnityEngine;
using System;
using System.Collections;

namespace JoA.Video {

	[RequireComponent (typeof (AudioSource))]
	[RequireComponent (typeof (GUITexture))]
	public class VideoController : MonoBehaviour {

		private GUITexture videoPlayer;
		private MovieTexture movie;
		private AudioSource audioSrc;

		public event EventHandler VideoStopped;

		private bool startPlay = false;

		void Awake () {
			videoPlayer = GetComponent<GUITexture> ();
			audioSrc = GetComponent<AudioSource> ();
			movie = (MovieTexture) videoPlayer.texture;
			videoPlayer.enabled = false;
			movie.loop = false;
			UpdateVideoVolume ();
		}

		void Update () {
			if (startPlay && !movie.isPlaying) {
				VideoStopped (this, EventArgs.Empty);
			}
		}

		public void UpdateVideoVolume () {
			audioSrc.volume = Globe.masterVolume;
		}

		public void PlayVideo () {
			videoPlayer.enabled = true;
			startPlay = true;
			videoPlayer.pixelInset = new Rect (Screen.width * 0.05f, Screen.height * 0.05f, Screen.width * 0.9f, Screen.height * 0.9f);
			movie.Play ();
		}

	}

}