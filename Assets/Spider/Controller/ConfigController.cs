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
		public bool Full = false;

		public ConfigChange(string s, string k, string v) {
			Section = s;
			Key = k;
			Value = v;
		}

		public ConfigChange() {
			Full = true;
		}

	}

	public class ConfigurationUnit {
		public ConfigurationUnit() { }

		[XmlAttribute("Key")]
		public string Key;
		[XmlAttribute("Value")]
		public string Value;

		public ConfigurationUnit(string k, string v){
			Key = k.ToUpper();
			Value = v.ToUpper();
		}
	}

	public class ConfigurationSection {
		public ConfigurationSection() {}
		[XmlAttribute("Name")]
		public string Name;
		[XmlArray("Units"), XmlArrayItem(typeof(ConfigurationUnit), ElementName = "Unit")]
		public List<ConfigurationUnit> Units;

		public ConfigurationSection(string name) {
			Name = name.ToUpper ();
			Units = new List<ConfigurationUnit> ();
		}
	}

	public class Configuration {
		[XmlArray("Sections"), XmlArrayItem(typeof(ConfigurationSection), ElementName = "Section")]
		public List<ConfigurationSection> Sections { get; private set;}

		public Configuration() {
			Sections = new List<ConfigurationSection> ();
		}

		public Configuration(Configuration conf) {
			Sections = new List<ConfigurationSection> (conf.Sections);
		}

		public bool TryGetValue(string section, string key, ref string value) {
			section = section.ToUpper ();
			key = key.ToUpper ();

			foreach (ConfigurationSection cs in Sections) {
				if (cs.Name == section) {
					foreach(ConfigurationUnit cu in cs.Units) {
						if (cu.Key == key) {
							value = cu.Value;
							return true;
						}
					}
				}
			}
			return false;
		}

		public string GetValue(string section, string key) {
			section = section.ToUpper ();
			key = key.ToUpper ();
			
			foreach (ConfigurationSection cs in Sections) {
				if (cs.Name == section) {
					foreach(ConfigurationUnit cu in cs.Units) {
						if (cu.Key == key) {
							return cu.Value;
						}
					}
				}
			}
			return "";
		}

		public bool TryGetSection (string section, ref ConfigurationSection sect) {
			section = section.ToUpper ();
			foreach (ConfigurationSection cs in Sections) {
				if (cs.Name == section){
					sect = cs;
					return true;
				}
			}
			return false;
		}

		public void AddValue(string section, string key, string value) {
			section = section.ToUpper ();
			key = key.ToUpper ();
			value = value.ToUpper ();

			foreach (ConfigurationSection cs in Sections) {
				if (cs.Name == section) {
					foreach(ConfigurationUnit cu in cs.Units) {
						if (cu.Key == key) {
							cu.Value = value;
							EventController.Instance.TriggerEvent(new ConfigChange(section, key, value));
							return;
						}
					}
					cs.Units.Add(new ConfigurationUnit(key, value));
					return;
				}
			}

			ConfigurationSection sect = new ConfigurationSection(section);
			sect.Units.Add (new ConfigurationUnit (key, value));
			Sections.Add (sect);
		}

		public void AddSection(ConfigurationSection section) {
			foreach (ConfigurationSection cs in Sections) {
				if (cs.Name == section.Name)
					return;
			}
			Sections.Add (section);
		}

	}

	public class ConfigController : SpiderController<ConfigController>, IController {
		[SerializeField]
		public bool ResetOnInit = false;
		public Configuration Config { get; private set;}

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


		public void ReplaceConfig(Configuration conf) {
			Config = conf;
			EventController.Instance.TriggerEvent (new ConfigChange ());
		}

		public bool LoadConfig() {
			XmlParser xml = new XmlParser ("");
			Configuration tmpConfig = new Configuration ();
			if (xml.TryLoad<Configuration> (_configName, ref tmpConfig)) { 
				Config = tmpConfig;
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

			ConfigurationSection general = new ConfigurationSection ("general");
			general.Units.Add (new ConfigurationUnit ("Lang", "rus"));
			Config.AddSection (general);

			ConfigurationSection sound = new ConfigurationSection ("Sound");
			sound.Units.Add (new ConfigurationUnit ("general", "100"));
			sound.Units.Add (new ConfigurationUnit ("music", "100"));
			sound.Units.Add (new ConfigurationUnit ("effect", "100"));
			sound.Units.Add (new ConfigurationUnit ("sound", "100"));
			sound.Units.Add (new ConfigurationUnit ("enable", "1"));
			Config.AddSection (sound);

			ConfigurationSection graph = new ConfigurationSection ("graphics");
			graph.Units.Add (new ConfigurationUnit ("quality", "height"));
			graph.Units.Add (new ConfigurationUnit ("resolution", "1600x900"));
			graph.Units.Add (new ConfigurationUnit ("contrast", "100"));
			graph.Units.Add (new ConfigurationUnit ("lightness", "100"));
			Config.AddSection (graph);

			SaveConfig ();
		}

		public override void OnAppStop ()
		{
			if (Config != null)
				SaveConfig ();
		}
	}
}