using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace JoA.UI {

	public class DialogManager : MonoBehaviour {

		[SerializeField] private string xmlPath;

		private Scene scene;
		private Object obj;

		private Dictionary<int, Dictionary<Lang, string>> dialogContent;

		void Awake () {
			scene = SceneManager.GetActiveScene ();
			obj = gameObject;
			dialogContent = GetDialogContent ();
		}

		Dictionary<int, Dictionary<Lang, string>> GetDialogContent () {
			#if UNITY_EDITOR
			if (!File.Exists (Application.dataPath + "/Resources/" + xmlPath + ".xml")) {
				Debug.Log ("File not exist.");
				return null;
			}
			#endif
			Dictionary<int, Dictionary<Lang, string>> content = new Dictionary<int, Dictionary<Lang, string>> ();
			XmlDocument xmlDoc = new XmlDocument ();
			xmlDoc.LoadXml (Resources.Load (xmlPath).ToString ());
			XmlNode gameNode = xmlDoc.SelectSingleNode ("game");
			foreach (XmlElement sceneNode in gameNode.ChildNodes) {
				if (sceneNode.GetAttribute ("name") == scene.name) {
					foreach (XmlElement objectNode in sceneNode.ChildNodes) {
						if (objectNode.GetAttribute ("name") == obj.name) {
							foreach (XmlElement contentNode in objectNode.ChildNodes) {
								string en = contentNode.SelectSingleNode (Lang.en.ToString ()).InnerText;
								string cn = contentNode.SelectSingleNode (Lang.cn.ToString ()).InnerText;
								Dictionary<Lang, string> langContent = new Dictionary<Lang, string> (2);
								langContent [Lang.en] = en;
								langContent [Lang.cn] = cn;
								content [System.Convert.ToInt32 (contentNode.GetAttribute ("id"))] = langContent;
							}
						}
					}
				}
			}
			return content;
		}

		public string GetDialogContent (int contentId, Lang lang) {
			return dialogContent [contentId][lang];
		}

	}

}