using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Spider.Controller;

namespace Spdier.UI.Config {
	public class ConfigSlider : ConfigStream {

		Slider _s;

	    void Start() {
			if (!base.OnStart<Slider> (ref _s)) {
				_s.enabled = false;
				return;
			}

			float tmp;
			if (!float.TryParse (Value, out tmp)) {
				Debug.LogError(Value);
				Debug.LogError("Bad format data at key: " + Key + "; section: " + Section );
				return;
			}
			
			_s.value = tmp;
			_s.onValueChanged.AddListener (ChangeValue);
		}

		public void ChangeValue(float value) {
			_config.AddValue(Section, Key, value.ToString());
			Value = value.ToString();
		}

		public override void GetValue (ConfigChange cc)
		{
			base.GetValue (cc);
			if (float.Parse (Value) == _s.value)
				return;
			_s.value = float.Parse (Value);
		}
	}
}