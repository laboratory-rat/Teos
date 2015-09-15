using UnityEngine;
using System.Collections;
using Spider.Controller;


namespace Spider.Sound {

	public class BackgroundMusic : MonoBehaviour {

		public AudioClip[] Lib;
		public bool RandomFirst = true;

		AudioSource _sourse;
		int _index;
		float _time = 0f;

		void Awake() {
			_sourse = this.gameObject.GetComponent<AudioSource> ();
		}

		void Start() {
			if (Lib.Length == 0) {
				Debug.LogError("No audio clips on " + this.ToString());
				Destroy(this.gameObject);
			}
			if (_sourse == null) {
				_sourse = this.gameObject.AddComponent<AudioSource>();
				_sourse.loop = false;
				_sourse.playOnAwake = false;
				_sourse.outputAudioMixerGroup = SoundController.Instance.Mixer.outputAudioMixerGroup;
			}

			if (RandomFirst && Lib.Length > 1) 
				_index = Random.Range(0, Lib.Length);
			else 
				_index = 0;

			_time = Lib [_index].length;
			_sourse.PlayOneShot (Lib [_index], 1f);
		}

		void Update () {
			_time -= Time.deltaTime;
			if (_time <= 0f) {
				_index = ((_index + 1) == Lib.Length) ? 0 : _index + 1;
				_time = Lib[_index].length;
				_sourse.PlayOneShot(Lib[_index]);
			}
		}


	}
}