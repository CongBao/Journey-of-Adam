using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;

namespace JoA.Save {

	public class ConfigManager : MonoBehaviour {

		private string configPath;

		void Awake () {
			configPath = Application.persistentDataPath + "/Config/preference.xml";
		}

		void OnApplicationQuit () {
			WriteXML ();
		}

		public void WriteConfig () {
			WriteXML ();
		}

		public void ReadConfig () {
			if (!File.Exists (configPath)) {
				WriteXML ();
			}
			ReadXML ();
		}

		void WriteXML () {
			if (File.Exists (configPath)) {
				File.Delete (configPath);
			}
			XmlDocument xmlDoc = new XmlDocument ();
			XmlElement gameNode = xmlDoc.CreateElement ("game");
			gameNode.SetAttribute ("name", Globe.GAME_NAME);

			XmlElement languageNode = xmlDoc.CreateElement ("language");
			XmlElement textspeedNode = xmlDoc.CreateElement ("textspeed");
			XmlElement resolutionNode = xmlDoc.CreateElement ("resolution");
			XmlElement shadowresNode = xmlDoc.CreateElement ("shadowres");
			XmlElement antialiasingNode = xmlDoc.CreateElement ("antialiasing");
			XmlElement vsyncsNode = xmlDoc.CreateElement ("vsyncs");
			XmlElement mastervolumeNode = xmlDoc.CreateElement ("mastervolume");
			XmlElement bgmvolumeNode = xmlDoc.CreateElement ("bgmvolume");
			XmlElement sfxvolumeNode = xmlDoc.CreateElement ("sfxvolume");

			languageNode.InnerText = Globe.language.ToString ();
			textspeedNode.InnerText = Globe.textSpeed.ToString ();
			resolutionNode.InnerText = Globe.currentRes.ToString ();
			shadowresNode.InnerText = QualitySettings.shadowResolution.ToString ();
			antialiasingNode.InnerText = QualitySettings.antiAliasing.ToString ();
			vsyncsNode.InnerText = QualitySettings.vSyncCount.ToString ();
			mastervolumeNode.InnerText = Globe.masterVolume.ToString ();
			bgmvolumeNode.InnerText = Globe.bgmVolume.ToString ();
			sfxvolumeNode.InnerText = Globe.sfxVolume.ToString ();

			gameNode.AppendChild (languageNode);
			gameNode.AppendChild (textspeedNode);
			gameNode.AppendChild (resolutionNode);
			gameNode.AppendChild (shadowresNode);
			gameNode.AppendChild (antialiasingNode);
			gameNode.AppendChild (vsyncsNode);
			gameNode.AppendChild (mastervolumeNode);
			gameNode.AppendChild (bgmvolumeNode);
			gameNode.AppendChild (sfxvolumeNode);
			xmlDoc.AppendChild (gameNode);

			xmlDoc.Save (configPath);
		}

		void ReadXML () {
			if (!File.Exists (configPath)) {
				return;
			}
			XmlDocument xmlDoc = new XmlDocument ();
			xmlDoc.Load (configPath);
			XmlNode gameNode = xmlDoc.SelectSingleNode ("game");

			XmlNode languageNode = gameNode.SelectSingleNode ("language");
			XmlNode textspeedNode = gameNode.SelectSingleNode ("textspeed");
			XmlNode resolutionNode = gameNode.SelectSingleNode ("resolution");
			XmlNode shadowresNode = gameNode.SelectSingleNode ("shadowres");
			XmlNode antialiasingNode = gameNode.SelectSingleNode ("antialiasing");
			XmlNode vsyncsNode = gameNode.SelectSingleNode ("vsyncs");
			XmlNode mastervolumeNode = gameNode.SelectSingleNode ("mastervolume");
			XmlNode bgmvolumeNode = gameNode.SelectSingleNode ("bgmvolume");
			XmlNode sfxvolumeNode = gameNode.SelectSingleNode ("sfxvolume");

			string language = languageNode.InnerText;
			string textspeed = textspeedNode.InnerText;
			string resolution = resolutionNode.InnerText;
			string shadowres = shadowresNode.InnerText;
			string antialiasing = antialiasingNode.InnerText;
			string vsyncs = vsyncsNode.InnerText;
			string mastervolume = mastervolumeNode.InnerText;
			string bgmvolume = bgmvolumeNode.InnerText;
			string sfxvolume = sfxvolumeNode.InnerText;

			if (language == Lang.en.ToString ()) {
				Globe.language = Lang.en;
			} else if (language == Lang.cn.ToString ()) {
				Globe.language = Lang.cn;
			}

			Globe.textSpeed = float.Parse (textspeed);

			string[] res = resolution.Split ('x', '@', 'H');
			Globe.currentRes = new Resolution { width = int.Parse(res [0].Trim ()), height = int.Parse(res [1].Trim ()), refreshRate = int.Parse(res [2].Trim ()) };

			if (shadowres == ShadowResolution.Low.ToString ()) {
				QualitySettings.shadowResolution = ShadowResolution.Low;
			} else if (shadowres == ShadowResolution.Medium.ToString ()) {
				QualitySettings.shadowResolution = ShadowResolution.Medium;
			} else if (shadowres == ShadowResolution.High.ToString ()) {
				QualitySettings.shadowResolution = ShadowResolution.High;
			} else if (shadowres == ShadowResolution.VeryHigh.ToString ()) {
				QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
			}

			QualitySettings.antiAliasing = int.Parse (antialiasing);
			QualitySettings.vSyncCount = int.Parse (vsyncs);
			Globe.masterVolume = float.Parse (mastervolume);
			Globe.bgmVolume = float.Parse (bgmvolume);
			Globe.sfxVolume = float.Parse (sfxvolume);
		}

	}

}