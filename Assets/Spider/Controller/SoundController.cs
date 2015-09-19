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

		EventController _eventC;
		Configuration _config;

		float _music = 100f;
		float _effect = 100f;
		float _sound = 100f;
		float _general = 100f;
		bool _enable = true;
		float _oldGeneral = 100f;

		public override void OnInit() {
			_eventC = EventController.Instance;
			_config = ConfigController.Instance.Config;

			GetValues ();
		}

		void GetValues() {

			ConfigurationSection section = new ConfigurationSection ();
			if (_config.TryGetSection ("sound", ref section)) {
				foreach(ConfigurationUnit cu in section.Units) {
					switch(cu.Key) {
					case "GENERAL":
						_general = float.Parse (cu.Value);
						break;
					case "MUSIC":
						_music = float.Parse (cu.Value);
						break;
					case "EFFECT":
						_effect = float.Parse(cu.Value);
						break;
					case "SOUND":
						_sound = float.Parse(cu.Value);
						break;
					case "ENABLE":
						int tmp = int.Parse(cu.Value);
						_enable = (tmp == 1) ? true : false;
						break;
					default:
						break;
					}
				}
			} else {
				Debug.LogWarning("No sound configuration! Will use default values.");
			}

			Mixer.SetFloat ("GENERAL", _general - 80f);
			Mixer.SetFloat ("MUSIC", _music - 80f);
			Mixer.SetFloat ("EFFECT", _effect - 80f);
			Mixer.SetFloat("SOUND", _sound - 80f);

			if (_enable) {
				Mixer.SetFloat ("GENERAL", _general - 80f);
				_oldGeneral = 0f;
			}
			else {
				_oldGeneral = _general;
				Mixer.SetFloat ("GENERAL", -80f);
			}
		}

		void ChangeValue(ConfigChange cc) {
			if (cc.Full) {
				GetValues();
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
				_music = float.Parse (cc.Value);
				Mixer.SetFloat ("BACKGROUND", _music - 80f);
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