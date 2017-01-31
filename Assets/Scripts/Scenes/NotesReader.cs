using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using JoA.CrossPlatformInput;
using JoA.UI;

namespace JoA.Scenes {

	[RequireComponent (typeof (Collider2D))]
	public class NotesReader : MonoBehaviour {

		[SerializeField] private int noteId;

		private GameObject noteObj;
		private DialogManager dialogManager;
		private Text note;

		private bool isEnter = false;

		void Awake () {
			noteObj = GameObject.FindGameObjectWithTag ("Notes");
			dialogManager = noteObj.GetComponentInChildren<DialogManager> (true);
			note = noteObj.GetComponentInChildren<Text> (true);
		}

		void Update () {
			if (isEnter && CrossPlatformInputManager.GetButtonDown ("Obtain")) {
				noteObj.transform.Find ("NotePanel").gameObject.SetActive (true);
				note.text = GetText (noteId);
			}
		}

		void OnTriggerEnter2D (Collider2D coll) {
			isEnter = true;
		}

		void OnTriggerExit2D (Collider2D coll) {
			isEnter = false;
		}

		private string GetText (int id) {
			return dialogManager.GetDialogContent (id, Globe.language);
		}

	}

}