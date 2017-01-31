using UnityEngine;
using System.Collections;
using System.IO;
using JoA.Save;

namespace JoA.Scenes.Logo {

	[RequireComponent (typeof (ConfigManager))]
	public class GameStart : MonoBehaviour {

		private ConfigManager configManager;

		void Start () {
			CreateFilePath ();
			configManager = GetComponent<ConfigManager> ();
			configManager.ReadConfig ();
		}

		void CreateFilePath () {
			string savePath = Application.persistentDataPath + "/Saves";
			string configPath = Application.persistentDataPath + "/Config";
			if (!Directory.Exists (savePath)) {
				Directory.CreateDirectory (savePath);
			}
			if (!Directory.Exists (configPath)) {
				Directory.CreateDirectory (configPath);
			}
		}

	}

}