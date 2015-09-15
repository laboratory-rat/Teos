using UnityEngine;
using System.Collections;
using Spider.Parser;
using System.IO;
using Spider;

namespace Spider.Controller {
	
	public class ConfigChanged : GameEvent {
		public bool FullConfig = false;
		public string Key = "";
		public string Section = "";
		public string Value = "";

		public ConfigChanged(string key, string section, string value) {
			Key = key;
			Section = section;
			Value = value;
		}

		public ConfigChanged() {
			FullConfig = true;
		}
	}

	[System.Serializable]
	public struct ConfigUnit {
		public string Key;
		public string Value;
		public string Section;
	}

	public class ConfigController : SpiderController<ConfigController>, IController {

		[SerializeField]
		public bool ResetConfigOnStart = false;
		[SerializeField]
		public string ConfigName;
		[SerializeField]
		public ConfigUnit[] StandartConfig;

		private IniParser _config;
		private EventController _event;
		string _path;

		public override void OnInit() {
			_path = Path.Combine(Application.dataPath, ConfigName);

			if (!File.Exists (_path)) {
				ResetConfigOnStart = true;
				File.Create(_path);
			}
			_config = new IniParser (_path);
			if (ResetConfigOnStart) {
				ResetConfig ();
			} else {
				foreach(ConfigUnit cu in StandartConfig) {
					cu.Key.ToUpper();
					cu.Value.ToUpper();
					if (cu.Section != "") 
						cu.Section.ToUpper();
				}
				_config.SaveSettings();
				foreach (ConfigUnit ku in StandartConfig) {
					string value = (ku.Section == "") ? _config.GetSetting(ku.Key) : _config.GetSetting(ku.Key, ku.Section);
					if (value == "") {
						ResetConfig();
						break;
					}
				}
			}

			_event = EventController.Instance;
		}

		private void ResetConfig() {
			_config.ClearIni ();
			foreach (ConfigUnit ku in StandartConfig) {
				if (ku.Section == "")
					_config.AddSetting(ku.Key.ToUpper(), ku.Value.ToUpper());
				else
					_config.AddSetting(ku.Key.ToUpper(), ku.Value.ToUpper(), ku.Section.ToUpper());
			}
			_config.SaveSettings ();
		}

		// Public config functions

		public string GetValue(string key, string section = "") {
			string value = (section == "") ? _config.GetSetting (key) : _config.GetSetting (key, section);
			return value.ToUpper ();
		}

		public bool SetValue(string key, string value, string section = "") {
			key = key.ToUpper ();
			section = section.ToUpper ();
			value = value.ToUpper ();
			string test = (section == "") ? _config.GetSetting (key) : _config.GetSetting (key, section);
			if (test == "") {
				Debug.LogError("No section in config whith key = " + key + " and section = " + section);
				return false;
			}
			if (value == test)
				return true;

			if (section == "")
				_config.AddSetting (key, value);
			else 
				_config.AddSetting (key, value, section);
			_event.TriggerEvent (new ConfigChanged (key, section, value));
			return true;
		}

		public bool SectionExists (string key, string section = "") {
			key.ToUpper ();
			section.ToUpper ();
			string value = (section == "") ? _config.GetSetting (key) : _config.GetSetting (key, section);
			if (value == "")
				return false;
			return true;
		}

		/// <summary>
		/// Creates or rewrite new section.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		/// <param name="section">Section.</param>
		public void CreateNewSection (string key, string value, string section = "") {
			key.ToUpper ();
			value.ToUpper ();
			section.ToUpper ();
			if (section == "") 
				_config.AddSetting (key, value);
			else 
				_config.AddSetting (key, value, section);

			_event.TriggerEvent (new ConfigChanged (key, section, value));
		}

		public IniParser GetConfig() {
			return new IniParser(_config);
		}

		public void SetConfig(IniParser value) {
			_config = new IniParser (value);
			_config.SaveSettings ();
			_event.TriggerEvent (new ConfigChanged ());
		}

		public void SaveConfig() {
			_config.SaveSettings ();
		}

		// On Quit App

		public override void OnAppStop() {
			if (_config != null)
				_config.SaveSettings ();
		}
	}
}