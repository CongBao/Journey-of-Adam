using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections;
using JoA.Adam;

namespace JoA.UI {

	public class MenuManager : MonoBehaviour {

		[SerializeField] private RectTransform menuBtn;
		[SerializeField] private Animator initiallyOpen;
		[SerializeField] private Animator gameOverPanel;

		private int openParamId;
		private Animator openPanel;
		private GameObject previouslySelected;

		private AdamCharacter character;

		const string openTransitionName = "Open";
		const string closedStateName = "Closed";

		void Awake () {
			GameObject adam = GameObject.FindGameObjectWithTag ("Player");
			if (adam != null) {
				character = adam.GetComponent<AdamCharacter> ();
				character.AdamDead += OnAdamDead;
			}
		}

		void OnEnable () {
			openParamId = Animator.StringToHash (openTransitionName);
			if (initiallyOpen == null) {
				return;
			}
			OpenPanel (initiallyOpen);
		}

		void OnAdamDead (object sender, EventArgs e) {
			if (gameOverPanel == null) {
				return;
			}
			CloseMenuBtn ();
			OpenPanel (gameOverPanel);
			character.AdamDead -= OnAdamDead;
		}

		public void OpenPanel (Animator panel) {
			if (openPanel == panel) {
				return;
			}
			panel.gameObject.SetActive (true);
			var newPreviouslySelected = EventSystem.current.currentSelectedGameObject;
			panel.transform.SetAsLastSibling ();
			CloseCurrentPanel ();
			previouslySelected = newPreviouslySelected;
			openPanel = panel;
			openPanel.SetBool (openParamId, true);
			// SetSelected (FindFirstEnabledSelectable (panel.gameObject));
		}

		public void CloseCurrentPanel () {
			if (openPanel == null) {
				return;
			}
			openPanel.SetBool (openParamId, false);
			SetSelected (previouslySelected);
			StartCoroutine (DisablePanelDeleyed (openPanel));
			openPanel = null;
		}

		public void OpenMenuBtn () {
			if (menuBtn == null) {
				return;
			}
			menuBtn.gameObject.SetActive (true);
			Globe.allowControl = true;
		}

		public void CloseMenuBtn () {
			if (menuBtn == null) {
				return;
			}
			menuBtn.gameObject.SetActive (false);
			Globe.allowControl = false;
		}

		private IEnumerator DisablePanelDeleyed (Animator anim) {
			bool closedStatedReached = false;
			bool wantToClose = true;
			while (!closedStatedReached && wantToClose) {
				if (!anim.IsInTransition (0)) {
					closedStatedReached = anim.GetCurrentAnimatorStateInfo (0).IsName (closedStateName);
				}
				wantToClose = !anim.GetBool (openParamId);
				yield return new WaitForEndOfFrame ();
			}
			if (wantToClose) {
				anim.gameObject.SetActive (false);
			}
		}

		private void SetSelected (GameObject go) {
			EventSystem.current.SetSelectedGameObject (go);
		}

		static GameObject FindFirstEnabledSelectable (GameObject gameObj) {
			GameObject go = null;
			var selectables = gameObj.GetComponentsInChildren<Selectable> (true);
			foreach (var selectable in selectables) {
				if (selectable.IsActive () && selectable.IsInteractable ()) {
					go = selectable.gameObject;
					break;
				}
			}
			return go;
		}

		// game settings
		public void ChooseEN (bool isEn) {
			if (isEn) {
				Globe.language = Lang.en;
			} else {
				Globe.language = Lang.cn;
			}
		}

		public void ChooseCN (bool isCn) {
			if (isCn) {
				Globe.language = Lang.cn;
			} else {
				Globe.language = Lang.en;
			}
		}

		public void ChangeTextSpeed (float speed) {
			Globe.textSpeed = 0.2f - speed;
		}

		// graphic settings
		public void ChangeResolution (int option) {
			Globe.currentRes = Globe.supportResMap [option];
			Screen.SetResolution (Globe.supportResMap [option].width, Globe.supportResMap [option].height, Globe.isFullScreen);
		}

		public void ChangeFullScreen (bool isFull) {
			if (isFull) {
				Globe.isFullScreen = true;
				Screen.fullScreen = true;
			} else {
				Globe.isFullScreen = false;
				Screen.fullScreen = false;
			}
		}

		public void ChangeQualityLevel (int level) {
			QualitySettings.SetQualityLevel (level);
		}

		public void ChangeShadowRes (int option) {
			switch (option) {
			case 0:
				QualitySettings.shadowResolution = ShadowResolution.Low;
				break;
			case 1:
				QualitySettings.shadowResolution = ShadowResolution.Medium;
				break;
			case 2:
				QualitySettings.shadowResolution = ShadowResolution.High;
				break;
			case 3:
				QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
				break;
			default:
				return;
			}
		}

		public void ChangeAA (float value) {
			if (value == 3f) {
				QualitySettings.antiAliasing = 8;
			} else {
				QualitySettings.antiAliasing = (int)value * 2;
			}
		}

		public void ChangeVSyncs (float value) {
			QualitySettings.vSyncCount = (int)value;
		}

		// audio settings
		public void ChangeMasterVolume (float volume) {
			Globe.masterVolume = volume;
		}

		public void ChangeBgmVolume (float volume) {
			Globe.bgmVolume = volume;
		}

		public void ChangeSfxVolume (float volume) {
			Globe.sfxVolume = volume;
		}

	}

}