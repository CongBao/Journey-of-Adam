using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace JoA.UI.Initializer {

	public enum ToggleType {
		FULL_SCREEN, LANG_EN, LANG_CN
	}

	[RequireComponent (typeof (Toggle))]
	public class ToggleInitializer : MonoBehaviour {

		[SerializeField] private ToggleType unitType;

		private Toggle toggleUnit;

		void Awake () {
			toggleUnit = GetComponent<Toggle> ();
			toggleUnit.isOn = InitUnit ();
		}

		bool InitUnit () {
			switch (unitType) {
			case ToggleType.FULL_SCREEN:
				return Globe.isFullScreen;
			case ToggleType.LANG_EN:
				if (Globe.language == Lang.en) {
					return true;
				}
				break;
			case ToggleType.LANG_CN:
				if (Globe.language == Lang.cn) {
					return true;
				}
				break;
			}
			return false;
		}

	}

}