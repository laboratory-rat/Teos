using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Spider.Controller;

namespace Spdier.UI.Config {

	public class ConfigToggle : ConfigStream {

		private Toggle _toggle;

		void Start() {
			if (!base.OnStart<Toggle> (ref _toggle)) {
				_toggle.enabled = false;
				return;
			}

			switch (Value) {
			case "0" :
				_toggle.isOn = false;
				break;
			case "1" :
				_toggle.isOn = true;
				break;
			default:
				return;
			}

			_toggle.onValueChanged.AddListener (ChangeValue);
		}
		
		public void ChangeValue(bool b) {
			Value = (b) ? "1" : "0";
			_configC.SetValue (KeySection.Key, Value, KeySection.Section);
		}
		
		public override void GetValue (ConfigChanged cc)
		{
			base.GetValue (cc);
			if ( (_toggle.isOn && Value == "1") || (!_toggle.isOn && Value == "0") )
				return;
			_toggle.isOn = (Value == "1") ? true : false;
		}

	}
}