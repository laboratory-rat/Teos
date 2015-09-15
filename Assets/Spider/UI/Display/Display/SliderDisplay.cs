using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Spider.UI.Display {

	public class SliderDisplay : MonoBehaviour {

		[SerializeField]
		public Slider Target;
		public string Value;

		private Text _display;

		void Start() {
			_display = this.gameObject.GetComponent<Text> ();
			if (Target == null || _display == null)
				return;
			Target.onValueChanged.AddListener (DisplayValue);
			DisplayValue (Target.value);
		}

		public void DisplayValue(float value) {
			if (Value == value.ToString ())
				return;
			Value = value.ToString ();
			_display.text = Value;
		}
	}
}