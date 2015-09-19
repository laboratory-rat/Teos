using UnityEngine;
using System.Collections;
using Spider.Controller;

namespace Spdier.UI.Config {

	[System.Serializable]
	public struct SectionPair {
		public string Key;
		public string Section;
	}

	public class ConfigStream : MonoBehaviour
	{
		[SerializeField]
		public string Section = "";
		[SerializeField]
		public string Key = "";
		public string Value = "";

		protected ConfigController _configC;
		protected EventController _eventC;
		protected string _error = "";

		protected Configuration _config;

		protected bool OnStart<T>(ref T target) {

			_config = ConfigController.Instance.Config;

			if (_config == null || Section == "" || Key == "")
				return false;

			if (!_config.TryGetValue (Section, Key, ref Value)) {
				_error = "No such secttion in configuration file";
				return false;
			}


			target = this.gameObject.GetComponentInChildren<T> ();
			if (target == null) {
				_error = "No component!";
				return false;
			}

			EventController.Instance.AddListener<ConfigChange> (GetValue);

			return true;
		}

		public virtual void GetValue(ConfigChange cc) {
			if (cc.Full || (cc.Section == Section && cc.Key == Key))
				Value = _config.GetValue (Section, Key);
		}
	}
}
