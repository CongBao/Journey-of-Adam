using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using JoA.UI;
using JoA.CrossPlatformInput;

namespace JoA.Scenes {

	[RequireComponent (typeof (Collider2D))]
	public class EventTrigger : MonoBehaviour {

		[SerializeField] private string storyLineId = "";
		[SerializeField] private int startId = 0;
		[SerializeField] private int stopId = 0;
		[SerializeField] private bool willNotComplete = false;
		[SerializeField] private string[] preStoryLineIds = null;

		private GameObject dialogObj;
		private DialogManager dialogManager;
		private Text dialog;

		private bool hasTriggered = false;
		private bool bufLock = false;
		private int textId;

		void Awake () {
			dialogObj = GameObject.FindGameObjectWithTag ("Dialog");
			dialogManager = dialogObj.GetComponentInChildren<DialogManager> (true);
			dialog = dialogObj.GetComponentInChildren<Text> (true);
			textId = startId - 1;
		}

		void Update () {
			if (storyLineId != "" && Globe.storyLineMap [storyLineId]) {
				return;
			}
			if (bufLock && CrossPlatformInputManager.GetButtonDown ("Confirm")) {
				if (textId == stopId) {
					bufLock = false;
					if (!willNotComplete)
						Globe.storyLineMap [storyLineId] = true;
					dialogObj.transform.Find ("DialogPanel").gameObject.SetActive (false);
					return;
				}
				dialog.text = "";
				StartCoroutine (BufferedGetText (GetText (++textId)));
			}
		}

		void OnTriggerEnter2D (Collider2D coll) {
			if (hasTriggered || (storyLineId != "" && Globe.storyLineMap [storyLineId])) {
				return;
			}
			if (preStoryLineIds != null)
				foreach (string s in preStoryLineIds)
					if (!Globe.storyLineMap [s])
						return;
			hasTriggered = true;
			dialogObj.transform.Find ("DialogPanel").gameObject.SetActive (true);
			dialog.text = "";
			StartCoroutine (BufferedGetText (GetText (++textId)));
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