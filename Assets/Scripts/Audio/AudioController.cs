using UnityEngine;
using System.Collections;

namespace JoA.Audio {

	public class AudioController : MonoBehaviour {

		[SerializeField] private AudioSource[] audioSrc;
		[SerializeField] private AudioType[] audioType;

		void Awake () {
			UpdateVolume ();
		}

		public void UpdateVolume () {
			if (audioSrc.Length != audioType.Length) {
				return;
			}
			for (int i = 0; i < audioSrc.Length; i++) {
				if (audioSrc[i] == null)
					continue;
				audioSrc[i].volume = Globe.masterVolume;
				if (audioType[i] == AudioType.bgm) {
					audioSrc[i].volume *= Globe.bgmVolume;
				}
				if (audioType[i] == AudioType.sfx) {
					audioSrc[i].volume *= Globe.sfxVolume;
				}
			}
		}

	}

}