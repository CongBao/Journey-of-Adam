using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace JoA.UI.Initializer {

	public enum DropdownType {
		RESOLUTION, QUALITY_LEVEL, SHADOW_RES
	}

	[RequireComponent (typeof (Dropdown))]
	public class DropdownInitializer : MonoBehaviour {

		[SerializeField] private DropdownType unitType;

		private Dropdown dropdownUnit;

		void Awake () {
			dropdownUnit = GetComponent<Dropdown> ();
			dropdownUnit.value = InitUnit ();
		}

		int InitUnit () {
			switch (unitType) {
			case DropdownType.RESOLUTION:
				return Globe.GetKeyFormValue (Globe.supportResMap, Globe.currentRes);
			case DropdownType.QUALITY_LEVEL:
				return QualitySettings.GetQualityLevel ();
			case DropdownType.SHADOW_RES:
				switch (QualitySettings.shadowResolution) {
				case ShadowResolution.Low:
					return 0;
				case ShadowResolution.Medium:
					return 1;
				case ShadowResolution.High:
					return 2;
				case ShadowResolution.VeryHigh:
					return 3;
				}
				return -1;
			}
			return -1;
		}

	}

}