using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace JoA.UI.Initializer {

	public enum SliderType {
		ANTI_ALIASING, V_SYNCS,
		TEXT_SPEED,
		MASTER_VOLUME, BGM_VOLUME, SFX_VOLUME
	}

	[RequireComponent (typeof (Slider))]
	public class SliderInitializer : MonoBehaviour {

		[SerializeField] private SliderType unitType;

		private Slider sliderUnit;

		void Awake () {
			sliderUnit = GetComponent<Slider> ();
			sliderUnit.value = InitUnit ();
		}

		float InitUnit () {
			switch (unitType) {
			case SliderType.ANTI_ALIASING:
				if (QualitySettings.antiAliasing == 8) {
					return 3f;
				} else {
					return (float)QualitySettings.antiAliasing / 2f;
				}
			case SliderType.V_SYNCS:
				return (float)QualitySettings.vSyncCount;
			case SliderType.TEXT_SPEED:
				return 0.2f - Globe.textSpeed;
			case SliderType.MASTER_VOLUME:
				return Globe.masterVolume;
			case SliderType.BGM_VOLUME:
				return Globe.bgmVolume;
			case SliderType.SFX_VOLUME:
				return Globe.sfxVolume;
			}
			return -1f;
		}

	}

}