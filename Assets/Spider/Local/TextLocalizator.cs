using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Spider.Controller;

namespace Spider.Local {

	public class TextLocalizator : MonoBehaviour {

		public string Section = "";

		Text _current;
		LocalController _local;

		void Awake() {
			_current = this.gameObject.GetComponent<Text> ();
			if (_current == null)
				Destroy (this);
		}

		void Start() {
			if (_current == null)
				return;
			_local = LocalController.Instance;
			string tmp = _current.text;
			_current.text = _local.Local (tmp, Section);
		}

	}
}