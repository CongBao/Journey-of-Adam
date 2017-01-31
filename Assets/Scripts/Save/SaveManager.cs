using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using JoA.Camera;

namespace JoA.Save {

	[RequireComponent (typeof (ApplicationManager))]
	[RequireComponent (typeof (CaptureScreen))]
	public class SaveManager : MonoBehaviour {

		[SerializeField] private GameObject[] saves;

		private ApplicationManager appManager;
		private CaptureScreen capturer;

		private string savePath;
		private static Dictionary<string, string>[] saveData;

		const int SAVE_NUM = 10;
		Rect rect = new Rect (0f, 0f, 1920f, 1080f);

		void Awake () {
			appManager = GetComponent<ApplicationManager> ();
			capturer = GetComponent<CaptureScreen> ();
			savePath = Application.persistentDataPath + "/Saves";
			ReadXML ();
			ShowAllRecord ();
		}

		public void SaveData (int dataNo) {
			WriteXML (dataNo);

			string imgPath = savePath + "/save" + dataNo + ".jpg";
			Texture2D pic = capturer.CaptureCamera (rect);
			byte[] bytes = pic.EncodeToJPG ();
			File.WriteAllBytes (imgPath, bytes);

			ReadXML ();
			ShowAllRecord ();
		}

		public void LoadData (int dataNo) {
			ShowAllRecord ();
			if (saveData [dataNo - 1] == null) {
				return;
			}
			string sce = saveData [dataNo - 1] ["scene"];
			appManager.LoadScene (sce);

			string posStr = saveData [dataNo - 1] ["position"];
			posStr = posStr.Substring (1, posStr.Length - 2);
			string[] posStrs = posStr.Split (',');
			Vector3 pos = new Vector3 (float.Parse (posStrs [0].Trim ()), float.Parse (posStrs [1].Trim ()), float.Parse (posStrs [2].Trim ()));
			Globe.birthPosition = pos;

			string battery = saveData [dataNo - 1] ["battery"];
			Globe.batteryPower = float.Parse (battery);

			string storyLine = saveData [dataNo - 1] ["storyline"];
			Globe.storyLineMap = Globe.StringToStoryLineMap (storyLine);

			Globe.allowControl = true;
		}

		public void ShowAllRecord () {
			GameObject thisSave;
			Image img;
			Text txt;
			Texture2D pic;
			for (int i = 0; i < SAVE_NUM; i++) {
				if (saveData [i] != null) {
					pic = new Texture2D ((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
					pic.LoadImage (ReadImage (savePath + "/save" + (i + 1) + ".jpg"));
					thisSave = saves [i];
					img = thisSave.GetComponentsInChildren<Image> (true) [1];
					img.sprite = Sprite.Create (pic, rect, new Vector2 (0.5f, 0.5f));
					txt = thisSave.GetComponentInChildren<Text> (true);
					txt.text = saveData [i] ["time"];
				}
			}
		}

		void WriteXML (int dataNo) {
			string xmlPath = savePath + "/save" + dataNo + ".xml";
			if (File.Exists (xmlPath)) {
				File.Delete (xmlPath);
			}
			XmlDocument xmlDoc = new XmlDocument ();
			XmlElement gameNode = xmlDoc.CreateElement ("game");
			gameNode.SetAttribute ("name", Globe.GAME_NAME);

			XmlElement timeNode = xmlDoc.CreateElement ("time");
			XmlElement sceneNode = xmlDoc.CreateElement ("scene");
			XmlElement positionNode = xmlDoc.CreateElement ("position");
			XmlElement batteryNode = xmlDoc.CreateElement ("battery");
			XmlElement storyLineNode = xmlDoc.CreateElement ("storyline");
			timeNode.InnerText = DateTime.Now.ToString ("yy-M-d, H:m:s");
			sceneNode.InnerText = SceneManager.GetActiveScene ().name;
			positionNode.InnerText = GameObject.FindGameObjectWithTag ("Player").transform.localPosition.ToString ();
			batteryNode.InnerText = Globe.batteryPower.ToString ();
			storyLineNode.InnerText = Globe.MapToString (Globe.storyLineMap);

			gameNode.AppendChild (timeNode);
			gameNode.AppendChild (sceneNode);
			gameNode.AppendChild (positionNode);
			gameNode.AppendChild (batteryNode);
			gameNode.AppendChild (storyLineNode);
			xmlDoc.AppendChild (gameNode);

			xmlDoc.Save (xmlPath);
		}

		void ReadXML () {
			saveData = new Dictionary<string, string>[SAVE_NUM];
			for (int i = 0; i < SAVE_NUM; i++) {
				string xmlPath = savePath + "/save" + (i + 1) + ".xml";
				if (!File.Exists (xmlPath)) {
					saveData [i] = null;
					continue;
				}
				XmlDocument xmlDoc = new XmlDocument ();
				xmlDoc.Load (xmlPath);
				XmlNode gameNode = xmlDoc.SelectSingleNode ("game");
				XmlNode timeNode = gameNode.SelectSingleNode ("time");
				XmlNode sceneNode = gameNode.SelectSingleNode ("scene");
				XmlNode positionNode = gameNode.SelectSingleNode ("position");
				XmlNode batteryNode = gameNode.SelectSingleNode ("battery");
				XmlNode storyLineNode = gameNode.SelectSingleNode ("storyline");
				saveData [i] = new Dictionary<string, string> (5);
				saveData [i] ["time"] = timeNode.InnerText;
				saveData [i] ["scene"] = sceneNode.InnerText;
				saveData [i] ["position"] = positionNode.InnerText;
				saveData [i] ["battery"] = batteryNode.InnerText;
				saveData [i] ["storyline"] = storyLineNode.InnerText;
			}
		}

		byte[] ReadImage (string imgPath) {
			FileStream fileStream = new FileStream (imgPath, FileMode.Open, FileAccess.Read);
			fileStream.Seek (0, SeekOrigin.Begin);
			byte[] bytes = new byte[fileStream.Length];
			fileStream.Read (bytes, 0, (int)fileStream.Length);
			fileStream.Close ();
			fileStream.Dispose ();
			fileStream = null;
			return bytes;
		}

	}

}