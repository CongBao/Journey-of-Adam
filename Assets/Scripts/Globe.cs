using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JoA {

	public enum Lang {
		en, cn
	}

	public enum AudioType {
		bgm, sfx
	}

	public static class Globe {

		// game
		public const string GAME_NAME = "Journey of Adam";

		// language
		public static Lang language = Lang.en; // config ***** en

		// graphic
		public static Resolution currentRes = Screen.currentResolution; // config
		public static bool isFullScreen = Screen.fullScreen; // config

		// audio
		public static float masterVolume = 0.5f; // config
		public static float bgmVolume = 0.5f; // config
		public static float sfxVolume = 0.5f; // config

		// text
		public static float textSpeed = 0.1f; // config ***** 0.1f

		// character
		public static bool allowControl = true;
		public static float batteryPower = 1.0f; // save, load

		// scene
		public const string LOADING_SCENE = "Loading";
		public static string currentScene; // save, load
		public static Vector3 birthPosition; // ***** null

		// static dictionaries
		public static Dictionary<string, Vector3> birthPosMap;
		public static Dictionary<int, Resolution> supportResMap;
		public static Dictionary<string, bool> storyLineMap; // save, load

		static Globe () {
			birthPosMap = new Dictionary<string, Vector3> ();
			birthPosMap ["Logo"] = Vector3.zero;
			birthPosMap ["Title"] = Vector3.zero;
			birthPosMap ["Loading"] = Vector3.zero;
			birthPosMap ["Prologue"] = Vector3.zero;
			birthPosMap ["Building"] = new Vector3 (-46.75f, 14.65f, 0f);
			birthPosMap ["EasterEgg"] = new Vector3 (-43.23f, -25.16f, 0f);

			supportResMap = new Dictionary<int, Resolution> ();
			supportResMap [0] = new Resolution { width = 1280, height = 600, refreshRate = 60 };
			supportResMap [1] = new Resolution { width = 1280, height = 720, refreshRate = 60 };
			supportResMap [2] = new Resolution { width = 1280, height = 768, refreshRate = 60 };
			supportResMap [3] = new Resolution { width = 1360, height = 768, refreshRate = 60 };
			supportResMap [4] = new Resolution { width = 1366, height = 768, refreshRate = 60 };
			supportResMap [5] = new Resolution { width = 1600, height = 900, refreshRate = 60 };
			supportResMap [6] = new Resolution { width = 1920, height = 1080, refreshRate = 60 };

			storyLineMap = new Dictionary<string, bool> ();
			// C = Chapter, S = Scene, T = Trigger
			storyLineMap ["C1S1"] = false; // JoA.Scenes.Building.SceneStart  ***** false
			storyLineMap ["C1S1T1"] = false; // building adam room            ***** false
			storyLineMap ["C1S1T2"] = false; // building parent's room
			storyLineMap ["C1S1T3"] = false; // building door 04
			storyLineMap ["C1S1T4"] = false; // building double door 01
		}

		// static methods
		public static K GetKeyFormValue<K, V> (Dictionary<K, V> dic, V value) {
			return dic.FirstOrDefault (q => q.Value.Equals (value)).Key;
		}

		public static IEnumerable<K> GetKeysFormValue<K, V> (Dictionary<K, V> dic, V value) {
			return dic.Where (q => q.Value.Equals (value)).Select (q => q.Key);
		}

		public static string MapToString<K, V> (Dictionary<K, V> dic) {
			string str = "";
			foreach (KeyValuePair<K, V> kvp in dic) {
				str += kvp.Key.ToString ();
				str += ",";
				str += kvp.Value.ToString ();
				str += ";";
			}
			return str;
		}

		public static Dictionary<string, bool> StringToStoryLineMap (string str) {
			Dictionary<string, bool> dic = new Dictionary<string, bool> ();
			string[] kvps = str.Split (';');
			foreach (string s in kvps) {
				if (s.Contains (",")) {
					string[] kvs = s.Split (',');
					dic [kvs [0]] = bool.Parse (kvs [1]);
				}
			}
			return dic;
		}

	}

}