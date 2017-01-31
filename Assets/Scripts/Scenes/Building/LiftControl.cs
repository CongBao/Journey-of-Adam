using UnityEngine;
using UnityEngine.SceneManagement;

namespace JoA.Scenes.Building {

	public class LiftControl : MonoBehaviour {

		[SerializeField] private float speed = 5.0f;
		[SerializeField] private float maxY = 30.0f;
		[SerializeField] private float minY = -30.0f;

		private Transform lift;

		void Awake () {
			lift = gameObject.transform;
		}

		void Update () {
			Vector3 pos = lift.localPosition;
			if (pos.y >= minY && pos.y <= maxY) {
				pos.y += speed * Time.deltaTime;
				lift.localPosition = pos;
			}
			if (pos.y > maxY || pos.y < minY) {
				speed *= -1;
				pos.y += speed * Time.deltaTime;
				lift.localPosition = pos;
			}
		}

	}

}