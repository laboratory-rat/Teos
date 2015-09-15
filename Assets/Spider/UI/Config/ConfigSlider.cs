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
				Debug.LogError("Bad format data at key: " + KeySection.Key + ( (KeySection.Section == "") ? "" : " / " + KeySection.Section) );
				return;
			}
			
			_s.value = tmp;
			_s.onValueChanged.AddListener (ChangeValue);
		}

		public void ChangeValue(float value) {
			_configC.SetValue (KeySection.Key, value.ToString (), KeySection.Section);
			Value = value.ToString();
		}

		public override void GetValue (ConfigChanged cc)
		{
			base.GetValue (cc);
			if (float.Parse (Value) == _s.value)
				return;
			_s.value = float.Parse (Value);
		}
	}
}