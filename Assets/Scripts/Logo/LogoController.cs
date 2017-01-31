using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using JoA.Scenes;

namespace JoA.Logo {

	public class LogoController : CinematicsController {

		protected override void Awake () {
			base.Awake ();
			CinematicsEnd += OnCinematicsEnd;
		}

		protected override void Update () {
			base.Update ();
		}

		void OnCinematicsEnd (object sender, EventArgs e) {
			SceneManager.LoadScene ("Title");
			CinematicsEnd -= OnCinematicsEnd;
		}

	}

}