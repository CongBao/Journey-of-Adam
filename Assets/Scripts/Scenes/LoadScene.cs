using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

namespace JoA.Scenes {

	public class LoadScene : MonoBehaviour {

		[SerializeField] private RectTransform trans;

		private int progress;
		private Text text;
		private AsyncOperation async;

		void Start () {
			progress = 0;
			text = trans.GetComponent<Text> ();
			StartCoroutine (Load ());
		}

		void Update () {
			progress = (int)(async.progress * 100);
			text.text = progress + "%";
		}

		IEnumerator Load () {
			async = SceneManager.LoadSceneAsync (Globe.currentScene);
			yield return async;
		}

	}

}