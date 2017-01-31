using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using JoA.CrossPlatformInput;

namespace JoA.Scenes {

	public class CinematicsController : MonoBehaviour {

		[SerializeField] protected Image targetImage;
		[SerializeField] private Sprite[] cgImages;
		[SerializeField] private float speed = 0.25f;
		[SerializeField] private float waitTime = 3f;
		[SerializeField] private bool allowSkip = false;

		private int totalStep;
		private float timeStamp;
		private bool isFinished;

		private int step = 1;

		public event EventHandler CinematicsEnd;

		protected virtual void Awake () {
			targetImage.sprite = cgImages [0];
			totalStep = cgImages.Length * 3;
			timeStamp = Time.time;
			isFinished = false;
		}

		protected virtual void Update () {
			if (isFinished) {
				return;
			}
			if (allowSkip && CrossPlatformInputManager.GetButtonDown ("Cancel")) {
				step = ((step - 1) / 3) * 3 + 4;
				targetImage.color = new Color (1f, 1f, 1f, 0f);
				targetImage.sprite = cgImages [step < totalStep ? step / 3 : 0];
			}
			if (step <= totalStep) {
				if (step % 3 == 1) {
					ToClear (targetImage);
					if (targetImage.color.a > 0.98f) {
						timeStamp = Time.time;
						step++;
					}
				} else if (step % 3 == 2) {
					if (Time.time - timeStamp > waitTime) {
						step++;
					}
				} else if (step % 3 == 0) {
					ToDark (targetImage);
					if (targetImage.color.a < 0.02f) {
						targetImage.sprite = cgImages [step < totalStep ? step / 3 : 0];
						step++;
					}
				}
			} else {
				CinematicsEnd (this, EventArgs.Empty);
				isFinished = true;
			}
		}

		protected void ToClear (Image img) {
			Color col = img.color;
			col.a += speed * Time.deltaTime;
			img.color = col;
		}

		protected void ToDark (Image img) {
			Color col = img.color;
			col.a -= speed * Time.deltaTime;
			img.color = col;
		}

	}

}