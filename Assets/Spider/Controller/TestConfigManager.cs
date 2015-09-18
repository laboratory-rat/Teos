using UnityEngine;
using System.Collections;
using Spider;
using System.Collections.Generic;
using Spider.Parser;
using System.Xml;
using System.Xml.Serialization;

namespace Spider.Controller {
	
	public class ConfigChange : GameEvent {

		public string Section;
		public string Key;
		public string Value;

		public ConfigChange(string s, string k, string v) {
			Section = s;
			Key = k;
			Value = v;
		}

	}

	[System.Serializable]
	public class ConfigurationSection
	{
		[XmlAttribute()]
		public string Name { get; private set; }
		public Dictionary<string, string> Elements { get; private set; }

		public ConfigurationSection() { }

		public ConfigurationSection (string name) {
			Name = name.ToUpper();
			Elements = new Dictionary<string, string>();
		}
		
		public void AddElement(string key, string value) {
			key = key.ToUpper();
			value = value.ToUpper();
			if (Elements.ContainsKey (key))
				Elements [key] = value;
			else 
				Elements.Add (key, value);
		}
		
		public string GetElement(string key) {
			key = key.ToUpper ();
			if (!Elements.ContainsKey (key))
				return "";
			return Elements [key];
		}
		
		public bool TryGetElement(string key, ref string value) {
			key = key.ToUpper ();
			if (!Elements.ContainsKey (key)) {
				value = "";
				return false;
			}
			value = Elements [key]; 
			return true;
		}
		
		public Dictionary<string, string> GetAllElements() {
			return Elements;
		}
	}

	[System.Serializable]
	public class Configuration {
		public List<ConfigurationSection> Sections { get; private set;}
		EventController _event;

		public Configuration() {
			Sections = new List<ConfigurationSection> ();
			_event = EventController.Instance;
		}

		public void AddEmptySection(string name) {
			name = name.ToUpper ();
			foreach (ConfigurationSection cs in Sections) {
				if (cs.Name == name)
					return;
			}
			Sections.Add (new ConfigurationSection (name));
		}

		public void AddSection(ConfigurationSection cs) {
			if (Sections.Count == 0) {
				Sections.Add(cs);
				return;
			}
			foreach (ConfigurationSection sect in Sections) {
				if (sect.Name == cs.Name)
					return;
			}
			Sections.Add (cs);
		}

		public bool TryGetSection(string name, ref ConfigurationSection section) {
			name = name.ToUpper ();
			foreach (ConfigurationSection cs in Sections) {
				if (cs.Name == name) {
					section = cs;
					return true;
				}
			}
			return false;
		}

		public void ChangeValue(string section, string key, string value) {
			section = section.ToUpper ();
			foreach (ConfigurationSection cs in Sections) {
				if(cs.Name == section) {
					cs.AddElement(key, value);
					ConfigChange ch = new ConfigChange(section, key, value);
					_event.TriggerEvent(ch);
					return;
				}
			}
		}

		public bool TryGetValue(string section, string key, ref string value) {
			section = section.ToUpper ();
			foreach (ConfigurationSection cs in Sections) {
				if (cs.Name == section) {
					string val = "";
					if (cs.TryGetElement(key, ref val)) {
						value = val;
						return true;
					}
					return false;
				}
			}
			return false;
		}
	}

	public class TestConfigManager : SpiderController<TestConfigManager>, IController {
		[SerializeField]
		public bool ResetOnInit = false;
		public Configuration Config { get; private set; }
		private string _configName = "Config";

		public override void OnInit ()
		{
			Config = new Configuration ();
			if (!ResetOnInit) {
				if (!LoadConfig())
					CreateNewConfig();
				return;
			}
			else
				CreateNewConfig ();

		}


		public bool LoadConfig() {
			XmlParser xml = new XmlParser (Application.dataPath);
			Configuration tmpConfig = new Configuration ();
			if (xml.TryLoad<Configuration> (_configName, ref tmpConfig)) { 
				Config = tmpConfig;
				print (Application.dataPath.ToString());
				return true;
			}
			Debug.LogWarning ("Can`t load Config file");
			return false;
		}

		public void SaveConfig() {
			XmlParser xml = new XmlParser ("");
			xml.Save<Configuration> (_configName, Config);
		}

		public void CreateNewConfig() {
			Config = new Configuration ();

			ConfigurationSection general = new ConfigurationSection("General");
			general.AddElement ("language", "Rus");
			Config.AddSection (general);

			ConfigurationSection sound = new ConfigurationSection("Sound");
			sound.AddElement("general", "100");
			sound.AddElement("music", "100");
			sound.AddElement("effect", "100");
			sound.AddElement("sound", "100");
			Config.AddSection(sound);

			ConfigurationSection graphics = new ConfigurationSection("Graphics");
			graphics.AddElement("quality", "height");
			graphics.AddElement("resolution", "1600x900");
			graphics.AddElement("contrast", "100");
			graphics.AddElement("lightness", "100");
			Config.AddSection (graphics);

			SaveConfig ();
		}

		public override void OnAppStop ()
		{
			if (Config != null)
				SaveConfig ();
			print ("I am ok!");
		}
	}
}