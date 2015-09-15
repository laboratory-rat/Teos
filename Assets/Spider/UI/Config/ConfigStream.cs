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
		public string Value = "";
		[SerializeField]
		public SectionPair KeySection;

		protected ConfigController _configC;
		protected EventController _eventC;
		protected string _error = "";

		protected bool OnStart<T>(ref T target) {

			_configC = ConfigController.Instance;

			if (_configC == null || KeySection.Key == "")
				return false;

			Value = _configC.GetValue (KeySection.Key, KeySection.Section);
			if (Value == "")
				return false;

			target = this.gameObject.GetComponentInChildren<T> ();
			if (target == null)
				return false;

			_eventC = _configC.gameObject.GetComponent<EventController> ();
			if (_eventC == null)
				return false;

			_eventC.AddListener<ConfigChanged> (GetValue);

			return true;
		}

		public virtual void GetValue(ConfigChanged cc) {
			if ((cc.FullConfig) || (cc.Key == KeySection.Key && cc.Section == KeySection.Section && cc.Value != Value)) {
				Value = _configC.GetValue(KeySection.Key, KeySection.Section);
			}
		}
	}
}
