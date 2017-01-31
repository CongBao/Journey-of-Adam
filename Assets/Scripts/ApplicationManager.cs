using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace JoA {

	public class ApplicationManager : MonoBehaviour {

		public void Quit () {
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#else
			Application.Quit ();
			#endif
		}

		public void LoadScene (string scene) {
			Globe.currentScene = scene;
			Globe.birthPosition = Globe.birthPosMap [scene];
			SceneManager.LoadSceneAsync (Globe.LOADING_SCENE);
		}

		public void ReloadScene () {
			Globe.currentScene = SceneManager.GetActiveScene ().name;
			Globe.birthPosition = GameObject.FindGameObjectWithTag ("Player").transform.localPosition;
			SceneManager.LoadSceneAsync (Globe.LOADING_SCENE);
		}

	}

}