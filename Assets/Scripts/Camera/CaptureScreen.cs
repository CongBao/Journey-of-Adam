using UnityEngine;
using System.Collections;
using System.IO;

namespace JoA.Camera {

	public class CaptureScreen : MonoBehaviour {

		[SerializeField] private UnityEngine.Camera cam;

		public Texture2D CaptureCamera (Rect rect) {
			RenderTexture rt = new RenderTexture ((int)rect.width, (int)rect.height, 0);
			cam.targetTexture = rt;
			cam.Render ();
			RenderTexture.active = rt;
			Texture2D screenShot = new Texture2D ((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
			screenShot.ReadPixels (rect, 0, 0);
			screenShot.Apply ();

			cam.targetTexture = null;
			RenderTexture.active = null;
			GameObject.Destroy (rt);

			return screenShot;
		}

	}

}