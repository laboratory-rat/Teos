using UnityEngine;
using System.Collections;
using Spider;
using Spider.Controller;
using UnityEngine.Audio;

namespace Spider.Controller {

	public class SoundController : SpiderController<SoundController>, IController {


		// Public attributes
		[SerializeField]
		public AudioMixer Mixer;
		// Private attributes

		ConfigController _configC;
		EventController _eventC;

		float _back;
		float _effect;
		float _sound;
		float _general;
		bool _enable;
		float _oldGeneral;

		public override void OnInit() {
			_configC = ConfigController.Instance;
			_eventC = EventController.Instance;

			GetValues ();

			_eventC.AddListener<ConfigChanged> (ChangeValue);
		}

		void GetValues() {
			_back = float.Parse(_configC.GetValue ("BACKGROUND", "SOUND"));
			_effect = float.Parse(_configC.GetValue ("EFFECT", "SOUND"));
			_sound = float.Parse(_configC.GetValue ("SOUND", "SOUND"));
			_general = float.Parse(_configC.GetValue ("GENERAL", "SOUND"));
			_enable = (_configC.GetValue ("ENABLESOUND", "SOUND") == "1") ? true : false;
			_oldGeneral = _general;

			Mixer.SetFloat ("BACKGROUND", _back - 80f);
			Mixer.SetFloat ("EFFECT", _effect - 80f);
			Mixer.SetFloat("SOUND", _sound - 80f);

			if (_enable) {
				Mixer.SetFloat ("MASTER", _general - 80f);
				_oldGeneral = 0f;
			}
			else {
				_oldGeneral = _general;
				Mixer.SetFloat ("MASTER", -80f);
			}
		}

		void ChangeValue(ConfigChanged cc) {
			if (cc.FullConfig) {
				GetValues ();
				return;
			}
			if (cc.Section != "SOUND")
				return;
			if (cc.Key == "GENERAL") {
				if ((_general).ToString () != cc.Value) {
					_general = float.Parse (cc.Value);
					if (_enable) 
						Mixer.SetFloat ("MASTER", _general - 80f);
					_oldGeneral = _general;

				}
			} else if (cc.Key == "ENABLESOUND") {
				_enable = (cc.Value == "1") ? true : false;
				if (_enable) {
					_general = _oldGeneral;
					Mixer.SetFloat ("MASTER", _general - 80f);
				} else { 
					_oldGeneral = _general;
					_general = 0f;
					Mixer.SetFloat ("MASTER", _general - 80f);
				}
			} else if (cc.Key == "BACKGROUND") {
				_back = float.Parse (cc.Value);
				Mixer.SetFloat ("BACKGROUND", _back - 80f);
			} else if (cc.Key == "EFFECT") {
				_effect = float.Parse (cc.Value);
				Mixer.SetFloat ("EFFECT", _effect - 80f);
			} else if (cc.Key == "SOUND") {
				_sound = float.Parse (cc.Value);
				Mixer.SetFloat ("SOUND", _sound - 80f);
			}
		}
	}
}