using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace JoA.Editor {

	public class DialogEditor : EditorWindow {

		[MenuItem ("Custom/Dialog Editor")]
		static void OpenEditor () {
			Rect rect = new Rect (0, 0, 500, 500);
			DialogEditor de = (DialogEditor) EditorWindow.GetWindowWithRect (typeof (DialogEditor), rect, true, "Dialog Editor");
			de.Show ();
		}

		private string xmlPath;

		private string sceneName;
		private Object dialogObj;
		private int contentId;
		private AnimBool enSupport;
		private AnimBool cnSupport;
		private string enText;
		private string cnText;

		private string helpMsg;
		private MessageType msgType;

		void OnEnable () {
			xmlPath = Application.dataPath + "/Resources/";
			sceneName = "";
			dialogObj = null;
			contentId = 0;
			enText = "";
			cnText = "";

			enSupport = new AnimBool (true);
			cnSupport = new AnimBool (true);
			enSupport.valueChanged.AddListener (Repaint);
			cnSupport.valueChanged.AddListener (Repaint);

			helpMsg = "Please enter parameters.";
			msgType = MessageType.Info;
		}

		void OnGUI () {
			xmlPath = EditorGUILayout.TextField ("File Path", xmlPath);
			EditorGUILayout.Space ();
			EditorGUILayout.LabelField ("Project Name", Globe.GAME_NAME);
			sceneName = EditorGUILayout.TextField ("Scene Name", sceneName);
			dialogObj = EditorGUILayout.ObjectField ("Dialog Object", dialogObj, typeof (Object), true);
			contentId = EditorGUILayout.IntField ("Content ID", contentId);
			enSupport.target = EditorGUILayout.ToggleLeft ("English", enSupport.target);
			if (EditorGUILayout.BeginFadeGroup (enSupport.faded)) {
				EditorGUI.indentLevel++;
				enText = EditorGUILayout.TextArea (enText, GUILayout.Height (100f));
				EditorGUI.indentLevel--;
			}
			cnSupport.target = EditorGUILayout.ToggleLeft ("Chinese", cnSupport.target);
			if (EditorGUILayout.BeginFadeGroup (cnSupport.faded)) {
				EditorGUI.indentLevel++;
				cnText = EditorGUILayout.TextArea (cnText, GUILayout.Height (100f));
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.Space ();
			if (GUILayout.Button ("Submit")) {
				Submit ();
			}
			EditorGUILayout.Space ();
			EditorGUILayout.HelpBox (helpMsg, msgType);
		}

		private void Submit () {
			if (File.Exists (xmlPath)) {
				XmlDocument xmlDoc = new XmlDocument ();
				xmlDoc.Load (xmlPath);
				XmlElement gameNode = xmlDoc.SelectSingleNode ("game") as XmlElement;
				XmlElement sceneNode = null;
				XmlElement objectNode = null;
				XmlElement contentNode = null;
				XmlElement enNode = null;
				XmlElement cnNode = null;
				int nodeCase = 1;
				foreach (XmlElement scene in gameNode.ChildNodes) {
					if (scene.GetAttribute ("name") == sceneName) {
						sceneNode = scene;
						nodeCase++;
						foreach (XmlElement obj in scene.ChildNodes) {
							if (obj.GetAttribute ("name") == dialogObj.name) {
								objectNode = obj;
								nodeCase++;
								foreach (XmlElement content in obj.ChildNodes) {
									if (content.GetAttribute ("id") == contentId.ToString ()) {
										contentNode = content;
										enNode = content.SelectSingleNode ("en") as XmlElement;
										cnNode = content.SelectSingleNode ("cn") as XmlElement;
										nodeCase++;
									}
								}
							}
						}
					}
				}
				helpMsg = "";
				switch (nodeCase) {
				case 1:
					sceneNode = xmlDoc.CreateElement ("scene");
					sceneNode.SetAttribute ("name", sceneName);
					helpMsg += "Scene #" + sceneName + "# added under Game #" + gameNode.GetAttribute ("name") + "#.\n";
					goto case 2;
				case 2:
					objectNode = xmlDoc.CreateElement ("object");
					objectNode.SetAttribute ("name", dialogObj.name);
					helpMsg += "Object #" + dialogObj.name + "# added under Scene #" + sceneNode.GetAttribute ("name") + "#.\n";
					goto case 3;
				case 3:
					contentNode = xmlDoc.CreateElement ("content");
					contentNode.SetAttribute ("id", contentId.ToString ());
					helpMsg += "Content #" + contentId + "# added under Object #" + objectNode.GetAttribute ("name") + "#.\n";
					enNode = xmlDoc.CreateElement ("en");
					cnNode = xmlDoc.CreateElement ("cn");
					break;
				default:
					break;
				}
				if (enSupport.target) {
					enNode.InnerText = enText;
				}
				if (cnSupport.target) {
					cnNode.InnerText = cnText;
				}
				contentNode.AppendChild (enNode);
				contentNode.AppendChild (cnNode);
				objectNode.AppendChild (contentNode);
				sceneNode.AppendChild (objectNode);
				gameNode.AppendChild (sceneNode);
				xmlDoc.AppendChild (gameNode);
				xmlDoc.Save (xmlPath);

				contentId++;
				enText = "";
				cnText = "";
				helpMsg += "Update finished.";
				msgType = MessageType.Info;
			} else {
				helpMsg = "File not exist.";
				msgType = MessageType.Error;
			}
			Repaint ();
		}

	}

}