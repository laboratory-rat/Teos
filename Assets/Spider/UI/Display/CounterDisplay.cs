using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Spider.UI.Display
{
	public class CounterDisplay : MonoBehaviour {

		public Slider target = null;
		Text self = null;

		void Start() {

			self = gameObject.GetComponent<Text> ();

			if (target == null)
				return;

			SetTarget ();
		}

		public void SetTarget(Slider s = null) {
			if ( (s == null && target == null) || (s == target) || (self == null) )
				return;

			if (target != null) 
				target.onValueChanged.RemoveListener(DisplayChanges);

			if (s != null)
				target = s;

			target.onValueChanged.AddListener (DisplayChanges);

		}

		public void RemoveTarget() {
			if (target == null)
				return;
			target.onValueChanged.RemoveListener (DisplayChanges);
			target = null;
		}

		public void DisplayChanges(float value) {
			self.text = value.ToString();
		}

		void OnApplicationQuit() {
			RemoveTarget ();
		}
	}
}